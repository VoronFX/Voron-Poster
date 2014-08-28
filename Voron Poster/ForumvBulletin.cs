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

        public override async Task Login()
        {
            // Getting loging page
           StatusMessage = "Авторизация: Загрузка страницы";
            var Response =  await GetAndLog(Properties.ForumMainPage);
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Login += 66;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress.Login += 30;

            // Search for Id's and calculate hash
           StatusMessage = "Авторизация: Поиск переменных";
            Html = Html.ToLower();
            Progress.Login += 13;
            SecurityToken = GetFieldValue(Html, "securitytoken");
            Progress.Login += 12;
           StatusMessage = "Авторизация: Подготовка данных";
            string HashPswd = MD5HashStringForUTF8String(str_to_ent(AccountToUse.Password.Trim()));
            string HashPswdUtf = MD5HashStringForUTF8String(AccountToUse.Password.Trim());
            Progress.Login += 13;
            var PostData =
                new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("vb_login_username", AccountToUse.Username.ToLower()),
                        new KeyValuePair<string, string>("do", "login"),
                        new KeyValuePair<string, string>("vb_login_md5password", HashPswd),        
                        new KeyValuePair<string, string>("vb_login_md5password_utf", HashPswdUtf), 
                     });
            Progress.Login += 12;

            // Send data to login and wait response
           StatusMessage = "Авторизация: Запрос авторизации";
            Response = await PostAndLog(Properties.ForumMainPage + "login.php?do=login", PostData);
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Login += 66;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress.Login += 30;

            // Check if login successfull
            Cancel.Token.ThrowIfCancellationRequested();
            if (Html.IndexOf("login.php?do=login") >= 0)
                throw new Exception("Ошибка при авторизации");
            else
            {
               StatusMessage = "Успешно авторизирован";
                Progress.Login += 13;
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

        public override async Task PostMessage(Uri TargetBoard, string Subject, string Message)
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
            Progress.Post += 7 / Progress.PostCount;
            Cancel.Token.ThrowIfCancellationRequested();

            // Get the post page
           StatusMessage = "Публикация: Загрузка страницы";
            var Response = await GetAndLog(PostUrl + "?do=new" + Target + "&" + TargetParameter + "=" + TargetValue);
            Progress.Post += 60 / Progress.PostCount;
            string Html = await Response.Content.ReadAsStringAsync();
            Progress.Post += 30 / Progress.PostCount;
            Cancel.Token.ThrowIfCancellationRequested();

           StatusMessage = "Публикация: Поиск переменных";
            string posthash = GetFieldValue(Html, "posthash");
            Progress.Post += 10 / Progress.PostCount;
            string poststarttime = GetFieldValue(Html, "poststarttime");
            Progress.Post += 10 / Progress.PostCount;
            string loggedinuser = GetFieldValue(Html, "loggedinuser");
            Progress.Post += 10 / Progress.PostCount;
            SecurityToken = GetFieldValue(Html, "securitytoken");
            Progress.Post += 10 / Progress.PostCount;

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
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Post += 7 / Progress.PostCount;

            // Send data
           StatusMessage = "Публикация: Отправка запроса";
            Response = await PostAndLog(PostUrl, PostData);
            Cancel.Token.ThrowIfCancellationRequested();
            Progress.Login += 60;
            Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress.Login += 30;

            // Check if login successfull
            Cancel.Token.ThrowIfCancellationRequested();
            if (Html.IndexOf("main error message") >= 0 ||
                Html.IndexOf(@"<!--/posterror") >= 0)
                throw new Exception("Сайт вернул ошибку");
            else
            {
               StatusMessage = "Опубликовано";
                Progress.Post += 21 / Progress.PostCount;
            }
        }
    }
}
