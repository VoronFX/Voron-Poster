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
    
        public override async Task Login()
        {
            // Getting loging page
           StatusMessage = "Авторизация: Загрузка страницы";
            var Response = await GetAndLog(Properties.ForumMainPage + "index.php?action=login");
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Login += 50;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress.Login += 25;

            // Search for Id's and calculate hash
           StatusMessage = "Авторизация: Поиск переменных";
            Html = Html.ToLower();
            Progress.Login += 8;
            CurrSessionID = GetBetweenStrAfterStr(Html, "hashloginpassword", "'", "'");
            Progress.Login += 8;
            AnotherID = GetBetweenStrAfterStr(Html, "value=\"" + CurrSessionID + "\"", "\"", "\"");
            Progress.Login += 8;
           StatusMessage = "Авторизация: Подготовка данных";
            string HashPswd = hashLoginPassword(AccountToUse.Username, AccountToUse.Password, CurrSessionID);
            Progress.Login += 8;

            var PostData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user", AccountToUse.Username.ToLower()),
                        new KeyValuePair<string, string>("cookielength", "-1"),
                        new KeyValuePair<string, string>("passwrd", AccountToUse.Password),        
                        new KeyValuePair<string, string>("hash_passwrd", HashPswd),   
                        new KeyValuePair<string, string>(AnotherID, CurrSessionID)
                     });
            Progress.Login += 8;

            // Send data to login and wait response
           StatusMessage = "Авторизация: Запрос авторизации";
            Response = await PostAndLog(Properties.ForumMainPage + "index.php?action=login2", PostData);
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Login += 60;

           StatusMessage = "Авторизация: Загрузка страницы";
            Response = await GetAndLog(Properties.ForumMainPage);
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Login += 50;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress.Login += 17;

            // Check if login successfull
            Cancel.Token.ThrowIfCancellationRequested();
            if (Html.IndexOf("index.php?action=logout") < 0)
                throw new Exception("Ошибка при авторизации");
            else {
               StatusMessage = "Успешно авторизирован";
                Progress.Login += 13;
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
           StatusMessage = "Публикация: Загрузка капчи";
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

        public override async Task PostMessage(Uri TargetBoard, string Subject, string Message)
        {

            // Get the post page
           StatusMessage = "Публикация: Загрузка страницы";
           Cancel.Token.ThrowIfCancellationRequested();
            var Response = await GetAndLog(Properties.ForumMainPage + "index.php" + TargetBoard.Query + "&action=post");
            Progress.Post += 50 / Progress.PostCount;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress.Post += 20 / Progress.PostCount;
            Cancel.Token.ThrowIfCancellationRequested();
          
            // Get post url
           StatusMessage = "Публикация: Поиск переменных";
            Html = Html.ToLower();
            Progress.Post += 11 / Progress.PostCount;
            string Topic = HttpUtility.ParseQueryString(TargetBoard.Query.Replace(';', '&')).Get("topic");
            if (Topic == null) Topic = "0";
            if (!TryGetPostUrl(Html, out TargetBoard))
                throw new Exception("Не удалось извлечь ссылку для публикации");
            Progress.Post += 11 / Progress.PostCount;
            string SeqNum = GetFieldValue(Html, "seqnum");
            Progress.Post += 11 / Progress.PostCount;

            string Captcha = null;
            // Check and ask if captcha
            Cancel.Token.ThrowIfCancellationRequested();
            if (Uri.TryCreate(GetBetweenStrAfterStr(Html, "class=\"verification_control\"", "src=\"", "\"").Replace(';', '&'),
                UriKind.Absolute, out CaptchaUri) && (CaptchaUri.Scheme == Uri.UriSchemeHttp || CaptchaUri.Scheme == Uri.UriSchemeHttps))
            {
                Progress.Post += 10 / Progress.PostCount;
                //await Task.Run(() => CaptchaForm.IsFree.WaitOne());
                WaitingForQueue = true;
               StatusMessage = "Публикация: В очереди";
                await WaitFor(CaptchaForm.IsFree);
                Progress.Post += 20 / Progress.PostCount;
                CaptchaForm.RefreshFunction = GetCaptcha;
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                Cancel.Token.ThrowIfCancellationRequested();
                Application.OpenForms[0].Invoke((Action)(() => CaptchaForm.ShowDialog()));
                Captcha = CaptchaForm.Result.Text;
                CaptchaForm.IsFree.Set();
                Progress.Post += 20 / Progress.PostCount;
            }
            else Progress.Post += 50 / Progress.PostCount;

            // Form data and post
           StatusMessage = "Публикация: Подготовка данных";
           Cancel.Token.ThrowIfCancellationRequested();
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
                Progress.Post += 11 / Progress.PostCount;

                // Send post
               StatusMessage = "Публикация: Отправка запроса";
               Cancel.Token.ThrowIfCancellationRequested();
                Response =  await PostAndLog(TargetBoard.AbsoluteUri, FormData);
                Progress.Post += 50 / Progress.PostCount;
                Html = await Response.Content.ReadAsStringAsync();
                Progress.Post += 20 / Progress.PostCount;
                Html = Html.ToLower();
                Progress.Post += 11 / Progress.PostCount;

                // Check if success
                Cancel.Token.ThrowIfCancellationRequested();
                if (Html.IndexOf("errorbox") > 0 || Html.IndexOf(Subject.ToLower()) < 0)
                    throw new Exception("Сайт вернул ошибку");
                else
                {
                   StatusMessage = "Опубликовано";
                    Progress.Post += 10 / Progress.PostCount;
                }
            }
        }
    }
}
