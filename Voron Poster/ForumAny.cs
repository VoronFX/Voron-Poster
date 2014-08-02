#define DEBUGANYFORUM
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
namespace Voron_Poster
{
    public class ForumAny : Forum
    {
        public ForumAny() : base() {
            Regex.CacheSize = 100;
        }

        protected static async Task<HtmlAgilityPack.HtmlDocument> InitHtml(HttpResponseMessage response)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            // Otherwise forms have no children. Why it doesn't work out of the box =_= 
            HtmlNode.ElementsFlags.Remove("form");

            // Detect encoding by our own if stupid server such as rutracker.org forgets to say it in header
            Encoding Encoding = null;
            try
            {
                if (!String.IsNullOrEmpty(response.Content.Headers.ContentType.CharSet))
                    Encoding = Encoding.GetEncoding(response.Content.Headers.ContentType.CharSet);
            }
            catch { }
            if (Encoding == null)
            {
                string Html = await response.Content.ReadAsStringAsync();
                Match CharsetMatch = new Regex(@"(?i)charset\s*=[\s""']*([^\s""'/>]*)").Match(Html);
                string Charset = null;
                if (CharsetMatch != null)
                    Charset = new Regex(@"(?i)charset\s*=[\s""']*").Replace(CharsetMatch.Value, String.Empty);
                try
                {
                    if (!String.IsNullOrEmpty(Charset))
                        Encoding = Encoding.GetEncoding(Charset);
                }
                catch { }
            }
            if (Encoding == null) Encoding = Encoding.Default;
            Stream stream = await response.Content.ReadAsStreamAsync();
            doc.Load(stream, Encoding);
            doc.ClearScriptsStylesComments();
            return doc;
        }

        protected static class Expr
        {
            public static string[] NotSupported = {   "www.nulled.cc", // ip ban after post request 
                                                      "www.maultalk.com", // hard captcha request
                                                      "www.master-x.com" // "not found" on login post request 
                                                  };
            //     public static string[] EasySkipCaptcha = { "www.maultalk.com" }; // not working

            public static class Url
            {
                public static string Action = @"(?i)[&\?]?(act(ion)?|do|mode)=";
                public static string Login = @"(?i)(?<!b)(sign|log)[\W_]?in|auth\w*"; //|user|member|acc?ount
                public static string Register = @"(?i)register|sign[\W_]?up|(create|new)[\W_]?(user|member|acc?ount)?";
                public static string Reply = @"(?i)reply|(?<!new[\W_]?)post|no[\W_]?quot(e|ing)";
                public static string NewTopic = @"(?i)new[\W_]?(topic|thread|post)";
                public static string CaptchaImage = @"(?i)verif(y|ication)|code|capt?cha|anti[\W_]?bot";
                public static string Quote = @"(?i)(?<!no[\W_]?)quot(e|ing)";
                public static string Poll = @"(?i)poll";
                public static string LoggedIn = @"(?i)" +
                                                @"(sign|log)[\W_]?out|" +
                                                @"unauth|" +
                                                @"edit[\W_]?profile|" +
                                                @"profile[\W_]?(settings|options|edit)";
            }
            public static class Text
            {
                public class LocalText
                {
                    public class LinkText
                    {
                        public string Login;
                        public string Register;
                        public string Reply;
                        public string NewTopic;
                        public string Quote;
                        public string Poll;
                        public string CaptchaImage;
                        public string LoggedIn;
                        public static LinkText operator +(LinkText a, LinkText b)
                        {
                            if (a == null && b == null) return null;
                            else if (a == null) return b;
                            else if (b == null) return a;
                            return new LinkText
                            {
                                Login = JoinExpression(a.Login, b.Login),
                                Register = JoinExpression(a.Register, b.Register),
                                Reply = JoinExpression(a.Reply, b.Reply),
                                NewTopic = JoinExpression(a.NewTopic, b.NewTopic),
                                Quote = JoinExpression(a.Quote, b.Quote),
                                CaptchaImage = JoinExpression(a.CaptchaImage, b.CaptchaImage),
                                Poll = JoinExpression(a.Poll, b.Poll),
                                LoggedIn = JoinExpression(a.LoggedIn, b.LoggedIn)
                            };
                        }
                    }
                    public class MessageText
                    {
                        public string LoginSuccess;
                        public string PostSuccess;
                        public string Error;
                        public static MessageText operator +(MessageText a, MessageText b)
                        {
                            if (a == null && b == null) return null;
                            else if (a == null) return b;
                            else if (b == null) return a;
                            return new MessageText
                            {
                                LoginSuccess = JoinExpression(a.LoginSuccess, b.LoginSuccess),
                                PostSuccess = JoinExpression(a.PostSuccess, b.PostSuccess),
                                Error = JoinExpression(a.Error, b.Error)
                            };
                        }
                    }
                    public class FieldsNames
                    {
                        public string Username;
                        public string Password;
                        public string RepeatPassword;
                        public string Subject;
                        public string Message;
                        public string Captcha;
                        public string Preview;
                        public static FieldsNames operator +(FieldsNames a, FieldsNames b)
                        {
                            if (a == null && b == null) return null;
                            else if (a == null) return b;
                            else if (b == null) return a;
                            return new FieldsNames
                            {
                                Username = JoinExpression(a.Username, b.Username),
                                Password = JoinExpression(a.Password, b.Password),
                                RepeatPassword = JoinExpression(a.RepeatPassword, b.RepeatPassword),
                                Subject = JoinExpression(a.Subject, b.Subject),
                                Message = JoinExpression(a.Message, b.Message),
                                Captcha = JoinExpression(a.Captcha, b.Captcha),
                                Preview = JoinExpression(a.Preview, b.Preview)
                            };
                        }
                    }

