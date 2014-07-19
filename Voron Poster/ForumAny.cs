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

namespace Voron_Poster
{
    public class ForumAny : Forum
    {
        public ForumAny() : base() { }


        protected static HtmlDocument InitHtml(Stream html)
        {
            HtmlDocument doc = new HtmlDocument();
            // Otherwise forms have no children. Why it doesn't work out of the box =_= 
            HtmlNode.ElementsFlags.Remove("form");
            doc.Load(html);
            return doc;
        }

        protected static class Expr
        {
            public static string ActionUrl = @"[&\?]?(action|do|mode)=";
            public static string LoginUrl = @"(sign|log)\w?in|auth\w*|(user|member|acc?ount)?";
            public static string RegisterUrl = @"register|sign\w?up|(create|new)\w?(user|member|acc?ount)?";
            public static string LoginTextRu = @"вход|войти|авториз(ация|ироваться)|запомнить|логин";
            public static string RegisterTextRu = @"(за)?регистр(ировать|ация)|"+
                                                  @"(нов(ый|ого)|создать)\s+(пользовател(ь|я)|акк?аунт)|"+
                                                  @"первый\s+раз";
            /// <summary>
            /// Russian success login message expressions
            /// </summary>
            public static string LoginSuccesTextRu = @"вы\s+(з[за|во]шли|авторизировались)\s+как";
            /// <summary>
            /// Russian error message expressions
            /// </summary>
            public static string ErrorTextRu = @"((обнаружен[ыа]|(возникл|произошл)[иа])\s+(cледующ(ие|ая))?\s+ошибк[иа])|"+
                                               @"(неверн[ое|ый]\s+(имя\w+пользователя|логин|пароль))|"+
                                               @"((вве(сти|дите)|неправильный)\s+код)";
            /// <summary>
            /// English error message expressions
            /// </summary>
            public static string ErrorTextEn = @"(login|user(name)?|password)\s+incorrect";
        }

        protected class LoginForm
        {
            public HtmlNode Form;
            public string Method;
            public string Action;
            public List<string> UsernameFields = new List<string>();
            public List<string> PasswordFields = new List<string>();
            public Dictionary<string, string> OtherFields = new Dictionary<string, string>();

            public static LoginForm Find(HtmlDocument doc, Uri mainPage)
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
                        Uri ActionUri;
                        if (Uri.TryCreate(TempForm.GetAttributeValue("action", ""), UriKind.RelativeOrAbsolute, out ActionUri) &&
                        (ActionUri.IsAbsoluteUri && (ActionUri.Scheme == Uri.UriSchemeHttp || ActionUri.Scheme == Uri.UriSchemeHttps)
                         && ActionUri.Host.EndsWith(mainPage.Host, StringComparison.OrdinalIgnoreCase)))
                        {
                            if (!ActionUri.IsAbsoluteUri) ActionUri = new Uri(mainPage, ActionUri);
                            Form.Action = ActionUri.AbsoluteUri;
                        }
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

                                    else if (//Inputs[i2].GetAttributeValue("type", String.Empty) != "submit" &&
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
           
            public static List<KeyValuePair<string, int>> FindLinks(HtmlDocument doc, Uri mainPage)
            {
                var AllLinks = doc.DocumentNode.SelectNodes(@"//*[@href]");
                var LoginLinks = new Dictionary<string, int>();
                for (int i = 0; i < AllLinks.Count; i++)
                {
                    Uri Url;
                    if (Uri.TryCreate(AllLinks[i].GetAttributeValue("href", ""), UriKind.RelativeOrAbsolute, out Url) &&
                        (Url.IsAbsoluteUri && (Url.Scheme == Uri.UriSchemeHttp || Url.Scheme == Uri.UriSchemeHttps)
                         && Url.Host.EndsWith(mainPage.Host, StringComparison.OrdinalIgnoreCase)))
                    {
                        string Text = AllLinks[i].InnerText;
                        int Score = Url.OriginalString.MatchCount(Expr.ActionUrl)
                                  + Url.OriginalString.MatchCount(Expr.LoginUrl)
                                  - Url.OriginalString.MatchCount(Expr.RegisterUrl)
                                  + Text.MatchCount(Expr.LoginUrl)
                                  + Text.MatchCount(Expr.LoginTextRu)
                                  - Text.MatchCount(Expr.RegisterUrl)
                                  - Text.MatchCount(Expr.RegisterTextRu);
                        if (!Url.IsAbsoluteUri) Url = new Uri(mainPage, Url);
                        if (Score > 0 && !LoginLinks.ContainsKey(Url.AbsoluteUri))
                            LoginLinks.Add(Url.AbsoluteUri, Score);
                    }
                }
                return LoginLinks.ToList().OrderBy(x => x.Value).ThenBy(x => x.Key.Length).ToList();
            }

            public StringContent PostData(LoginForm form, TaskBaseProperties.AccountData account)
        {
            var PostString = new StringBuilder();
            string Username = account.Username.ToLower();
            for (int i = 0; i < form.UsernameFields.Count; i++)
            {              
                PostString.Append("&");
                PostString.Append(form.UsernameFields[i]);
                PostString.Append("=");
                PostString.Append(HttpUtility.UrlEncode(Username, Encoding.Default));
            }
            string Password = account.Password;
            for (int i = 0; i < form.PasswordFields.Count; i++)
            {
                PostString.Append("&");
                PostString.Append(form.PasswordFields[i]);
                PostString.Append("=");
                PostString.Append(HttpUtility.UrlEncode(Password, Encoding.Default));
            }
            List<KeyValuePair<string, string>> OtherFields = form.OtherFields.ToList();
            for (int i = 0; i < form.OtherFields.Count; i++)
            {
                PostString.Append("&");
                PostString.Append(OtherFields[i].Key);
                PostString.Append("=");
                PostString.Append(HttpUtility.UrlEncode(OtherFields[i].Value, Encoding.Default));
            }
            PostString.Remove(0, 1);
            return new StringContent(PostString.ToString(), Encoding.Default,
                        "application/x-www-form-urlencoded");
        }

            public bool Validate()
            {
                return !String.IsNullOrEmpty(Method) && !String.IsNullOrEmpty(Action)
                    && UsernameFields.Count > 0 && PasswordFields.Count > 0
                    && Form != null && Method == "post";
            }

            public int Match()
            {
                int Score = Action.MatchCount(Expr.LoginUrl);
                Score -= Action.MatchCount(Expr.RegisterUrl);
                Score -= PasswordFields.Where(x => x.MatchCount(@"match|repeat") > 0).Count();
                return Score;
            }
        }


