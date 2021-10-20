using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AlexBox
{
    public abstract class LocalNetworkGame : GameBase
    {
        public IMessageSender MessageSender
        {
            get;
        }

        public IFormatter Formatter
        {
            get;
        }


        protected void HandleMessage(object sender, MessageRecievedArgs args)
        {
            var data = args.Message;
            try
            {
                var message = Formatter.Deserialize<PlayerSubmitArgs>(data);
                InvokePlayerSubmit(this, message);
            }
            catch
            {
                try
                {
                    var message = Formatter.Deserialize<PlayerLoginArgs>(data);
                    TryAddPlayer(this, message);
                    args.Result = message.Result.ToString();
                }
                catch
                {
                    try
                    {
                        var name = Formatter.Deserialize<string>(data);
                        var message = new PlayerLoginArgs(new Player(name));
                        TryAddPlayer(this, message);
                        args.Result = message.Result.ToString();
                    }
                    catch
                    {
                        var name = "Еблан присоединился";
                        var message = new PlayerLoginArgs(new Player(name));
                        TryAddPlayer(this, message);
                        args.Result = message.Result.ToString();
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

        public LocalNetworkGame(IMessageSender messageSender, IFormatter serializer)
        {
            Formatter = serializer;
            MessageSender = messageSender;

            messageSender.MessageRecieved += HandleMessage;
            messageSender.StartRecievingMessagesAsync();
            PlayerSubmit += HandleSubmit;
        }

        public LocalNetworkGame() : this(new TCPMessageSender(), new BinaryFormatter())
        { }
    }
}
