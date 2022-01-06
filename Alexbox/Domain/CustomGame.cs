using System;
using System.Collections.Generic;

namespace Alexbox.Domain
{
    public sealed class CustomGame
    {
        public readonly List<Player> Players = new();
        public readonly List<Viewer> Viewers = new();
        public GameStatus GameStatus { get; set; }
        public Stage CurrentStage { get; set; }
        public readonly int MaxPlayers;
        public readonly string Name;
        public readonly int MinPlayers;
        public readonly Queue<Stage> Stages;
        public List<Task> Tasks { get; private set; }
        public List<Task> SentTasks { get; set; }
        public int CurrentRound;

        public CustomGame(int minPlayers, int maxPlayers, string name)
        {
            GameStatus = GameStatus.WaitingForPlayers;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            Name = name;
            Stages = new Queue<Stage>();
        }

        public Player GetBestPlayer()
        {
            return Players.Max(player => player.Score);
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
        
        public CustomGame AddStages(Func<Task,Stage> func)
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