                    public LinkText Link;
                    public MessageText Message;
                    public FieldsNames Fields;

                    protected static string JoinExpression(string a, string b)
                    {
                        if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return null;
                        else if (String.IsNullOrEmpty(a)) return b;
                        else if (String.IsNullOrEmpty(b)) return a;
                        return a + '|' + b;
                    }
                    public static LocalText operator +(LocalText a, LocalText b)
                    {
                        if (a == null && b == null) return null;
                        else if (a == null) return b;
                        else if (b == null) return a;
                        return new LocalText
                        {
                            Link = a.Link + b.Link,
                            Message = a.Message + b.Message,
                            Fields = a.Fields + b.Fields
                        };
                    }
                    public static LocalText Join(LocalText[] array)
                    {
                        LocalText t = new LocalText();
                        for (int i = 0; i < array.Length; i++)
                            t += array[i];
                        return t;
                    }
                }

                #region English
                public static LocalText English = new LocalText
                {
                    #region Link
                    Link = new LocalText.LinkText
                    {
                        Login = @"(?i)(?<!b)(sign|log)\s?in|auth\w*",

                        Register = @"(?i)register|sign\s?up|(create|new)\s(user|member|acc?ount)",

                        Reply = @"(?i)reply",
                        NewTopic = @"(?i)(create|open|new)\s(topic|thread)",
                        Quote = @"(?i)(?<!no\s?)quot(e|ing)",
                        Poll = @"(?i)poll",
                        CaptchaImage = Expr.Url.CaptchaImage
                    },
                    #endregion

                    #region Message
                    Message = new LocalText.MessageText
                    {
                        Error = @"(?i)" +
                                @"(login|user(name)?|password)\s+(incorrect|wrong)|" +
                                @"errors?\s+occurred|please\s+enter\s+a\s+valid\s+message|" +
                                @"do\s?n['o]t\s+have\s+permission|" +
                                @"user.+?(could\s+)?n['o]t\s+(be\s+)?found|" +
                                @"must\s+be\s+logg?ed(-?\s?in)|" +
                                @"(incorrect|wrong)\s+password|please[\s\W]+try\s+again"
                    },
                    #endregion

                    #region Fields
                    Fields = new LocalText.FieldsNames
                    {
                        Username = @"(?i)user|name|login|e[\W_]?mail|nick",
                        Password = @"(?i)pass(wr?d|word)?",
                        RepeatPassword = @"(?i)match|repeat",
                        Subject = @"(?i)subject|title",
                        Message = @"(?i)post|reply|message|editor|text|txt",
                        Captcha = @"(?i)capt?cha|code",
                        Preview = @"(?i)preview"
                    }
                    #endregion
                };
                #endregion

                #region Russian
                public static LocalText Russian = new LocalText
                {
                    #region Link
                    Link = new LocalText.LinkText
                    {
                        Login = @"(?i)(вход|войти|авториз(ация|ироваться)|запомнить|логин)\b",

                        Register = @"(?i)" +
                                   @"(за)?регистр(ир(овать|уйтесь)|ация)|" +
                                   @"(нов(ый|ого)|создать)\s+(пользовател[ья]|акк?аунт)|" +
                                   @"первый\s+раз|нет\s+акк?аунта",

                        Reply = @"(?i)ответ(ить)?\b",
                        NewTopic = @"(?i)(создать|открыть|нов(ая|ую))\s+тем[ау]",
                        Quote = @"(?i)цитат(а|ировать)",
                        Poll = @"(?i)опрос\b",
                        LoggedIn = @"(?i)" +
                                   @"ваш\s+последний\s+визит|" +
                                   @"(редактировать|мо[йи]|ваши?)(\s+мой)?\s+(кабинет|профиль|закладки)|" +
                                   @"(личные)\s+сообщения|выход\b" //(новые|личные)\s+сообщения bad 
                    },
                    #endregion

                    #region Message
                    Message = new LocalText.MessageText
                    {
                        PostSuccess = @"(?i)спасибо\s+за\s+соо?бщение",
                        LoginSuccess = @"(?i)" +
                                       @"вы\s+((за|во)шли|авторизировались)\s+как(?![\s\W]+(гость|guest))|" +
                                       @"благодарим\s+за\s+визит|" +
                                       @"спасибо,?\s+что\s+зашли",

                        Error = @"(?i)" +
                                String.Format(@"{0}\s{1}|{1}\s{0}|",
                                                @"((обнаруж|допущ)ен[ыа]|(возникл|произошл)[иа])",
                                                @"(следующ(ие|ая)\s+)?ошибк[иа]") +
                                @"((неверн|неправильн|введите\s+правильн)(ое?|ый|ые))\s+(введен\s+)?(имя|логин|пароль|код|данные)|" +
                                @"(нет|не\s+имеете)\s+доступа?|доступе?\s+(закрыт|отказано)|попробуйте\s+ещё\s+раз|повторите\s+попытку|" +
                                @"логин\s+или\s+пароль\s+неверны|" +
                                @"вы\s+не\s+авторизованы|недостаточно\s+прав|" +
                                @"вы\s+были\s+заблокированы" +
                                @"дата\s+снятия\s+блокировки|" +
                                @"(пожалуйста[\s\W]+)?(войдите\s+или\s+зарегистрируйтесь|попробуйте\s+чуть\s+позже)|" +
                                @"(такого\s+)?пользовател[яь]\s+([\w\W]*\s+)?не\s+(существует|найден)|" +
                                @"соо?бщение\s+(было\s+)?(слишком\s+короткое|оставлено\s+пустым)" //к\s+сож[ае]лению| bad
                    },
                    #endregion

                    #region Fields
                    Fields = new LocalText.FieldsNames
                    {
                        Subject = @"(?i)предмет|тема|заголовок",
                        Message = @"(?i)сообщение|текст",
                        Captcha = @"(?i)код"
                    }
                    #endregion
                };
                #endregion

