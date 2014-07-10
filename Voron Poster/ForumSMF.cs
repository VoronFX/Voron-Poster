using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Voron_Poster
{

    public class ForumSMF : Forum
    {
        private Uri CaptchaUri;
        private string CurrSessionID;
        private string AnotherID;
        public ForumSMF() : base() { }

        private static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        private static string hashLoginPassword(string Username, string Password, string cur_session_id)
        {
            return SHA1HashStringForUTF8String(SHA1HashStringForUTF8String(Username.ToLower() + Password) + cur_session_id);
        }
    
        public override async Task<Exception> Login()
        {
            // Getting loging page
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            var Response = await GetAndLog(Properties.ForumMainPage + "index.php?action=login");
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 50;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[0] += 25;

            // Search for Id's and calculate hash
            lock (Log) Log.Add("Авторизация: Поиск переменных");
            Html = Html.ToLower();
            Progress[0] += 8;
            CurrSessionID = GetBetweenStrAfterStr(Html, "hashloginpassword", "'", "'");
            Progress[0] += 8;
            AnotherID = GetBetweenStrAfterStr(Html, "value=\"" + CurrSessionID + "\"", "\"", "\"");
            Progress[0] += 8;
            lock (Log) Log.Add("Авторизация: Подготовка данных");
            string HashPswd = hashLoginPassword(Properties.Username, Properties.Password, CurrSessionID);
            Progress[0] += 8;

            var PostData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user", Properties.Username.ToLower()),
                        new KeyValuePair<string, string>("cookielength", "-1"),
                        new KeyValuePair<string, string>("passwrd", Properties.Password),        
                        new KeyValuePair<string, string>("hash_passwrd", HashPswd),   
                        new KeyValuePair<string, string>(AnotherID, CurrSessionID)
                     });
            Progress[0] += 8;

            // Send data to login and wait response
            lock (Log) Log.Add("Авторизация: Запрос авторизации");
            Response = await PostAndLog(Properties.ForumMainPage + "index.php?action=login2", PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 60;

            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            Response = await GetAndLog(Properties.ForumMainPage);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 50;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress[0] += 17;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if (Html.IndexOf("index.php?action=logout") < 0)
                return new Exception("Ошибка при авторизации");
            else {
                lock (Log) Log.Add("Успешно авторизирован");
                Progress[0] += 13;
                return null;
            }
        }

        //private bool TryGetStartBoard(string BoardUri, out string Start, out string Board)
        //{
        //    BoardUri = BoardUri.ToLower();
        //    Start = String.Empty;
        //    Board = String.Empty;
        //    int b = BoardUri.LastIndexOf("board=");
        //    if (b < 0 || b + 6 >= BoardUri.Length) return false;
        //    b += 6;
        //    int p = BoardUri.IndexOf('.', b);
        //    if (p <= 0) return false;
        //    for (int i = p + 1; i < BoardUri.Length; i++) Start += BoardUri[i].ToString();
        //    for (int i = b; i < p; i++) Board += BoardUri[i].ToString();
        //    return true;
        //}

        //private bool TryGetStartBoard(string BoardUri, out string StartBoard)
        //{
        //    BoardUri = BoardUri.ToLower();
        //    StartBoard = "start=";
        //    int b = BoardUri.LastIndexOf("board=");
        //    if (b < 0 || b + 6 >= BoardUri.Length) return false;
        //    b += 6;
        //    int p = BoardUri.IndexOf('.', b);
        //    if (p <= 0) return false;
        //    for (int i = p + 1; i < BoardUri.Length; i++) StartBoard += BoardUri[i].ToString();
        //    StartBoard += ";board=";
        //    for (int i = b + 1; i < p; i++) StartBoard += BoardUri[i].ToString();
        //    return true;
        //}

        private bool TryGetPostUrl(string Html, out Uri PostUri)
        {
            PostUri = null;
            int b = Html.IndexOf("index.php?action=post2");
            if (b < 0) return false;
            int e = Html.IndexOf("\"", b + 1);
            if (e < 0) return false;
            b = Html.LastIndexOf("\"", b);
            if (b < 0) return false;
            if (Uri.TryCreate(Html.Substring(b + 1, e - b - 1), UriKind.Absolute,
                out PostUri) && (PostUri.Scheme == Uri.UriSchemeHttp || PostUri.Scheme == Uri.UriSchemeHttps))
                return true;
            else return false;
        }

        private async Task<Bitmap> GetCaptcha()
        {
            lock (Log) Log.Add("Публикация: Загрузка капчи");
            var Response = await Client.GetAsync(CaptchaUri, Cancel.Token);
            return new Bitmap(await Response.Content.ReadAsStreamAsync());
            //Random r = new Random();
            //string NewRand = String.Empty;
            //int n = HttpUtility.ParseQueryString(CaptchaUri.Query).Get("rand").Length;
            //for (int i = 0; i < n; i++)
            //{
            //    int ran = r.Next(15);
            //    if (ran > 9)
            //        NewRand += ((char)(ran + 87)).ToString();
            //    else NewRand += ((char)ran).ToString();
            //}
            //HttpUtility.ParseQueryString(CaptchaUri.Query).Set("rand", NewRand);
        }

        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {

            // Get the post page
            lock (Log) Log.Add("Публикация: Загрузка страницы");
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            var Response = await GetAndLog(Properties.ForumMainPage + "index.php" + TargetBoard.Query + "&action=post");
            Progress[2] += 50 / Progress[3];
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[2] += 20 / Progress[3];
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
          
            // Get post url
            lock (Log) Log.Add("Публикация: Поиск переменных");
            Html = Html.ToLower();
            Progress[2] += 11 / Progress[3];
            string Topic = HttpUtility.ParseQueryString(TargetBoard.Query.Replace(';', '&')).Get("topic");
            if (Topic == null) Topic = "0";
            if (!TryGetPostUrl(Html, out TargetBoard))
                return new Exception("Не удалось извлечь ссылку для публикации");
            Progress[2] += 11 / Progress[3];
            string SeqNum = GetFieldValue(Html, "seqnum");
            Progress[2] += 11 / Progress[3];

            string Captcha = null;
            // Check and ask if captcha
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if (Uri.TryCreate(GetFieldValue(Html, "class", "verification_control", "src").Replace(';', '&'),
                UriKind.Absolute, out CaptchaUri) && (CaptchaUri.Scheme == Uri.UriSchemeHttp || CaptchaUri.Scheme == Uri.UriSchemeHttps))
            {
                Progress[2] += 10 / Progress[3];
                //await Task.Run(() => CaptchaForm.IsFree.WaitOne());
                WaitingForQueue = true;
                lock (Log) Log.Add("Публикация: В очереди");
                await WaitFor(CaptchaForm.IsFree);
                Progress[2] += 20 / Progress[3];
                CaptchaForm.RefreshFunction = GetCaptcha;
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                Application.OpenForms[0].Invoke((Action)(() => CaptchaForm.ShowDialog()));
                Captcha = CaptchaForm.Result.Text;
                CaptchaForm.IsFree.Set();
                Progress[2] += 20 / Progress[3];
            }
            else Progress[2] += 50 / Progress[3];

            // Form data and post
            lock (Log) Log.Add("Публикация: Подготовка данных");
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            using (var FormData = new MultipartFormDataContent())
            {
                FormData.Add(new StringContent(Topic), "topic");
                FormData.Add(new StringContent(Subject), "subject");
                FormData.Add(new StringContent(Message), "message");
                if (Captcha != null)
                    FormData.Add(new StringContent(Captcha), "post_vv[code]");
                FormData.Add(new StringContent(SeqNum), "seqnum");
                FormData.Add(new StringContent("0"), "message_mode");
                FormData.Add(new StringContent(CurrSessionID), AnotherID);

                FormData.Add(new StringContent("0"), "additional_options");
                FormData.Add(new StringContent("0"), "lock");
                FormData.Add(new StringContent("0"), "notify");
                //FormData.Add(new StringContent(""), "sel_color");
                //FormData.Add(new StringContent(""), "sel_size");
                //FormData.Add(new StringContent(""), "sel_face");
                //FormData.Add(new StringContent("xx"), "icon");            
                Progress[2] += 11 / Progress[3];

                // Send post
                lock (Log) Log.Add("Публикация: отправка запроса");
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                Response =  await PostAndLog(TargetBoard.AbsoluteUri, FormData);
                Progress[2] += 50 / Progress[3];
                Html = await Response.Content.ReadAsStringAsync();
                Progress[2] += 20 / Progress[3];
                Html = Html.ToLower();
                Progress[2] += 11 / Progress[3];

                // Check if success
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                if (Html.IndexOf("errorbox") > 0 || Html.IndexOf(Subject.ToLower()) < 0)
                    return new Exception("Сайт вернул ошибку");
                else
                {
                    lock (Log) Log.Add("Опубликовано");
                    Progress[2] += 10 / Progress[3];
                    return null;
                }
            }
        }
    }
}
