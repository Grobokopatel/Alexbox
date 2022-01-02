namespace Alexbox.Domain
{
    public class Stage
    {
        public int ShowRoundResults { get; private set; } = -1;
        public string Paragraph { get; set; }
        public int TimeOutInMs { get; private set; }
        public string[] Captions { get; private set; }

        public Stage WaitForTimeout(int milliseconds)
        {
            TimeOutInMs = milliseconds;
            return this;
        }

        public Stage WithCaptions(string[] captions)
        {
            Captions = captions;
            return this;
        }

        public Stage WithResults(int roundNumber)
        {
            ShowRoundResults = roundNumber;
            return this;
        }

        public Stage WithParagraph(string paragraph)
        {
            Paragraph = paragraph;
            return this;
        }
    }
}