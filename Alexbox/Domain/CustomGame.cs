﻿using System;
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
        public Stage CurrentStage { get; set; }

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
        
    }
}