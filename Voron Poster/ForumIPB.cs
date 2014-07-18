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
            lock (Log) Log.Add("Авторизация: Подготовка данных");

            var PostData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("UserName", AccountToUse.Username.ToLower()),
                    new KeyValuePair<string, string>("PassWord", AccountToUse.Password)
                 });
            Progress[0] += 40;

            // Send data to login and wait response
            lock (Log) Log.Add("Авторизация: Запрос авторизации");
            var Response = await PostAndLog(Properties.ForumMainPage + "index.php?act=Login&CODE=01", PostData);
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 120;
            string Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            Progress[0] += 60;

            // Check if login successfull
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            if ((Html.IndexOf("act=login&amp;code=03") < 0 &&
                Html.IndexOf("Вы вошли как", StringComparison.OrdinalIgnoreCase) < 0) ||
                Html.IndexOf("class=\"errorwrap\"") != -1 ||
                Html.IndexOf("обнаружена ошибка") != -1)
                return new Exception("Ошибка при авторизации");
            else
            {
                lock (Log) Log.Add("Успешно авторизирован");
                Progress[0] += 35;
                return null;
            }
        }

        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {

            // Get post url
            lock (Log) Log.Add("Публикация: Подготовка данных");
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
                lock (Log) Log.Add("Публикация: Загрузка страницы");
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
            Progress[2] += 40 / Progress[3];
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

            //var PostData = new FormUrlEncodedContent(new[]
            //        {
            //            new KeyValuePair<string, string>("act", "post"),
            //            new KeyValuePair<string, string>("do", Do),
            //            new KeyValuePair<string, string>("t", TargetTopic),        
            //            new KeyValuePair<string, string>("f", TargetForum)
            //         });

            // Get the post page
            lock (Log) Log.Add("Публикация: Загрузка страницы");
            var Response = await GetAndLog(Properties.ForumMainPage + "index.php?act=Post&do="
                + Do + "&f=" + TargetForum + "&t=" + TargetTopic);
            Progress[2] += 60 / Progress[3];
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[2] += 25 / Progress[3];
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();

            lock (Log) Log.Add("Публикация: Поиск переменных");
            string auth_key = GetFieldValue(Html, "auth_key");
            string code = GetFieldValue(Html, "code");
            string attach_post_key = GetFieldValue(Html, "attach_post_key");
            Progress[2] += 10 / Progress[3];

            lock (Log) Log.Add("Публикация: Подготовка данных");
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
                Progress[2] += 10 / Progress[3];

                lock (Log) Log.Add("Публикация: Отправка запроса");
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                Response = await PostAndLog(Properties.ForumMainPage + "index.php?", FormData);
                Progress[2] += 60 / Progress[3];
                Html = await Response.Content.ReadAsStringAsync();
                Progress[2] += 25 / Progress[3];
                Html = Html.ToLower();
                Progress[2] += 10 / Progress[3];

                // Check if success
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                if ((Html.IndexOf("ошибки") >= 0 && Html.IndexOf("обнаружены") >= 0)
                    || Html.IndexOf("class=\"errorwrap\"") != -1 ||
                         Html.IndexOf("обнаружена ошибка") != -1)
                    return new Exception("Сайт вернул ошибку");
                else
                {
                    lock (Log) Log.Add("Опубликовано");
                    Progress[2] += 15 / Progress[3];
                    return null;
                }
            }
        }
    }
}

