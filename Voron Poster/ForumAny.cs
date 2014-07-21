using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;
using System.Net.Http;
using System.Web;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Voron_Poster
{
    public class ForumAny : Forum
    {
        public ForumAny() : base() { }

        protected static HtmlAgilityPack.HtmlDocument InitHtml(Stream html)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            // Otherwise forms have no children. Why it doesn't work out of the box =_= 
            HtmlNode.ElementsFlags.Remove("form");
            doc.Load(html);
            return doc.ClearScriptsStylesComments();
        }

        protected static class Expr
        {
            public static class Url
            {
                public static string Action = @"[&\?]?(act(ion)?|do|mode)=";
                public static string Login = @"(?<!b)(sign|log)(\W|_)?in|auth\w*"; //|user|member|acc?ount
                public static string Register = @"register|sign(\W|_)?up|(create|new)(\W|_)?(user|member|acc?ount)?";
                public static string Reply = @"reply|(?<!new(\W|_)?)post|no(\W|_)?quot(e|ing)";
                public static string NewTopic = @"new(\W|_)?(topic|thread|post)";
                public static string CaptchaImage = @"verif(y|ication)|code|capt?cha|anti(\W|_)?bot";
                public static string Quote = @"(?<!no(\W|_)?)quot(e|ing)";
                public static string Poll = @"poll";
                public static string LoggedIn = @"(sign|log)(\W|_)?out|" +
                                                @"edit(\W|_)?profile|" +
                                                @"profile(\W|_)?(settings|options|edit)";
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
                                Poll = JoinExpression(a.Poll, b.Poll)
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
                        Login = @"(?<!b)(sign|log)\s?in|auth\w*",

                        Register = @"register|sign\s?up|(create|new)\s(user|member|acc?ount)",

                        Reply = @"reply",
                        NewTopic = @"(create|open|new)\s(topic|thread)",
                        Quote = @"(?<!no\s?)quot(e|ing)",
                        Poll = @"poll"
                    },
                    #endregion

                    #region Message
                    Message = new LocalText.MessageText
                    {
                        Error = @"(login|user(name)?|password)\s+incorrect|" +
                                @"errors?\s+occurred"
                    },
                    #endregion

                    #region Fields
                    Fields = new LocalText.FieldsNames
                    {
                        Subject = @"subject|title",
                        Message = @"message|editor|text",
                        Captcha = @"capt?cha|code",
                        Preview = @"preview"
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
                        Login = @"вход|войти|авториз(ация|ироваться)|запомнить|логин",

                        Register = @"(за)?регистр(ир(овать|уйтесь)|ация)|" +
                                         @"(нов(ый|ого)|создать)\s+(пользовател(ь|я)|акк?аунт)|" +
                                         @"первый\s+раз|нет\s+акк?аунта",

                        Reply = @"ответ(ить)",
                        NewTopic = @"(создать|открыть|нов(ая|ую))\s+тем[ау]",
                        Quote = @"цитат(а|ировать)",
                        Poll = @"опрос"
                    },
                    #endregion

                    #region Message
                    Message = new LocalText.MessageText
                    {
                        LoginSuccess = @"вы\s+((за|во)шли|авторизировались)\s+как(?!(\s|\W)+(гость|guest))",

                        Error = @"((обнаружен[ыа]|(возникл|произошл)[иа])\s+(cледующ(ие|ая))?\s+ошибк[иа])|" +
                                @"(неверн(ое|ый)\s+(имя\w+пользователя|логин|пароль))|" +
                                @"((вве(сти|дите)|неправильный)\s+код)|" +
                                @"нет\s+доступа|доступе?\s+(закрыт|отказано)|" +
                                @"к\s+сож[ае]лению"
                    },
                    #endregion

                    #region Fields
                    Fields = new LocalText.FieldsNames
                    {
                        Subject = @"предмет|тема|заголовок",
                        Message = @"сообщение|текст",
                        Captcha = @"код"
                    }
                    #endregion
                };
                #endregion

                // Keep this in the bottom! Initialization order matters. 
                public static LocalText[] Localities = { English, Russian };
                public static LocalText Global = LocalText.Join(Localities);
            }

            public static int ErrorNodes(HtmlAgilityPack.HtmlDocument doc)
            {
                int Error = 0;
                IEnumerable<HtmlNode> Nodes = doc.DocumentNode.Descendants().Where(x => x.HasAttributes);
                foreach (HtmlNode Node in Nodes)
                {
                    for (int i2 = 0; i2 < Node.Attributes.Count; i2++)
                    {
                        if (Node.Attributes[i2].ValueDecoded().MatchCount(@"error|warn") > 0)
                        {
                            Error++;
                            break;
                        }
                    }
                }
                return Error;
            }

            public static int LoggedInNodes(HtmlAgilityPack.HtmlDocument doc)
            {
                int LoggedIn = 0;
                IEnumerable<HtmlNode> Links = doc.DocumentNode.Descendants().Where(
                    x => !String.IsNullOrEmpty(x.GetAttributeValueDecoded("href")));
                foreach (HtmlNode Node in Links)
                {
                    for (int i2 = 0; i2 < Node.Attributes.Count; i2++)
                    {
                        if ((Node.Attributes[i2].Name == "href" || Node.Attributes[i2].Name == "onclick") &&
                            Node.Attributes[i2].ValueDecoded().MatchCount(Expr.Url.LoggedIn) > 0)
                        {
                            LoggedIn++;
                            break;
                        }
                    }
                }
                return LoggedIn;
            }
        }

        protected abstract class WebForm
        {
            public HtmlNode Form;
            public string Enctype;
            public string Method;
            public string Action;
            public string AcceptCharset;
            public Dictionary<string, string> OtherFields = new Dictionary<string, string>();

            public abstract int Match();
            public abstract bool Validate();

            public Encoding ChooseEncoding(Encoding encoding)
            {
                try
                {
                    return Encoding.GetEncoding(AcceptCharset);
                }
                catch { return encoding; }
            }

            protected abstract void FormProcessor(Uri mainPage);
            protected static WebForm Find(HtmlAgilityPack.HtmlDocument doc, Uri mainPage, Func<WebForm> formConstructor)
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
                    Form.Form = TempForm;
                    Uri ActionUri;
                    if (Uri.TryCreate(TempForm.GetAttributeValueDecoded("action", ""), UriKind.RelativeOrAbsolute, out ActionUri) &&
                    (!ActionUri.IsAbsoluteUri || ((ActionUri.Scheme == Uri.UriSchemeHttp || ActionUri.Scheme == Uri.UriSchemeHttps)
                     && ActionUri.Host.EndsWith(mainPage.Host, StringComparison.OrdinalIgnoreCase))))
                    {
                        if (!ActionUri.IsAbsoluteUri) ActionUri = new Uri(mainPage, ActionUri);
                        Form.Action = ActionUri.AbsoluteUri;
                    }
                    Form.Method = TempForm.GetAttributeValueDecoded("method", "");
                    Form.Enctype = TempForm.GetAttributeValueDecoded("enctype", "");
                    Form.AcceptCharset = TempForm.GetAttributeValueDecoded("accept-charset", "");
                    Form.FormProcessor(mainPage);

                    if (Form.Validate())
                    {
                        int Score = Form.Match();
                        if (Score > BestFormScore && Score >= 0)
                        {
                            BestForm = Form;
                            BestFormScore = Score;
                        }
                    }
                }
                return BestForm;
            }

            protected static int MatchLinkNode(string linkUrl, HtmlNode linkNode,
                                                 string regExprLinkPattern, string regExprTextPattern)
            {
                int Score = linkUrl.MatchCount(regExprLinkPattern)
                          + linkNode.InnerTextDecoded().MatchCount(regExprTextPattern);
                foreach (HtmlNode Node in linkNode.DescendantsAndSelf())
                    Score += Node.Attributes.Sum(x => x.ValueDecoded().MatchCount(regExprLinkPattern))
                           + Node.Attributes.Sum(x => x.ValueDecoded().MatchCount(regExprTextPattern));
                return Score;
            }

            protected static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri mainPage,
                                                              Func<string, HtmlNode, int> scoreFunction)
            {
                IEnumerable<HtmlNode> AllLinks = doc.DocumentNode.Descendants().Where(
                    x => !String.IsNullOrEmpty(x.GetAttributeValueDecoded("href")));
                var LoginLinks = new Dictionary<string, int>();
                foreach (HtmlNode Link in AllLinks)
                {
                    Uri Url;
                    if (Uri.TryCreate(Link.GetAttributeValueDecoded("href", ""), UriKind.RelativeOrAbsolute, out Url) &&
                        (!Url.IsAbsoluteUri || ((Url.Scheme == Uri.UriSchemeHttp || Url.Scheme == Uri.UriSchemeHttps)
                         && Url.Host.EndsWith(mainPage.Host, StringComparison.OrdinalIgnoreCase))))
                    {
                        int Score = scoreFunction(Url.OriginalString, Link);
                        if (!Url.IsAbsoluteUri) Url = new Uri(mainPage, Url);
                        if (Score > 0 && !LoginLinks.ContainsKey(Url.AbsoluteUri))
                            LoginLinks.Add(Url.AbsoluteUri, Score);
                    }
                }
                return LoginLinks.ToList().OrderByDescending(x => x.Value).ThenBy(x => x.Key.Length).Take(15).ToList();
            }
        }

        protected class LoginForm : WebForm
        {

            public List<string> UsernameFields = new List<string>();
            public List<string> PasswordFields = new List<string>();

            protected override void FormProcessor(Uri mainPage)
            {
                foreach (HtmlNode Input in Form.SelectNodesSafe(@".//input"))
                {
                    if (!String.IsNullOrEmpty(Input.GetAttributeValueDecoded("name")) &&
                        !String.IsNullOrEmpty(Input.GetAttributeValueDecoded("type")))
                    {
                        if (Input.GetAttributeValueDecoded("type") == "text")
                            UsernameFields.Add(Input.GetAttributeValueDecoded("name", String.Empty));

                        else if (Input.GetAttributeValueDecoded("type") == "password")
                            PasswordFields.Add(Input.GetAttributeValueDecoded("name", String.Empty));

                        else if (//Input.Attribute("type", String.Empty) != "submit" &&
                            (Input.GetAttributeValueDecoded("type") != "radio" ||
                            Input.GetAttributeValueDecoded("checked") == "checked") &&
                            !OtherFields.ContainsKey(Input.GetAttributeValueDecoded("name", String.Empty)))
                            OtherFields.Add(Input.GetAttributeValueDecoded("name", String.Empty),
                               Input.GetAttributeValueDecoded("value", String.Empty));
                    }
                }
            }

            public static LoginForm Find(HtmlAgilityPack.HtmlDocument doc, Uri mainPage)
            {
                return Find(doc, mainPage, () => new LoginForm()) as LoginForm;
            }

            public static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri mainPage)
            {
                return WebForm.FindLinks(doc, mainPage, (linkUrl, linkNode) =>
                       linkUrl.MatchCount(Expr.Url.Action)
                     + MatchLinkNode(linkUrl, linkNode, Expr.Url.Login, Expr.Text.Global.Link.Login)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Register, Expr.Text.Global.Link.Register)
                    );
            }

            public HttpContent PostData(TaskBaseProperties.AccountData account, Encoding encoding)
            {
                var PostString = new StringBuilder();
                string Username = account.Username.ToLower();
                for (int i = 0; i < UsernameFields.Count; i++)
                {
                    PostString.Append("&" + UsernameFields[i] + "=");
                    PostString.Append(HttpUtility.UrlEncode(Username, encoding));
                }
                string Password = account.Password;
                for (int i = 0; i < PasswordFields.Count; i++)
                {
                    PostString.Append("&" + PasswordFields[i] + "=");
                    PostString.Append(HttpUtility.UrlEncode(Password, encoding));
                }
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

            public override bool Validate()
            {
                return !String.IsNullOrEmpty(Method) && !String.IsNullOrEmpty(Action)
                    && UsernameFields.Count > 0 && PasswordFields.Count > 0
                    && Form != null && Method == "post";
            }

            public override int Match()
            {
                int Score = Action.MatchCount(Expr.Url.Login)
                          - Action.MatchCount(Expr.Url.Register)
                          - Action.MatchCount(Expr.Url.Reply)
                          - Action.MatchCount(Expr.Url.NewTopic)
                          - PasswordFields.Where(x => x.MatchCount(@"match|repeat") > 0).Count();
                return Score;
            }
        }

        protected class PostForm : WebForm
        {
            public string SubjectFieldName;
            public string MessageFieldName;
            public string CaptchaFieldName;
            public string CaptchaPictureUrl;
            protected override void FormProcessor(Uri mainPage)
            {
                int BestSubjectScore = int.MinValue;
                int BestCaptchaScore = int.MinValue;
                foreach (HtmlNode Input in Form.SelectNodesSafe(@".//input"))
                {
                    if (!String.IsNullOrEmpty(Input.GetAttributeValueDecoded("name")) &&
                        !String.IsNullOrEmpty(Input.GetAttributeValueDecoded("type")))
                    {
                        if (Input.GetAttributeValueDecoded("type") == "text")
                        {
                            string name = Input.GetAttributeValueDecoded("name", String.Empty);
                            int SubjectScore = name.MatchCount(Expr.Text.Global.Fields.Subject);
                            int CaptchaScore = name.MatchCount(Expr.Text.Global.Fields.Captcha);
                            if (SubjectScore > BestSubjectScore)
                            {
                                SubjectFieldName = name;
                                BestSubjectScore = SubjectScore;
                            } if (CaptchaScore > BestCaptchaScore)
                            {
                                CaptchaFieldName = name;
                                BestCaptchaScore = CaptchaScore;
                            }
                        }
                        else if (//Input.Attribute("type", String.Empty) != "submit" &&
                            (Input.GetAttributeValueDecoded("type") != "radio" ||
                            Input.GetAttributeValueDecoded("checked") == "checked") &&
                            !OtherFields.ContainsKey(Input.GetAttributeValueDecoded("name", String.Empty)))
                            OtherFields.Add(Input.GetAttributeValueDecoded("name", String.Empty),
                               Input.GetAttributeValueDecoded("value", String.Empty));
                    }
                }

                HtmlNode TextArea = Form.SelectSingleNode(@".//textarea");
                if (TextArea != null) MessageFieldName = TextArea.GetAttributeValueDecoded("name", String.Empty);

                // Search for captcha image
                if (!String.IsNullOrEmpty(CaptchaFieldName))
                {
                    int BestCaptchaImgScore = int.MinValue;
                    foreach (HtmlNode Image in Form.SelectNodesSafe(@".//img"))
                    {
                        Uri Src;
                        if (Uri.TryCreate(Image.GetAttributeValueDecoded("src", String.Empty), UriKind.RelativeOrAbsolute, out Src) &&
                            (!Src.IsAbsoluteUri || (Src.Scheme == Uri.UriSchemeHttp || Src.Scheme == Uri.UriSchemeHttps)))
                        {
                            if (!Src.IsAbsoluteUri) Src = new Uri(mainPage, Src);

                            if (Src.AbsoluteUri.MatchCount(Expr.Url.CaptchaImage) > BestCaptchaImgScore)
                            {
                                BestCaptchaImgScore = Src.AbsoluteUri.MatchCount(Expr.Url.CaptchaImage);
                                CaptchaPictureUrl = Src.AbsoluteUri;
                            }
                        }
                    }
                }
            }

            public static PostForm Find(HtmlAgilityPack.HtmlDocument doc, Uri mainPage)
            {
                return Find(doc, mainPage, () => new PostForm()) as PostForm;
            }

            public static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri mainPage)
            {
                List<KeyValuePair<string, int>> ReplyLinks = WebForm.FindLinks(doc, mainPage, (linkUrl, linkNode) =>
                       linkUrl.MatchCount(Expr.Url.Action)
                     + MatchLinkNode(linkUrl, linkNode, Expr.Url.Reply, Expr.Text.Global.Link.Reply)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Quote, Expr.Text.Global.Link.Quote)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Poll, Expr.Text.Global.Link.Poll)
                     ).OrderBy(x => x.Value).ThenByDescending(x => x.Key.Length).ToList();
                List<KeyValuePair<string, int>> NewTopicLinks = WebForm.FindLinks(doc, mainPage, (linkUrl, linkNode) =>
                       linkUrl.MatchCount(Expr.Url.Action)
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
                    if (OtherFieldsList[i].Key.MatchCount(Expr.Text.Global.Fields.Preview) > 0)
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

            public override bool Validate()
            {
                return !String.IsNullOrEmpty(Method) && !String.IsNullOrEmpty(Action)
                    && !String.IsNullOrEmpty(MessageFieldName) && Form != null && Method == "post";
            }

            public override int Match()
            {
                int Score = -Action.MatchCount(Expr.Url.Login)
                            - Action.MatchCount(Expr.Url.Register)
                            + Action.MatchCount(Expr.Url.Reply)
                            + Action.MatchCount(Expr.Url.NewTopic);

                if (!String.IsNullOrEmpty(SubjectFieldName))
                    Score += SubjectFieldName.MatchCount(Expr.Text.Global.Fields.Subject);

                if (!String.IsNullOrEmpty(MessageFieldName))
                    Score += MessageFieldName.MatchCount(Expr.Text.Global.Fields.Message);

                if (!String.IsNullOrEmpty(CaptchaFieldName))
                    Score += CaptchaFieldName.MatchCount(Expr.Text.Global.Fields.Captcha);

                if (!String.IsNullOrEmpty(CaptchaPictureUrl))
                    Score += CaptchaPictureUrl.MatchCount(Expr.Url.CaptchaImage);

                return Score;
            }
        }

        public override async Task<Exception> Login()
        {
            // Searching LoginForm
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            var Response = await GetAndLog(Properties.ForumMainPage);
            Progress[0] = 40;
            lock (Log) Log.Add("Авторизация: Поиск формы авторизации");
            var Html = InitHtml(await Response.Content.ReadAsStreamAsync());
            string LoginUrl = Properties.ForumMainPage;
            LoginForm LoginForm = LoginForm.Find(Html, new Uri(Properties.ForumMainPage));
            Progress[0] = 50;

            // Check other pages for LoginForm
            if (LoginForm == null)
            {
                lock (Log) Log.Add("Авторизация: Поиск страницы авторизации");
                List<KeyValuePair<string, int>> LoginLinks = LoginForm.FindLinks(Html, new Uri(Properties.ForumMainPage));
                Progress[0] = 55;
                int i = 0;
                while (LoginForm == null && i < LoginLinks.Count)
                {
                    LoginUrl = LoginLinks[i].Key;
                    lock (Log) Log.Add("Авторизация: Загрузка страницы");
                    Response = await GetAndLog(LoginUrl);
                    Progress[0] += 80 / LoginLinks.Count;
                    lock (Log) Log.Add("Авторизация: Поиск формы авторизации");
                    Html = InitHtml(await Response.Content.ReadAsStreamAsync());
                    LoginForm = LoginForm.Find(Html, new Uri(Properties.ForumMainPage));
                    Progress[0] += 20 / LoginLinks.Count;
                    i++;
                }
            }
            if (LoginForm == null) return new Exception("Форма авторизации не найдена");
            Progress[0] = 155;

            // LoginPage preanalyse for future success detection
            string Text = Html.ClearDisplayNone().DocumentNode.InnerTextDecoded();
            int SuccessScore = +Expr.ErrorNodes(Html) - Expr.LoggedInNodes(Html);

            // Send authorization request
            lock (Log) Log.Add("Авторизация: Запрос авторизации");

            Response = await PostAndLog(LoginForm.Action, 
                LoginForm.PostData(AccountToUse, LoginForm.ChooseEncoding(Html.Encoding)));
            Progress[0] = 195;
            Html = InitHtml(await Response.Content.ReadAsStreamAsync()).ClearDisplayNone();
            Text = Html.DocumentNode.InnerTextDecoded();
            Progress[0] = 205;

            // Analyze response
            SuccessScore += Text.MatchCount(Expr.Text.Global.Message.LoginSuccess)
                          - Text.MatchCount(Expr.Text.Global.Message.Error)
                          - Expr.ErrorNodes(Html);

            // Load LoginPage again and analyze it now (after we've tryed to login)
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            Response = await GetAndLog(LoginUrl);
            Progress[0] = 245;
            lock (Log) Log.Add("Авторизация: Проверка авторизации");
            Html = InitHtml(await Response.Content.ReadAsStreamAsync()).ClearDisplayNone();
            Text = Html.DocumentNode.InnerTextDecoded();

            // Summarize analyzies and return conclusion if login was successfull
            if (LoginForm.Find(Html, new Uri(Properties.ForumMainPage)) == null) SuccessScore += 5;
            SuccessScore += Expr.LoggedInNodes(Html);
            Progress[0] = 255;
            Console.WriteLine("AuthSuccessScore: " + SuccessScore);
            if (SuccessScore <= 0) return new Exception("Авторизация не удалась");
            lock (Log) Log.Add("Авторизация: Успешно");
            return null;
        }

        public override async Task<Exception> PostMessage(Uri targetBoard, string subject, string message)
        {
            // Searching PostForm
            lock (Log) Log.Add("Постинг: Загрузка страницы");
            var Response = await GetAndLog(targetBoard.AbsoluteUri);
            Progress[2] += 40 / Progress[3];
            lock (Log) Log.Add("Постинг: Поиск формы постинга");
            var Html = InitHtml(await Response.Content.ReadAsStreamAsync());
            string PostUrl = targetBoard.AbsoluteUri;
            PostForm PostForm = PostForm.Find(Html, targetBoard);
            Progress[2] = +10 / Progress[3];

            // Check other pages for PostForm
            int TempProgress = Progress[2];
            if (PostForm == null)
            {
                lock (Log) Log.Add("Постинг: Поиск страницы постинга");
                List<KeyValuePair<string, int>> PostLinks = PostForm.FindLinks(Html, targetBoard);
                Progress[2] += 5 / Progress[3];
                int i = 0;
                while (PostForm == null && i < PostLinks.Count)
                {
                    PostUrl = PostLinks[i].Key;
                    lock (Log) Log.Add("Постинг: Загрузка страницы");
                    Response = await GetAndLog(PostUrl);
                    Progress[2] += (80 / Progress[3]) / PostLinks.Count;
                    lock (Log) Log.Add("Постинг: Поиск формы постинга");
                    Html = InitHtml(await Response.Content.ReadAsStreamAsync());
                    PostForm = PostForm.Find(Html, targetBoard);
                    Progress[2] += (20 / Progress[3]) / PostLinks.Count;
                    i++;
                }
            }
            if (PostForm == null) return new Exception("Форма постинга не найдена");
            Progress[2] = TempProgress + (100 / Progress[3]);

            // PostPage preanalyse for future success detection
            string Text = Html.ClearDisplayNone().DocumentNode.InnerTextDecoded();
            int SuccessScore = +Expr.ErrorNodes(Html);

            // Ask user for captcha if one was found
            string Captcha = String.Empty;
            if (!String.IsNullOrEmpty(PostForm.CaptchaFieldName) &&
                !String.IsNullOrEmpty(PostForm.CaptchaPictureUrl))
            {
                WaitingForQueue = true;
                lock (Log) Log.Add("Постинг: В очереди");
                await WaitFor(CaptchaForm.IsFree);
                Progress[2] += 10 / Progress[3];
                lock (Log) Log.Add("Постинг: Загрузка капчи");
                Response = await Client.GetAsync(PostForm.CaptchaPictureUrl, Cancel.Token);
                Progress[2] += 20 / Progress[3];
                CaptchaForm.Picture.Image = new Bitmap(await Response.Content.ReadAsStreamAsync());
                CaptchaForm.CancelFunction = () => Cancel.Cancel();
                lock (Log) Log.Add("Постинг: Ожидание ввода капчи");
                Application.OpenForms[0].Invoke((Action)(() => CaptchaForm.ShowDialog()));
                Captcha = CaptchaForm.Result.Text;
                CaptchaForm.IsFree.Set();
                Progress[2] += 20 / Progress[3];
            }
            else Progress[2] += 50 / Progress[3];

            // Send post message request
            lock (Log) Log.Add("Постинг: Отправка сообщения");
            Response = await PostAndLog(PostForm.Action,
                PostForm.PostData(AccountToUse, PostForm.ChooseEncoding(Html.Encoding), subject, message, Captcha));
            Progress[2] += 40 / Progress[3];
            Html = InitHtml(await Response.Content.ReadAsStreamAsync()).ClearDisplayNone();
            Text = Html.DocumentNode.InnerTextDecoded();

            // Analyze response and return conslusion if message posted successfully
            if (PostForm.Find(Html, targetBoard) != null) SuccessScore -= 5;
            SuccessScore += Text.MatchCount(Expr.Text.Global.Message.PostSuccess)
                          - Text.MatchCount(Expr.Text.Global.Message.Error)
                          - Expr.ErrorNodes(Html);

            Progress[2] += 10 / Progress[3];
            Console.WriteLine(SuccessScore);
            if (SuccessScore < 0) return new Exception("Постинг не удался");
            lock (Log) Log.Add("Успешно опубликовано");
            return null;
        }
    }
}