using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Net;
using Roslyn.Scripting.CSharp;

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

        public Task<bool> Task;
        public TaskBaseProperties Properties = new TaskBaseProperties();
        public TimeSpan RequestTimeout;
        public List<string> Log;
        public int Progress;
        public CancellationTokenSource Cancel;
        public Forum()
        {
            Log = new List<string>();
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

        public abstract Task<bool> Login();
        public abstract Task<bool> PostMessage(Uri TargetBoard, string Subject, string BBText);

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

        public Task<bool> ExecuteScripts(ScriptData ScriptData)
        {
            Task<bool> Processing = new Task<bool>(() =>
            {
                var Session = InitScriptEngine(ScriptData);
                for (int i = 0; i < Properties.PreProcessingScripts.Count; i++)
                {
                    Session.Execute(System.IO.File.ReadAllText(MainForm.GetScriptPath(Properties.PreProcessingScripts[i])));
                    if (Cancel.IsCancellationRequested) return false;
                }
                return true;
            });
            Processing.Start();
            return Processing;
        }

        public async Task<bool> Run(Uri TargetBoard, string Subject, string Message)
        {
            try
            {
                Cancel = new CancellationTokenSource();
                Task<bool> LoginProcess = Login();
                LoginProcess.Start();
                var ScriptData = new ScriptData(new ScriptData.PostMessage(Subject, Message));
                await ExecuteScripts(ScriptData);
                if (Cancel.IsCancellationRequested) return false;
                await LoginProcess;
                if (Cancel.IsCancellationRequested) return false;
                for (int i = 0; i < ScriptData.Output.Count; i++)
                {
                    await PostMessage(TargetBoard, ScriptData.Output[i].Subject, ScriptData.Output[i].Message);
                    if (Cancel.IsCancellationRequested) return false;
                }
                return true;
            }
            catch (Exception e)
            {
            }
            return true;
        }
    }

}
