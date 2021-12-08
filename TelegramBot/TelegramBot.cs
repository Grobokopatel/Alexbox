using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
#pragma warning disable 618


namespace Alexbox
{

    public static class TelegramBot
    {
        private static TelegramBotClient _client;

        public static void Run()
        {
            _client = new TelegramBotClient("5019524624:AAHqK_HiVu77AthmnbAFTYrJrBnAhwPv6Zc");
            _client.OnMessage += BotClientOnOnMessage;
            _client.StartReceiving();
            Thread.Sleep(-1);
        }

        private static void BotClientOnOnMessage(object sender, MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            var text = e.Message.Text;
            if (text == "/start")
            {
                _client.SendTextMessageAsync(id, $"Введите идентификатор вашей комнаты. Например: 1234");
            }
            else _client.SendTextMessageAsync(id, $"You sent: {text}");
        }
    }
}