using System;

namespace Alexbox.Domain
{
    public class Stage
    {
        private Func<int, int, int, int> ScoreFormula { get; set; }
        public int TaskPerPlayer { get; private set; }
        public int GroupSize { get; private set; }
        public bool ShowScores { get; private set; }
        public string Paragraph { get; private set; }
        public int TimeOutInMs { get; private set; }
        public bool ShowRoundSubmits { get; private set; }
        public bool WaitForReplies { get; set; }
        public bool SendingTasks { get; set; }

        public Distribution<long, Task> Distribution { get; set; }


        public Stage WaitForTimeOutOrReplies(int milliseconds)
        {
            TimeOutInMs = milliseconds;
            return this;
        }

        public Stage WithRoundSubmits()
        {
            ShowRoundSubmits = true;
            return this;
        }
        
        public Stage WithParagraph(string paragraph)
        {
            Paragraph = paragraph;
            return this;
        }


        public Stage WithSendingTasks(int taskPerPlayer, int groupSize)
        {
            SendingTasks = true;
            TaskPerPlayer = taskPerPlayer;
            GroupSize = groupSize;
            return this;
        }

        public Stage WithScoreCounting(Func<int, int, int, int> formula)
        {
            ScoreFormula = formula;
            return this;
        }
    }
}