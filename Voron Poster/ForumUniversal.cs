using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Voron_Poster
{

    public class ForumUniversal : Forum
    {
        public ForumUniversal()
            : base()
        {
            InitExpression();
        }
        Form a;
        private WebBrowser WB;
        private AutoResetEvent WaitLoad = new AutoResetEvent(true);
        public override void Reset()
        {
            base.Reset();
            if (a != null) a.Dispose();
             a = new Form();
            if (WB != null)

                WB.Dispose();
            WB = new WebBrowser();
            WB.Visible = true;
            WB.ScriptErrorsSuppressed = true;
            WB.Parent = a;
            a.WindowState = FormWindowState.Maximized;
            a.Controls.Add(WB);
            a.Show();
            var b = new Button();
            b.Parent = a;
            b.Click += async (o, e) =>
            {
                await Task.Delay(1000);
                await WaitNavigate("https://ssl.aukro.ua/fnd/authentication/");
            };
            b.Dock = DockStyle.Top;
            a.Controls.Add(b);
            WB.Dock = DockStyle.Fill;
            WB.DocumentCompleted += WB_DocumentComplete;

        }

        class LoginForm
        {
            public HtmlElement Form = null;
            public HtmlElement Login = null;
            public HtmlElement Password = null;
            public HtmlElement Submit = null;
            public int FormMatch = -500;
        }

        #region Browser

        private void InitBrowser()
        {

        }

        private void WB_DocumentComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();
            var browser = (WebBrowser)sender;

            if (!(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                // in AJAX     
            }
            if (e.Url.AbsolutePath != browser.Url.AbsolutePath)
            {
                // IFRAME           
            }
            else
            {
                // REAL DOCUMENT COMPLETE
                WaitLoad.Set();
            }
        }

        #endregion

        #region Expressions

        private static bool ExpressionInited = false;
        private static void InitExpression()
        {
            if (ExpressionInited) return;


            ExpressionInited = true;
        }

        protected static int MatchRate(string Html, SearchExpression[] Expressions)
        {
            int ResultMatch = 0;
            for (int i = 0; i < Expressions.Length; i++)
            {
                if (Html.IndexOf("name=\"" + Expressions[i].Expression + "\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value * 4;
                if (Html.IndexOf("id=\"" + Expressions[i].Expression + "\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value * 4;
                if (Html.IndexOf("class=\"" + Expressions[i].Expression + "\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value * 2;
                if (Html.IndexOf("value=\"" + Expressions[i].Expression + "\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
                if (Html.IndexOf("form_" + Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
                if (Html.IndexOf("form/" + Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
                if (Html.IndexOf("login_" + Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
                if (Html.IndexOf(Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
            }
            return ResultMatch;
        }

        static class Expr
        {

            public static SearchExpression[] LoginInput = new SearchExpression[]{
               // new SearchExpression("type=text", 150), // IE don't show type=text it thinks it's default
                new SearchExpression("login", 30),
                new SearchExpression("user", 30),
                new SearchExpression("username", 30),
                new SearchExpression("email", 20),
                new SearchExpression("e-mail", 20),
                new SearchExpression("nick", 10),
                new SearchExpression("nickname", 10),
                new SearchExpression("Form/Email", 10),
                new SearchExpression("login_username", 30),
                new SearchExpression("логин", 10),
                new SearchExpression("почта", 10),
                new SearchExpression("имя", 10),
                new SearchExpression("ник", 10)
            };
            public static SearchExpression[] PassInput = new SearchExpression[]{
                new SearchExpression("type=password", 300),    
                new SearchExpression("password", 20),
                new SearchExpression("passwrd", 20),
                new SearchExpression("passwd", 20),
                new SearchExpression("pass", 10),
                new SearchExpression("hash", -20),
                new SearchExpression("пароль", 10)
            };
            public static SearchExpression[] LoginFormBad1Input = new SearchExpression[]{
                new SearchExpression("type=hidden", 50),    
                new SearchExpression("type=checkbox", 50), 
                new SearchExpression("type=submit", 50),
                new SearchExpression("type=button", 50),
                new SearchExpression("type=", 150),
                new SearchExpression("display: none", 20)
            };

            public static SearchExpression[] LoginFormBad2Input = new SearchExpression[]{
                new SearchExpression("repeat", 50),
                new SearchExpression("signup", 20),
                new SearchExpression("sign_up", 20),
                new SearchExpression("register", 20),
                new SearchExpression("reset", 20),
                new SearchExpression("new", 20),    
                new SearchExpression("search", 20),  
            };
            public static SearchExpression[] LoginFormStuffInput = new SearchExpression[]{
                new SearchExpression("remember", 50),    
                new SearchExpression("cookie", 50), 
                new SearchExpression("repeat", 50),
                new SearchExpression("hash", 20),
                new SearchExpression("запомнить", 50),
                new SearchExpression("вход", 50),
                new SearchExpression("войти", 20),
                new SearchExpression("signin", 20),
                new SearchExpression("sign_in", 20),
            };
            public static SearchExpression[] LoginFormSubmitInput = new SearchExpression[]{
                new SearchExpression("type=submit", 100),
                new SearchExpression("type=button", 50),
                new SearchExpression("type=hidden", -100),    
                new SearchExpression("type=checkbox", -100),
                new SearchExpression("type=text", -100),
                new SearchExpression("type=password", -50),
                new SearchExpression("submit", 100),
                new SearchExpression("display: none", -200)
            };
        }

        #endregion

        private LoginForm GetLoginForm(HtmlElementCollection Forms)
        {
            LoginForm BestForm = new LoginForm();
            int BestFormRate = int.MinValue;
            for (int i = 0; i < Forms.Count; i++)
            {
                int BestLogin = -500; int BestLoginIndex = -1;
                int BestPass = -500; int BestPassIndex = -1;
                int BestSubmit = -500; int BestSubmitIndex = -1;
                for (int i2 = 0; i2 < Forms[i].All.Count; i2++)
                {
                    string OuterHtml = Forms[i].All[i2].OuterHtml.Replace("'", "").Replace("\"", "");
                    if (Forms[i].All[i2].TagName == "INPUT")
                    {
                        int RateLogin = MatchRate(OuterHtml, Expr.LoginInput) + 150;
                        int RatePass = MatchRate(OuterHtml, Expr.PassInput);
                        int RateBad1 = MatchRate(OuterHtml, Expr.LoginFormBad1Input);
                        int RateBad2 = MatchRate(OuterHtml, Expr.LoginFormBad2Input);
                        int RateStuff = MatchRate(OuterHtml, Expr.LoginFormStuffInput);
                        int RateBadAll = RateBad1 + RateBad2 + RateStuff;
                        int CurrLoginRate = RateLogin * 3 - RatePass - RateBadAll;
                        int CurrPassRate = RatePass * 3 - RateLogin - RateBadAll;
                        if (CurrLoginRate > BestLogin)
                        {
                            BestLogin = CurrLoginRate;
                            BestLoginIndex = i2;
                        }
                        if (CurrPassRate > BestPass)
                        {
                            BestPass = CurrPassRate;
                            BestPassIndex = i2;
                        }
                    }
                    if (Forms[i].All[i2].GetAttribute("type") == "submit")
                    {
                        int RateSubmit = MatchRate(OuterHtml, Expr.LoginFormSubmitInput);
                        if (RateSubmit > BestSubmit)
                        {
                            BestSubmit = RateSubmit;
                            BestSubmitIndex = i2;
                        }
                    }
                }
                string FormOuterHtml = Forms[i].OuterHtml.Replace("'", "").Replace("\"", "");
                int FormRate = MatchRate(FormOuterHtml, Expr.LoginInput)
                             + MatchRate(FormOuterHtml, Expr.PassInput)
                             + MatchRate(FormOuterHtml, Expr.LoginFormStuffInput)
                             - MatchRate(FormOuterHtml, Expr.LoginFormBad2Input);
                int SummRate = FormRate + BestLogin + BestPass;
                if (BestLogin > 100 && BestPass > 100 && BestSubmit > 90 && SummRate > BestFormRate)
                {
                    BestForm.Form = Forms[i];
                    BestForm.Login = Forms[i].All[BestLoginIndex];
                    BestForm.Password = Forms[i].All[BestPassIndex];
                    BestForm.Submit = Forms[i].All[BestSubmitIndex];
                    BestFormRate = SummRate;
                }
            }
            if (BestFormRate >= 100)
                return BestForm;
            else return null;
        }

        private List<Uri> GetPossibleLoginPageLinks(HtmlElementCollection Links)
        {
            List<Uri> LoginLinks = new List<Uri>();
            string CurrentHost = new Uri(Properties.ForumMainPage).Host;
            for (int i = 0; i < Links.Count; i++)
            {
                string Href = Links[i].GetAttribute("href");
                Uri Url;
                if (Uri.TryCreate(Href, UriKind.RelativeOrAbsolute, out Url) &&
                    !Url.IsAbsoluteUri || Url.Host.EndsWith(CurrentHost, StringComparison.OrdinalIgnoreCase))
                {
                    if (Url.OriginalString.IndexOf("login", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        Url.OriginalString.IndexOf("sign", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        Url.OriginalString.IndexOf("auth", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (Url.OriginalString.IndexOf("/login", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            Url.OriginalString.IndexOf("=login", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            Url.OriginalString.IndexOf("index.php", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            Url.OriginalString.IndexOf("/sign", StringComparison.OrdinalIgnoreCase) >= 0)
                            LoginLinks.Insert(0, Url);
                        else
                            LoginLinks.Add(Url);
                    }
                }
            }
            return LoginLinks;
        }


        private async Task<bool> WaitNavigate(string Url)
        {
            return await WaitNavigate(new Uri(Url));
        }

        private async Task<bool> WaitNavigate(Uri Url)
        {
            int i = 0;
            for (i = 0; i < 3; i++)
            {
                WaitLoad.Reset();
                var AbortTimeout = new CancellationTokenSource();
                Task.Delay(RequestTimeout, AbortTimeout.Token).ContinueWith((uselessvar) =>
                {
                    WB.BeginInvoke((Action)(() => { WB.Stop(); }));
                }, TaskContinuationOptions.NotOnCanceled);
                WB.BeginInvoke((Action)(() => WB.Navigate(Url.AbsoluteUri)));
                await WaitFor(WaitLoad);
                AbortTimeout.Cancel();
                bool DocNotNull = true;
                WB.Invoke((Action)(() => { DocNotNull = WB.Document != null; }));
                if (DocNotNull) break;
            }
            return i < 3;
        }

        public override async Task<Exception> Login()
        {

            //Activity = Activity.ContinueWith<Exception>((PrevTask) =>
            //{
            //    WB.BeginInvoke((Action)(() => WB.Dispose()));
            //    return PrevTask.Result;
            //});

            if (!await WaitNavigate(Properties.ForumMainPage)) return new Exception("Сервер не отвечает");
            HtmlElementCollection Links = null;
            WB.Invoke((Action)(() =>
            {
                //  if (WB.Document != null) 
                Links = WB.Document.Links;
            }));
            List<Uri> LoginLinks = GetPossibleLoginPageLinks(Links);
            LoginForm LoginForm = null;
            int LinkIndex = -1;
            while (LoginForm == null && LinkIndex < LoginLinks.Count)
            {
                if (LinkIndex >= 0)
                    if (!await WaitNavigate(LoginLinks[LinkIndex])) return new Exception("Сервер не отвечает");

                HtmlElementCollection Forms = null;
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();

                WB.Invoke((Action)(() =>
                {
                    //if (WB.Document != null) 
                    Forms = WB.Document.Forms;
                }));
                LoginForm = GetLoginForm(Forms);
                if (LoginForm == null) LinkIndex++;
            }
            if (LoginForm == null) return new Exception("Форма авторизации не найдена");
            WaitLoad.Reset();
            LoginForm.Login.SetAttribute("value", Properties.Username);
            LoginForm.Password.SetAttribute("value", Properties.Password);
            LoginForm.Submit.InvokeMember("click");
            //WB.Document.
            await WaitFor(WaitLoad);
            Console.WriteLine(LinkIndex);
            if (LinkIndex == -1)
                Console.WriteLine(Properties.ForumMainPage);
            else Console.WriteLine(LoginLinks[LinkIndex].OriginalString);






            for (int i = -1; i < LinkIndex; i++)
            {

            }
            //form.InvokeMember("submit");

            int ff = 4;
            //            string s = WB.Document.Url.AbsoluteUri;

            //lock (Log) Log.Add("Авторизация: Подготовка данных");

            //var PostData = new FormUrlEncodedContent(new[]
            //    {
            //        new KeyValuePair<string, string>("UserName", Properties.Username.ToLower()),
            //        new KeyValuePair<string, string>("PassWord", Properties.Password)
            //     });
            //Progress[0] += 40;

            //// Send data to login and wait response
            //lock (Log) Log.Add("Авторизация: Запрос авторизации");
            //var Response = await PostAndLog(Properties.ForumMainPage + "index.php?act=Login&CODE=01", PostData);
            //if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            //Progress[0] += 120;
            //string Html = (await Response.Content.ReadAsStringAsync()).ToLower();
            //Progress[0] += 60;

            //// Check if login successfull
            //if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            //if (Html.IndexOf("act=login&amp;code=03") < 0)
            //    return new Exception("Ошибка при авторизации");
            //else
            //{
            //    lock (Log) Log.Add("Успешно авторизирован");
            //    Progress[0] += 35;
            return new Exception("full shit");
            //}
        }


        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {
            return null;
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
            if (TargetForum == null) return new Exception("Неправильная ссылка на тему или раздел");
            string Do = "reply_post";
            if (TargetTopic == null) Do = "new_post";
            Progress[2] += 10 / Progress[3];
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
            Progress[2] += 70 / Progress[3];
            string Html = await Response.Content.ReadAsStringAsync();
            Progress[2] += 30 / Progress[3];
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

                lock (Log) Log.Add("Публикация: отправка запроса");
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                Response = await PostAndLog(Properties.ForumMainPage + "index.php?", FormData);
                Progress[2] += 70 / Progress[3];
                Html = await Response.Content.ReadAsStringAsync();
                Progress[2] += 30 / Progress[3];
                Html = Html.ToLower();
                Progress[2] += 10 / Progress[3];

                // Check if success
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                if (Html.IndexOf("ошибки") >= 0 && Html.IndexOf("обнаружены") >= 0)
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