                // Keep this in the bottom! Initialization order matters. 
                public static LocalText[] Localities = { English, Russian };
                public static LocalText Global = LocalText.Join(Localities);
            }

            /// <summary>
            /// Safe version of Expr.IsMatch(). (Null argument accepted)
            /// </summary>
            public static bool IsMatch(string input, string pattern)
            {
                return Regex.IsMatch(input ?? String.Empty, pattern);
            }

            /// <summary>
            /// Safe version of Expr.Matches(). (Null argument accepted)
            /// </summary>
            public static MatchCollection Matches(string input, string pattern)
            {
                return Regex.Matches(input ?? String.Empty, pattern);
            }
        }

        protected abstract class WebForm
        {
            public HtmlNode FormNode;
            public string Enctype;
            public string Method;
            public string Action;
            public string AcceptCharset;
            public string BaseUrl;
            public string CaptchaFieldName;
            public string CaptchaPictureUrl;
            public bool IsHidden;
            public Dictionary<string, string> OtherFields = new Dictionary<string, string>();

            public abstract int Score();

            public Encoding ChooseEncoding(Encoding encoding)
            {
                if (String.IsNullOrEmpty(AcceptCharset)) return encoding;
                try
                {
                    return Encoding.GetEncoding(AcceptCharset);
                }
                catch { return encoding; }
            }

            protected static void SelectBest(ref int bestScore, ref string bestText, string thisText, string matchPattern)
            {
                int Score = Expr.Matches(thisText, matchPattern).Count;
                if (Score > 0 && Score > bestScore)
                {
                    bestScore = Score;
                    bestText = thisText;
                }
            }

            protected static string GetBase(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                string BaseUrl = defaultMainPage.AbsoluteUri;
                foreach (HtmlNode Base in doc.DocumentNode.Descendants("base"))
                {
                    string Href = Base.GetAttributeValueDecoded("href");
                    if (!String.IsNullOrEmpty(Href))
                    {
                        BaseUrl = Href;
                    }
                }
                return BaseUrl;
            }

            protected abstract void FormProcessor();
            protected static WebForm Find(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage, Func<WebForm> formConstructor)
            {
                IEnumerable<HtmlNode> Forms = doc.DocumentNode.Descendants("form");
                WebForm BestForm = null;
                int BestFormScore = int.MinValue;
                foreach (HtmlNode HtmlForm in Forms)
                {
                    HtmlNode TempForm = HtmlForm.Clone();

                    // Remove children forms
                    IEnumerable<HtmlNode> SubForms = TempForm.Descendants("form");
                    foreach (HtmlNode SubForm in SubForms)
                        SubForm.Remove();
                    WebForm Form = formConstructor();
                    Form.FormNode = TempForm;
                    Form.BaseUrl = GetBase(doc, defaultMainPage);
                    Uri ActionUri;
                    if (Uri.TryCreate(TempForm.GetAttributeValueDecoded("action", ""), UriKind.RelativeOrAbsolute, out ActionUri) &&
                    (!ActionUri.IsAbsoluteUri || ((ActionUri.Scheme == Uri.UriSchemeHttp || ActionUri.Scheme == Uri.UriSchemeHttps)
                     && ActionUri.Host.EndsWith(new Uri(Form.BaseUrl).Host, StringComparison.OrdinalIgnoreCase))))
                    {
                        if (!ActionUri.IsAbsoluteUri) ActionUri = new Uri(new Uri(Form.BaseUrl), ActionUri);
                        Form.Action = ActionUri.AbsoluteUri;
                    }
                    Form.Method = TempForm.GetAttributeValueDecoded("method", "").ToLower();
                    Form.Enctype = TempForm.GetAttributeValueDecoded("enctype", "").ToLower();
                    Form.AcceptCharset = TempForm.GetAttributeValueDecoded("accept-charset", "").ToLower();

                    int BestCaptchaScore = int.MinValue;
                    foreach (HtmlNode Input in Form.FormNode.Descendants("input"))
                    {
                        if (Input.GetAttributeValueDecoded("type") == "text")
                        {
                            SelectBest(ref BestCaptchaScore, ref Form.CaptchaFieldName,
                                Input.GetAttributeValueDecoded("name", String.Empty), Expr.Text.Global.Fields.Captcha);
                        }
                    }

                    // Search for captcha image
                    if (!String.IsNullOrEmpty(Form.CaptchaFieldName))
                    {
                        int BestCaptchaImgScore = int.MinValue;
                        foreach (HtmlNode Image in Form.FormNode.Descendants("img"))
                        {
                            Uri Src;
                            if (Uri.TryCreate(Image.GetAttributeValueDecoded("src", String.Empty), UriKind.RelativeOrAbsolute, out Src) &&
                                (!Src.IsAbsoluteUri || (Src.Scheme == Uri.UriSchemeHttp || Src.Scheme == Uri.UriSchemeHttps)))
                            {
                                if (!Src.IsAbsoluteUri) Src = new Uri(new Uri(Form.BaseUrl), Src);

                                int CaptchaImgScore = MatchLinkNode(Src.AbsoluteUri, Image, Expr.Url.CaptchaImage, Expr.Text.Global.Link.CaptchaImage);
                                if (CaptchaImgScore > BestCaptchaImgScore)
                                {
                                    Form.CaptchaPictureUrl = Src.AbsoluteUri;
                                    BestCaptchaImgScore = CaptchaImgScore;
                                }
                            }
                        }
                    }

                    Form.FormProcessor();
                    Form.IsHidden = HtmlForm.IsNoDisplay();
                    int Score = Form.Score();

                    if (Score > BestFormScore && Score > 0)
                    {
                        BestForm = Form;
                        BestFormScore = Score;
                    }
                }
#if DEBUG && DEBUGANYFORUM
                if (BestForm != null)
                    Console.WriteLine("FormScore: " + BestFormScore + " ActionUrl: " + BestForm.Action);
#endif
                return BestForm;
            }

