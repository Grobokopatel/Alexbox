namespace Alexbox.Domain
{
    public class Stage
    {
        
        public string Paragraph { get; set; }
        public int TimeOutInMs { get; private set; }
        public Stage WaitForTimeout(int milliseconds)
        {
            TimeOutInMs = milliseconds;
            return this;
        }

        public Stage WithParagraph(string paragraph)
        {
            Paragraph = paragraph;
            return this;
        }

    }
}