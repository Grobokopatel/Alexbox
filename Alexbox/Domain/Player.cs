using System.Collections.Generic;
using System.Linq;

namespace Alexbox.Domain
{
    public class Player : Entity
    {
        //Ключ - номер раунда, значение - список сабмитов
        private readonly Dictionary<int, List<string>> _submissions = new();

        public double Score { get; private set; }

        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }

        public IReadOnlyList<string> GetSubmissions(int round)
        {
            return _submissions[round];
        }

        public IReadOnlyList<string> GetLastSubmissions()
        {
            return _submissions[_submissions.Keys.OrderByDescending(rn => rn).First()];
        }

        public void AddSubmission(string submission, int round = 0)
        {
            if (_submissions.TryGetValue(round, out var submissionList))
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