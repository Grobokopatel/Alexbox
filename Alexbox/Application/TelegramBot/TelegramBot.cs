using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
        private static Dictionary<long, Queue<Task>> _taskByUser;
        public static CustomGame CurrentGame { get; private set; }

        public static void Run(CustomGame currentGame)
        {
            CurrentGame = currentGame;
            Client.StartReceiving();
            Client.OnMessage += BotClientOnMessage;
            Client.OnCallbackQuery += BotClientOnCallbackQuery;
        }

        private static void BotClientOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var id = e.CallbackQuery.From.Id;
            CurrentGame.Players[id].AddSubmission(e.CallbackQuery.Data);
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

            if (CurrentGame.GameStatus == GameStatus.WaitingForPlayers &&
                (!CurrentGame.Players.ContainsKey(id) && !CurrentGame.Viewers.ContainsKey(id)))
            {
                if (CurrentGame.Players.Count < CurrentGame.MaxPlayers)
                {
                    CurrentGame.Players[id] = new Player(e.Message.From.FirstName);
                    await Client.SendTextMessageAsync(id, "Вы успешно были добавлены в игру");
                }
                else
                {
                    CurrentGame.Viewers[id] = new Viewer(e.Message.From.FirstName);
                    await Client.SendTextMessageAsync(id, "Вы были добавлены как зритель");
                }
            }
            else if (CurrentGame.GameStatus == GameStatus.WaitingForPlayers)
            {
                await Client.SendTextMessageAsync(id, "Вы уже были добавлены!");
            }
            else if (CurrentGame.GameStatus == GameStatus.WaitingForReplies && CurrentGame.Players.ContainsKey(id))
            {
                CurrentGame.Players[id].AddSubmission(text);
                await Client.SendTextMessageAsync(id, "Ваш ответ был записан");
                if (_taskByUser[id].Count > 0)
                {
                    await Client.SendTextMessageAsync(id, _taskByUser[id].Dequeue().Description);
                }
                else if (_taskByUser.Select(keyValuePair => keyValuePair.Value.Count == 0).All(t => t))
                {
                    CurrentGame.GameStatus = GameStatus.Voting;
                    AllPlayersAnswered?.Invoke();
                }
            }
            else if (CurrentGame.GameStatus == GameStatus.WaitingForReplies && CurrentGame.Viewers.ContainsKey(id))
            {
                await Client.SendTextMessageAsync(id, "Вы зритель, ждите голосования");
            }
            else if (CurrentGame.GameStatus == GameStatus.WaitingForReplies)
            {
                CurrentGame.Viewers[id] = new Viewer(e.Message.From.FirstName);
                await Client.SendTextMessageAsync(id, "Вы были добавлены как зритель");
            }
            else if (CurrentGame.GameStatus == GameStatus.Voting)
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
        

        public static async void SendMessageWithButtonsToUser(long id, string message, IEnumerable<string> textButtons) =>
            await Client.SendTextMessageAsync(id, message, replyMarkup: CreateButtons(textButtons));

        public static async void SendTasks()
        {
            _taskByUser = new Dictionary<long, Queue<Task>>();
            foreach (var (id, tasks) in CurrentGame.Distribution.Tasks)
            {
                foreach (var task in tasks)
                {
                    if (!_taskByUser.ContainsKey(id))
                    {
                        _taskByUser[id] = new Queue<Task>();
                    }
                    _taskByUser[id].Enqueue(task);
                }
            }

            foreach (var (id,tasks) in _taskByUser)
            {
                await Client.SendTextMessageAsync(id, tasks.Dequeue().Description);
            }
        }

        public static void Finish()
        {
            Client.StopReceiving();
        }
    }
}