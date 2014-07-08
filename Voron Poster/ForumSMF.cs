using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Voron_Poster
{
    class ForumSMF : Forum
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

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        private static string hashLoginPassword(string Username, string Password, string cur_session_id)
        {
            return SHA1HashStringForUTF8String(SHA1HashStringForUTF8String(Username.ToLower() + Password) + cur_session_id);
        }

        private string GetBetweenStrAfterStr(string Html, string After, string Beg, string End)
        {
            int b = Html.IndexOf(After);
            if (b < 0 || b + After.Length >= Html.Length) return "";
            b = Html.IndexOf(Beg, b + After.Length);
            if (b < 0 || b + Beg.Length >= Html.Length) return "";
            int e = Html.IndexOf(End, b + Beg.Length);
            if (e > 0)
                return Html.Substring(b + Beg.Length, e - b - Beg.Length);
            return "";
        }

        public override async Task<Exception> Login()
        {
            lock (Log) Log.Add("Cоединение");

            // Creating client
            if (Client != null) Client.Dispose();
            Client = new HttpClient();
            Client.Timeout = RequestTimeout;
            Progress[0] += 12;

            // Getting loging page
            var Response = await Client.GetAsync(Properties.ForumMainPage + "index.php?action=login", Cancel.Token);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 60;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[0] += 30;

            // Search for Id's and calculate hash
            lock (Log) Log.Add("Авторизация");
            Html = Html.ToLower();
            Progress[0] += 10;
            CurrSessionID = GetBetweenStrAfterStr(Html, "hashloginpassword", "'", "'");
            Progress[0] += 10;
            string HashPswd = hashLoginPassword(Properties.Username, Properties.Password, CurrSessionID);
            Progress[0] += 10;
            AnotherID = GetBetweenStrAfterStr(Html, "value=\"" + CurrSessionID + "\"", "\"", "\"");
            Progress[0] += 10;
            var PostData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user", Properties.Username.ToLower()),
                        new KeyValuePair<string, string>("cookielength", "-1"),
                        new KeyValuePair<string, string>("passwrd", Properties.Password),        
                        new KeyValuePair<string, string>("hash_passwrd", HashPswd),   
                        new KeyValuePair<string, string>(AnotherID, CurrSessionID)
                     });
            Progress[0] += 10;

            // Send data to login and wait response
            Response = await Client.PostAsync(Properties.ForumMainPage + "index.php?action=login2", PostData, Cancel.Token);
            if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
            Progress[0] += 60;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress[0] += 30;

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
            lock (Log) Log.Add("Загружаю капчу");
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
            if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
            var Response = await Client.GetAsync(Properties.ForumMainPage
                + "index.php" + TargetBoard.Query + "&action=post", Cancel.Token);
            Progress[1] += 50;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[1] += 20;
            if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
          
            // Get post url
            lock (Log) Log.Add("Подготовка");
            Html = Html.ToLower();
            Progress[1] += 11;
            string Topic = HttpUtility.ParseQueryString(TargetBoard.Query.Replace(';', '&')).Get("topic");
            if (Topic == null) Topic = "0";
            if (!TryGetPostUrl(Html, out TargetBoard))
                return new Exception("Не удалось извлечь ссылку для публикации");
            Progress[1] += 11;
            string SeqNum = GetBetweenStrAfterStr(Html, "name=\"seqnum\"", "value=\"", "\"");
            Progress[1] += 11;

            // Check and ask if captcha
            if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
            if (Uri.TryCreate(GetBetweenStrAfterStr(Html, "class=\"verification_control\"", "src=\"", "\"").Replace(';', '&'),
                UriKind.Absolute, out CaptchaUri) && (CaptchaUri.Scheme == Uri.UriSchemeHttp || CaptchaUri.Scheme == Uri.UriSchemeHttps))
            {
                Progress[1] += 10;
                Task Wait = new System.Threading.Tasks.Task(() => CaptchaForm.IsFree.WaitOne());
                Wait.Start();
                await Wait;
                Progress[1] += 20;
                CaptchaForm.RefreshFunction = GetCaptcha;
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                CaptchaForm.ShowDialog();
                Progress[1] += 20;
            }
            else Progress[1] += 50;

            // Form data and post
            lock (Log) Log.Add("Публикация");
            if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
            using (var FormData = new MultipartFormDataContent())
            {
                FormData.Add(new StringContent(Topic), "topic");
                FormData.Add(new StringContent(Subject), "subject");
                FormData.Add(new StringContent(Message), "message");
                if (CaptchaForm != null)
                    FormData.Add(new StringContent(CaptchaForm.Result.Text), "post_vv[code]");
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
                Progress[1] += 11;

                // Send post
                if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                Response = await Client.PostAsync(TargetBoard.AbsoluteUri, FormData, Cancel.Token);
                Progress[1] += 50;
                Html = await Response.Content.ReadAsStringAsync();
                Progress[1] += 20;
                Html = Html.ToLower();
                Progress[1] += 11;

                // Check if success
                if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                if (Html.IndexOf("errorbox") > 0 || Html.IndexOf(Subject.ToLower()) < 0)
                    return new Exception("Сайт вернул ошибку");
                else
                {
                    lock (Log) Log.Add("Опубликовано");
                    Progress[1] += 10;
                    return null;
                }
            }
        }
    }
}
