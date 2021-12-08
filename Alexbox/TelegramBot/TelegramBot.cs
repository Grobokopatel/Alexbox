using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

#pragma warning disable 618


namespace Alexbox.TelegramBot
{
    public static class TelegramBot
    {
        private const string Token = "5019524624:AAHqK_HiVu77AthmnbAFTYrJrBnAhwPv6Zc";
        private static TelegramBotClient _client = new(Token);
        public static Dictionary<string, HashSet<long>> _usersById = new();

        public static void Run()
        {
            _client.StartReceiving();
            _client.OnMessage += BotClientOnOnMessage;
        }

        private static void BotClientOnOnMessage(object sender, MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            var text = e.Message.Text;
            if (e.Message.Type != MessageType.Text) _client.SendTextMessageAsync(id, "Бот распознает только текст");
            if (text == "/start")
            {
                _client.SendTextMessageAsync(id, "Введите идентификатор вашей комнаты. Например: 0352");
            }
            else if (_usersById.ContainsKey(text))
            {
                _usersById[text].Add(id);
                _client.SendTextMessageAsync(id, $"Вы успешно подключены к комнате: {text}");
            }
            else _client.SendTextMessageAsync(id, "Узнайте идентификатор комнаты у хоста");

        }

        public static string GenerateLobbyId()
        {
            var random = new Random();
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                stringBuilder.Append(random.Next(0, 9));
            }

            var result = stringBuilder.ToString();
            if (_usersById.ContainsKey(result))
                return GenerateLobbyId();
            _usersById.Add(result, new HashSet<long>());
            return stringBuilder.ToString();
        }
    }
}