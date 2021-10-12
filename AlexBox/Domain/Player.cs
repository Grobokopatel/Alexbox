using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public class Player
    {
        public double Score
        {
            get;
            private set;
        }

        public string Name
        {
            get;
        }

        private void AddDeltaPoints(double delta)
        {
            Score += delta;
        }

        public Player(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Player))
            {
                var player = (Player)obj;
                return Name == player.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
