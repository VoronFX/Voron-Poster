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
    abstract class Forum
    {

        #region Detect Forum Engine
        enum ForumEngine { Unknown, SMF }

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

        private ForumEngine DetectForumEngine(string Html)
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

        protected HttpClient Client;
        HttpClientHandler ClientHandler;
        public int ReqTimeout;
        public List<string> Log;
        public int Progress;
        public Uri MainPage;
        protected CookieContainer Cookies;
        public CancellationTokenSource Cancel;
        public Forum()
        {
            Log = new List<string>();
            Progress = 0;
            Cancel = new CancellationTokenSource();
            Cookies = new CookieContainer();
            ClientHandler = new HttpClientHandler() { CookieContainer = Cookies };
            Client = new HttpClient(ClientHandler);
        }

        ~Forum()
        {
            ClientHandler.Dispose();
            Client.Dispose();
        }

        public abstract Task<bool> Login(string Username, string Password);
        public abstract Task<bool> PostMessage(Uri TargetBoard, string Subject, string BBText);

    }
}
