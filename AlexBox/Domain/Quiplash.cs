using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public class Quiplash : JackBoxLikeGame
    {
        public override int MinPlayers => 3;

        public override int MaxPlayers => 8;
    }
}
