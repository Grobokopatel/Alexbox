using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alexbox.Domain;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

#pragma warning disable 618


namespace Alexbox.Application.TelegramBot
{
    public static class TelegramBot
    {
        public static event Action AllPlayersAnswered;
        private static readonly string Token = new StreamReader("token.token").ReadLine();
        private static readonly TelegramBotClient Client = new(Token);
        private static Dictionary<Player, Queue<Task>> _tasksQueueByPlayer;
        public static Dictionary<Task, List<Player>> PlayersBySentTask;
        private static CustomGame _currentGame;

        public static void Run(CustomGame currentGame)
        {
            _currentGame = currentGame;
            Client.StartReceiving();
            Client.OnMessage += BotClientOnMessage;
            Client.OnCallbackQuery += BotClientOnCallbackQuery;
        }

        private static void BotClientOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var id = e.CallbackQuery.From.Id;
            var player = _currentGame.Players.First(player => player.Id == id);
            //player.AddSubmission(.CallbackQuery.Data);
        }

        private static async void BotClientOnMessage(object sender, MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            var text = e.Message.Text;
            if (e.Message.Type != MessageType.Text)
            {
                await Client.SendTextMessageAsync(id, "Бот распознает только текст");
                return;
            }

            if (_currentGame.GameStatus == GameStatus.WaitingForPlayers &&
                _currentGame.Players.Select(player => player.Id).All(pId => pId != id) &&
                _currentGame.Viewers.Select(viewer => viewer.Id).All(vId => vId != id))
            {
                if (_currentGame.Players.Count < _currentGame.MaxPlayers)
                {
                    _currentGame.Players.Add(new Player(e.Message.From.FirstName, id));
                    await Client.SendTextMessageAsync(id, "Вы успешно были добавлены в игру");
                }
                else
                {
                    _currentGame.Viewers.Add(new Viewer(e.Message.From.FirstName, id));
                    await Client.SendTextMessageAsync(id, "Вы были добавлены как зритель");
                }
            }
            else if (_currentGame.GameStatus == GameStatus.WaitingForPlayers)
            {
                await Client.SendTextMessageAsync(id, "Вы уже были добавлены!");
            }
            else if (_currentGame.GameStatus == GameStatus.WaitingForReplies &&
                     _currentGame.Players.Select(player => player.Id).Any(pId => pId == id))
            {
                var player = _currentGame.Players.First(player => player.Id == id);
                player.AddSubmission(_currentGame.CurrentRound, player.CurrentTask, text);
                await Client.SendTextMessageAsync(id, "Ваш ответ был записан");
                if (_tasksQueueByPlayer[player].Count > 0)
                {
                    var task = _tasksQueueByPlayer[player].Dequeue();
                    if(PlayersBySentTask.TryGetValue(task, out var players))
                    {
                        players.Add(player);
                    }
                    else
                    {
                        PlayersBySentTask[task] = new List<Player>();
                        PlayersBySentTask[task].Add(player);
                    }

                    await Client.SendTextMessageAsync(id, task.Description);

                }
                else if (_tasksQueueByPlayer.All(keyValuePair => keyValuePair.Value.Count == 0))
                {
                    _currentGame.GameStatus = GameStatus.Voting;
                    _currentGame.SentTasks = PlayersBySentTask.Keys.ToList();
                    AllPlayersAnswered?.Invoke();
                }
            }
            else if (_currentGame.GameStatus == GameStatus.WaitingForReplies &&
                     _currentGame.Viewers.Any(viewer => viewer.Id == id))
            {
                await Client.SendTextMessageAsync(id, "Вы зритель, ждите голосования");
            }
            else if (_currentGame.GameStatus == GameStatus.WaitingForReplies)
            {
                _currentGame.Viewers.Add(new Viewer(e.Message.From.FirstName, id));
                await Client.SendTextMessageAsync(id, "Вы были добавлены как зритель");
            }
            else if (_currentGame.GameStatus == GameStatus.Voting)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private static InlineKeyboardMarkup CreateButtons(IEnumerable<string> textButtons)
        {
            return new InlineKeyboardMarkup(textButtons
                .Select(text => new InlineKeyboardButton {Text = text, CallbackData = text})
                .Select(button => new[] {button}).ToArray());
        }


        public static async void
            SendMessageWithButtonsToUser(long id, string message, IEnumerable<string> textButtons) =>
            await Client.SendTextMessageAsync(id, message, replyMarkup: CreateButtons(textButtons));

        public static async void SendTasks()
        {
            PlayersBySentTask = new Dictionary<Task, List<Player>>();
            _tasksQueueByPlayer = new Dictionary<Player, Queue<Task>>();
            foreach (var (id, tasks) in _currentGame.CurrentStage.Distribution.Tasks)
            {
                var player = _currentGame.Players.First(player => player.Id == id);
                foreach (var task in tasks)
                {
                    if (!_tasksQueueByPlayer.ContainsKey(player))
                    {
                        _tasksQueueByPlayer[player] = new Queue<Task>();
                    }

                    _tasksQueueByPlayer[player].Enqueue(task);
                }
            }

            foreach (var (player, tasks) in _tasksQueueByPlayer)
            {
                var task = tasks.Dequeue();
                if (!PlayersBySentTask.ContainsKey(task)) PlayersBySentTask[task] = new List<Player>();
                PlayersBySentTask[task].Add(player);
                player.CurrentTask = task;
                await Client.SendTextMessageAsync(player.Id, task.Description);
            }
        }

        /*public static void SendPlayerAnswers()
        {
            foreach (var (task, usersId) in _playersBySentTask)
            {
                var answers = usersId.Select(id =>
                    CurrentGame.Players.First(player => player.Id == id).GetSubmission(task, CurrentGame.CurrentRound));
                foreach (var player in CurrentGame.Players.Where(player => usersId.All(id => player.Id != id)))
                {
                    SendMessageWithButtonsToUser(player.Id, task.Description, answers);
                }

                foreach (var viewer in CurrentGame.Viewers)
                {
                    SendMessageWithButtonsToUser(viewer.Id, task.Description, answers);
                }
                _currentGame.CurrentRound += 1;
            }
        }*/
    }
}