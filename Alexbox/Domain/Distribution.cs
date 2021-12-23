using System;
using System.Collections.Generic;
using System.Linq;

namespace Alexbox.Domain
{
    public class Distribution
    {
        private readonly int _playerAmount;
        private int _tasksPerPlayer;

        private int TasksAmount
        {
            get;
        }

        //Ключ - индекс задания, значение - индексы игроков  
        private Dictionary<int, List<int>> Groups
        {
            get;
        }

        //Ключ - индекс игрока, значение - индексы заданий
        public Dictionary<int, List<int>> GetTasks()
        {
            var tasks = new Dictionary<int, List<int>>();

            for (var i = 0; i < _playerAmount; ++i)
            {
                tasks.Add(i, new List<int>());
                foreach (var playerId in Groups[i])
                {
                    tasks[i].Add(playerId);
                }
            }

            return tasks;
        }

        public Distribution(int playerNumber, int tasksPerPlayer, int groupSize)
        {
            if (groupSize > playerNumber || tasksPerPlayer * playerNumber % groupSize != 0)
                throw new ArgumentException("Невозможно сделать такое разбиение");
            TasksAmount = tasksPerPlayer * playerNumber / groupSize;
            _playerAmount = playerNumber;
            _tasksPerPlayer = tasksPerPlayer;

            var players = Enumerable.Repeat(tasksPerPlayer, playerNumber).ToArray();
            for (var i = 0; i < playerNumber; ++i)
            {
                players[i] = tasksPerPlayer;
            }
            Groups = new Dictionary<int, List<int>>();

            var start = new Random().Next(playerNumber);
            var k = 0;
            for (var i = 0; i < TasksAmount; ++i)
            {
                var group = new List<int>();

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
                Groups.Add(i,group);
                group.Clear();
            }
        }
    }
}
