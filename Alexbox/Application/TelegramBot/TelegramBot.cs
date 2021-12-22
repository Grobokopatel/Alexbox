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
        private static readonly string Token = new StreamReader("token.token").ReadLine();
        private static readonly TelegramBotClient Client = new(Token);
        public static CustomGame CurrentGame { get; } = new(3, 8, "CustomGame");

        public static void Run()
        {
            Client.StartReceiving();
            Client.OnMessage += BotClientOnMessage;
            Client.OnCallbackQuery += BotClientOnCallbackQuery;
        }

        private static void BotClientOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var id = e.CallbackQuery.From.Id;
            CurrentGame.Players[id].AddSubmission(e.CallbackQuery.Data);
        }
        private static void BotClientOnMessage(object sender, MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            var text = e.Message.Text;
            if (e.Message.Type != MessageType.Text)
            {
                Client.SendTextMessageAsync(id, "Бот распознает только текст");
                return;
            }

            switch (CurrentGame.GameStatus)
            {
                case GameStatus.WaitingForPlayers
                    when !CurrentGame.Players.ContainsKey(id) && !CurrentGame.Viewers.ContainsKey(id):
                {
                    if (CurrentGame.Players.Count < CurrentGame.MaxPlayers)
                    {
                        CurrentGame.Players[id] = new Player(e.Message.From.FirstName);
                        SendMessageToUser(id, "Вы успешно были добавлены в игру");
                    }
                    else
                    {
                        CurrentGame.Viewers[id] = new Viewer(e.Message.From.FirstName);
                        SendMessageToUser(id, "Вы были добавлены как зритель");
                    }

                    break;
                }
                case GameStatus.WaitingForPlayers:
                    SendMessageToUser(id, "Вы уже были добавлены!");
                    break;
                case GameStatus.WaitingForReplies when !CurrentGame.Players.ContainsKey(id):
                    CurrentGame.Players[id].AddSubmission(text);
                    SendMessageToUser(id, "Ваш ответ был записан");
                    break;
                case GameStatus.WaitingForReplies when !CurrentGame.Viewers.ContainsKey(id):
                    SendMessageToUser(id, "Вы зритель, ждите голосования");
                    break;
                case GameStatus.WaitingForReplies:
                    CurrentGame.Viewers[id] = new Viewer(e.Message.From.FirstName);
                    SendMessageToUser(id, "Вы были добавлены как зритель");
                    break;
                case GameStatus.Voting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static InlineKeyboardMarkup CreateButtons(IEnumerable<string> textButtons)
        {
            return new InlineKeyboardMarkup(textButtons
                .Select(text => new InlineKeyboardButton {Text = text, CallbackData = text})
                .Select(button => new[] {button}).ToArray());
        }

        private static void SendMessageToUser(long id, string message) => Client.SendTextMessageAsync(id, message);

        private static void SendMessageWithButtonsToUser(long id, string message, IEnumerable<string> textButtons) =>
            Client.SendTextMessageAsync(id, message, replyMarkup: CreateButtons(textButtons));
    }
}