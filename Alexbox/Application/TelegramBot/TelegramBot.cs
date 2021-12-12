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
            var possiblePlayer = new Player(e.Message.Chat.Id, e.Message.From.FirstName);
            var text = e.Message.Text;
            if (e.Message.Type != MessageType.Text) Client.SendTextMessageAsync(id, "Бот распознает только текст");
            if (_currentGame._gameStatus == GameStatus.WaitingForPlayers)
            {
                if (!_currentGame._players.Contains(possiblePlayer))
                {
                    if (_currentGame._players.Count == _currentGame._maxPlayers)
                    {
                        _currentGame._players.Add(possiblePlayer);
                        SendMessageToUser(id, "Вы успешно были добавлены в игру");
                    }
                    else
                    {
                        _currentGame._viewers.Add(new Viewer(id, e.Message.From.FirstName));
                        SendMessageToUser(id, "Вы были добавлены как зритель");
                    }
                }
                else SendMessageToUser(id, "Вы уже были добавлены!");
            }

            if (_currentGame._gameStatus == GameStatus.WaitingForReplies)
            {
                if (!_currentGame._players.Contains(possiblePlayer))
                    SendMessageToUser(id, "ЛОЛ, Ты зритель. Жди!");
                else
                {
                    var isPlayer = false;
                    foreach (var player in _currentGame._players)
                    {
                        if (player.Equals(possiblePlayer))
                        {
                            player.AddSubmission(text);
                            isPlayer = true;
                            break;
                        }
                    }

                    if (!isPlayer && !_currentGame._viewers.Contains(new Viewer(id, e.Message.From.FirstName)))
                        _currentGame._viewers.Add(new Viewer(id, e.Message.From.FirstName));
                }
            }

            if (_currentGame._gameStatus == GameStatus.Voting)
            {
            }
        }

        private static void SendMessageToUser(long id, string message) => Client.SendTextMessageAsync(id, message);
    }
}