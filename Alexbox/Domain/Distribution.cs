using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    /*public class Distribution<TMember, TTask>
    {
        public class Group
        {
            public TTask Task { get; set; }
            public List<TMember> GroupMembers;
            public Group(TTask value, List<TMember> groupMembers) => (Task, GroupMembers) = (value, groupMembers);
        }

        public class Property
        {
            public TMember GroupMember { get; set; }
            public List<TTask> Tasks { get; set; }
            public Property(TMember groupMember, List<TTask> values) => (GroupMember, Tasks) = (groupMember, values);
        }

        private int playerAmount;
        private int tasksPerPlayer;
        public int TasksAmount
        {
            get;
        }
        private int[] players;

        //Ключ - индекс задания, значение - индексы игроков  
        public ILookup<TTask, TMember> Groups
        {
            get;
        }

        //Ключ - индекс игрока, значение - индексы заданий
        public ILookup<TMember, TTask> Tasks
        {
            get
            {
                var tasks = new Dictionary<TMember, List<TTask>>();

                for (var i = 0; i < playerAmount; ++i)
                {
                    tasks.Add(i, new List<int>());
                    foreach (var playerId in Groups[i])
                    {
                        tasks[i].Add(playerId);
                    }
                }

                return tasks.ToLookup(kv => kv.Key, kv => kv.Value);
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

            Groups = (ILookup<TTask, TMember>)new Dictionary<TTask, List<TMember>>();

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
                Groups.Add(i, group);
                group.Clear();
            }
        }
    }*/

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
