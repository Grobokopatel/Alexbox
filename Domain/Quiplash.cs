using System;
using System.Collections.Generic;
using System.Text;

namespace Alexbox.Domain
{
    public class Quiplash : LocalNetworkGame
    {
        public override int MinPlayers => 3;

        public override int MaxPlayers => 8;

        public Quiplash(MessageSender messageSender) : base(messageSender)
        { }
    }
}
