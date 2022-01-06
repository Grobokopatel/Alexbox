﻿using System;

namespace Alexbox.Domain
{
    public class Stage
    {
        private Func<int, int, int, int> ScoreFormula { get; set; }
        public bool ShowRoundResults { get; private set; }
        public int TaskPerPlayer { get; private set; }
        public int GroupSize { get; private set; }
        public string Paragraph { get; private set; }
        public int TimeOutInMs { get; private set; }
        public string[] Captions { get; private set; }
        public bool WaitForVotes { get; set; }
        public bool SendingTasks { get; private set; }

        public Distribution<long, Task> Distribution { get; set; }
        

        public Stage WaitForTimeOutOrReplies(int milliseconds)
        {
            TimeOutInMs = milliseconds;
            return this;
        }

        public Stage WithCaptions(string[] captions)
        {
            Captions = captions;
            return this;
        }

        public Stage WithResults()
        {
            ShowRoundResults = true;
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

        public Stage WithScoreCounting(Func<int,int,int,int> formula)
        {
            ScoreFormula = formula;
            return this;
        }
        
    }
}