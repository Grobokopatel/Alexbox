using System;
using System.Timers;
using System.Collections.Generic;

namespace Alexbox.Domain
{
    public sealed class CustomGame
    {
        public readonly Dictionary<long, Player> Players = new();
        public readonly Dictionary<long, Viewer> Viewers = new();
        public readonly GameStatus GameStatus;
        public readonly int MaxPlayers;
        public readonly string Name;
        private readonly int MinPlayers;
        public readonly Queue<Stage> Stages;
        private List<Task> Tasks;
        private Distribution Distribution;
        public event Action StageEnded;
        public event Action StopProgram;
        public Stage CurrentStage { get; private set; }

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
            return Players.Max(item => item.Value.Score).Value;
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

        public void Start()
        {
            StageEnded += ChangeStage;
            ChangeStage();
        }

        private void ChangeStage()
        {
            CurrentStage = Stages.Dequeue();
            if (CurrentStage.TimeOutInMs != 0)
                StartTimer();
        }

        public CustomGame WithDistribution(Distribution distribution)
        {
            Distribution = distribution;
            return this;
        }

        public CustomGame WithTaskList(List<Task> tasks)
        {
            if (tasks.Count == 0)
                throw new ArgumentException("List of task must not be empty");
            Tasks = tasks;
            return this;
        }

        private void StartTimer()
        {
            var timer = new Timer();
            timer.Interval = CurrentStage.TimeOutInMs;
            timer.Start();
            timer.Elapsed += (_, _) =>
            {
                timer.Stop();
                if (Stages.Count == 0) StopProgram?.Invoke();
                else
                {
                    StageEnded?.Invoke();
                }
            };
        }
    }
}