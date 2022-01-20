namespace Alexbox.Domain
{
    public class Task
    {
        public readonly string Description;
        public string FalseAnswer { get; set; }

        public Task(string description,string falseAnswer = null)
        {
            Description = description;
            FalseAnswer = falseAnswer;
        }
    }
}