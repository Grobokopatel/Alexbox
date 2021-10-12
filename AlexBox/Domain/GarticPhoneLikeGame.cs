using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public class GarticPhoneLikeGame : GameBase
    {
        protected IMessageSender messageSender;
        protected ISerializer serializer;
        public override int MinPlayers => 3;
        public override int MaxPlayers => 8;

        protected Dictionary<Player, string> messages = new Dictionary<Player, string>();

        protected void HandleMessage(object sender, byte[] args)
        {
            var message = serializer.Deserialize<PlayerMessageArgs>(args);
            OnPlayerSubmit(this, message);
        }

        protected void HandleSubmit(object sender, PlayerMessageArgs args)
        {
            if(!messages.ContainsKey(args.Player))
            {
                messages[args.Player] += args.Message;
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
            this.messageSender = messageSender;

            messageSender.StartRecievingMessages();
            messageSender.MessageRecieved += HandleMessage;
            PlayerSubmit += HandleSubmit;
        }
    }
}
