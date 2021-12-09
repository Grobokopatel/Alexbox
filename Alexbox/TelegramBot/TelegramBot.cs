using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#pragma warning disable 618


namespace Alexbox.TelegramBot
{
    public static class TelegramBot
    {
        private const string Token = "5019524624:AAHqK_HiVu77AthmnbAFTYrJrBnAhwPv6Zc";
        private static readonly TelegramBotClient Client = new(Token);
        private static readonly Dictionary<string, IList<User>> UsersById = new();

        public static void Run()
        {
            Client.StartReceiving();
            Client.OnMessage += BotClientOnOnMessage;
        }

        private static void BotClientOnOnMessage(object sender, MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            var user = e.Message.From;
            var text = e.Message.Text;
            if (e.Message.Type != MessageType.Text) Client.SendTextMessageAsync(id, "Бот распознает только текст");
            if (text == "/start")
            {
                Client.SendTextMessageAsync(id, "Введите идентификатор вашей комнаты. Например: 0352");
            }
            else if (UsersById.ContainsKey(text))
            {
                UsersById[text].Add(user);
                Client.SendTextMessageAsync(id, $"Вы успешно подключены к комнате: {text}");
            }
            else Client.SendTextMessageAsync(id, "Узнайте идентификатор комнаты у хоста");
        }

        public static IList<User> GetUsersByLobbyId(string lobbyId) =>
            UsersById.ContainsKey(lobbyId) ? UsersById[lobbyId] : new List<User>();

        public static void DeleteLobby(string id) => UsersById.Remove(id);

        public static void SendMessageToUser(long id, string message) => Client.SendTextMessageAsync(id, message);

        public static string GenerateLobbyId()
        {
            var random = new Random();
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                stringBuilder.Append(random.Next(0, 9));
            }

            var result = stringBuilder.ToString();
            if (UsersById.ContainsKey(result))
                return GenerateLobbyId();
            UsersById.Add(result, new List<User>());
            return stringBuilder.ToString();
        }
    }
}