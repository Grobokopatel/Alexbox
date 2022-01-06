using System;
using Alexbox.Application.TelegramBot;

namespace Alexbox.Domain
{
    public class Stage
    {
        public bool ShowScores { get; private set; }
        public string Paragraph { get; private set; }
        public int TimeOutInMs { get; private set; }
        public int? ShowRoundSubmits { get; private set; }
        public bool WaitForReplies { get; set; }
        public bool SendingTasks { get; set; }

        public Stage WaitForTimeout(int milliseconds)
        {
            TimeOutInMs = milliseconds;
            return this;
        }
        
        public Stage WaitForTimeOutOrReplies(int milliseconds)
        {
            TimeOutInMs = milliseconds;
            WaitForReplies = true;
            return this;
        }

        public Stage WithRoundSubmits(int roundNumber = -1)
        {
            ShowRoundSubmits = roundNumber;
            return this;
        }

        public Stage WithScores()
        {
            ShowScores = true;
            return this;
        }

        public Stage WithParagraph(string paragraph)
        {
            Paragraph = paragraph;
            return this;
        }
        public Stage WithSendingTasks()
        {
            SendingTasks = true;
            return this;
        }
    }
}