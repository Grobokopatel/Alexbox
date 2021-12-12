using System.IO;
using Alexbox.Domain;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

#pragma warning disable 618


namespace Alexbox.Application.TelegramBot
{
    public static class TelegramBot
    {
        private static readonly string Token = new StreamReader("token.token").ReadLine();
        private static readonly TelegramBotClient Client = new(Token);
        private static CustomGame _currentGame;

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
            if (_currentGame._gameStatus == GameStatus.WaitingForPlayers)
            {
                if (!_currentGame._players.Contains(new Player(id, user.FirstName)))
                {
                    if (_currentGame._players.Count == _currentGame._maxPlayers)
                    {
                        _currentGame._players.Add(new Player(id, user.FirstName));
                        SendMessageToUser(id, "Вы успешно были добавлены в игру");
                    }
                    else
                    {
                        _currentGame._viewers.Add(new Viewer(id, user.FirstName));
                        SendMessageToUser(id, "Вы были добавлены как зритель");   
                    }
                }
                else SendMessageToUser(id, "Вы уже были добавлены!");
            }

            if (_currentGame._gameStatus == GameStatus.WaitingForReplies)
            {
                
            }

            if (_currentGame._gameStatus == GameStatus.Voting)
            {
                
            }
        }

        private static void SendMessageToUser(long id, string message) => Client.SendTextMessageAsync(id, message);
    }
}