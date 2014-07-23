using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Voron_Poster
{
    public class ForumAny : Forum
    {
        public ForumAny() : base() { }

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
                Match CharsetMatch = new Regex(@"charset\s*=[\s""']*([^\s""'/>]*)").Match(Html);
                string Charset = null;
                if (CharsetMatch != null)
                    Charset = new Regex(@"charset\s*=[\s""']*").Replace(CharsetMatch.Value, String.Empty);
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
            public static string[] NotSupported = { "www.nulled.cc" };


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
                        Login = @"(?<!b)(sign|log)\s?in|auth\w*",

                        Register = @"register|sign\s?up|(create|new)\s(user|member|acc?ount)",

                        Reply = @"reply",
                        NewTopic = @"(create|open|new)\s(topic|thread)",
                        Quote = @"(?<!no\s?)quot(e|ing)",
                        Poll = @"poll",
                        CaptchaImage = Expr.Url.CaptchaImage
                    },
                    #endregion

                    #region Message
                    Message = new LocalText.MessageText
                    {
                        Error = @"(login|user(name)?|password)\s+(incorrect|wrong)|" +
                                @"errors?\s+occurred|please\s+enter\s+a\s+valid\s+message|" +
                                @"do\s?n['o]t\s+have\s+permission|" +
                                @"user(\s|\W)+(\w+(\s|\W)+)(could\s?)?n['o]t\s+(be\s+)?found|" +
                                @"must\s+be\s+logg?ed(-?\s?in)|" +
                                @"(incorrect|wrong)\s+password|please(\s|\W)+try\s+again"
                    },
                    #endregion

                    #region Fields
                    Fields = new LocalText.FieldsNames
                    {
                        Username = @"user|name|login|e(\W|_)?mail|nick",
                        Password = @"pass(wr?d|word)?",
                        RepeatPassword = @"match|repeat",
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
                        Poll = @"опрос",
                        LoggedIn = @"ваш\s+последний\s+визит|"+
                                   @"(редактировать|мо[йи])(\s+мой)?\s+(кабинет|профиль|закладки)|"+
                                   @"(личные)\s+сообщения|выход" //(новые|личные)\s+сообщения bad 
                    },
                    #endregion

                    #region Message
                    Message = new LocalText.MessageText
                    {
                        PostSuccess = @"спасибо\s+за\s+соо?бщение",
                        LoginSuccess = @"вы\s+((за|во)шли|авторизировались)\s+как(?!(\s|\W)+(гость|guest))|" +
                                       @"спасибо,?\s+что\s+зашли",

                        Error = @"(((обнаруж|допущ)ен[ыа]|(возникл|произошл)[иа])\s+(cледующ(ие|ая))?\s+ошибк[иа])|" +
                                @"((неверн|неправильн|введите\s+правильн)(ое?|ый|ые))\s+(введен\s+)?(имя|логин|пароль|код|данные)|" +
                                @"(нет|не\s+имеете)\s+доступа?|доступе?\s+(закрыт|отказано)|попробуйте\s+ещё\s+раз|повторите\s+попытку|" +
                                @"вы\s+не\s+авторизованы|недостаточно\s+прав|" +
                                @"вы\s+были\s+заблокированы" +
                                @"дата\s+снятия\s+блокировки|" +
                                @"(пожалуйста(\s|\W)+)?(войдите\s+или\s+зарегистрируйтесь|попробуйте\s+чуть\s+позже)|" +
                                @"(такого\s+)?пользователя\s+не\s+существует|" +
                                @"соо?бщение\s+слишком\s+короткое" //к\s+сож[ае]лению| bad
 
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
        }

        protected abstract class WebForm
        {
            public HtmlNode FormNode;
            public string Enctype;
            public string Method;
            public string Action;
            public string AcceptCharset;
            public string BaseUrl;
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
                int Score = thisText.MatchCount(matchPattern);
                if (Score > bestScore)
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
                    Form.FormProcessor();
                    Form.IsHidden = HtmlForm.IsNoDisplay();
                    int Score = Form.Score();

                    if (Score > BestFormScore && Score > 0)
                    {
                        BestForm = Form;
                        BestFormScore = Score;
                    }
                }
#if DEBUG
                if (BestForm != null)
                Console.WriteLine("FormScore: " + BestFormScore + " ActionUrl: " + BestForm.Action);
#endif
                return BestForm;
            }

            public static int ErrorNodes(HtmlAgilityPack.HtmlDocument doc)
            {
                IEnumerable<HtmlNode> Nodes =
                    doc.DocumentNode.Descendants().Where(
                    x => x.Attributes.Sum(
                        y => y.ValueDecoded().MatchCount(@"error|warn")) > 0);
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
                int Score = linkUrl.MatchCount(regExprLinkPattern)
                          + linkNode.InnerTextDecoded().MatchCount(regExprTextPattern);
                foreach (HtmlNode Node in linkNode.DescendantsAndSelf())
                    Score += Node.Attributes.Sum(x => x.ValueDecoded().MatchCount(regExprLinkPattern))
                           + Node.Attributes.Sum(x => x.ValueDecoded().MatchCount(regExprTextPattern));
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

            public List<string> UsernameFields = new List<string>();
            public List<string> PasswordFields = new List<string>();

            protected override void FormProcessor()
            {
                foreach (HtmlNode Input in FormNode.Descendants("input"))
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

            public static LoginForm Find(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                return Find(doc, defaultMainPage, () => new LoginForm()) as LoginForm;
            }

            public static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                return WebForm.FindLinks(doc, defaultMainPage, (linkUrl, linkNode) =>
                       linkUrl.MatchCount(Expr.Url.Action)// <= 0 ? 0 : linkUrl.MatchCount(Expr.Url.Action)
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

            public override int Score()
            {
                int ActionLoginScore = Action.MatchCount(Expr.Url.Login);
                int UsernameScore = UsernameFields.Sum((x) => x.MatchCount(Expr.Text.Global.Fields.Username));
                int PasswordScore = PasswordFields.Sum((x) => x.MatchCount(Expr.Text.Global.Fields.Password));

                if (ActionLoginScore <= 0 || UsernameScore <= 0 || PasswordScore <= 0 ||
                    PasswordFields.Sum((x) => x.MatchCount(Expr.Text.Global.Fields.RepeatPassword)) > 0
                    || Method != "post") return 0;
                else return ActionLoginScore + UsernameScore + PasswordScore
                    - Action.MatchCount(Expr.Url.Register) + Action.MatchCount(Expr.Url.Action);
            }
        }

        protected class PostForm : WebForm
        {
            public string SubjectFieldName;
            public string MessageFieldName;
            public string CaptchaFieldName;
            public string CaptchaPictureUrl;

            protected override void FormProcessor()
            {
                int BestSubjectScore = int.MinValue;
                int BestCaptchaScore = int.MinValue;
                int BestMessageScore = int.MinValue;

                foreach (HtmlNode Input in FormNode.Descendants("input"))
                {
                    if (!String.IsNullOrEmpty(Input.GetAttributeValueDecoded("name")) &&
                        !String.IsNullOrEmpty(Input.GetAttributeValueDecoded("type")))
                    {
                        if (Input.GetAttributeValueDecoded("type") == "text")
                        {
                            string name = Input.GetAttributeValueDecoded("name", String.Empty);

                            SelectBest(ref BestSubjectScore, ref SubjectFieldName, name, Expr.Text.Global.Fields.Subject);

                            SelectBest(ref BestCaptchaScore, ref CaptchaFieldName, name, Expr.Text.Global.Fields.Captcha);

                        }
                        else if (//Input.Attribute("type", String.Empty) != "submit" &&
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

                // Search for captcha image
                if (!String.IsNullOrEmpty(CaptchaFieldName))
                {
                    int BestCaptchaImgScore = int.MinValue;
                    foreach (HtmlNode Image in FormNode.Descendants("img"))
                    {
                        Uri Src;
                        if (Uri.TryCreate(Image.GetAttributeValueDecoded("src", String.Empty), UriKind.RelativeOrAbsolute, out Src) &&
                            (!Src.IsAbsoluteUri || (Src.Scheme == Uri.UriSchemeHttp || Src.Scheme == Uri.UriSchemeHttps)))
                        {
                            if (!Src.IsAbsoluteUri) Src = new Uri(new Uri(BaseUrl), Src);

                            int CaptchaImgScore = MatchLinkNode(Src.AbsoluteUri, Image, Expr.Url.CaptchaImage, Expr.Text.Global.Link.CaptchaImage);
                            if (CaptchaImgScore > BestCaptchaImgScore)
                            {
                                CaptchaPictureUrl = Src.AbsoluteUri;
                                BestCaptchaImgScore = CaptchaImgScore;
                            }
                        }
                    }
                }

            }

            public static PostForm Find(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                return Find(doc, defaultMainPage, () => new PostForm()) as PostForm;
            }

            public static List<KeyValuePair<string, int>> FindLinks(HtmlAgilityPack.HtmlDocument doc, Uri defaultMainPage)
            {
                List<KeyValuePair<string, int>> ReplyLinks = WebForm.FindLinks(doc, defaultMainPage, (linkUrl, linkNode) =>
                       linkUrl.MatchCount(Expr.Url.Action)// <= 0 ? 0 : linkUrl.MatchCount(Expr.Url.Action)
                     + MatchLinkNode(linkUrl, linkNode, Expr.Url.Reply, Expr.Text.Global.Link.Reply)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Quote, Expr.Text.Global.Link.Quote)
                     - MatchLinkNode(linkUrl, linkNode, Expr.Url.Poll, Expr.Text.Global.Link.Poll)
                     ).OrderBy(x => x.Value).ThenByDescending(x => x.Key.Length).ToList();
                List<KeyValuePair<string, int>> NewTopicLinks = WebForm.FindLinks(doc, defaultMainPage, (linkUrl, linkNode) =>
                       linkUrl.MatchCount(Expr.Url.Action)// <= 0 ? 0 : linkUrl.MatchCount(Expr.Url.Action)
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

            public override int Score()
            {
                int ActionLoginScore = Action.MatchCount(Expr.Url.NewTopic) + Action.MatchCount(Expr.Url.Reply);
                int MessageScore = MessageFieldName.MatchCount(Expr.Text.Global.Fields.Message);

                if (ActionLoginScore <= 0 || MessageScore <= 0 || Method != "post") return 0;
                else return ActionLoginScore + MessageScore
                    + Action.MatchCount(Expr.Url.Action)
                    + SubjectFieldName.MatchCount(Expr.Text.Global.Fields.Subject)
                    + CaptchaFieldName.MatchCount(Expr.Text.Global.Fields.Captcha);
            }
        }

        public override async Task<Exception> Login()
        {
            // Check for bad site
            if (Expr.NotSupported.Contains(new Uri(Properties.ForumMainPage).Host))
                return new Exception("Сайт не поддерживается");

            // Searching LoginForm
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            var Response = await GetAndLog(Properties.ForumMainPage);
            Progress[0] = 40;
            lock (Log) Log.Add("Авторизация: Поиск формы авторизации");
            string ss = await Response.Content.ReadAsStringAsync();
            var Html = await InitHtml(Response);
            string LoginUrl = Properties.ForumMainPage;
            LoginForm LoginForm = LoginForm.Find(Html, new Uri(LoginUrl));
            Progress[0] = 50;

            // Check other pages for LoginForm
            if (LoginForm == null)
            {
                lock (Log) Log.Add("Авторизация: Поиск страницы авторизации");
                List<KeyValuePair<string, int>> LoginLinks = LoginForm.FindLinks(Html, new Uri(LoginUrl));
                Progress[0] = 55;
                int i = 0;
                while (LoginForm == null && i < LoginLinks.Count)
                {
                    LoginUrl = LoginLinks[i].Key;
                    lock (Log) Log.Add("Авторизация: Загрузка страницы");
                    Response = await GetAndLog(LoginUrl);
                    Progress[0] += 80 / LoginLinks.Count;
                    lock (Log) Log.Add("Авторизация: Поиск формы авторизации");
                    Html = await InitHtml(Response);
                    LoginForm = LoginForm.Find(Html, new Uri(LoginUrl));
                    Progress[0] += 20 / LoginLinks.Count;
                    i++;
                }
            }
            if (LoginForm == null) return new Exception("Форма авторизации не найдена");
            Progress[0] = 155;

#if DEBUG
                Console.WriteLine("LoginLink: {0}", LoginUrl);
#endif

            // LoginPage preanalyse for future success detection
            Html.ClearDisplayNone();
            string Text = Html.DocumentNode.InnerTextDecoded();
            //int SuccessScore = +WebForm.ErrorNodes(Html) - WebForm.LoggedInNodes(Html);

#if DEBUG
                Console.WriteLine("Before: Success: {0} Error: {1} ErrorNodes: {2} LoggeInNodes: {3}",
                Text.MatchCount(Expr.Text.Global.Message.LoginSuccess),
                Text.MatchCount(Expr.Text.Global.Message.Error),
                WebForm.ErrorNodes(Html),
                WebForm.LoggedInNodes(Html));
            //return null;
#endif

            // Send authorization request
            lock (Log) Log.Add("Авторизация: Запрос авторизации");

            Response = await PostAndLog(LoginForm.Action,
                LoginForm.PostData(AccountToUse, LoginForm.ChooseEncoding(Html.Encoding)));
            Progress[0] = 195;
            Html = await InitHtml(Response);
            Html.ClearDisplayNone();
            Text = Html.DocumentNode.InnerTextDecoded();
            Progress[0] = 205;
            // Analyze response
            int SuccessScore = Text.MatchCount(Expr.Text.Global.Message.LoginSuccess)
                             - Text.MatchCount(Expr.Text.Global.Message.Error)
                             - WebForm.ErrorNodes(Html);

#if DEBUG
            Console.WriteLine("Answer: Success: {0} Error: {1} ErrorNodes: {2} LoggeInNodes: {3}",
            Text.MatchCount(Expr.Text.Global.Message.LoginSuccess),
            Text.MatchCount(Expr.Text.Global.Message.Error),
            WebForm.ErrorNodes(Html),
            WebForm.LoggedInNodes(Html));
            //return null;
#endif

            // Load LoginPage again and analyze it now (after we've tryed to login)
            lock (Log) Log.Add("Авторизация: Загрузка страницы");
            Response = await GetAndLog(LoginUrl);
            Progress[0] = 245;
            lock (Log) Log.Add("Авторизация: Проверка авторизации");
            Html = await InitHtml(Response);
            LoginForm AfterLoginForm = LoginForm.Find(Html, new Uri(LoginUrl));


            // Summarize analyzies and return conclusion if login was successfull
            if (AfterLoginForm == null || (AfterLoginForm.IsHidden && !LoginForm.IsHidden)) SuccessScore += 3;
            else if (AfterLoginForm != null && !AfterLoginForm.IsHidden) SuccessScore -= 2;
            Html.ClearDisplayNone();
            Text = Html.DocumentNode.InnerTextDecoded();
            SuccessScore += WebForm.LoggedInNodes(Html);
            Progress[0] = 255;
#if DEBUG
            Console.WriteLine("After: Success: {0} Error: {1} ErrorNodes: {2} LoggeInNodes: {3}",
            Text.MatchCount(Expr.Text.Global.Message.LoginSuccess),
            Text.MatchCount(Expr.Text.Global.Message.Error),
            WebForm.ErrorNodes(Html),
            WebForm.LoggedInNodes(Html));
            //return null;
#endif
#if DEBUG
            Console.WriteLine("AuthSuccessScore: " + SuccessScore+'\n');
#endif
            if (SuccessScore <= 0) return new Exception("Авторизация не удалась");
            lock (Log) Log.Add("Авторизация: Успешно");
            return null;
        }

        public override async Task<Exception> PostMessage(Uri targetBoard, string subject, string message)
        {
            if (targetBoard.Host == "seodor.biz")
            {
                WaitingForQueue = true;
                lock (Log) Log.Add("Задержка 5 сек");
                Task.Delay(5000).Wait(Cancel.Token);
                WaitingForQueue = false;
            }

            // Searching PostForm
            lock (Log) Log.Add("Постинг: Загрузка страницы");
            var Response = await GetAndLog(targetBoard.AbsoluteUri);
            Progress[2] += 40 / Progress[3];
            lock (Log) Log.Add("Постинг: Поиск формы постинга");
            var Html = await InitHtml(Response);
            string PostUrl = targetBoard.AbsoluteUri;
            PostForm PostForm = PostForm.Find(Html, new Uri(PostUrl));
            Progress[2] = +10 / Progress[3];

            // Check other pages for PostForm
            int TempProgress = Progress[2];
            if (PostForm == null)
            {
                lock (Log) Log.Add("Постинг: Поиск страницы постинга");
                List<KeyValuePair<string, int>> PostLinks = PostForm.FindLinks(Html, new Uri(PostUrl));
#if DEBUG
                Console.WriteLine("PostLinks: {0}", PostUrl);
         //       foreach (var Link in PostLinks) Console.WriteLine(Link.Key.ToString());
#endif
                Progress[2] += 5 / Progress[3];
                int i = 0;
                while (PostForm == null && i < PostLinks.Count)
                {
                    PostUrl = PostLinks[i].Key;
                    lock (Log) Log.Add("Постинг: Загрузка страницы");
                    Response = await GetAndLog(PostUrl);
                    Progress[2] += (80 / Progress[3]) / PostLinks.Count;
                    lock (Log) Log.Add("Постинг: Поиск формы постинга");
                    Html = await InitHtml(Response);
                    PostForm = PostForm.Find(Html, new Uri(PostUrl));
                    Progress[2] += (20 / Progress[3]) / PostLinks.Count;
                    i++;
                }
            }
            if (PostForm == null) return new Exception("Форма постинга не найдена");
            Progress[2] = TempProgress + (100 / Progress[3]);

            // PostPage preanalyse for future success detection
            Html.ClearDisplayNone();
            string Text = Html.DocumentNode.InnerTextDecoded();
            int SuccessScore = 0; // +WebForm.ErrorNodes(Html);
#if DEBUG
            Console.WriteLine("After: Success: {0} Error: {1} ErrorNodes: {2}",
            Text.MatchCount(Expr.Text.Global.Message.PostSuccess),
            Text.MatchCount(Expr.Text.Global.Message.Error),
            WebForm.ErrorNodes(Html));
            return null;
#endif

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
            Html = await InitHtml(Response);
            PostForm AfterPostForm = PostForm.Find(Html, new Uri(PostUrl));
            Html.ClearDisplayNone();
            Text = Html.DocumentNode.InnerTextDecoded();

            // Analyze response and return conslusion if message posted successfully
           
            //if (AfterPostForm != null && !AfterPostForm.IsHidden) SuccessScore -= 5; bad idea

            SuccessScore += Text.MatchCount(Expr.Text.Global.Message.PostSuccess)
                          - Text.MatchCount(Expr.Text.Global.Message.Error)
                          - WebForm.ErrorNodes(Html);

            Progress[2] += 10 / Progress[3];
#if DEBUG
            Console.WriteLine("After: Success: {0} Error: {1} ErrorNodes: {2}",
            Text.MatchCount(Expr.Text.Global.Message.PostSuccess),
            Text.MatchCount(Expr.Text.Global.Message.Error),
            WebForm.ErrorNodes(Html));
            //return null;
#endif
#if DEBUG
            Console.WriteLine("PostSuccessScore: " + SuccessScore);
#endif
            if (SuccessScore < 0) return new Exception("Постинг не удался");
            lock (Log) Log.Add("Успешно опубликовано");
            return null;
        }
    }
}