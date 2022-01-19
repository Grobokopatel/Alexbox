using System;
using System.Collections.Generic;

namespace Alexbox.Domain
{
    public sealed class CustomGame
    {
        public readonly List<Player> Players = new();
        public readonly List<Viewer> Viewers = new();
        public GameStatus GameStatus { get; set; }
        public List<long> LastVoteId { get; set; }
        public Dictionary<Task,List<Player>> PlayersToVote { get; set; }
        public Stage CurrentStage { get; set; }
        public readonly int MaxPlayers;
        public readonly string Name;
        public readonly int MinPlayers;
        public readonly Queue<Stage> Stages;
        public List<Task> Tasks { get; private set; }
        public Dictionary<Task, List<Player>> PlayersBySentTask { get; set; }
        public List<Task> SentTasks { get; set; }
        public readonly int CurrentRound = 0;

        public CustomGame(int minPlayers, int maxPlayers, string name)
        {
            GameStatus = GameStatus.WaitingForPlayers;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            Name = name;
            Stages = new Queue<Stage>();
        }
        
        public CustomGame AddStage(Stage stage)
        {
            Stages.Enqueue(stage);
            return this;
        }

        public CustomGame AddStages(IEnumerable<Stage> stages)
        {
            foreach (var stage in stages)
            {
                Stages.Enqueue(stage);
            }

            return this;
        }

        public CustomGame AddStages(Func<Task, Stage> func)
        {
            foreach (var task in SentTasks)
            {
                AddStage(func(task));
            }

            return this;
        }

        public CustomGame WithTaskList(List<Task> tasks)
        {
            if (tasks.Count == 0)
                throw new ArgumentException("List of task must not be empty");
            Tasks = tasks;
            return this;
        }
    }
}