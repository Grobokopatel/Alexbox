using System.Collections.Generic;
using System.IO;
using Alexbox.Domain;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Game = Alexbox.Domain.Game;

#pragma warning disable 618


namespace Alexbox.TelegramBot
{
    public static class TelegramBot
    {
        private static readonly string Token = new StreamReader("token.token").ReadLine();
        private static readonly TelegramBotClient Client = new(Token);
        private static readonly Game CurrentGame = new(new List<Player>()); 
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
            if (!CurrentGame._players.Contains(new Player(id,user.FirstName)))
            {
                CurrentGame._players.Add(new Player(id,user.FirstName));
                SendMessageToUser(id, "Вы успешно были добавлены в игру");
            }
            else SendMessageToUser(id,"Вы уже были добавлены!");
        }

        private static void SendMessageToUser(long id, string message) => Client.SendTextMessageAsync(id, message);
    }
}