            public static int ErrorNodes(HtmlAgilityPack.HtmlDocument doc)
            {
                IEnumerable<HtmlNode> Nodes =
                    doc.DocumentNode.Descendants().Where(
                    x => x.Attributes.Any(
                        y => Expr.IsMatch(y.ValueDecoded(), @"(?i)error|warn"))
                        && !Expr.IsMatch(x.InnerText, @"(?i)отключен\s+javascript"));
                return Nodes.Count();
            }

            public static int LoggedInNodes(HtmlAgilityPack.HtmlDocument doc)
            {
                int LoggedIn = 0;
                IEnumerable<HtmlNode> Links = doc.DocumentNode.Descendants().Where(
                    x => !String.IsNullOrEmpty(x.GetAttributeValueDecoded("href")));
                foreach (HtmlNode Node in Links)
                {
                    if (MatchLinkNode(String.Empty, Node, Expr.Url.LoggedIn, Expr.Text.Global.Link.LoggedIn) > 0)
                        LoggedIn++;
                }
                return LoggedIn;
            }

            protected static int MatchLinkNode(string linkUrl, HtmlNode linkNode,
                                                 string regExprLinkPattern, string regExprTextPattern)
            {
                int Score = Expr.Matches(linkUrl, regExprLinkPattern).Count
                          + Expr.Matches(linkNode.InnerTextDecoded(), regExprTextPattern).Count;
                foreach (HtmlNode Node in linkNode.DescendantsAndSelf())
                    Score += Node.Attributes.Sum(x => Expr.Matches(x.ValueDecoded(),regExprLinkPattern).Count)
                           + Node.Attributes.Sum(x => Expr.Matches(x.ValueDecoded(),regExprTextPattern).Count);
                return Score;
            }

