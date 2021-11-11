using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    public static class IMessageSenderExtensions
    {
        public static async Task<byte[]> SendAsync<TObject>(this IMessageSender messageSender,
            string address, int port, TObject data)
        {
            return await messageSender.SendAsync(address, port, messageSender.Formatter.Serialize(data));
        }
    }
}
