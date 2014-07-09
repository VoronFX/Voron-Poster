using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Voron_Poster
{
    public class ForumvBulletin : Forum
    {
        private string SecurityToken;
        public ForumvBulletin()
            : base()
        {
            var x = MD5HashStringForUTF8String("LEVEL1");
        }

        private static string MD5HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        public override async Task<Exception> Login()
        {
            lock (Log) Log.Add("Cоединение");

            Progress[0] += 12;

            // Getting loging page
            var Response =  await GetAndLog(Properties.ForumMainPage);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 60;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[0] += 30;

            // Search for Id's and calculate hash
            lock (Log) Log.Add("Авторизация");
            Html = Html.ToLower();
            Progress[0] += 13;
            SecurityToken = GetBetweenStrAfterStr(Html, "securitytoken", "value=\"", "\"");
            Progress[0] += 12;
            string HashPswd = MD5HashStringForUTF8String(str_to_ent(Properties.Password.Trim()));
            string HashPswdUtf = MD5HashStringForUTF8String(Properties.Password.Trim());
            Progress[0] += 13;
            var PostData =
                new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("vb_login_username", Properties.Username.ToLower()),
                        new KeyValuePair<string, string>("do", "login"),
                        new KeyValuePair<string, string>("vb_login_md5password", HashPswd),        
                        new KeyValuePair<string, string>("vb_login_md5password_utf", HashPswdUtf), 
                     });
            Progress[0] += 12;

            // Send data to login and wait response
            Response = await PostAndLog(Properties.ForumMainPage + "login.php?do=login", PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 60;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress[0] += 30;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if (Html.IndexOf("login.php?do=login") >= 0)
                return new Exception("Ошибка при авторизации");
            else
            {
                lock (Log) Log.Add("Успешно авторизирован");
                Progress[0] += 13;
                return null;
            }
        }

        private string str_to_ent(string str)
        {
            var result = String.Empty;
            int i;

            for (i = 0; i < str.Length; i++)
            {
                var c = str[i];
                var tmp = String.Empty;

                if (c > 255)
                {

                    while (c >= 1)
                    {
                        tmp = "0123456789"[c % 10] + tmp;
                        c = (char)(c / 10);
                    }

                    if (tmp == String.Empty)
                    {
                        tmp = "0";
                    }
                    tmp = "#" + tmp;
                    tmp = "&" + tmp;
                    tmp = tmp + ";";

                    result += tmp;
                }
                else
                {
                    result += str[i];
                }
            }
            return result;
        }

        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {
            string PostUrl;
            string TargetParameter = "t";
            string Target = "reply";
            if (TargetBoard.AbsoluteUri.IndexOf("f=") >= 0)
            {
                TargetParameter = "f";
                Target = "thread";
            }
            string TargetValue = HttpUtility.ParseQueryString(TargetBoard.Query.Replace(';', '&')).Get(TargetParameter);
            PostUrl = Properties.ForumMainPage + @"new" + Target + ".php";
            Progress[2] += 7 / Progress[3];
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

            // Get the post page
            lock (Log) Log.Add("Подготовка");
            var Response = await GetAndLog(PostUrl + "?do=new" + Target + "&" + TargetParameter + "=" + TargetValue);
            Progress[2] += 60 / Progress[3];
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[2] += 30 / Progress[3];
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

            string posthash = GetBetweenStrAfterStr(Html, "name=\"posthash\"", "value=\"", "\"");
            Progress[2] += 10 / Progress[3];
            string poststarttime = GetBetweenStrAfterStr(Html, "name=\"poststarttime\"", "value=\"", "\"");
            Progress[2] += 10 / Progress[3];
            string loggedinuser = GetBetweenStrAfterStr(Html, "name=\"loggedinuser\"", "value=\"", "\"");
            Progress[2] += 10 / Progress[3];
            SecurityToken = GetBetweenStrAfterStr(Html, "name=\"securitytoken\"", "value=\"", "\"");
            Progress[2] += 10 / Progress[3];

            // Not working by unkknown encoding problem 
            // PostData = new FormUrlEncodedContent(new[]
            //    {   
            //        new KeyValuePair<string, string>("subject", Subject),
            //        new KeyValuePair<string, string>("message", Message),
            //        new KeyValuePair<string, string>("securitytoken", SecurityToken),        
            //        new KeyValuePair<string, string>("posthash", posthash),  
            //        new KeyValuePair<string, string>("poststarttime", poststarttime),  
            //        new KeyValuePair<string, string>("loggedinuser", loggedinuser),  
            //        new KeyValuePair<string, string>(TargetParameter, TargetValue),  
            //        new KeyValuePair<string, string>("do", "post"+Target)
            //    });
            lock (Log) Log.Add("Публикация");
            StringContent PostData = new StringContent("subject="+Subject
                        + "&message="+Message
                        + "&securitytoken=" + SecurityToken
                        + "&posthash=" + posthash
                        + "&poststarttime=" + poststarttime
                        + "&loggedinuser=" + loggedinuser
                        + "&" + TargetParameter + "=" + TargetValue
                        + "&do=" + "post" + Target, UTF8Encoding.Default,
                        "application/x-www-form-urlencoded");
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[2] += 7 / Progress[3];

            // Send data
            Response = await PostAndLog(PostUrl, PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 60;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress[0] += 30;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if (Html.IndexOf("main error message") >= 0 ||
                Html.IndexOf(@"<!--/posterror") >= 0)
                return new Exception("Сайт вернул ошибку");
            else
            {
                lock (Log) Log.Add("Опубликовано");
                Progress[2] += 21 / Progress[3];
                return null;
            }
        }
    }
}