            protected static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage,
                                                              Func<string, HtmlNode, int> scoreFunction)
            {
                Uri BaseUrl = new Uri(GetBase(doc, defaultMainPage));
                IEnumerable<HtmlNode> AllLinks = doc.DocumentNode.Descendants().Where(
                    x => !String.IsNullOrEmpty(x.GetAttributeValueDecoded("href")));
                var LoginLinks = new Dictionary<string, int>();
                foreach (HtmlNode Link in AllLinks)
                {
                    Uri Url;
                    if (Uri.TryCreate(Link.GetAttributeValueDecoded("href", ""), UriKind.RelativeOrAbsolute, out Url) &&
                        (!Url.IsAbsoluteUri || ((Url.Scheme == Uri.UriSchemeHttp || Url.Scheme == Uri.UriSchemeHttps)
                         && Url.Host.EndsWith(BaseUrl.Host, StringComparison.OrdinalIgnoreCase))))
                    {
                        int Score = scoreFunction(Url.OriginalString, Link);
                        if (!Url.IsAbsoluteUri) Url = new Uri(BaseUrl, Url);
                        if (Score > 0 && !LoginLinks.ContainsKey(Url.AbsoluteUri))
                            LoginLinks.Add(Url.AbsoluteUri, Score);
                    }
                }
                return LoginLinks.ToList().OrderByDescending(x => x.Value).ThenBy(x => x.Key.Length).Take(15).ToList();
            }
        }

        protected class LoginForm : WebForm
        {

            public string UsernameFieldName;
            public List<string> PasswordFields = new List<string>();

            protected override void FormProcessor()
            {
                int BestUsernameScore = int.MinValue;
                foreach (HtmlNode Input in FormNode.Descendants("input"))
                {
                    if (!String.IsNullOrEmpty(Input.GetAttributeValueDecoded("name")) &&
                        !String.IsNullOrEmpty(Input.GetAttributeValueDecoded("type")))
                    {
                        if (Input.GetAttributeValueDecoded("type") == "text")
                            SelectBest(ref BestUsernameScore, ref UsernameFieldName,
                                  Input.GetAttributeValueDecoded("name", String.Empty), Expr.Text.Global.Fields.Username);
                        else if (Input.GetAttributeValueDecoded("type") == "password")
                            PasswordFields.Add(Input.GetAttributeValueDecoded("name", String.Empty));

                        else if (Input.GetAttributeValueDecoded("type", String.Empty) != "submit" &&
                            (Input.GetAttributeValueDecoded("type") != "radio" ||
                            Input.GetAttributeValueDecoded("checked") == "checked") &&
                            !OtherFields.ContainsKey(Input.GetAttributeValueDecoded("name", String.Empty)))
                            OtherFields.Add(Input.GetAttributeValueDecoded("name", String.Empty),
                               Input.GetAttributeValueDecoded("value", String.Empty));
                    }
                }

                //// Easy skip captcha on stupid sites
                //if (Expr.EasySkipCaptcha.Contains(new Uri(BaseUrl).Host))
                //{
                //    if (OtherFields.ContainsKey("recaptcha_response_field"))
                //    OtherFields.Remove("recaptcha_response_field");
                //}

            }

            public static LoginForm Find(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                return Find(doc, defaultMainPage, () => new LoginForm()) as LoginForm;
            }

            public static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                return WebForm.FindLinks(doc, defaultMainPage, (linkUrl, linkNode) =>
                       Expr.Matches(linkUrl, Expr.Url.Action).Count// <= 0 ? 0 : linkUrl.MatchCount(Expr.Url.Action)
                     + MatchLinkNode(linkUrl, linkNode, Expr.Url.Login, Expr.Text.Global.Link.Login)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Register, Expr.Text.Global.Link.Register)
                    );
            }

            public HttpContent PostData(TaskBaseProperties.AccountData account, string captcha, Encoding encoding)
            {
                var PostString = new StringBuilder();
                PostString.Append("&" + UsernameFieldName + "=");
                PostString.Append(HttpUtility.UrlEncode(account.Username.ToLower(), encoding));
                string Password = account.Password;
                for (int i = 0; i < PasswordFields.Count; i++)
                {
                    PostString.Append("&" + PasswordFields[i] + "=");
                    PostString.Append(HttpUtility.UrlEncode(Password, encoding));
                }
                
                if (!String.IsNullOrEmpty(CaptchaFieldName))
                    PostString.Append("&" + CaptchaFieldName + "=");
                
                PostString.Append(HttpUtility.UrlEncode(captcha, encoding));
                List<KeyValuePair<string, string>> OtherFieldsList = OtherFields.ToList();
                for (int i = 0; i < OtherFieldsList.Count; i++)
                {
                    PostString.Append("&" + OtherFieldsList[i].Key + "=");
                    PostString.Append(HttpUtility.UrlEncode(OtherFieldsList[i].Value, encoding));
                }

                PostString.Remove(0, 1);
                return new StringContent(PostString.ToString(), encoding,
                            "application/x-www-form-urlencoded");
            }

            public override int Score()
            {
                int UsernameScore = Expr.Matches(UsernameFieldName, Expr.Text.Global.Fields.Username).Count;
                int PasswordScore = PasswordFields.Sum((x) => Expr.Matches(x, Expr.Text.Global.Fields.Password).Count);

                if (UsernameScore <= 0 || PasswordScore <= 0 || String.IsNullOrEmpty(Action) ||
                    PasswordFields.Any((x) => Expr.IsMatch(x, Expr.Text.Global.Fields.RepeatPassword))
                    || Method != "post") return 0;
                else return UsernameScore + PasswordScore
                    + Expr.Matches(Action, Expr.Url.Action).Count
                    + Expr.Matches(Action, Expr.Url.Login).Count
                    -Expr.Matches( Action, Expr.Url.Register).Count
                    + MatchLinkNode(String.Empty, FormNode, Expr.Url.Login, Expr.Text.Global.Link.Login);
            }
        }

        protected class PostForm : WebForm
        {
            public string SubjectFieldName;
            public string MessageFieldName;

            protected override void FormProcessor()
            {
                int BestSubjectScore = int.MinValue;
                int BestMessageScore = int.MinValue;

                foreach (HtmlNode Input in FormNode.Descendants("input"))
                {
                    if (!String.IsNullOrEmpty(Input.GetAttributeValueDecoded("name")) &&
                        !String.IsNullOrEmpty(Input.GetAttributeValueDecoded("type")))
                    {
                        if (Input.GetAttributeValueDecoded("type") == "text")
                        {
                            SelectBest(ref BestSubjectScore, ref SubjectFieldName,
                                Input.GetAttributeValueDecoded("name", String.Empty), Expr.Text.Global.Fields.Subject);
                        }
                        else if (Input.GetAttributeValueDecoded("type", String.Empty) != "submit" &&
                            (Input.GetAttributeValueDecoded("type") != "radio" ||
                            Input.GetAttributeValueDecoded("checked") == "checked") &&
                            !OtherFields.ContainsKey(Input.GetAttributeValueDecoded("name", String.Empty)))
                            OtherFields.Add(Input.GetAttributeValueDecoded("name", String.Empty),
                               Input.GetAttributeValueDecoded("value", String.Empty));
                    }
                }

                foreach (HtmlNode TextArea in FormNode.Descendants("textarea"))
                {
                    SelectBest(ref BestMessageScore, ref MessageFieldName,
                        TextArea.GetAttributeValueDecoded("name", String.Empty), Expr.Text.Global.Fields.Message);
                }

            }

            public static PostForm Find(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                return Find(doc, defaultMainPage, () => new PostForm()) as PostForm;
            }

            public static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                List<KeyValuePair<string, int>> ReplyLinks = WebForm.FindLinks(doc, defaultMainPage, (linkUrl, linkNode) =>
                      Expr.Matches(linkUrl, (Expr.Url.Action)).Count// <= 0 ? 0 : linkUrl.MatchCount(Expr.Url.Action)
                     + MatchLinkNode(linkUrl, linkNode, Expr.Url.Reply, Expr.Text.Global.Link.Reply)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Quote, Expr.Text.Global.Link.Quote)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Poll, Expr.Text.Global.Link.Poll)
                     ).OrderBy(x => x.Value).ThenByDescending(x => x.Key.Length).ToList();
                List<KeyValuePair<string, int>> NewTopicLinks = WebForm.FindLinks(doc, defaultMainPage, (linkUrl, linkNode) =>
                       Expr.Matches(linkUrl, Expr.Url.Action).Count// <= 0 ? 0 : linkUrl.MatchCount(Expr.Url.Action)
                     + MatchLinkNode(linkUrl, linkNode, Expr.Url.NewTopic, Expr.Text.Global.Link.NewTopic)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Quote, Expr.Text.Global.Link.Quote)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Poll, Expr.Text.Global.Link.Poll)
                     ).OrderBy(x => x.Value).ThenByDescending(x => x.Key.Length).ToList();
                List<KeyValuePair<string, int>> Result = new List<KeyValuePair<string, int>>();
                Action<List<KeyValuePair<string, int>>> Peek = (x) =>
                {
                    Result.Add(x.Last());
                    x.RemoveAt(x.Count - 1);
                };
                while (ReplyLinks.Count > 0 || NewTopicLinks.Count > 0)
                {
                    if (ReplyLinks.Count <= 0) Peek(NewTopicLinks);
                    else if (NewTopicLinks.Count <= 0) Peek(ReplyLinks);
                    else if (NewTopicLinks.Last().Value > ReplyLinks.Last().Value) Peek(NewTopicLinks);
                    else Peek(ReplyLinks);
                }
                return Result.Distinct().Take(10).ToList();
            }

            public HttpContent PostData(TaskBaseProperties.AccountData account, Encoding encoding,
                                            string subject, string message, string captcha)
            {
                List<KeyValuePair<string, string>> OtherFieldsList = OtherFields.ToList();
                for (int i = OtherFieldsList.Count - 1; i >= 0; i--)
                    if (Expr.IsMatch(OtherFieldsList[i].Key, Expr.Text.Global.Fields.Preview))
                        OtherFieldsList.RemoveAt(i);
                if (!String.IsNullOrEmpty(Enctype) && Enctype == "multipart/form-data")
                {
                    var FormData = new MultipartFormDataContent();

                    if (!String.IsNullOrEmpty(SubjectFieldName))
                        FormData.Add(new StringContent(subject, encoding), SubjectFieldName);

                    FormData.Add(new StringContent(message, encoding), MessageFieldName);

                    if (!String.IsNullOrEmpty(CaptchaFieldName))
                        FormData.Add(new StringContent(captcha, encoding), CaptchaFieldName);

                    for (int i = 0; i < OtherFieldsList.Count; i++)
                        FormData.Add(new StringContent(OtherFieldsList[i].Value, encoding), OtherFieldsList[i].Key);
                    return FormData;
                }
                else
                {
                    var PostString = new StringBuilder();

                    if (!String.IsNullOrEmpty(SubjectFieldName))
                        PostString.Append("&" + SubjectFieldName + "=");
                    PostString.Append(HttpUtility.UrlEncode(subject, encoding));

                    PostString.Append("&" + MessageFieldName + "=");
                    PostString.Append(HttpUtility.UrlEncode(message, encoding));

                    if (!String.IsNullOrEmpty(CaptchaFieldName))
                        PostString.Append("&" + CaptchaFieldName + "=");
                    PostString.Append(HttpUtility.UrlEncode(captcha, encoding));

                    for (int i = 0; i < OtherFieldsList.Count; i++)
                    {
                        PostString.Append("&" + OtherFieldsList[i].Key + "=");
                        PostString.Append(HttpUtility.UrlEncode(OtherFieldsList[i].Value, encoding));
                    }
                    PostString.Remove(0, 1);
                    return new StringContent(PostString.ToString(), encoding,
                                "application/x-www-form-urlencoded");
                }
            }

            public override int Score()
            {
                int MessageScore = Expr.Matches(MessageFieldName, Expr.Text.Global.Fields.Message).Count;

                if (MessageScore <= 0 || Method != "post" || String.IsNullOrEmpty(Action)) return 0;
                else return MessageScore +
                    + Expr.Matches(Action, Expr.Url.Action).Count
                    + Expr.Matches(Action, Expr.Url.NewTopic).Count + Expr.Matches(Action, Expr.Url.Reply).Count
                    + Expr.Matches(SubjectFieldName, Expr.Text.Global.Fields.Subject).Count
                    + Expr.Matches(CaptchaFieldName, Expr.Text.Global.Fields.Captcha).Count
                    + MatchLinkNode(String.Empty, FormNode, Expr.Url.NewTopic, Expr.Text.Global.Link.NewTopic)
                    + MatchLinkNode(String.Empty, FormNode, Expr.Url.Reply, Expr.Text.Global.Link.Reply);
            }
        }

        public override async Task<Exception> Login()
        {
            // Check for bad site
            if (Expr.NotSupported.Contains(new Uri(Properties.ForumMainPage).Host))
                return new Exception("Сайт не поддерживается");

            // Some site workaround
            if (new Uri(Properties.ForumMainPage).Host == "gidtalk.ru")
                Cookies.Add(new Uri("http://gidtalk.ru/"), new Cookie("beget", "begetok"));

            // Searching LoginForm
            StatusMessage = "Авторизация: Загрузка страницы";
            var Response = await GetAndLog(Properties.ForumMainPage);
            progress.Login = 40;

            // If Windows authetification
            if (Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Client.Dispose();
                var handler = new HttpClientHandler();
                handler.Credentials = new NetworkCredential(AccountToUse.Username, AccountToUse.Password);
                Client = new HttpClient(handler);
                StatusMessage = "Авторизация: Запрос авторизации";
                Response = await GetAndLog(Properties.ForumMainPage);
                progress.Login = 205;
                if (Response.IsSuccessStatusCode)
                {
                    StatusMessage = "Авторизация: Успешно";
                    return null;
                }
                else return new Exception("Авторизация не удалась");
            }

            StatusMessage = "Авторизация: Поиск формы авторизации";
            var Html = await InitHtml(Response);
            string LoginUrl = Properties.ForumMainPage;
            LoginForm LoginForm = LoginForm.Find(Html, new Uri(LoginUrl));
            progress.Login = 50;

            // Check other pages for LoginForm
            if (LoginForm == null)
            {
                StatusMessage = "Авторизация: Поиск страницы авторизации";
                List<KeyValuePair<string, int>> LoginLinks = LoginForm.FindLinks(Html, new Uri(LoginUrl));
                progress.Login = 55;
                int i = 0;
                while (LoginForm == null && i < LoginLinks.Count)
                {
                    LoginUrl = LoginLinks[i].Key;
                    StatusMessage = "Авторизация: Загрузка страницы";
                    Response = await GetAndLog(LoginUrl);
                    progress.Login += 50 / LoginLinks.Count;
                    StatusMessage = "Авторизация: Поиск формы авторизации";
                    Html = await InitHtml(Response);
                    LoginForm = LoginForm.Find(Html, new Uri(LoginUrl));
                    progress.Login += 10 / LoginLinks.Count;
                    i++;
                }
            }
            if (LoginForm == null) return new Exception("Форма авторизации не найдена");
            progress.Login = 155;

#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("LoginLink: {0}", LoginUrl);
#endif

            // LoginPage preanalyse for future success detection
            Html.ClearHidden();
            string Text = Html.DocumentNode.InnerTextDecoded();
            bool ErrorNodesValid = WebForm.ErrorNodes(Html) == 0;
            bool LoggedInNodesValid = WebForm.LoggedInNodes(Html) == 0;

#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("Before: Success: {0} Error: {1} ErrorNodes: {2} LoggeInNodes: {3}",
            Expr.Matches(Text, Expr.Text.Global.Message.LoginSuccess).Count,
            Expr.Matches(Text, Expr.Text.Global.Message.Error).Count,
            WebForm.ErrorNodes(Html),
            WebForm.LoggedInNodes(Html));
            //return null;
