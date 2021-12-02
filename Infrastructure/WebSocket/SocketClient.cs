using Alexbox.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Alexbox.Infrastructure
{
    public class SocketClient
    {
        private readonly Socket socket;

        private readonly Dictionary<string, Action<object>> handlers =
            new Dictionary<string, Action<object>>();

        public SocketClient()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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

            await socket.SendAsync(buffer, SocketFlags.None);
        }

        public void Connect(string address, int port)
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket.Connect(tcpEndPoint);

            var buffer = new byte[256];
            var message = new List<byte>();

            while(true)
            {
                do
                {
                    var size = socket.Receive(buffer);
                    message.AddRange(buffer.Take(size));
                    //data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (socket.Available > 0);

                MessageRecieved(this, new MessageRecievedEventArgs(message.ToArray()));
                message.Clear();
            }
        }

        public void StopAsync()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void Handle(byte[] buffer)
        {
            var message = JsonSerializer.Deserialize<SocketMessage>(buffer);

            if (message == null)
                return;

            if(!handlers.TryGetValue(message.Command, out var handler))
            {
                throw new ArgumentException("Обработчика с таким именем нет");
            }

            var type = handler.GetType().GetGenericArguments()[0];
            handler(JsonSerializer.Deserialize(message.Body, type));
        }

        public void On<T>(string name, Action<T> handler)
            where T : class
        {
            handlers.Add(name, param => handler((T)param));
        }
    }
}
