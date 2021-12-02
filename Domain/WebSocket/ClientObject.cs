using Alexbox.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alexbox.Infrastructure
{
    public class ClientObject
    {
        public Socket client;
        public Guid guid;

        public ClientObject(Socket client, Guid guid)
        {
            this.client = client;
            this.guid = guid;
        }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public async void SendAsync<T>(string command, T message)
        {
            var socketMessage = new SocketMessage
            {
                Command = command,
                Body = JsonSerializer.Serialize(message)
            };

            var buffer = new ArraySegment<byte>(JsonSerializer.SerializeToUtf8Bytes(socketMessage));

            await client.SendAsync(buffer, SocketFlags.None);
        }

        public void StartProcessing()
        {
            byte[] data = new byte[256]; // буфер для получаемых данных
            var message = new List<byte>();
            while (true)
            {
                do
                {
                    var bytes = client.Receive(data);
                    message.AddRange(data.Take(bytes));
                }
                while (client.Available > 0);

                MessageRecieved(this, new MessageRecievedEventArgs(message.ToArray()));
                message.Clear();
            }
        }

    }
}
