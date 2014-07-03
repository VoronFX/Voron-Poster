using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Net;

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

        private int SearchForExpressions(string Html, FESearchExpression[] Expressions)
        {
            int ResultMatch = 0;
            foreach (FESearchExpression CurrExpression in Expressions)
            {
                if (Html.IndexOf(CurrExpression.SearchExpression.ToLower()) >= 0)
                    ResultMatch += CurrExpression.Value;
            }
            return ResultMatch;
        }

        public ForumEngine DetectForumEngine(string Html)
        {
            int[] Match = new int[Enum.GetNames(typeof(ForumEngine))
                     .Length];
            Html = Html.ToLower();
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
        public struct TaskBaseProperties
        {
            public ForumEngine Engine;
            public string ForumMainPage;
            public bool UseGlobalAccount;
            public string Username;
            public string Password;
            public List<String> PreProcessingScripts;
            public TaskBaseProperties(TaskBaseProperties Data)
            {
                Engine = Data.Engine;
                ForumMainPage = Data.ForumMainPage;
                UseGlobalAccount = Data.UseGlobalAccount;
                Username = Data.Username;
                Password = Data.Password;
                PreProcessingScripts = new List<string>(Data.PreProcessingScripts);
            }
        }

        public Task<bool> Task;
        public TaskBaseProperties Properties = new TaskBaseProperties();
        public int RequestTimeout;
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
        public async Task<bool> Run(Uri TargetBoard, string Subject, string BBText)
        {
            try
            {
                Cancel = new CancellationTokenSource();
                await Login();
                return await PostMessage(TargetBoard, Subject, BBText);
            }
            catch (Exception e)
            {
            }
            return true;
        }
    }

}