#endif
            // Ask user for captcha if one was found
            string Captcha = String.Empty;
            if (!String.IsNullOrEmpty(LoginForm.CaptchaFieldName) &&
                !String.IsNullOrEmpty(LoginForm.CaptchaPictureUrl))
            {
                WaitingForQueue = true;
                StatusMessage = "Авторизация: В очереди";
                await WaitFor(CaptchaForm.IsFree);
                progress.Login += 10;
                StatusMessage = "Авторизация: Загрузка капчи";
                Response = await Client.GetAsync(LoginForm.CaptchaPictureUrl, Cancel.Token);
                progress.Login += 15;
                CaptchaForm.Picture.Image = new Bitmap(await Response.Content.ReadAsStreamAsync());
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                StatusMessage = "Авторизация: Ожидание ввода капчи";
                Application.OpenForms[0].Invoke((Action)(() => CaptchaForm.ShowDialog()));
                Captcha = CaptchaForm.Result.Text;
                CaptchaForm.IsFree.Set();
                progress.Login += 15;
            }
            else progress.Login += 40;


            // Send authorization request
            StatusMessage = "Авторизация: Запрос авторизации";

            Response = await PostAndLog(LoginForm.Action,
                LoginForm.PostData(AccountToUse, Captcha, LoginForm.ChooseEncoding(Html.Encoding)));
            progress.Login = 195;
            Html = await InitHtml(Response);
            Html.ClearHidden();
            Text = Html.DocumentNode.InnerTextDecoded();
            progress.Login = 205;
            // Analyze response
            int SuccessScore = Expr.Matches(Text, Expr.Text.Global.Message.LoginSuccess).Count
                             - Expr.Matches(Text, Expr.Text.Global.Message.Error).Count;
            if (ErrorNodesValid)
                SuccessScore -= WebForm.ErrorNodes(Html);

