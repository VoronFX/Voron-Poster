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

namespace Voron_Poster
{
    public abstract class Forum
    {

        #region Detect Forum Engine
        public enum ForumEngine { Unknown, SMF }

        struct FESearchExpression
        {
            public string SearchExpression;
            public int Value;

            public FESearchExpression(string newSearchExpression, int newValue)
            {
                SearchExpression = newSearchExpression;
                Value = newValue;
            }
        }

        private static int SearchForExpressions(string Html, FESearchExpression[] Expressions)
        {
            int ResultMatch = 0;
            foreach (FESearchExpression CurrExpression in Expressions)
            {
                if (Html.IndexOf(CurrExpression.SearchExpression.ToLower()) >= 0)
                    ResultMatch += CurrExpression.Value;
            }
            return ResultMatch;
        }

        public static async Task<ForumEngine> DetectForumEngine(string Url, HttpClient Client, CancellationToken Cancel)
        {
            int[] Match = new int[Enum.GetNames(typeof(ForumEngine))
                     .Length];
            string Html = await (await Client.GetAsync(Url, Cancel)).Content.ReadAsStringAsync();
            Match[(int)ForumEngine.Unknown] = 9;

            Match[(int)ForumEngine.SMF] += SearchForExpressions(Html, new FESearchExpression[] {
            new FESearchExpression("Powered by SMF", 20),
            new FESearchExpression("Simple Machines Forum", 20),
            new FESearchExpression("http://www.simplemachines.org/about/copyright.php", 10),
            new FESearchExpression("http://www.simplemachines.org/", 10),
            new FESearchExpression("Simple Machines", 10)});

            ForumEngine PossibleEngine = ForumEngine.Unknown;
            for (int i = 0; i < Match.Length; i++)
            {
                if (Match[i] > Match[(int)PossibleEngine])
                    PossibleEngine = (ForumEngine)i;
            }
            return PossibleEngine;
        }

        #endregion
        public class TaskBaseProperties
        {
            public ForumEngine Engine;
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

        public Exception Error;
        public Task Task;
        public TaskBaseProperties Properties = new TaskBaseProperties();
        public TimeSpan RequestTimeout = new TimeSpan(0, 0, 10);
        public List<string> Log;
        public int Progress;
        public CancellationTokenSource Cancel;
        public static CaptchaForm CaptchaForm = new CaptchaForm();
        public Forum()
        {
            Log = new List<string>();
            Log.Add("Ожидание");
            Progress = 0;
        }

        public static Forum New(ForumEngine Engine)
        {
            switch (Engine)
            {
                case ForumEngine.SMF: return new ForumSMF();
                default: return null;
            }
        }

        protected HttpClient Client;


        ~Forum()
        {
            if (Client != null) Client.Dispose();
        }

        public abstract Task Login();
        public abstract Task PostMessage(Uri TargetBoard, string Subject, string Message);

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

        public Exception ExecuteScripts()
        {
            Console.WriteLine("Script start");
            try
            {
                var Session = InitScriptEngine(CurrentScriptData);
                for (int i = 0; i < Properties.PreProcessingScripts.Count; i++)
                {
                    Session.Execute(System.IO.File.ReadAllText(MainForm.GetScriptPath(Properties.PreProcessingScripts[i])));
                    if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                }
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
            Console.WriteLine("Script end");
        }

        public void Run(Uri TargetBoard, string Subject, string Message)
        {
            Console.WriteLine("Run start");
            try
            {
                if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                Task LoginProcess = Login();
                CurrentScriptData = new ScriptData(new ScriptData.PostMessage(Subject, Message));
              //  Task<Exception> Processing = new Task<Exception>(new Func<Exception>(ExecuteScripts), Cancel.Token);
                //Processing.Start();
               // Processing.Wait();
               // if (Processing.Result != null) throw Processing.Result;
                //  Processing.e
                var Session = InitScriptEngine(CurrentScriptData);
                for (int i = 0; i < Properties.PreProcessingScripts.Count; i++)
                {
                    Session.Execute(System.IO.File.ReadAllText(MainForm.GetScriptPath(Properties.PreProcessingScripts[i])));
                    if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                }
                LoginProcess.Wait();
                if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
               
                if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                for (int i = 0; i < CurrentScriptData.Output.Count; i++)
                {
                    PostMessage(TargetBoard, CurrentScriptData.Output[i].Subject, CurrentScriptData.Output[i].Message).Wait();
                    if (Cancel.IsCancellationRequested) throw new OperationCanceledException();
                }
            }
            catch (Exception e)
            {
                if (Cancel.IsCancellationRequested || e is OperationCanceledException)
                    lock (Log) Log.Add("Отменено");
                else
                    lock (Log) Log.Add("Ошибка: " + e.Message);
                Error = e;
            }
            Console.WriteLine("Run end");
        }

    }

}
