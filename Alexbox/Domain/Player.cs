using System.Collections.Generic;

namespace Alexbox.Domain
{
    public class Player : Entity
    {
        // Список ответов игроков на задание
        private readonly List<Dictionary<Task, string>> _submissions = new();

        public double Score { get; private set; }
        public Task CurrentTask { get; set; }
        public string Name { get; }

        public Player(string name, long id)
        {
            Name = name;
            Id = id;
        }

        public string GetSubmission(int round,Task task)
        {
            return _submissions[round][task];
        }

        public void AddSubmission(int round,Task task,string submission)
        {
            if (_submissions.Count == round) _submissions.Add(new Dictionary<Task, string>());
            _submissions[round][task] = submission;
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

        private void AddDeltaPoints(double delta)
        {
            Score += delta;
        }
    }
}