#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("Answer: Success: {0} Error: {1} ErrorNodes: {2} LoggeInNodes: {3}",
            Expr.Matches(Text, Expr.Text.Global.Message.LoginSuccess).Count,
            Expr.Matches(Text, Expr.Text.Global.Message.Error).Count,
            WebForm.ErrorNodes(Html),
            WebForm.LoggedInNodes(Html));
            //return null;
#endif

            // Load LoginPage again and analyze it now (after we've tryed to login)
            StatusMessage = "Авторизация: Загрузка страницы";
            Response = await GetAndLog(LoginUrl);
            progress.Login = 245;
            StatusMessage = "Авторизация: Проверка авторизации";
            Html = await InitHtml(Response);
            LoginForm AfterLoginForm = LoginForm.Find(Html, new Uri(LoginUrl));


            // Summarize analyzies and return conclusion if login was successfull
            Html.ClearHidden();
            Text = Html.DocumentNode.InnerTextDecoded();
            int LoggedInScore = 0;
            if (LoggedInNodesValid)
            {
                LoggedInScore = WebForm.LoggedInNodes(Html);
                SuccessScore += LoggedInScore;
            }
            if (AfterLoginForm == null || (AfterLoginForm.IsHidden && !LoginForm.IsHidden)) SuccessScore += 3;
            else if (LoggedInScore == 0 && AfterLoginForm != null && !AfterLoginForm.IsHidden) SuccessScore -= 2;
     
            progress.Login = 255;
#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("After: Success: {0} Error: {1} ErrorNodes: {2} LoggeInNodes: {3}",
            Expr.Matches(Text, Expr.Text.Global.Message.LoginSuccess).Count,
            Expr.Matches(Text, Expr.Text.Global.Message.Error).Count,
            WebForm.ErrorNodes(Html),
            WebForm.LoggedInNodes(Html));
            //return null;
