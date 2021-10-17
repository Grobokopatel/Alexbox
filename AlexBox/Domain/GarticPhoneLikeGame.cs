using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AlexBox
{
    public class GarticPhoneLikeGame : GameBase
    {
        public IMessageSender MessageSender
        {
            get;
        }

        public IFormatter Formatter
        {
            get;
        }
        public override int MinPlayers => 3;
        public override int MaxPlayers => 8;

        protected Dictionary<Player, string> messages = new Dictionary<Player, string>();

        protected void HandleMessage(object sender, byte[] args)
        {
            try
            {
                var message = Formatter.Deserialize<PlayerSubmitArgs>(args);
                InvokePlayerSubmit(this, message);
            }
            catch
            {
                try
                {
                    var message = Formatter.Deserialize<PlayerLoginArgs>(args);
                    InvokePlayerLogin(this, message);
                }
                catch
                {
                    try
                    {
                        var name = Formatter.Deserialize<string>(args);
                        var message = new PlayerLoginArgs(new Player(name));
                        InvokePlayerLogin(this, message);
                    }
                    catch
                    {
                        var name = "Еблан присоединился";
                        var message = new PlayerLoginArgs(new Player(name));
                        InvokePlayerLogin(this, message);
                    }
                }
            }
        }

        protected void HandleSubmit(object sender, PlayerSubmitArgs args)
        {
            if (!messages.ContainsKey(args.Player))
            {
                messages[args.Player] = args.Message;
            }
            else
            {
                messages[args.Player] += "===" + args.Message;
            }
        }

        public void SendMessage<TMessage, TObject>(Player player, TObject obj)
        {
            //var convertedMessage = converter.Serialize<TObject, TMessage>(obj);
            //messageSender.Send(convertedMessage, "123.123.214");
            //player.Send()
        }

        public GarticPhoneLikeGame(IMessageSender messageSender, IFormatter serializer)
        {
            Formatter = serializer;
            MessageSender = messageSender;

            messageSender.MessageRecieved += HandleMessage;
            messageSender.StartRecievingMessagesAsync();
            PlayerSubmit += HandleSubmit;
        }

        public GarticPhoneLikeGame() : this(new TCPMessageSender(), new BinaryFormatter())
        { }
    }
}
