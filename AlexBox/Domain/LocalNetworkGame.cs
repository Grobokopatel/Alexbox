using Alexbox.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Alexbox.Domain
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

        public LocalNetworkGame(IMessageSender messageSender = null, IFormatter formatter = null)
        {
            MessageSender = messageSender ?? new TCPMessageSender();
            Formatter = formatter ?? new BinaryFormatter();

            MessageSender.MessageRecieved += HandleMessage;
        }

        protected void HandleMessage(object sender, MessageRecievedEventArgs args)
        {
            var data = args.Message;
            try
            {
                var message = Formatter.Deserialize<PlayerSubmitEventArgs>(data);
                InvokePlayerSubmit(this, message);
            }
            catch
            {
                try
                {
                    var message = Formatter.Deserialize<PlayerLoginEventArgs>(data);
                    TryAddPlayer(this, message);
                    args.Result = message.Result.ToString();
                }
                catch
                {
                    try
                    {
                        var name = Formatter.Deserialize<string>(data);
                        var playerLoginArgs = new PlayerLoginEventArgs(new Player(name));
                        TryAddPlayer(this, playerLoginArgs);
                        args.Result = playerLoginArgs.Result.ToString();
                    }
                    catch
                    {
                        var name = "Еблан присоединился";
                        var message = new PlayerLoginEventArgs(new Player(name));
                        TryAddPlayer(this, message);
                        args.Result = message.Result.ToString();
                    }
                }
            }
        }

        /*public void SendMessage<TMessage, TObject>(Player player, TObject obj)
        { Не помню, зачем я это написал, пусть пока здесь лежит
            var convertedMessage = converter.Serialize<TObject, TMessage>(obj);
            messageSender.Send(convertedMessage, "123.123.214");
            player.Send()
        }*/
    }
}