#endif
#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("AuthSuccessScore: " + SuccessScore + '\n');
#endif
            if (SuccessScore < 0) return new Exception("Авторизация не удалась");
            StatusMessage = "Авторизация: Успешно";
            return null;
        }

        public override async Task<Exception> PostMessage(Uri targetBoard, string subject, string message)
        {
            // return null;
            // Delay needed on some sites     
            if (targetBoard.Host == "seodor.biz"
                || targetBoard.Host == "webledi.ru")
            {
                WaitingForQueue = true;
                StatusMessage = "Задержка 5 сек";
                Task.Delay(5000).Wait(Cancel.Token);
                WaitingForQueue = false;
            }

            // Searching PostForm
            StatusMessage = "Постинг: Загрузка страницы";
            var Response = await GetAndLog(targetBoard.AbsoluteUri);
            progress.Post += 40 / progress.PostCount;
            StatusMessage = "Постинг: Поиск формы постинга";
            var Html = await InitHtml(Response);
            string PostUrl = targetBoard.AbsoluteUri;
            PostForm PostForm = PostForm.Find(Html, new Uri(PostUrl));
            progress.Post = +10 / progress.PostCount;

            // Check other pages for PostForm
            int TempProgress = progress.Post;
            if (PostForm == null)
            {
                StatusMessage = "Постинг: Поиск страницы постинга";
                List<KeyValuePair<string, int>> PostLinks = PostForm.FindLinks(Html, new Uri(PostUrl));
#if DEBUG && DEBUGANYFORUM
                Console.WriteLine("PostLinks: {0}", PostUrl);
                //       foreach (var Link in PostLinks) Console.WriteLine(Link.Key.ToString());
#endif
                progress.Post += 5 / progress.PostCount;
                int i = 0;
                while (PostForm == null && i < PostLinks.Count)
                {
                    PostUrl = PostLinks[i].Key;
                    StatusMessage = "Постинг: Загрузка страницы";
                    Response = await GetAndLog(PostUrl);
                    progress.Post += (80 / progress.PostCount) / PostLinks.Count;
                    StatusMessage = "Постинг: Поиск формы постинга";
                    Html = await InitHtml(Response);
                    PostForm = PostForm.Find(Html, new Uri(PostUrl));
                    progress.Post += (20 / progress.PostCount) / PostLinks.Count;
                    i++;
                }
            }
            if (PostForm == null) return new Exception("Форма постинга не найдена");
            progress.Post = TempProgress + (100 / progress.PostCount);

            // PostPage preanalyse for future success detection
            Html.ClearHidden();
            string Text = Html.DocumentNode.InnerTextDecoded();
            int SuccessScore = 0;
            bool ErrorNodesValid = WebForm.ErrorNodes(Html) == 0;
            bool ErrorValid = !Expr.IsMatch(Text, Expr.Text.Global.Message.Error);

#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("After: Success: {0} Error: {1} ErrorNodes: {2}",
            Expr.Matches(Text, Expr.Text.Global.Message.PostSuccess).Count,
            Expr.Matches(Text, Expr.Text.Global.Message.Error).Count,
            WebForm.ErrorNodes(Html));
            //     return null;
#endif

            // Ask user for captcha if one was found
            string Captcha = String.Empty;
            if (!String.IsNullOrEmpty(PostForm.CaptchaFieldName) &&
                !String.IsNullOrEmpty(PostForm.CaptchaPictureUrl))
            {
                WaitingForQueue = true;
                StatusMessage = "Постинг: В очереди";
                await WaitFor(CaptchaForm.IsFree);
                progress.Post += 10 / progress.PostCount;
                StatusMessage = "Постинг: Загрузка капчи";
                Response = await Client.GetAsync(PostForm.CaptchaPictureUrl, Cancel.Token);
                progress.Post += 20 / progress.PostCount;
                CaptchaForm.Picture.Image = new Bitmap(await Response.Content.ReadAsStreamAsync());
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                StatusMessage = "Постинг: Ожидание ввода капчи";
                Application.OpenForms[0].Invoke((Action)(() => CaptchaForm.ShowDialog()));
                Captcha = CaptchaForm.Result.Text;
                CaptchaForm.IsFree.Set();
                progress.Post += 20 / progress.PostCount;
            }
            else progress.Post += 50 / progress.PostCount;

            // Send post message request
            StatusMessage = "Постинг: Отправка сообщения";
            Response = await PostAndLog(PostForm.Action,
                PostForm.PostData(AccountToUse, PostForm.ChooseEncoding(Html.Encoding), subject, message, Captcha));
            progress.Post += 40 / progress.PostCount;
            Html = await InitHtml(Response);
            PostForm AfterPostForm = PostForm.Find(Html, new Uri(PostUrl));
            Html.ClearHidden();
            Text = Html.DocumentNode.InnerTextDecoded();

            // Analyze response and return conslusion if message posted successfully

            //if (AfterPostForm != null && !AfterPostForm.IsHidden) SuccessScore -= 5; bad idea

            SuccessScore += Expr.Matches(Text, Expr.Text.Global.Message.PostSuccess).Count;
            if (ErrorValid)
                SuccessScore -= Expr.Matches(Text, Expr.Text.Global.Message.Error).Count;
            if (ErrorNodesValid)
                SuccessScore -= WebForm.ErrorNodes(Html);

            progress.Post += 10 / progress.PostCount;
#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("After: Success: {0} Error: {1} ErrorNodes: {2}",
            Expr.Matches(Text, Expr.Text.Global.Message.PostSuccess).Count,
            Expr.Matches(Text, Expr.Text.Global.Message.Error).Count,
            WebForm.ErrorNodes(Html));
            //return null;
#endif
#if DEBUG && DEBUGANYFORUM
            Console.WriteLine("PostSuccessScore: " + SuccessScore);
#endif
            if (SuccessScore < 0) return new Exception("Постинг не удался");
            StatusMessage = "Успешно Опубликовано";
            return null;
        }
    }
}