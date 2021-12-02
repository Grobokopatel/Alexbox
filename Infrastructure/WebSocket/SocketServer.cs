using Alexbox.Domain;
using Alexbox.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Alexbox.Infrastructure
{
    public class SocketServer
    {
        private readonly Dictionary<Guid, ClientObject> connections = new Dictionary<Guid, ClientObject>();

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void SendAll<T>(string command, T message)
        {
            foreach (var connection in connections)
            {
                connection.Value.SendAsync(command, message);
            }
        }

        public void Send<T>(Guid[] guids, string command, T message)
        {
            foreach(var guid in guids)
            {
                Send(guid, command, message);
            }
        }

        public void Send<T>(Guid guid, string command, T message)
        {
            connections[guid].SendAsync(command, message);
        }

        public async void StartRecievingMessagesAsync()
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 0);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // связываем сокет с локальной точкой, по которой будем принимать данные
            listenSocket.Bind(ipPoint);

            // начинаем прослушивание
            listenSocket.Listen(10);

            while (true)
            {
                var client = await listenSocket.AcceptAsync();
                var id = Guid.NewGuid();
                var clientObject = new ClientObject(client, id);
                clientObject.MessageRecieved += MessageRecieved;
                connections.Add(id, clientObject);

                // создаем новый поток для обслуживания нового клиента
                var clientThread = new Thread(new ThreadStart(clientObject.StartProcessing));
                clientThread.Start();
            }
        }
    }
}
