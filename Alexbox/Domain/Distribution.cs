using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    public class Distribution
    {
        private int playerAmount;
        private int tasksPerPlayer;
        public int TasksAmount
        {
            get;
        }
        private int[] players;

        //Ключ - индекс задания, значение - индексы игроков  
        public Dictionary<int, List<int>> Groups
        {
            get;
        }

        //Ключ - индекс игрока, значение - индексы заданий
        public Dictionary<int, List<int>> Tasks
        {
            get
            {
                var tasks = new Dictionary<int, List<int>>();

                foreach(var (taskId, members) in Groups)
                {
                    foreach (var member in members)
                    {
                        if(tasks.TryGetValue(member, out var task))
                        {
                            task.Add(taskId);
                        }
                        else
                        {
                            tasks.Add(member, new List<int> { taskId });
                        }
                    }
                }

                return tasks;
            }
        }

        public Distribution(int playerNumber, int tasksPerPlayer, int groupSize)
        {
            if (groupSize > playerNumber || tasksPerPlayer * playerNumber % groupSize != 0)
                throw new ArgumentException("Невозможно сделать такое разбиение");
            TasksAmount = tasksPerPlayer * playerNumber / groupSize;
            this.playerAmount = playerNumber;
            this.tasksPerPlayer = tasksPerPlayer;

            players = Enumerable.Repeat(tasksPerPlayer, playerNumber).ToArray();
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
            }
        }
    }
}