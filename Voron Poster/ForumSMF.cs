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
        public string h;
        string CurrSessionID;
        string AnotherID;
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

        public override async Task<bool> Login(string Username, string Password)
        {
            lock (Log) { Log.Add("Cоединение с сервером"); }
            try
            {
                HttpResponseMessage RespMes = await Client.GetAsync(MainPage.AbsoluteUri + "index.php?action=login", Cancel.Token);
                Progress++;
                string Html = await RespMes.Content.ReadAsStringAsync();
                lock (Log) { Log.Add("Авторизация"); Progress++; }
                Html = Html.ToLower();
                CurrSessionID = GetBetweenStrAfterStr(Html, "hashloginpassword", "'", "'");
                string HashPswd = hashLoginPassword(Username, Password, CurrSessionID);
                AnotherID = GetBetweenStrAfterStr(Html, "value=\"" + CurrSessionID + "\"", "\"", "\"");
                var PostData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user", Username.ToLower()),
                        new KeyValuePair<string, string>("cookielength", "-1"),
                        new KeyValuePair<string, string>("passwrd", Password),        
                        new KeyValuePair<string, string>("hash_passwrd", HashPswd),   
                        new KeyValuePair<string, string>(AnotherID, CurrSessionID)
                     });
                Progress++;
                RespMes = await Client.PostAsync(MainPage.AbsoluteUri + "index.php?action=login2", PostData, Cancel.Token);
                Progress++;
                Html = await RespMes.Content.ReadAsStringAsync();
                if (Html.ToLower().IndexOf("index.php?action=logout") >= 0)
                {
                    lock (Log) { Log.Add("Успешно авторизирован"); Progress++; }
                    return true;
                }
                lock (Log) { Log.Add("Ошибка при авторизации"); }
                return false;
            }
            catch (Exception e)
            {
                lock (Log) { Log.Add("Ошибка: " + e.Message); }
                return false;
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
                out PostUri) && PostUri.Scheme == Uri.UriSchemeHttp)
                return true;
            else return false;
        }


        private async Task<bool> GetCaptcha(CaptchaForm CaptchaForm)
        {
            CaptchaForm.button3.Enabled = false;
            Bitmap Captcha = new Bitmap(10, 10);
            try
            {
                HttpResponseMessage RespMes = await Client.GetAsync(CaptchaUri, Cancel.Token);
                lock (Log) { Log.Add("Загружаю каптчу"); };
                CaptchaForm.pictureBox1.Image = new Bitmap(await RespMes.Content.ReadAsStreamAsync());
                CaptchaForm.ClientSize = CaptchaForm.ClientSize - CaptchaForm.pictureBox1.Size + CaptchaForm.pictureBox1.Image.Size;
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
                return true;
            }
            catch (Exception e)
            {
                lock (Log) { Log.Add("Ошибка: " + e.Message); }
                return false;
            }
            finally
            {
                CaptchaForm.button3.Enabled = true;
            }
        }

        public override async Task<bool> PostMessage(Uri TargetBoard, string Subject, string BBText)
        {
            CaptchaForm CaptchaForm = null;
            try
            {
                HttpResponseMessage RespMes = await Client.GetAsync(MainPage.AbsoluteUri
                    + "index.php" + TargetBoard.Query + "&action=post", Cancel.Token);
                Progress++;
                string Html = await RespMes.Content.ReadAsStringAsync();
                lock (Log) { Log.Add("Подготовка"); Progress++; }
                Html = Html.ToLower();
                string Topic = HttpUtility.ParseQueryString(TargetBoard.Query.Replace(';', '&')).Get("topic");
                if (Topic == null) Topic = "0";
                if (!TryGetPostUrl(Html, out TargetBoard))
                {
                    lock (Log) { Log.Add("Ошибка не удалось извлечь ссылку для публикации"); }
                    return false;
                }
                string SeqNum = GetBetweenStrAfterStr(Html, "name=\"seqnum\"", "value=\"", "\"");
                if (Uri.TryCreate(GetBetweenStrAfterStr(Html, "class=\"verification_control\"", "src=\"", "\"").Replace(';', '&'),
                    UriKind.Absolute, out CaptchaUri) && CaptchaUri.Scheme == Uri.UriSchemeHttp)
                {
                    CaptchaForm = new CaptchaForm();
                    CaptchaForm.func = GetCaptcha;
                    CaptchaForm.button2.Click += new System.EventHandler((object o, EventArgs e) => { Cancel.Cancel(); });
                    await GetCaptcha(CaptchaForm);
                    Progress++;
                    CaptchaForm.ShowDialog();
                }
                else Progress++;
                lock (Log) { Log.Add("Публикация"); }
                using (var FormData = new MultipartFormDataContent())
                {
                    FormData.Add(new StringContent(Topic), "topic");
                    FormData.Add(new StringContent(Subject), "subject");
                    FormData.Add(new StringContent(BBText), "message");
                    if (CaptchaForm != null)
                        FormData.Add(new StringContent(CaptchaForm.textBox1.Text), "post_vv[code]");
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

                    RespMes = await Client.PostAsync(TargetBoard.AbsoluteUri, FormData, Cancel.Token);
                    Progress++;
                    Html = await RespMes.Content.ReadAsStringAsync();
                    Html = Html.ToLower();
                    if (Html.IndexOf("errorbox") > 0 || Html.IndexOf(Subject) < 0)
                    {
                        lock (Log) { Log.Add("Ошибка"); Progress++; }
                        return false;
                    }
                    else
                    {
                        lock (Log) { Log.Add("Тема создана"); Progress++; }
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                lock (Log) { Log.Add("Ошибка: " + e.Message); }
                return false;
            }
            finally
            {
                CaptchaForm.Dispose();
            }
        }
    }
}
