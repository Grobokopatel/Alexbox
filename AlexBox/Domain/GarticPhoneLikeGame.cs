using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public class GarticPhoneLikeGame : GameBase
    {
        public IMessageSender MessageSender
        {
            get;
        }

        protected ISerializer serializer;
        public override int MinPlayers => 3;
        public override int MaxPlayers => 8;

        protected Dictionary<Player, string> messages = new Dictionary<Player, string>();

        protected void HandleMessage(object sender, byte[] args)
        {
            try
            {
                var message = serializer.Deserialize<PlayerSubmitArgs>(args);
                InvokePlayerSubmit(this, message);
            }
            catch
            {
                try
                {
                    var message = serializer.Deserialize<PlayerLoginArgs>(args);
                    InvokePlayerLogin(this, message);
                }
                catch
                {
                    try
                    {
                        var name = serializer.Deserialize<string>(args);
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

        public GarticPhoneLikeGame(IMessageSender messageSender, ISerializer serializer)
        {
            this.serializer = serializer;
            this.MessageSender = messageSender;

            messageSender.StartRecievingMessagesAsync();
            messageSender.MessageRecieved += HandleMessage;
            PlayerSubmit += HandleSubmit;

        }

        public GarticPhoneLikeGame() : this(new TCPMessageSender(), new BinaryFormatterSerializer())
        { }
    }
}
