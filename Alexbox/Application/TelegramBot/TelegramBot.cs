using System.IO;
using System.Linq;
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
        public static readonly CustomGame CurrentGame = new(2, 8, "CustomGame");

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
            if (CurrentGame.GameStatus == GameStatus.WaitingForPlayers)
            {
                if (!CurrentGame._players.Contains(possiblePlayer))
                {
                    if (CurrentGame._players.Count < CurrentGame._maxPlayers)
                    {
                        CurrentGame._players.Add(possiblePlayer);
                        SendMessageToUser(id, "Вы успешно были добавлены в игру");
                    }
                    else
                    {
                        CurrentGame._viewers.Add(new Viewer(id, e.Message.From.FirstName));
                        SendMessageToUser(id, "Вы были добавлены как зритель");
                    }
                }
                else SendMessageToUser(id, "Вы уже были добавлены!");
            }

            if (CurrentGame.GameStatus == GameStatus.WaitingForReplies)
            {
                if (!CurrentGame._players.Contains(possiblePlayer))
                    SendMessageToUser(id, "ЛОЛ, Ты зритель. Жди!");
                else
                {
                    var isPlayer = false;
                    foreach (var player in CurrentGame._players.Where(player => player.Equals(possiblePlayer)))
                    {
                        player.AddSubmission(text);
                        isPlayer = true;
                        break;
                    }

                    if (!isPlayer && !CurrentGame._viewers.Contains(new Viewer(id, e.Message.From.FirstName)))
                        CurrentGame._viewers.Add(new Viewer(id, e.Message.From.FirstName));
                }
            }

            if (CurrentGame.GameStatus == GameStatus.Voting)
            {
            }
        }

        private static void SendMessageToUser(long id, string message) => Client.SendTextMessageAsync(id, message);
    }
}