using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public class Player : Entity
    {
        //Ключ - номер раунда, значение - список сабмитов
        private readonly Dictionary<int, List<string>> submissions = new Dictionary<int, List<string>>();

        public double Score
        {
            get;
            private set;
        }

        public string Name
        {
            get;
        }

        public Player(string name)
        {
            Name = name;
        }

        public IReadOnlyList<string> GetSubmissions(int round = 0)
        {
            return submissions[round];
        }

        public void AddSubmission(string submission, int round = 0)
        {
            if(submissions.TryGetValue(round, out var submissionsList))
            {
                submissionsList.Add(submission);
            }
            else
            {
                submissions.Add(round, new List<string>());
                submissions[round].Add(submission);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Player player)
            {
                return Name == player.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        private void AddDeltaPoints(double delta)
        {
            Score += delta;
        }
    }
}
