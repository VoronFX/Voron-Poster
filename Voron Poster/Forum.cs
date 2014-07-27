using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Net;
using Roslyn.Scripting.CSharp;
using System.Runtime.Remoting.Messaging;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace Voron_Poster
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns the input typed as a generic IEnumerable of the groups
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static IEnumerable<System.Text.RegularExpressions.Group> AsEnumerable(this System.Text.RegularExpressions.GroupCollection gc)
        {
            foreach (System.Text.RegularExpressions.Group g in gc)
            {
                yield return g;
            }
        }
        /// <summary>
        /// Returns the input typed as a generic IEnumerable of the matches
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public static IEnumerable<System.Text.RegularExpressions.Match> AsEnumerable(this System.Text.RegularExpressions.MatchCollection mc)
        {
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                yield return m;
            }
        }
        /// <summary>
        /// Gets the element that occurs most frequently in the collection.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Returns the element that occurs most frequently in the collection.
        /// If all elements occur an equal number of times, a random element in
        /// the collection will be returned.</returns>
        public static T Mode<T>(this IEnumerable<T> list)
        {
            // Initialize the return value
            T mode = default(T);

            // Test for a null reference and an empty list
            if (list != null && list.Count() > 0)
            {
                // Store the number of occurences for each element
                Dictionary<T, int> counts = new Dictionary<T, int>();

                // Add one to the count for the occurence of a character
                foreach (T element in list)
                {
                    if (counts.ContainsKey(element))
                        counts[element]++;
                    else
                        counts.Add(element, 1);
                }

                // Loop through the counts of each element and find the 
                // element that occurred most often
                int max = 0;

                foreach (KeyValuePair<T, int> count in counts)
                {
                    if (count.Value > max)
                    {
                        // Update the mode
                        mode = count.Key;
                        max = count.Value;
                    }
                }
            }

            return mode;
        }
        /// <summary>
        /// Checks if string contains any of words. Case ignored.
        /// </summary>
        public static bool ContainsAny(this string s, string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (s.IndexOf(words[i], StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns number of Matches. Case ignored.
        /// </summary>
        public static int MatchCount(this string s, string regExprPattern)
        {
            return String.IsNullOrEmpty(regExprPattern) || String.IsNullOrEmpty(s)
                ? 0 : (new Regex(regExprPattern, RegexOptions.IgnoreCase)).Matches(s).Count;
        }
        /// <summary>
        /// Returns number of Matches.
        /// </summary>
        public static int MatchCount(this string s, string regExprPattern, RegexOptions options)
        {
            return String.IsNullOrEmpty(regExprPattern) || String.IsNullOrEmpty(s)
                ? 0 : (new Regex(regExprPattern, options)).Matches(s).Count;
        }

        #region HtmlAgilityPackFixes

        public static string GetAttributeValueDecoded(this HtmlAgilityPack.HtmlNode node, string name)
        {
            return HttpUtility.HtmlDecode(node.GetAttributeValue(name, null));
        }

        public static string GetAttributeValueDecoded(this HtmlAgilityPack.HtmlNode node, string name, string defaultValue)
        {
            return HttpUtility.HtmlDecode(node.GetAttributeValue(name, defaultValue));
        }

        public static string ValueDecoded(this HtmlAgilityPack.HtmlAttribute attribute)
        {
            return HttpUtility.HtmlDecode(attribute.Value);
        }

        public static void ClearScriptsStylesComments(this HtmlAgilityPack.HtmlDocument doc)
        {
            var Bad = doc.DocumentNode.Descendants("script");
            while (Bad.Count() > 0) Bad.First().Remove();
            Bad = doc.DocumentNode.Descendants("style");
            while (Bad.Count() > 0) Bad.First().Remove();
            Bad = doc.DocumentNode.Descendants("#comment");
            while (Bad.Count() > 0) Bad.First().Remove();
        }

        public static void ClearHidden(this HtmlAgilityPack.HtmlDocument doc)
        {
            IEnumerable<HtmlNode> NoDisplay = doc.DocumentNode.DescendantsAndSelf().Where(
                x => x.GetAttributeValueDecoded("style", String.Empty).MatchCount(@"display:\s*none") > 0
                || x.GetAttributeValueDecoded("class", String.Empty).MatchCount(@"hidden") > 0);
            while (NoDisplay.Count() > 0) NoDisplay.First().Remove();
        }

        public static string InnerTextDecoded(this HtmlAgilityPack.HtmlNode node)
        {
            return HttpUtility.HtmlDecode(node.InnerText);
        }

        /// <summary>
        /// Checks if node or any parent node has "display: none" style
        /// </summary>
        public static bool IsNoDisplay(this HtmlAgilityPack.HtmlNode node)
        {
            bool NoDisplay = false;
            do
            {
                NoDisplay = node.GetAttributeValueDecoded("style", String.Empty).MatchCount(@"display:\s*none") > 0;
                node = node.ParentNode;
            }
            while (!NoDisplay && node.ParentNode != null);
            return NoDisplay;
        }

        /// <summary>
        /// Don't use it! Use LINQ instead of XPath because here it's buggy
        /// </summary>
        public static HtmlNodeCollection SelectNodesSafe(this HtmlAgilityPack.HtmlNode node, string xpath)
        {
            HtmlNodeCollection Selected = node.SelectNodes(xpath);
            return Selected == null ? new HtmlNodeCollection(node) : Selected;
        }

        #endregion
    }

    public abstract class Forum
    {

        static ConcurrentDictionary<string, AutoResetEvent> DomainQueue
            = new ConcurrentDictionary<string, AutoResetEvent>();
        public bool WaitingForQueue;
        private Task WaitOrAdd(string Domain)
        {
            AutoResetEvent WaitHandle = null;
            if (!DomainQueue.TryGetValue(Domain, out WaitHandle))
            {
                WaitHandle = new AutoResetEvent(true);
                DomainQueue.TryAdd(Domain, WaitHandle);
            }
            if (WaitHandle.WaitOne(0))
            {

                if (WaitHandle == null) MessageBox.Show("WaitHandler null");
                if (Activity == null) MessageBox.Show("Activity null");

                WaitHandle.Reset();
                Activity.ContinueWith((uselessvar) => WaitHandle.Set());
                return Task.FromResult(true);
            }
            else
                return WaitFor(WaitHandle).ContinueWith((uselessvar) =>
                {
                    try { Task.Delay(3000).Wait(Cancel.Token); }
                    catch { }
                });
        }

        #region Detect Engine
        public enum Engine { Unknown, SMF, vBulletin, IPB, Universal, AnyForum }

        protected struct SearchExpression
        {
            public string Expression;
            public int Value;

            public SearchExpression(string newSearchExpression, int newValue)
            {
                Expression = newSearchExpression;
                Value = newValue;
            }
        }

        protected static int SearchForExpressions(string Html, SearchExpression[] Expressions)
        {
            int ResultMatch = 0;
            for (int i = 0; i < Expressions.Length; i++)
            {
                if (Html.IndexOf(Expressions[i].Expression, StringComparison.OrdinalIgnoreCase) >= 0)
                    ResultMatch += Expressions[i].Value;
            }
            return ResultMatch;
        }

        public static async Task<Engine> DetectEngine(string Url, HttpClient Client, CancellationToken Cancel)
        {
            int[] Match = new int[Enum.GetNames(typeof(Engine))
                     .Length];
            string Html = await (await Client.GetAsync(Url, Cancel)).Content.ReadAsStringAsync();
            Match[(int)Engine.Unknown] = 9;

            Match[(int)Engine.SMF] += SearchForExpressions(Html, new SearchExpression[] {
            new SearchExpression("Powered by SMF", 20),
            new SearchExpression("Simple Machines Forum", 20),
            new SearchExpression("http://www.simplemachines.org/about/copyright.php", 10),
            new SearchExpression("http://www.simplemachines.org/", 10),
            new SearchExpression("Simple Machines", 10)});

            Match[(int)Engine.vBulletin] += SearchForExpressions(Html, new SearchExpression[] {
            new SearchExpression("vBulletin", 20)});

            Match[(int)Engine.IPB] += SearchForExpressions(Html, new SearchExpression[] {
            new SearchExpression("Powered By IP.Board", 20),
            new SearchExpression("Powered By IP Board", 20),
            new SearchExpression("Powered By IPB", 20),
            new SearchExpression("IP.Board", 10),
            new SearchExpression("IPBoard", 10),
            new SearchExpression("IPB", 10),
            new SearchExpression("http://www.invisionboard.com", 10)});


            Engine PossibleEngine = Engine.Unknown;
            for (int i = 0; i < Match.Length; i++)
            {
                if (Match[i] > Match[(int)PossibleEngine])
                    PossibleEngine = (Engine)i;
            }
            return PossibleEngine;
        }

        #endregion

        public class TaskBaseProperties
        {
            public Engine Engine;
            public string ForumMainPage;
            public bool UseLocalAccount;

            public struct AccountData
            {
                public byte[] username;
                public byte[] password;
                [XmlIgnore]
                public string Username
                {
                    get { return username != null ? Decrypt(username) : String.Empty; }
                    set { username = value != null ? Encrypt(value) : null; }
                }
                [XmlIgnore]
                public string Password
                {
                    get { return password != null ? Decrypt(password) : String.Empty; }
                    set { password = value != null ? Encrypt(value) : null; }
                }
                private static AesCryptoServiceProvider AES = new AesCryptoServiceProvider()
                {
                    Key = new byte[256 / 8] { 240, 119, 82, 224, 93, 215, 250, 43, 78, 192, 95, 229, 166, 27,                  
                    4, 105, 40, 251, 211, 19, 190, 77, 207, 34, 116, 39, 244, 211, 54, 212, 5, 205 },
                    IV = new byte[128 / 8] { 58, 41, 152, 142, 81, 7, 185, 181, 153, 139, 240, 86, 160, 125, 97, 233 }
                };
                public static byte[] Encrypt(string source)
                {
                    byte[] raw = Encoding.UTF8.GetBytes(source);
                    return AES.CreateEncryptor().TransformFinalBlock(raw, 0, raw.Length);
                }
                public static string Decrypt(byte[] encrypted)
                {
                    return Encoding.UTF8.GetString(AES.CreateDecryptor().
                        TransformFinalBlock(encrypted, 0, encrypted.Length));
                }
            }
            public AccountData Account;

            public List<String> PreProcessingScripts;
            public TaskBaseProperties()
            {
                PreProcessingScripts = new List<string>();
            }
            public TaskBaseProperties(TaskBaseProperties Data)
            {
                Engine = Data.Engine;
                ForumMainPage = Data.ForumMainPage;
                UseLocalAccount = Data.UseLocalAccount;
                Account.Username = Data.Account.Username;
                Account.Password = Data.Account.Password;
                PreProcessingScripts = new List<string>(Data.PreProcessingScripts);
            }
        }
        [XmlIgnore]
        protected List<KeyValuePair<object, string>> HttpLog;
        [XmlIgnore]
        public Exception Error;
        [XmlIgnore]
        [NonSerialized]
        public Task<Exception> Activity;
        public TaskBaseProperties Properties = new TaskBaseProperties();
        [XmlIgnore]
        public TaskBaseProperties.AccountData AccountToUse;
        public TimeSpan RequestTimeout = new TimeSpan(0, 0, 20);
        protected List<string> Log;

        [XmlIgnore]
        public Progress<int> Progress = new Progress<int>();
   

        protected string status;
        public string StatusMessage
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    Log.Add(status);
                    if (StatusUpdate != null)
                    StatusUpdate(status);
                }
            }
        }
        [XmlIgnore]
        public Action<string> StatusUpdate;
     protected struct ForumRunProgress
        {
            int login, scripts, post, postcount;
            [XmlIgnore]
            public IProgress<int> Progress;
            public int Login { get { return login; } set { login = value; Progress.Report(Average); } }
            public int Scripts { get { return scripts; } set { scripts = value; Progress.Report(Average); } }
            public int Post { get { return post; } set { post = value; Progress.Report(Average); } }
            public int PostCount { get { return postcount; } set { postcount = value; } }
            public int Average
            {
                get { return Math.Min(561, login + scripts / 5 + post); }
                set { Login = value / 3; Scripts = value / 3; Post = value / 3; PostCount = 1; }
            }
        }
        protected ForumRunProgress progress;

        //  public int[] Progress;
        public CancellationTokenSource Cancel;
        public static CaptchaForm CaptchaForm = new CaptchaForm();
        public Forum()
        {
            progress.Progress = Progress;
            Reset();
        }

        public static Forum New(Engine Engine)
        {
            switch (Engine)
            {
                case Engine.SMF: return new ForumSMF();
                case Engine.vBulletin: return new ForumvBulletin();
                case Engine.IPB: return new ForumIPB();
                case Engine.Universal: return new ForumUniversal();
                case Engine.AnyForum: return new ForumAny();
                default: return null;
            }
        }

        protected Task<bool> WaitFor(AutoResetEvent waitHandle)
        {
            return WaitFor(waitHandle, Timeout.InfiniteTimeSpan);
        }

        protected Task<bool> WaitFor(AutoResetEvent waitHandle, TimeSpan Timeout)
        {
            var tcs = new TaskCompletionSource<bool>();
            Cancel.Token.Register(() => tcs.TrySetResult(false));
            var CancelCopy = Cancel; // Avoid changing Cancel meanwile we are waiting

            // Registering callback to wait till WaitHandle changes its state
            ThreadPool.RegisterWaitForSingleObject(
                waitObject: waitHandle,
                callBack: (o, timeout) =>
                {

                    if (CancelCopy.IsCancellationRequested)
                        // If main task is cancelled give signal to next in queue immediatly
                        waitHandle.Set();
                    else
                    {
                        // Give signal to to next in queue when main task ends
                        Activity.ContinueWith((uselessvar) => waitHandle.Set());
                        WaitingForQueue = false;
                    }
                    if (timeout)
                        tcs.TrySetResult(false);
                    else tcs.TrySetResult(true);
                },
                state: null,
                timeout: Timeout,
                executeOnlyOnce: true);
            return tcs.Task;
        }

        protected async Task<HttpResponseMessage> PostAndLog(string requestUri, HttpContent content)
        {
            string StringContent = await content.ReadAsStringAsync();
            var Response = await Client.PostAsync(requestUri, content, Cancel.Token);
            content.Dispose();
            HttpLog.Add(new KeyValuePair<object, string>(Response, StringContent));
            return Response;
        }

        protected async Task<HttpResponseMessage> GetAndLog(string requestUri)
        {
            var Response = await Client.GetAsync(requestUri, Cancel.Token);
            HttpLog.Add(new KeyValuePair<object, string>(Response, String.Empty));
            return Response;
        }

        public void ShowData(string Title)
        {
            var Xml = new System.Xml.Serialization.XmlSerializer(this.GetType());
            try
            {
                using (var Text = new System.IO.StringWriter())
                {
                    Xml.Serialize(Text, this);
                    Text.WriteLine();
                    if (Error != null)
                    {
                        Text.WriteLine("Error:");
                        Text.WriteLine("Message: \n" + Error.Message);
                        Text.WriteLine("InnerException: \n" + Error.InnerException);
                        Text.WriteLine("StackTrace: \n" + Error.StackTrace);
                    }
                    Text.WriteLine();
                    if (Activity != null)
                        Text.WriteLine("Task status: " + Activity.Status.ToString());
                    Text.WriteLine();
                    Text.WriteLine("Username: " + AccountToUse.Username);
                    Text.WriteLine("Password: " + AccountToUse.Password);
                    var LogOutput = new LogOutput(HttpLog, Text.ToString());
                    LogOutput.Text = Title;
                    LogOutput.Show();
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //public byte[] GetDump(bool inludeAccount)
        //{
        //    BinaryFormatter Dumper = new BinaryFormatter();
        //    var LocalAccountBackup = Properties.Account;
        //    var CurrentAccountBackup = AccountToUse;
        //    if (!inludeAccount)
        //    {
        //        Properties.Account = new TaskBaseProperties.AccountData();
        //        LocalAccountBackup = new TaskBaseProperties.AccountData();
        //    }
        //    byte[] Result;
        //    using (var Stream = new System.IO.MemoryStream()){
        //        Dumper.Serialize(Stream, this);
        //        Result = Stream.ToArray();
        //    }
        //    Properties.Account = LocalAccountBackup;
        //    AccountToUse = CurrentAccountBackup;
        //    return Result;
        //}

        protected HttpClient Client;

        public virtual void Reset()
        {

            Log = new List<string>();
            StatusMessage ="Остановлено";
            HttpLog = new List<KeyValuePair<object, string>>();
            Error = null;
            //      Activity = null;
            progress.Average = 0;
            Cancel = new CancellationTokenSource();
            WaitingForQueue = true;

            // Recreating client
            if (Client != null) Client.Dispose();
            Client = new HttpClient();
            Client.Timeout = RequestTimeout;
        }

        protected static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        protected string GetBetweenStrAfterStr(string Html, string After, string Beg, string End)
        {
            int b = Html.IndexOf(After);
            if (b < 0 || b + After.Length >= Html.Length) return "";
            b = Html.IndexOf(Beg, b + After.Length);
            if (b < 0 || b + Beg.Length >= Html.Length) return "";
            int e = Html.IndexOf(End, b + Beg.Length);
            if (e > 0)
                return Html.Substring(b + Beg.Length, e - b - Beg.Length);
            return "";
        }

        ~Forum()
        {
            if (Client != null) Client.Dispose();
        }

        protected string GetFieldValue(string Html, string Name)
        {
            return GetFieldValue(Html, Name, "value");
        }

        protected string GetFieldValue(string Html, string Name, string Attribute)
        {
            return GetFieldValue(Html, "name", Name, "value");
        }

        protected string GetFieldValue(string Html, string SearchAttr, string SearchValue, string GetAttr)
        {
            Html = Html.Replace('\'', '"');
            int b = Html.IndexOf(SearchAttr + "=\"" + SearchValue + "\"", StringComparison.OrdinalIgnoreCase);
            if (b < 0) return String.Empty;
            int TagBeg = Html.LastIndexOf("<", b, StringComparison.OrdinalIgnoreCase);
            int TagEnd = Html.IndexOf(">", b + SearchAttr.Length + SearchValue.Length + 2, StringComparison.OrdinalIgnoreCase);
            if (TagBeg < 0) TagBeg = 0;
            if (TagEnd < 0) TagEnd = Html.Length;
            string Tag = Html.Substring(TagBeg, TagEnd - TagBeg + 1);
            TagBeg = Tag.IndexOf(GetAttr + "=\"", StringComparison.OrdinalIgnoreCase);
            if (TagBeg < 0) return String.Empty;
            TagEnd = Tag.IndexOf("\"", TagBeg + GetAttr.Length + 2, StringComparison.OrdinalIgnoreCase);
            if (TagEnd < 0) return String.Empty;
            return Tag.Substring(TagBeg + GetAttr.Length + 2, TagEnd - (TagBeg + GetAttr.Length + 2));
        }


        public static string GetDomain(string Url)
        {
            string Domain = new String(Url.Replace("http://", String.Empty)
                .Replace("https://", String.Empty).TakeWhile(c => c != '/').ToArray());
            if (Domain.IndexOf('.') > 0 && Domain.IndexOf('.') < Domain.Length - 1)
                return Domain;
            return String.Empty;
        }

        public abstract Task<Exception> Login();
        public abstract Task<Exception> PostMessage(Uri TargetBoard, string Subject, string Message);

        public class ScriptData
        {
            public ScriptData(PostMessage InputData)
            {
                Input = InputData;
                Output = new List<PostMessage>();
            }
            public struct PostMessage
            {
                public string Subject;
                public string Message;
                public PostMessage(string nSubject, string nMessage)
                {
                    Subject = nSubject;
                    Message = nMessage;
                }
            }
            public PostMessage Input;
            public List<PostMessage> Output;
            public void Post(string Subject, string Message)
            {
                Output.Add(new PostMessage(Subject, Message));
            }
        }

        private static string[] References = new string[]{"System","System.IO","System.Linq","System.Data",
           "System.Xml","System.Web"};

        public static Roslyn.Scripting.Session InitScriptEngine(ScriptData ScriptData)
        {
            var ScriptEngine = new ScriptEngine();
            var Session = ScriptEngine.CreateSession(ScriptData);
            Session.AddReference(ScriptData.GetType().Assembly);
            foreach (string Reference in References)
            {
                Session.AddReference(Reference);
                Session.ImportNamespace(Reference);
            }
            Session.ImportNamespace("System.Text");
            Session.ImportNamespace("System.Collections.Generic");
            Session.ImportNamespace("System.Security.Cryptography");
            return Session;
        }

        private ScriptData CurrentScriptData;

        //public Exception ExecuteScripts()
        //{
        //    Console.WriteLine("Script start");
        //    try
        //    {
        //        var Session = InitScriptEngine(CurrentScriptData);
        //        for (int i = 0; i < Properties.PreProcessingScripts.Count; i++)
        //        {
        //            Session.Execute(System.IO.File.ReadAllText(MainForm.GetScriptPath(Properties.PreProcessingScripts[i])));
        //            if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return e;
        //    }
        //    return null;
        //    Console.WriteLine("Script end");
        //}

        public Task<Exception> Run(Uri TargetBoard, string Subject, string Message)
        {
            return Task.Run<Exception>(
            async () =>
            {
                // Clearing
                this.Reset();

                // Wait untill "Activity" will be assigned with this task,
                // sometimes it happens later then this task begins executing
                while (Activity == null) Task.Delay(100).Wait();

                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                WaitingForQueue = false;
                if (Properties.UseLocalAccount) AccountToUse = Properties.Account;
                // Async Magic. Wait for domain free and then run login operation 
                Task<Exception> LoginProcess = null;
                Task WaitingDomain = WaitOrAdd(GetDomain(Properties.ForumMainPage)).
                    ContinueWith((uselessvar) => { LoginProcess = Login(); });

                // Meanwile process the scripts
               StatusMessage = "Обработка скриптов";
                CurrentScriptData = new ScriptData(new ScriptData.PostMessage(Subject, Message));
                var Session = InitScriptEngine(CurrentScriptData);
                progress.Scripts += 50;
                for (int i = 0; i < Properties.PreProcessingScripts.Count; i++)
                {
                    Session.Execute(System.IO.File.ReadAllText(MainForm.GetScriptPath(Properties.PreProcessingScripts[i])));
                    progress.Scripts += (byte)(205 / Properties.PreProcessingScripts.Count);
                    if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                }
                progress.Scripts = 255;

                // Waiting for login end
                if (LoginProcess == null)
                {
                    WaitingForQueue = true;
                   StatusMessage = "Авторизация: В очереди";
                }
                await WaitingDomain;
                WaitingForQueue = false;
                if (await LoginProcess != null) return LoginProcess.Result;
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();

                // Post messages
                progress.PostCount = CurrentScriptData.Output.Count;
                for (int i = 0; i < CurrentScriptData.Output.Count; i++)
                {
                    Task<Exception> PostProcess = PostMessage(TargetBoard, CurrentScriptData.Output[i].Subject, CurrentScriptData.Output[i].Message);
                    if (await PostProcess != null) return PostProcess.Result;
                    await Task.Delay(1000);
                    if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                }
                progress.Post = 255;
                return null;
            });
        }

    }

}
