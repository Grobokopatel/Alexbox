using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    public class Distribution
    {
        private int playersNumber;
        private int tasksPerPlayer;
        public int TasksAmount
        {
            get;
        }
        private int[] players;

        public int[][] Groups
        {
            get;
        }

        public Distribution(int playerNumber, int tasksPerPlayer, int groupSize)
        {
            if (groupSize > playerNumber || tasksPerPlayer * playerNumber % groupSize != 0)
                throw new ArgumentException("Невозможно сделать такое разбиение");
            TasksAmount = tasksPerPlayer * playerNumber / groupSize;
            this.playersNumber = playerNumber;
            this.tasksPerPlayer = tasksPerPlayer;

            players = Enumerable.Repeat(tasksPerPlayer, playerNumber).ToArray();
            for (var i = 0; i < playerNumber; ++i)
            {
                players[i] = tasksPerPlayer;
            }
            Groups = new int[TasksAmount][];

            var start = new Random().Next(playerNumber);
            var k = 0;
            for (var i = 0; i < TasksAmount; ++i)
            {
                var group = new HashSet<int>();

                for (; group.Count != groupSize; k = (k + 1) % playerNumber)
                {
                    var h = (k + start) % playerNumber;
                    if (players[h] > 0)
                    {
                        --players[h];
                        group.Add(h);
                    }
                }
                --k;
                Groups[i] = group.ToArray();
                group.Clear();
            }
        }
    }
}
