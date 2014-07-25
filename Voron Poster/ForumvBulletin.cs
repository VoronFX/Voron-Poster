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
            // Getting loging page
           StatusMessage = "Авторизация: Загрузка страницы";
            var Response =  await GetAndLog(Properties.ForumMainPage);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            progress.Login += 66;
            string Html = await Response.Content.ReadAsStringAsync();
            progress.Login += 30;

            // Search for Id's and calculate hash
           StatusMessage = "Авторизация: Поиск переменных";
            Html = Html.ToLower();
            progress.Login += 13;
            SecurityToken = GetFieldValue(Html, "securitytoken");
            progress.Login += 12;
           StatusMessage = "Авторизация: Подготовка данных";
            string HashPswd = MD5HashStringForUTF8String(str_to_ent(AccountToUse.Password.Trim()));
            string HashPswdUtf = MD5HashStringForUTF8String(AccountToUse.Password.Trim());
            progress.Login += 13;
            var PostData =
                new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("vb_login_username", AccountToUse.Username.ToLower()),
                        new KeyValuePair<string, string>("do", "login"),
                        new KeyValuePair<string, string>("vb_login_md5password", HashPswd),        
                        new KeyValuePair<string, string>("vb_login_md5password_utf", HashPswdUtf), 
                     });
            progress.Login += 12;

            // Send data to login and wait response
           StatusMessage = "Авторизация: Запрос авторизации";
            Response = await PostAndLog(Properties.ForumMainPage + "login.php?do=login", PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            progress.Login += 66;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            progress.Login += 30;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if (Html.IndexOf("login.php?do=login") >= 0)
                return new Exception("Ошибка при авторизации");
            else
            {
               StatusMessage = "Успешно авторизирован";
                progress.Login += 13;
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
           StatusMessage = "Публикация: Подготовка данных";
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
            progress.Post += 7 / progress.PostCount;
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

            // Get the post page
           StatusMessage = "Публикация: Загрузка страницы";
            var Response = await GetAndLog(PostUrl + "?do=new" + Target + "&" + TargetParameter + "=" + TargetValue);
            progress.Post += 60 / progress.PostCount;
            string Html = await Response.Content.ReadAsStringAsync();
            progress.Post += 30 / progress.PostCount;
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

           StatusMessage = "Публикация: Поиск переменных";
            string posthash = GetFieldValue(Html, "posthash");
            progress.Post += 10 / progress.PostCount;
            string poststarttime = GetFieldValue(Html, "poststarttime");
            progress.Post += 10 / progress.PostCount;
            string loggedinuser = GetFieldValue(Html, "loggedinuser");
            progress.Post += 10 / progress.PostCount;
            SecurityToken = GetFieldValue(Html, "securitytoken");
            progress.Post += 10 / progress.PostCount;

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
           StatusMessage = "Публикация: Подготовка данных";
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
            progress.Post += 7 / progress.PostCount;

            // Send data
           StatusMessage = "Публикация: Отправка запроса";
            Response = await PostAndLog(PostUrl, PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            progress.Login += 60;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            progress.Login += 30;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if (Html.IndexOf("main error message") >= 0 ||
                Html.IndexOf(@"<!--/posterror") >= 0)
                return new Exception("Сайт вернул ошибку");
            else
            {
               StatusMessage = "Опубликовано";
                progress.Post += 21 / progress.PostCount;
                return null;
            }
        }
    }
}
