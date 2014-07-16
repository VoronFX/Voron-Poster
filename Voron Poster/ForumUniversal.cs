using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
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
            //InitExpression();
        }
        Form a;
        private WebBrowser WB;
        private AutoResetEvent WaitLoad = new AutoResetEvent(true);
        private Thread WBThread;
        public override void Reset()
        {
            base.Reset();
            if (WB != null && !WB.Disposing)
                WB.Invoke((Action)(() => { WB.Dispose(); }));

            //if (a != null) a.Dispose();
            //a = new Form();
            // WB = new WebBrowser();
            // WB.Visible = true;
            // WB.ScriptErrorsSuppressed = true;
            // WB.Parent = a;
            // a.WindowState = FormWindowState.Maximized;
            // //a.Controls.Add(WB);
            //     //   a.Show();
            // var b = new Button();
            // b.Parent = a;
            // b.Click += async (o, e) =>
            // {
            //     await Task.Delay(1000);
            //     await WaitNavigate("https://ssl.aukro.ua/fnd/authentication/", 2);
            // };
            // b.Dock = DockStyle.Top;
            // a.Controls.Add(b);

            //// WB.Dock = DockStyle.Fill;
            // WB.DocumentCompleted += WB_DocumentComplete;

            WBThread = new Thread(WBContext);
            WBThread.SetApartmentState(ApartmentState.STA);
        }

        private void WBContext()
        {
            WB = new WebBrowser();
            WB.ScriptErrorsSuppressed = true;
            WB.DocumentCompleted += WB_DocumentComplete;
            WB.Disposed += (o, e) => { WB = null; Application.ExitThread(); };
            Application.Run();
        }

        protected class LoginForm
        {
            public HtmlElement Form = null;
            public HtmlElement Login = null;
            public HtmlElement Password = null;
            public HtmlElement Submit = null;
            public int FormMatch = -500;
        }

        protected class PostForm
        {
            public HtmlElement Form = null;
            public HtmlElement Title = null;
            public HtmlElement Message = null;
            public HtmlElement CaptchaImg = null;
            public HtmlElement CaptchaRefresh = null;
            public HtmlElement Captcha = null;
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

        private const int INTERNET_OPTION_END_BROWSER_SESSION = 81;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        #endregion

        #region Expressions

        //private static bool ExpressionInited = false;
        //private static void InitExpression()
        //{
        //    if (ExpressionInited) return;


        //    ExpressionInited = true;
        //}

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
                if (Html.IndexOf("input_" + Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
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
            public static SearchExpression[] BadInput = new SearchExpression[]{
                new SearchExpression("type=hidden", 50),    
                new SearchExpression("type=checkbox", 50), 
                new SearchExpression("type=submit", 50),
                new SearchExpression("type=button", 50),
                new SearchExpression("type=", 150),
                new SearchExpression("display: none", 20),
                new SearchExpression("display:none", 20)
            };
            public static SearchExpression[] LoginFormBadInput = new SearchExpression[]{
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
            public static SearchExpression[] SubmitInput = new SearchExpression[]{
                new SearchExpression("type=submit", 100),
                new SearchExpression("type=button", 50),
                new SearchExpression("type=hidden", -100),    
                new SearchExpression("type=checkbox", -100),
                new SearchExpression("type=text", -100),
                new SearchExpression("type=password", -50),
                new SearchExpression("submit", 100),
                new SearchExpression("display: none", -200),
                new SearchExpression("display:none", -200)
            };
            public static SearchExpression[] LoginSuccess = new SearchExpression[]{
                new SearchExpression("/logout", 100),
                new SearchExpression("logout.php", 100),
                new SearchExpression("logout", 100),
                new SearchExpression("signout", 100),
                new SearchExpression("выход", 100),    
                new SearchExpression("выйти", 100),
                new SearchExpression("вы зашли как", 100),
            };
            public static SearchExpression[] PostFormMessage = new SearchExpression[]{
                new SearchExpression("message", 100),
                new SearchExpression("editor", 100),
                new SearchExpression("msg", 100),
                new SearchExpression("text", 100),
                new SearchExpression("post", 100),
            };
            public static SearchExpression[] SubjectInput = new SearchExpression[]{
               // new SearchExpression("type=text", 150), // IE don't show type=text it thinks it's default
                new SearchExpression("subject", 30),
                new SearchExpression("title", 30),
                new SearchExpression("caption", 30),
                new SearchExpression("text", 10),
                new SearchExpression("заголовок", 20),
                new SearchExpression("тема", 10),
                new SearchExpression("предмет", 10),
                new SearchExpression("название", 10),
            };
            public static SearchExpression[] PostFormStuffInput = new SearchExpression[]{
                new SearchExpression("preview", 50),    
                new SearchExpression("reply", 50), 
            };
            public static SearchExpression[] Captcha = new SearchExpression[]{
                new SearchExpression("captcha", 50),    
                new SearchExpression("capcha", 50),
                new SearchExpression("verification", 50),    
                new SearchExpression("code", 30),
                new SearchExpression("antibot", 30), 
                new SearchExpression("security", 30), 
                new SearchExpression("post_vv[code]", 50)
            };
            public static SearchExpression[] CaptchaReshresh = new SearchExpression[]{
                new SearchExpression("refresh", 50),    
                new SearchExpression("another", 50),
                new SearchExpression("change", 50),    
                new SearchExpression("can't see", 30),
                new SearchExpression("request", 30), 
                new SearchExpression("другое", 30), 
                new SearchExpression("другая", 50)
            };
            public static SearchExpression[] Error = new SearchExpression[]{
                new SearchExpression("обнаружена ошибка", 50),    
                new SearchExpression("обнаружены ошибки", 50),
                new SearchExpression("error occurred", 50),    
                new SearchExpression("errors occurred", 50),
                new SearchExpression("error", 30), 
                new SearchExpression("ошибка", 30), 
            };
        }

        #endregion

        protected LoginForm GetLoginForm(HtmlElementCollection Forms)
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
                        int RateBad1 = MatchRate(OuterHtml, Expr.BadInput);
                        int RateBad2 = MatchRate(OuterHtml, Expr.LoginFormBadInput);
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
                        int RateSubmit = MatchRate(OuterHtml, Expr.SubmitInput);
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
                             - MatchRate(FormOuterHtml, Expr.LoginFormBadInput);
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

        protected PostForm GetPostForm(HtmlElementCollection Forms)
        {
            PostForm BestForm = new PostForm();
            int BestFormRate = int.MinValue;
            for (int i = 0; i < Forms.Count; i++)
            {
                int BestMessage = -500; int BestMessageIndex = -1;
                int BestTitle = -500; int BestTitleIndex = -1;
                int BestSubmit = -500; int BestSubmitIndex = -1;
                int BestCaptcha = -500; int BestCaptchaIndex = -1;
                int BestCaptchaImg = -500; int BestCaptchaImgIndex = -1;
                int BestCaptchaRefresh = -500; int BestCaptchaRefreshIndex = -1;
                for (int i2 = 0; i2 < Forms[i].All.Count; i2++)
                {
                    string OuterHtml = Forms[i].All[i2].OuterHtml.Replace("'", "").Replace("\"", "");
                    if (Forms[i].All[i2].TagName == "TEXTAREA")
                    {
                        int RateMessage = MatchRate(OuterHtml, Expr.PostFormMessage);
                        if (RateMessage > BestMessage)
                        {
                            BestMessage = RateMessage;
                            BestMessageIndex = i2;
                        }
                    }
                    else if (Forms[i].All[i2].TagName == "INPUT")
                    {
                        int RateTitle = MatchRate(OuterHtml, Expr.SubjectInput) + 150;
                        if (RateTitle > BestTitle)
                        {
                            BestTitle = RateTitle;
                            BestTitleIndex = i2;
                        }
                        int RateCaptcha = MatchRate(OuterHtml, Expr.Captcha)
                                        - MatchRate(OuterHtml, Expr.BadInput) * 2;
                        if (RateCaptcha > BestCaptcha)
                        {
                            BestCaptcha = RateCaptcha;
                            BestCaptchaIndex = i2;
                        }
                    }
                    else if (Forms[i].All[i2].TagName == "IMG")
                    {
                        int RateCaptchaImg = MatchRate(OuterHtml, Expr.Captcha);
                        if (RateCaptchaImg > BestCaptchaImg)
                        {
                            BestCaptchaImg = RateCaptchaImg;
                            BestCaptchaImgIndex = i2;
                        }
                    }
                    if (Forms[i].All[i2].GetAttribute("href") != String.Empty)
                    {
                        int RateCaptchaRefresh = MatchRate(OuterHtml, Expr.Captcha)
                                               + MatchRate(OuterHtml, Expr.BadInput)
                                               + MatchRate(OuterHtml, Expr.CaptchaReshresh);
                        if (RateCaptchaRefresh > BestCaptchaRefresh)
                        {
                            BestCaptchaRefresh = RateCaptchaRefresh;
                            BestCaptchaRefreshIndex = i2;
                        }
                    }
                    if (Forms[i].All[i2].GetAttribute("type") == "submit")
                    {
                        int RateSubmit = MatchRate(OuterHtml, Expr.SubmitInput);
                        if (RateSubmit > BestSubmit)
                        {
                            BestSubmit = RateSubmit;
                            BestSubmitIndex = i2;
                        }
                    }
                }
                string FormOuterHtml = Forms[i].OuterHtml.Replace("'", "").Replace("\"", "");
                int FormRate = MatchRate(FormOuterHtml, Expr.PostFormMessage)
                             + MatchRate(FormOuterHtml, Expr.SubjectInput)
                             + MatchRate(FormOuterHtml, Expr.Captcha)
                             + MatchRate(FormOuterHtml, Expr.PostFormStuffInput);
                int SummRate = FormRate + BestMessage + BestTitle;
                if (BestMessage > 100 && BestSubmit > 90 && SummRate > BestFormRate)
                {
                    BestForm.Form = Forms[i];
                    BestForm.Message = Forms[i].All[BestMessageIndex];
                    BestForm.Title = Forms[i].All[BestTitleIndex];
                    BestForm.Submit = Forms[i].All[BestSubmitIndex];
                    if (BestCaptcha >= 30 && BestCaptchaImg >= 30)
                    {
                        BestForm.Captcha = Forms[i].All[BestCaptchaIndex];
                        BestForm.CaptchaImg = Forms[i].All[BestCaptchaImgIndex];
                        if (BestCaptchaRefresh >= 30)
                            BestForm.CaptchaRefresh = Forms[i].All[BestCaptchaRefreshIndex];
                    }
                    BestFormRate = SummRate;
                }
            }
            if (BestFormRate >= 100)
                return BestForm;
            else return null;
        }

        protected List<Uri> GetPossibleLoginPageLinks(HtmlElementCollection Links)
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

            return LoginLinks.Distinct().ToList();
        }

        protected List<Uri> GetPossiblePostPageLinks(HtmlElementCollection Links)
        {
            List<Uri> PostLinks = new List<Uri>();
            string CurrentHost = new Uri(Properties.ForumMainPage).Host;
            for (int i = 0; i < Links.Count; i++)
            {
                string Href = Links[i].GetAttribute("href");
                Uri Url;
                if (Uri.TryCreate(Href, UriKind.RelativeOrAbsolute, out Url) &&
                    !Url.IsAbsoluteUri || Url.Host.EndsWith(CurrentHost, StringComparison.OrdinalIgnoreCase))
                {
                    if (Url.OriginalString.IndexOf("post", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (Url.OriginalString.IndexOf("reply", StringComparison.OrdinalIgnoreCase) >= 0)
                            PostLinks.Insert(0, Url);
                        else
                            PostLinks.Add(Url);
                    }
                }
            }

            return PostLinks.Distinct().ToList();
        }

        protected async Task<bool> WaitNavigate(string Url, int Retry)
        {
            return await WaitNavigate(new Uri(Url), Retry);
        }

        protected async Task<bool> WaitNavigate(Uri Url, int Retry)
        {
            int i = -1;
            for (i = -1; i < Retry; i++)
            {
                WaitLoad.Reset();
                WB.BeginInvoke((Action)(() => WB.Navigate(Url.AbsoluteUri)));
                await WaitAndStop();
                bool DocNotNull = true;
                WB.Invoke((Action)(() =>
                {
                    DocNotNull = WB.Document != null;
                    LogPage();
                }));
                if (DocNotNull) break;
            }
            return i < Retry;
        }

        protected async Task WaitAndStop()
        {
            if (!await WaitFor(WaitLoad, RequestTimeout))
            {
                WB.BeginInvoke((Action)(() => WB.Stop()));
                await WaitFor(WaitLoad, RequestTimeout);
            }
        }

        protected void LogPage()
        {
            WB.Invoke((Action)(() =>
            {
                if (WB.Document != null)
                {
                    Stream documentStream = WB.DocumentStream;
                    using (StreamReader streamReader = new StreamReader(documentStream, Encoding.GetEncoding(WB.Document.Encoding)))
                    {
                        documentStream.Position = 0L;
                        string Url = String.Empty;
                        if (WB.Url != null) Url = WB.Url.AbsoluteUri;
                        HttpLog.Add(new KeyValuePair<object, string>(streamReader.ReadToEnd(), Url));
                    }
                }
            }));
        }

        // Hope to find safe implementation in future, now it's the only works
        unsafe void ClearCookies()
        {
            int option = (int)3/* INTERNET_SUPPRESS_COOKIE_PERSIST*/;
            int* optionPtr = &option;
            bool success = InternetSetOption(IntPtr.Zero, 81/*INTERNET_OPTION_SUPPRESS_BEHAVIOR*/,
                new IntPtr(optionPtr), sizeof(int));
        }

        protected Point GetOffset(HtmlElement el)
        {
            //get element pos
            Point pos = new Point(el.OffsetRectangle.Left, el.OffsetRectangle.Top);

            //get the parents pos
            HtmlElement tempEl = el.OffsetParent;
            while (tempEl != null)
            {
                pos.X += tempEl.OffsetRectangle.Left;
                pos.Y += tempEl.OffsetRectangle.Top;
                tempEl = tempEl.OffsetParent;
            }

            return pos;
        }




        protected Bitmap GetCaptchaSnapshot(HtmlElement Captcha)
        {
            Bitmap bitmap = null;
            Point LeftTop = new Point();
            WB.Invoke((Action)(() =>
            {
                WB.Size = WB.Document.Body.ScrollRectangle.Size;
                LeftTop = GetOffset(Captcha);
                bitmap = new Bitmap(WB.Width, WB.Height);
                WB.DrawToBitmap(bitmap, new Rectangle(new Point(), WB.Size));

            }));

            // IHTMLImgElement img = (IHTMLImgElement)Captcha.DomElement;
            //   IHTMLElementRender render = (IHTMLElementRender)img;

            //Bitmap bmp = new Bitmap(img.width, img.height);
            //Graphics g = Graphics.FromImage(bmp);
            //IntPtr hdc = g.GetHdc();
            //render.DrawToDC(ref );
            //g.ReleaseHdc(hdc);

            //  Captcha.Style[0].
            return bitmap.Clone(new Rectangle(LeftTop.X, LeftTop.Y, Captcha.ClientRectangle.Width, Captcha.ClientRectangle.Height), bitmap.PixelFormat);
        }

        public override async Task<Exception> Login()
        {
            // Run WebBrowser thread
            WBThread.Start();
            // Dispose browser after task done
            Activity = Activity.ContinueWith<Exception>((PrevTask) =>
            {
                WB.BeginInvoke((Action)(() => { if (WB != null) { WB.Dispose(); WB = null; } }));
                try
                {
                    return PrevTask.Result;
                }
                catch (Exception e) { return e; }
            });
            // Wait while wb is creating
            while (WB == null || !WB.IsHandleCreated) Task.Delay(100).Wait();

            // Clear cookies
            ClearCookies();

            // Loading main page
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            Uri LoginPage = new Uri(Properties.ForumMainPage);
            if (!await WaitNavigate(LoginPage, 2)) return new Exception("Сервер не отвечает");
            Progress[0] += 38;

            // Extracting possible login links
            lock (Log) Log.Add("Авторизация: Поиск страницы входа");
            HtmlElementCollection Links = null;
            WB.Invoke((Action)(() => { Links = WB.Document.Links; }));
            List<Uri> LoginLinks = GetPossibleLoginPageLinks(Links);
            LoginForm LoginForm = null;
            HtmlElementCollection Forms = null;
            Progress[0] += 18;
            int LinkIndex = -1;

            while (LoginForm == null && LinkIndex < LoginLinks.Count)
            {
                // Loading next possible page with login form
                if (LinkIndex >= 0)
                {
                    lock (Log) Log.Add("Авторизация: Загрузка страницы");
                    if (!await WaitNavigate(LoginLinks[LinkIndex], 2)) return new Exception("Сервер не отвечает");
                    Progress[0] += 87 / LoginLinks.Count;
                }
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();

                // Checking if any of forms is a login form
                lock (Log) Log.Add("Авторизация: Поиск формы входа");
                WB.Invoke((Action)(() => { Forms = WB.Document.Forms; }));
                LoginForm = GetLoginForm(Forms);
                if (LoginForm == null) LinkIndex++;
            }
            if (LoginForm == null) return new Exception("Форма авторизации не найдена");
            Progress[0] = 143;

            // Fill the form and submit
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            lock (Log) Log.Add("Авторизация: Запрос авторизации");
            if (LinkIndex != -1) LoginPage = LoginLinks[LinkIndex];
            LoginForm.Login.SetAttribute("value", AccountToUse.Username);
            LoginForm.Password.SetAttribute("value", AccountToUse.Password);
            Progress[0] += 18;
            WaitLoad.Reset();
            LoginForm.Submit.InvokeMember("click");
            await WaitAndStop();
            LogPage();
            Progress[0] += 38;

            // Load page with login form again and check if login successful
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            if (!await WaitNavigate(LoginPage, 2)) return new Exception("Сервер не отвечает");
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            Progress[0] += 38;
            string Html = String.Empty;
            WB.Invoke((Action)(() =>
            {
                Forms = WB.Document.Forms;
                Html = WB.Document.Body.OuterHtml;
            }));
            if (GetLoginForm(Forms) != null && MatchRate(Html, Expr.LoginSuccess) < 100)
                return new Exception("Авторизация не удалась");
            else
            {
                lock (Log) Log.Add("Успешно авторизирован");
                Progress[0] += 18;
            }
            return null;
        }


        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {
            // Loading main page
            lock (Log) Log.Add("Публикация: Загрузка страницы");
            Uri PostPage = TargetBoard;
            if (!await WaitNavigate(PostPage, 2)) return new Exception("Сервер не отвечает");
            Progress[2] += 38 / Progress[3];

            // Extracting possible post links
            lock (Log) Log.Add("Публикация: Поиск страницы публикации");
            HtmlElementCollection Links = null;
            WB.Invoke((Action)(() => { Links = WB.Document.Links; }));
            List<Uri> PostLinks = GetPossiblePostPageLinks(Links);
            PostForm PostForm = null;
            HtmlElementCollection Forms = null;
            Progress[2] += 18 / Progress[3];
            int LinkIndex = -1;

            int LastProgress = Progress[2];
            while (PostForm == null && LinkIndex < PostLinks.Count)
            {
                // Loading next possible page with post form
                if (LinkIndex >= 0)
                {
                    lock (Log) Log.Add("Публикация: Загрузка страницы");
                    if (!await WaitNavigate(PostLinks[LinkIndex], 2)) return new Exception("Сервер не отвечает");
                    Progress[2] += 87 / PostLinks.Count / Progress[3];
                }
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();

                // Checking if any of forms is a login form
                lock (Log) Log.Add("Публикация: Поиск формы публикации");
                WB.Invoke((Action)(() => { Forms = WB.Document.Forms; }));
                PostForm = GetPostForm(Forms);
                if (PostForm == null) LinkIndex++;
            }
            if (PostForm == null) return new Exception("Форма публикации не найдена");
            Progress[2] = LastProgress + (87 / Progress[3]);

            // Ask for captcha if any
            if (PostForm.Captcha != null)
            {
                Progress[2] += 10 / Progress[3];
                WaitingForQueue = true;
                lock (Log) Log.Add("Публикация: В очереди");
                await WaitFor(CaptchaForm.IsFree);
                Progress[2] += 20 / Progress[3];
                WB.Invoke((Action)(() =>
                {
                    PostForm.CaptchaImg.AttachEventHandler("onload", (o, e) => { WaitLoad.Set(); });
                }));
                CaptchaForm.Picture.Image = GetCaptchaSnapshot(PostForm.CaptchaImg);
                if (PostForm.CaptchaRefresh != null)
                    CaptchaForm.RefreshFunction = async () =>
                    {
                        lock (Log) Log.Add("Публикация: Загрузка капчи");
                        WaitLoad.Reset();
                        PostForm.CaptchaRefresh.InvokeMember("click");
                        await WaitAndStop();
                        return GetCaptchaSnapshot(PostForm.CaptchaImg);
                    };
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();

                Application.OpenForms[0].Invoke((Action)(() => CaptchaForm.ShowDialog()));
                PostForm.Captcha.SetAttribute("value", CaptchaForm.Result.Text);
                CaptchaForm.IsFree.Set();
                Progress[2] += 20 / Progress[3];
            }
            else Progress[2] += 50 / Progress[3];

            // Fill the form and submit
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            lock (Log) Log.Add("Публикация: Отправка запроса");
            if (LinkIndex != -1) PostPage = PostLinks[LinkIndex];
            if (PostForm.Title != null)
                PostForm.Title.SetAttribute("value", Subject);
            PostForm.Message.SetAttribute("value", Message);
            Progress[2] += 10 / Progress[3];
            WaitLoad.Reset();
            PostForm.Submit.InvokeMember("click");
            await WaitAndStop();
            LogPage();
            Progress[2] += 21 / Progress[3];

            // Check if no error was returned
            if (Cancel.IsCancellationRequested) return new OperationCanceledException();
            string Text = String.Empty;
            WB.Invoke((Action)(() =>
            {
                Forms = WB.Document.Forms;
                Text = WB.Document.Body.InnerText;
            }));
            Progress[2] += 21 / Progress[3];
            if (MatchRate(Text, Expr.Error) >= 30)
                return new Exception("Сайт вернул ошибку");
            else
            {
                lock (Log) Log.Add("Опубликовано");
                Progress[2] += 10 / Progress[3];
            }
            return null;
        }
    }
}

