using System.Collections.Generic;

namespace Alexbox.Domain
{
    public class Player : Entity
    {
        //Ключ - номер раунда, значение - список сабмитов
        private readonly Dictionary<int, List<string>> _submissions = new();

        public double Score
        {
            get;
            private set;
        }

        private string Name
        {
            get;
        }

        public Player(long id,string name)
        {
            Id = id;
            Name = name;
        }

        public IReadOnlyList<string> GetSubmissions(int round = 0)
        {
            return _submissions[round];
        }

        public void AddSubmission(string submission, int round = 0)
        {
            if(_submissions.TryGetValue(round, out var submissionList))
            {
                submissionList.Add(submission);
            }
            else
            {
                
                _submissions.Add(round, new List<string>());
                _submissions[round].Add(submission);
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
