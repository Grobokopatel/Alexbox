using System;
using System.Collections.Generic;
using System.Linq;
namespace Alexbox.Domain
{
    public class Distribution<TMember, TTask>
    {
        private int playerAmount;
        private int tasksPerPlayer;
        private int[] players;

        public int TasksRequired
        {
            get;
        }
        //Ключ - индекс задания, значение - индексы игроков  
        public Dictionary<TTask, List<TMember>> Groups
        {
            get;
        }

        //Ключ - индекс игрока, значение - индексы заданий
        public Dictionary<TMember, List<TTask>> Tasks
        {
            get
            {
                var tasks = new Dictionary<TMember, List<TTask>>();

                foreach (var (task, members) in Groups)
                {
                    foreach (var member in members)
                    {
                        if (tasks.TryGetValue(member, out var taskList))
                        {
                            taskList.Add(task);
                        }
                        else
                        {
                            tasks.Add(member, new List<TTask> { task });
                        }
                    }
                }

                return tasks;
            }
        }

        public static int GetNumberOfTasksRequired(int playerAmount, int tasksPerPlayer, int groupSize)
        {
            if (groupSize > playerAmount || tasksPerPlayer * playerAmount % groupSize != 0)
                throw new ArgumentException("Невозможно сделать такое разбиение");
            return tasksPerPlayer * playerAmount / groupSize;
        }

        public Distribution(int tasksPerPlayer, int groupSize, TMember[] members, IEnumerable<TTask> tasks)
        {
            playerAmount = members.Length;
            this.tasksPerPlayer = tasksPerPlayer;
            TasksRequired = GetNumberOfTasksRequired(playerAmount, tasksPerPlayer, groupSize);
            var tasksArray = tasks.Take(TasksRequired).ToArray();
            if (tasksArray.Length < TasksRequired)
                throw new ArgumentException("Заданий меньше, чем нужно");
            players = Enumerable.Repeat(tasksPerPlayer, playerAmount).ToArray();

            Groups = new Dictionary<TTask, List<TMember>>();

            var start = new Random().Next(playerAmount);
            var j = 0;
            for (var i = 0; i < TasksRequired; ++i)
            {
                var group = new List<TMember>();

                for (; group.Count != groupSize; j = (j + 1) % playerAmount)
                {
                    var k = (j + start) % playerAmount;
                    if (players[k] > 0)
                    {
                        --players[k];
                        group.Add(members[k]);
                    }
                }
                --j;
                Groups.Add(tasksArray[i], group);
            }
        }
    }

    public class Distribution : Distribution<int, int>
    {
        public Distribution(int playerNumber, int tasksPerPlayer, int groupSize) 
            : base(tasksPerPlayer,
                  groupSize,
                  Enumerable.Range(0, playerNumber).ToArray(),
                  For(0, 1, (i) => true))
        { }

        private static IEnumerable<int> For(int start, int step, Func<int,bool> @while)
        {
            for (var i = start; @while(i); i += step)
            {
                yield return i;
            }
        }
    }
}