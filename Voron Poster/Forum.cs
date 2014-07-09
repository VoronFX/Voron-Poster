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

namespace Voron_Poster
{
    public abstract class Forum
    {
        //struct DomainQueue
        //{

        //}

        #region Detect Engine
        public enum Engine { Unknown, SMF, vBulletin }

        struct SearchExpression
        {
            public string Expression;
            public int Value;

            public SearchExpression(string newSearchExpression, int newValue)
            {
                Expression = newSearchExpression;
                Value = newValue;
            }
        }

        private static int SearchForExpressions(string Html, SearchExpression[] Expressions)
        {
            int ResultMatch = 0;
            foreach (SearchExpression CurrExpression in Expressions)
            {
                if (Html.IndexOf(CurrExpression.Expression.ToLower()) >= 0)
                    ResultMatch += CurrExpression.Value;
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
            public string Username;
            public string Password;
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
                Username = Data.Username;
                Password = Data.Password;
                PreProcessingScripts = new List<string>(Data.PreProcessingScripts);
            }
        }
        [XmlIgnore]
        protected List<KeyValuePair<HttpResponseMessage, string>> HttpLog;
        [XmlIgnore]
        public Exception Error;
        [XmlIgnore]
        public Task<Exception> Activity;
        public TaskBaseProperties Properties = new TaskBaseProperties();
        public TimeSpan RequestTimeout = new TimeSpan(0, 0, 10);
        public List<string> Log;
        public int[] Progress;
        public CancellationTokenSource Cancel;
        public static CaptchaForm CaptchaForm = new CaptchaForm();
        public Forum()
        {
            Reset();
        }

        public static Forum New(Engine Engine)
        {
            switch (Engine)
            {
                case Engine.SMF: return new ForumSMF();
                case Engine.vBulletin: return new ForumvBulletin();
                default: return null;
            }
        }

        protected async Task<HttpResponseMessage> PostAndLog(string requestUri, HttpContent content)
        {
            string StringContent = await content.ReadAsStringAsync();
            var Response = await Client.PostAsync(requestUri, content, Cancel.Token);
            HttpLog.Add(new KeyValuePair<HttpResponseMessage, string>(Response, StringContent));
            return Response;
        }

        protected async Task<HttpResponseMessage> GetAndLog(string requestUri)
        {
            var Response = await Client.GetAsync(requestUri, Cancel.Token);
            HttpLog.Add(new KeyValuePair<HttpResponseMessage, string>(Response, String.Empty));
            return Response;
        }

        public void ShowData()
        {
            var LogOutput = new LogOutput(HttpLog);
            var Xml = new System.Xml.Serialization.XmlSerializer(this.GetType());
            try
            {
                using (var Text = new System.IO.StringWriter())
                {
                    Xml.Serialize(Text, this);
                    LogOutput.VariablesBox.Text = Text.ToString();
                }
            }
            catch (Exception)
            {
            }
            LogOutput.VariablesBox.IsReadOnly = true;
            LogOutput.Show();
        }

        [NonSerialized]
        protected HttpClient Client;

        public void Reset(){

            Log = new List<string>();
            Log.Add("Остановлено");
            HttpLog = new List<KeyValuePair<HttpResponseMessage, string>>();
            Error = null;
            Activity = null;
            Progress = new int[4] { 0, 0, 0, 1 };
            Cancel = new CancellationTokenSource();

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
            return Task.Run(async () =>
            {
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                Task<Exception> LoginProcess = Login(); // Run login operation

                // Meanwile process the scripts
                CurrentScriptData = new ScriptData(new ScriptData.PostMessage(Subject, Message));
                var Session = InitScriptEngine(CurrentScriptData);
                Progress[1] += 50;
                for (int i = 0; i < Properties.PreProcessingScripts.Count; i++)
                {
                    Session.Execute(System.IO.File.ReadAllText(MainForm.GetScriptPath(Properties.PreProcessingScripts[i])));
                    Progress[1] += (byte)(205 / Properties.PreProcessingScripts.Count);
                    if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                }
                Progress[1] = 255;

                // Waiting for login end
                if (await LoginProcess != null) return LoginProcess.Result;
                if (Cancel.IsCancellationRequested) return new OperationCanceledException();

                // Post messages
                Progress[3] = CurrentScriptData.Output.Count;
                for (int i = 0; i < CurrentScriptData.Output.Count; i++)
                {
                    Task<Exception> PostProcess = PostMessage(TargetBoard, CurrentScriptData.Output[i].Subject, CurrentScriptData.Output[i].Message);
                    if (await PostProcess != null) return PostProcess.Result;
                    if (Cancel.IsCancellationRequested) return new OperationCanceledException();
                }
                Progress[2] = 255;
                return null;
            });
        }

    }

}
