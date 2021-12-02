using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alexbox.Infrastructure
{
    public class SocketMessage
    {
        public string Command { get; set; }
        public string Body { get; set; }
    }
}
