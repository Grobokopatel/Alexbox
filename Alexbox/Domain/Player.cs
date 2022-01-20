using System.Collections.Generic;

namespace Alexbox.Domain
{
    public class Player : Entity
    {
        // Список ответов игроков на задание
        public readonly List<Dictionary<Task, string>> Submissions = new();

        public double Score { get; set; }
        public Task CurrentTask { get; set; }
        public string Name { get; }
        public int LastRoundVotes;

        public Player(string name, long id)
        {
            Name = name;
            Id = id;
        }

        public string GetSubmission(int round, Task task)
        {
            return Submissions[round][task];
        }

        public void AddSubmission(int round, Task task, string submission)
        {
            if (Submissions.Count == round)
                Submissions.Add(new Dictionary<Task, string>());
            Submissions[round][task] = submission;
        }

        public override bool Equals(object obj)
        {
            if (obj is Player player)
            {
                return Id == player.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}