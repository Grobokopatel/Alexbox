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
        public MessageSender MessageSender
        {
            get;
        }

        public LocalNetworkGame(MessageSender messageSender)
        {
            MessageSender = messageSender;

            MessageSender.MessageRecieved += HandleMessage;
        }

        protected void HandleMessage(object sender, MessageRecievedEventArgs args)
        {
            var formatter = MessageSender.Formatter;
            var data = args.Message;
            try
            {
                var message = formatter.Deserialize<PlayerSubmitEventArgs>(data);
                InvokePlayerSubmit(this, message);
            }
            catch
            {
                try
                {
                    var message = formatter.Deserialize<PlayerLoginEventArgs>(data);
                    TryAddPlayer(this, message);
                    args.Result = message.Result.ToString();
                }
                catch
                {
                    try
                    {
                        var name = formatter.Deserialize<string>(data);
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
