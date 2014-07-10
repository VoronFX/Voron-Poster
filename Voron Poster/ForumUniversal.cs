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

        private WebBrowser WB;
        private AutoResetEvent WaitLoad = new AutoResetEvent(true);
        public override void Reset()
        {
            base.Reset();
            if (WB != null)

                WB.Dispose();
            WB = new WebBrowser();
            WB.Visible = false;
            WB.ScriptErrorsSuppressed = true;
            WB.Parent = Application.OpenForms[0];
            WB.DocumentCompleted += (o, e) =>
            {
                WaitLoad.Set();
            };

        }


        struct LoginForm
        {
            public HtmlElement Form;
            public HtmlElement Login;
            public HtmlElement Password;
            public int FormMatch;
        }


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
                if (Html.IndexOf(Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
            }
            return ResultMatch;
        }

        static class Expr
        {

            public static SearchExpression[] LoginInput = new SearchExpression[]{
                new SearchExpression("type=text", 150),
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
                new SearchExpression("type=password", 150),    
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
                new SearchExpression("display: none", 20)
            };

            public static SearchExpression[] LoginFormBad2Input = new SearchExpression[]{
                new SearchExpression("repeat", 50),
                new SearchExpression("signup", 20),
                new SearchExpression("sign_up", 20),
                new SearchExpression("register", 20),
                new SearchExpression("reset", 20),
                new SearchExpression("new", 20),    
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
        }

        #endregion

        //private static int IsLoginForm(string FormHtml)
        //{
        //    SearchExpression[] LoginFormExpression = new SearchExpression[]{
        //        new SearchExpression("name=\"login\"", 30),
        //        new SearchExpression("name=\"user\"", 30),
        //        new SearchExpression("name=\"username\"", 30),
        //        new SearchExpression("name=\"password\"", 30),
        //        new SearchExpression("name=\"passwrd\"", 30),
        //        new SearchExpression("type=\"password\"", 30),
        //        new SearchExpression("type=\"pass\"", 30),
        //        new SearchExpression("login", 10),
        //        new SearchExpression("user", 10),
        //        new SearchExpression("username", 10),
        //        new SearchExpression("password", 20),
        //        new SearchExpression("passwrd", 20),
        //        new SearchExpression("pass", 10),
        //        new SearchExpression("signin", 20),
        //        new SearchExpression("sign_in", 20),
        //        new SearchExpression("rememberme", 10),
        //        new SearchExpression("email", 10),
        //        new SearchExpression("e-mail", 10),
        //        new SearchExpression("логин", 10),
        //        new SearchExpression("пароль", 10),
        //        new SearchExpression("войти", 10),
        //        new SearchExpression("вход", 10),
        //        new SearchExpression("имя", 10),
        //        new SearchExpression("cookie", 5),
        //        new SearchExpression("login.php", 20),
        //        new SearchExpression("method=\"post\"", 5),
        //        new SearchExpression("hash", 5),
        //    };
        //    return SearchForExpressions(FormHtml, LoginFormExpression);
        //}



        //private static int IsLoginInput(string InputHtml)
        //{
        //    return 0;


        //    //return SearchForExpressions(Input, LoginInputExpression);
        //}

        //private HtmlElement ChooseLoginForm(HtmlElementCollection Forms)
        //{
        //    int BestLoginFormIndex = -1, BestLoginFormMatch = 0;
        //    for (int i = 0; i < Forms.Count; i++)
        //    {
        //        int CurrMatch = IsLoginForm(Forms[i].OuterHtml.Replace("'", "").Replace("\"", ""));
        //        if (CurrMatch > BestLoginFormMatch)
        //        {
        //            BestLoginFormMatch = CurrMatch;
        //            BestLoginFormIndex = i;
        //        }
        //    }
        //    if (BestLoginFormMatch < 30 || BestLoginFormIndex < 0) return null;
        //    else return Forms[BestLoginFormIndex];
        //}

        private int GetLoginForm(HtmlElementCollection Forms, out LoginForm BestForm)
        {
            BestForm = new LoginForm();
            int BestFormRate = int.MinValue;
            for (int i = 0; i < Forms.Count; i++)
            {
                HtmlElementCollection Inputs = Forms[i].GetElementsByTagName("input");
                int BestLogin = -500; int BestLoginIndex = -1;
                int BestPass = -500; int BestPassIndex = -1;
                for (int i2 = 0; i2 < Inputs.Count; i2++)
                {
                    string OuterHtml = Inputs[i2].OuterHtml.Replace("'", "").Replace("\"", "");
                    int RateLogin = MatchRate(OuterHtml, Expr.LoginInput);
                    int RatePass = MatchRate(OuterHtml, Expr.PassInput);
                    int RateBad1 = MatchRate(OuterHtml, Expr.LoginFormBad1Input);
                    int RateBad2 = MatchRate(OuterHtml, Expr.LoginFormBad2Input);
                    int RateStuff = MatchRate(OuterHtml, Expr.LoginFormStuffInput);
                    int RateBadAll = RateBad1 + RateBad2 + RateStuff;
                    int CurrLoginRate = RateLogin*3 - RatePass - RateBadAll;
                    int CurrPassRate = RatePass*3 - RateLogin - RateBadAll;
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
                string FormOuterHtml = Forms[i].OuterHtml.Replace("'", "").Replace("\"", "");
                int FormRate = MatchRate(FormOuterHtml, Expr.LoginInput)
                             + MatchRate(FormOuterHtml, Expr.PassInput)
                             + MatchRate(FormOuterHtml, Expr.LoginFormStuffInput)
                             - MatchRate(FormOuterHtml, Expr.LoginFormBad2Input);
                int SummRate = FormRate + BestLogin + BestPass;
                if (BestLogin > 100 && BestPass > 100 && SummRate > BestFormRate)
                {
                    BestForm.Form = Forms[i];
                    BestForm.Login = Inputs[BestLoginIndex];
                    BestForm.Password = Inputs[BestPassIndex];
                    BestFormRate = SummRate;
                }
            }
            return BestFormRate;
        }

        public override async Task<Exception> Login()
        {

            Activity.ContinueWith((uselessvar) =>
            {
                Application.OpenForms[0].BeginInvoke((Action)(() => WB.Dispose()));
            });
            WaitLoad.Set();
            WaitLoad.Reset();
            WB.BeginInvoke((Action)(() => WB.Navigate(Properties.ForumMainPage)));
            // WB.BeginInvoke((Action)(() => WB.Navigate(@"http://www.anti-malware.ru/forum/index.php?")));
            await WaitFor(WaitLoad);


            HtmlElementCollection Forms = null;
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            
            
                WB.Invoke((Action)(() => { if (WB.Document != null) Forms = WB.Document.Forms; }));
                LoginForm LoginForm;
                GetLoginForm(Forms, out LoginForm);
            


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
            return null;
            //}
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

