namespace Alexbox.Domain
{
    public class Task
    {
        public readonly string Description;
        private readonly string[] _possibleAnswers;
        private readonly string _rightAnswer;

        public Task(string description, string[] possibleAnswers = null, string rightAnswer = null)
        {
            Description = description;
            _possibleAnswers = possibleAnswers;
            _rightAnswer = rightAnswer;
        }
    }
}