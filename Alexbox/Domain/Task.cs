namespace Alexbox.Domain
{
    public class Task
    {
        private readonly string _description;
        private readonly string[] _possibleAnswers;
        private readonly string _rightAnswer;

        public Task(string description, string[] possibleAnswers = null, string rightAnswer = null)
        {
            _description = description;
            _possibleAnswers = possibleAnswers;
            _rightAnswer = rightAnswer;
        }

    }
}