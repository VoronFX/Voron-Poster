using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Voron_Poster
{
    public class ForumIPB : Forum
    {
        public ForumIPB() : base() { }


        public override async Task<Exception> Login()
        {
           StatusMessage = "Авторизация: Подготовка данных";

            var PostData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("UserName", AccountToUse.Username.ToLower()),
                    new KeyValuePair<string, string>("PassWord", AccountToUse.Password)
                 });
            progress.Login += 40;

            // Send data to login and wait response
           StatusMessage = "Авторизация: Запрос авторизации";
            var Response = await PostAndLog(Properties.ForumMainPage + "index.php?act=Login&CODE=01", PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            progress.Login += 120;
            string Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            progress.Login += 60;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if ((Html.IndexOf("act=login&amp;code=03") < 0 &&
                Html.IndexOf("Вы вошли как", StringComparison.OrdinalIgnoreCase) < 0) ||
                Html.IndexOf("class=\"errorwrap\"") != -1 ||
                Html.IndexOf("обнаружена ошибка") != -1)
                return new Exception("Ошибка при авторизации");
            else
            {
               StatusMessage = "Успешно авторизирован";
                progress.Login += 35;
                return null;
            }
        }

        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {

            // Get post url
           StatusMessage = "Публикация: Подготовка данных";
            string TargetTopic;
            string TargetForum;
            var Query = HttpUtility.ParseQueryString(TargetBoard.Query.Replace(';', '&'));
            TargetForum = Query.Get("showforum");
            TargetTopic = Query.Get("showtopic");
            if (TargetForum == null) TargetForum = Query.Get("f");
            if (TargetTopic == null) TargetTopic = Query.Get("t");
            if (TargetTopic == null) TargetTopic = String.Empty;
            if (TargetForum == null)
            {
               StatusMessage = "Публикация: Загрузка страницы";
                var ResponseF = await GetAndLog(TargetBoard.AbsoluteUri);
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                string HtmlF = (await ResponseF.Content.ReadAsStringAsync());
                Regex RegFSearch = new Regex(@"[&\?;]f=\d+");
                MatchCollection FMatches = RegFSearch.Matches(HtmlF);
                if (FMatches.Count > 0)
                TargetForum = new string(FMatches.AsEnumerable().Mode().Value.SkipWhile(x => !char.IsDigit(x)).ToArray());
            }
            if (TargetForum == null) return new Exception("Неправильная ссылка на тему или раздел");
            string Do = "reply_post";
            if (TargetTopic == null) Do = "new_post";
            progress.Post += 40 / progress.PostCount;
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

            //var PostData = new FormUrlEncodedContent(new[]
            //        {
            //            new KeyValuePair<string, string>("act", "post"),
            //            new KeyValuePair<string, string>("do", Do),
            //            new KeyValuePair<string, string>("t", TargetTopic),        
            //            new KeyValuePair<string, string>("f", TargetForum)
            //         });

            // Get the post page
           StatusMessage = "Публикация: Загрузка страницы";
            var Response = await GetAndLog(Properties.ForumMainPage + "index.php?act=Post&do="
                + Do + "&f=" + TargetForum + "&t=" + TargetTopic);
            progress.Post += 60 / progress.PostCount;
            string Html = await Response.Content.ReadAsStringAsync();
            progress.Post += 25 / progress.PostCount;
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

           StatusMessage = "Публикация: Поиск переменных";
            string auth_key = GetFieldValue(Html, "auth_key");
            string code = GetFieldValue(Html, "code");
            string attach_post_key = GetFieldValue(Html, "attach_post_key");
            progress.Post += 10 / progress.PostCount;

           StatusMessage = "Публикация: Подготовка данных";
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            using (var FormData = new MultipartFormDataContent())
            {
                if (TargetTopic != String.Empty)
                    FormData.Add(new StringContent(TargetTopic), "t");
                FormData.Add(new StringContent(TargetForum), "f");
                FormData.Add(new StringContent(Subject, Encoding.Default), "TopicTitle");
                //FormData.Add(new StringContent(""), "TopicDesc");
                FormData.Add(new StringContent(Message, Encoding.Default), "Post");

                FormData.Add(new StringContent("Post"), "act");
                FormData.Add(new StringContent(auth_key), "auth_key");
                FormData.Add(new StringContent(code), "CODE");
                FormData.Add(new StringContent(attach_post_key), "attach_post_key");
                progress.Post += 10 / progress.PostCount;

               StatusMessage = "Публикация: Отправка запроса";
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                Response = await PostAndLog(Properties.ForumMainPage + "index.php?", FormData);
                progress.Post += 60 / progress.PostCount;
                Html = await Response.Content.ReadAsStringAsync();
                progress.Post += 25 / progress.PostCount;
                Html = Html.ToLower();
                progress.Post += 10 / progress.PostCount;

                // Check if success
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                if ((Html.IndexOf("ошибки") >= 0 && Html.IndexOf("обнаружены") >= 0)
                    || Html.IndexOf("class=\"errorwrap\"") != -1 ||
                         Html.IndexOf("обнаружена ошибка") != -1)
                    return new Exception("Сайт вернул ошибку");
                else
                {
                   StatusMessage = "Опубликовано";
                    progress.Post += 15 / progress.PostCount;
                    return null;
                }
            }
        }
    }
}