        protected int ErrorScore(HtmlDocument doc)
        {
            var All = doc.DocumentNode.SelectNodes(@"//*[@*]");
            int Error = 0;
            for (int i = 0; i < All.Count; i++)
            {
                bool IsError = false;
                bool IsVisible = true;
                if (All[i].Name == "script") continue;
                for (int i2 = 0; i2 < All[i].Attributes.Count; i2++)
                {
                    if (All[i].Attributes[i2].Value.MatchCount(@"display:\s*none") > 0)
                    {
                        IsVisible = false;
                        break;
                    }
                    if (All[i].Attributes[i2].Value.MatchCount(@"error|warn") > 0)
                    {
                        IsError = true;
                    }
                }
                if (IsError && IsVisible) Error++;
            }
            return Error;
        }

        protected int LoggedInScore(HtmlDocument doc)
        {
            var All = doc.DocumentNode.SelectNodes(@"//*[@href]");
            int LoggedIn = 0;
            for (int i = 0; i < All.Count; i++)
            {
                bool IsLoggedIn = false;
                bool IsVisible = true;
                if (All[i].Name == "script") continue;
                for (int i2 = 0; i2 < All[i].Attributes.Count; i2++)
                {
                    if (All[i].Attributes[i2].Value.MatchCount(@"display:\s*none") > 0)
                    {
                        IsVisible = false;
                        break;
                    }
                    if ((All[i].Attributes[i2].Name == "href" || All[i].Attributes[i2].Name == "onclick") &&
                        All[i].Attributes[i2].Value.MatchCount(@"(sign|log)\w*out|"+
                                                               @"edit\w*profile|"+
                                                               @"profile\w*(settings|options|edit)") > 0)
                                                                 
                    {
                        IsLoggedIn = true;
                    }
                }
                if (IsLoggedIn && IsVisible) LoggedIn++;
            }
            return LoggedIn;
        }

        public override async Task<Exception> Login()
        {
            // Searching LoginForm
            var Response = await GetAndLog(Properties.ForumMainPage);
            var Html = InitHtml(await Response.Content.ReadAsStreamAsync());
            string LoginUrl = Properties.ForumMainPage;
            LoginForm LoginForm = LoginForm.Find(Html, new Uri(Properties.ForumMainPage));

            // Check other pages for LoginForm
            if (LoginForm == null)
            {
                List<KeyValuePair<string, int>> LoginLinks = LoginForm.FindLinks(Html, new Uri(Properties.ForumMainPage));
                int i = 0;
                while (LoginForm == null && i < LoginLinks.Count)
                {
                    LoginUrl = LoginLinks[i].Key;
                    Response = await GetAndLog(LoginUrl);
                    Html = InitHtml(await Response.Content.ReadAsStreamAsync());
                    LoginForm = LoginForm.Find(Html, new Uri(Properties.ForumMainPage));
                    i++;
                }
            }
            if (LoginForm == null) return new Exception("Форма авторизации не найдена");
            
            string Text = Html.DocumentNode.InnerText;
            int SuccessScore = +ErrorScore(Html) - LoggedInScore(Html)
                - Text.MatchCount(Expr.LoginSuccesTextRu);
                
            Response = await PostAndLog(LoginForm.Action, LoginForm.PostData(LoginForm, AccountToUse));
            Html = InitHtml(await Response.Content.ReadAsStreamAsync());
            Text = Html.DocumentNode.InnerText;

            //SuccessScore += -ErrorScore(Html)
            //    - Text.MatchCount(Expr.ErrorTextRu)
            //    + Text.MatchCount(Expr.LoginSuccesTextRu);
            
            Response = await GetAndLog(LoginUrl);
            Html = InitHtml(await Response.Content.ReadAsStreamAsync());
            Text = Html.DocumentNode.InnerText;

            if (LoginForm.Find(Html, new Uri(Properties.ForumMainPage)) == null) SuccessScore += 5;
            SuccessScore += -ErrorScore(Html) + LoggedInScore(Html)
                + Text.MatchCount(Expr.LoginSuccesTextRu)
                - Text.MatchCount(Expr.ErrorTextEn)
                - Text.MatchCount(Expr.ErrorTextRu);


            Console.WriteLine(SuccessScore);
            if (SuccessScore <= 0) return new Exception("Авторизация не удалась");
            return null;
        }

        public override async Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message)
        {
            return null;
        }
    }
}
