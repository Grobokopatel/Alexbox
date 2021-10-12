using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public class GarticPhoneLikeGame : GameBase
    {
        protected IMessageSender messageSender;
        protected IConverter converter;
        public override int MinPlayers => 3;

        public override int MaxPlayers => 8;

        protected Dictionary<Player, string> submissions;

        protected void HandleSubmission(object sender, PlayerSubmitArgs args)
        {
            var player = (Player)sender;
            submissions[player] += args.Submission;
        }

        public void GetSubmissions<TMessage, TObject>()
        {
            //В бесконечном цикле принимает сабмиты
            var submisson = messageSender.Recieve<TMessage>();
            var obj = converter.ToObject<TObject, TMessage>(submisson);
              OnPlayerSubmit(new object(), new PlayerSubmitArgs(new Player("tmp"), "tmp"));
        }

        public void SendMessage<TMessage, TObject>(Player player, TObject obj)
        {
            var convertedMessage = converter.ToMessage<TObject, TMessage>(obj);
            messageSender.Send(convertedMessage, "123.123.214");
            //player.Send()
        }

        public GarticPhoneLikeGame(IMessageSender messageSender, IConverter converter)
        {
            this.converter = converter;
            this.messageSender = messageSender;
            PlayerSubmit += HandleSubmission;
        }
    }
}
