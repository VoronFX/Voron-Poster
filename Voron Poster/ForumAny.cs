using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Voron_Poster
{
    public class ForumAny : Forum
    {
        public ForumAny() : base() { }


        protected static HtmlDocument InitHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            // Otherwise forms have no children. Why it doesn't work out of the box =_= 
            HtmlNode.ElementsFlags.Remove("form");
            doc.LoadHtml(html);
            return doc;
        }

        protected static class Expr
        {
            public static string ActionUrl = @"[&\?]?(action|do)";
            public static string LoginUrl = @"log\w?in|sign\w?in|auth\w*|(user|member|acc?ount)?";
            public static string RegisterUrl = @"register|sign\w?up|(create|new)\w?(user|member|acc?ount)?";
            public static string LoginTextRu = @"вход|войти|авториз(ация|ироваться)|запомнить|логин";
            public static string RegisterTextRu = @"(за)?регистр(ировать|ация)
                                                   |(нов(ый|ого)|создать)\w(пользовател(ь|я)|акк?аунт)
                                                   |первый\w?раз";

        }

        protected class LoginForm
        {
            public HtmlNode Form;
            public string Method;
            public string Action;
            public List<string> UsernameFields = new List<string>();
            public List<string> PasswordFields = new List<string>();
            public Dictionary<string, string> OtherFields = new Dictionary<string, string>();
            public bool Validate()
            {
                return !String.IsNullOrEmpty(Method) && !String.IsNullOrEmpty(Action)
                    && UsernameFields.Count > 0 && PasswordFields.Count > 0
                    && Form != null;
            }
            public int Match()
            {
                int Score = Action.MatchCount(Expr.LoginUrl);
                Score -= Action.MatchCount(Expr.RegisterUrl);
                Score -= PasswordFields.Where(x => x.MatchCount(@"match|repeat") > 0).Count();
                return Score;
            }
        }

        protected LoginForm GetLoginForm(HtmlDocument doc)
        {
            var Forms = doc.DocumentNode.SelectNodes(@"//form");
            LoginForm BestForm = null;
            int BestFormScore = int.MinValue;
            if (Forms != null)
                for (int i = 0; i < Forms.Count; i++)
                {
                    var TempForm = Forms[i].Clone();

                    // Remove children forms
                    var SubForms = TempForm.SelectNodes(@".//form");
                    if (SubForms != null)
                        for (int i3 = 0; i3 < SubForms.Count; i3++)
                        {
                            SubForms[i3].Remove();
                        }

                    var Form = new LoginForm();
                    Form.Form = TempForm;
                    Form.Action = TempForm.GetAttributeValue("action", "");
                    Form.Method = TempForm.GetAttributeValue("method", "");
                    var Inputs = TempForm.SelectNodes(@".//input");
                    if (Inputs != null)
                        for (int i2 = 0; i2 < Inputs.Count; i2++)
                        {
                            if (Inputs[i2].GetAttributeValue("name", String.Empty) != String.Empty &&
                                Inputs[i2].GetAttributeValue("type", String.Empty) != String.Empty)
                            {
                                if (Inputs[i2].GetAttributeValue("type", String.Empty) == "text")
                                    Form.UsernameFields.Add(Inputs[i2].GetAttributeValue("name", String.Empty));

                                else if (Inputs[i2].GetAttributeValue("type", String.Empty) == "password")
                                    Form.PasswordFields.Add(Inputs[i2].GetAttributeValue("name", String.Empty));

                                else if (Inputs[i2].GetAttributeValue("type", String.Empty) != "submit" &&
                                    (Inputs[i2].GetAttributeValue("type", String.Empty) != "radio" ||
                                    Inputs[i2].GetAttributeValue("checked", String.Empty) == "checked") &&
                                    !Form.OtherFields.ContainsKey(Inputs[i2].GetAttributeValue("name", String.Empty)))
                                    Form.OtherFields.Add(Inputs[i2].GetAttributeValue("name", String.Empty),
                                        Inputs[i2].GetAttributeValue("value", String.Empty));
                            }
                        }
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

        protected Dictionary<Uri,int> GetLoginLinks(HtmlDocument doc){
             var AllLinks = doc.DocumentNode.SelectNodes(@"//*[@href]");
             var LoginLinks = new Dictionary<Uri, int>();
             string CurrentHost = new Uri(Properties.ForumMainPage).Host;
             for (int i = 0; i < AllLinks.Count; i++)
             {
                 Uri Url;
                 if (Uri.TryCreate(AllLinks[i].GetAttributeValue("href", ""),UriKind.RelativeOrAbsolute, out Url) &&
                     (Url.Scheme == Uri.UriSchemeHttp || Url.Scheme == Uri.UriSchemeHttps) &&
                     (Url.IsAbsoluteUri && Url.Host.EndsWith(CurrentHost, StringComparison.OrdinalIgnoreCase))
                     )
                 {
                     string Text = AllLinks[i].InnerText;
                     int Score = Url.OriginalString.MatchCount(Expr.LoginUrl);
                     Score -= Url.OriginalString.MatchCount(Expr.RegisterUrl);
                     Score += Text.MatchCount(Expr.LoginUrl);
                     Score += Text.MatchCount(Expr.LoginTextRu);
                     Score -= Text.MatchCount(Expr.RegisterUrl);
                     Score -= Text.MatchCount(Expr.RegisterTextRu);
                 }
             }
             return null;

        }

        public override async Task<Exception> Login()
        {
            var Response = await GetAndLog(Properties.ForumMainPage);
            var Html = InitHtml(await Response.Content.ReadAsStringAsync());

            GetLoginForm(Html);

            //       Console.WriteLine(FCount);
            return null;
        }

        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {
            return null;
        }
    }
}
