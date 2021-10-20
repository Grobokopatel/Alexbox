using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlexBox
{
    public class TCPMessageSender : IMessageSender
    {
        public event EventHandler<MessageRecievedArgs> MessageRecieved;

        private TcpListener server;
        public int Port
        {
            get => ((IPEndPoint)server.LocalEndpoint).Port;
        }

        public IPAddress LocalIPAddress
        {
            get;
            private set;
        }

        public TCPMessageSender()
        {
            IsRunning = true;
            LocalIPAddress = IPAddress.Any;
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public async void StartRecievingMessagesAsync()
        {
            server = new TcpListener(LocalIPAddress, 0);
            // Запуск в работу
            server.Start();
            // Бесконечный цикл
            while (IsRunning)
            {
                try
                {
                    // Подключение клиента
                    var client = await server.AcceptTcpClientAsync();
                    var stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead && IsRunning)
                        {
                            var buffer = new byte[1024];
                            var clientMessage = new List<byte>();
                            do
                            {
                                var numberOfBytesReaded = stream.Read(buffer, 0, buffer.Length);
                                clientMessage.AddRange(buffer.Take(numberOfBytesReaded));
                            }
                            while (stream.DataAvailable);
                            var message = clientMessage.ToArray();
                            var args = new MessageRecievedArgs(message);
                            MessageRecieved(this, args);
                            var responseData = new BinaryFormatter().Serialize(args.Result);
                            stream.Write(responseData, 0, responseData.Length);
                        }
                    }
                    finally
                    {
                        stream.Close();
                        client.Close();
                    }
                }
                catch
                {
                    server.Stop();
                    break;
                }
            }

            if (!IsRunning)
            {
                server.Stop();
            }
        }

        public async Task<byte[]> SendAsync(string address, int port, byte[] data)
        {
            // Инициализация
            var client = new TcpClient(address, port);
            var stream = client.GetStream();
            try
            {
                // Отправка сообщения
                await stream.WriteAsync(data, 0, data.Length);
                // Получение ответа
                var buffer = new byte[256];
                var serverResponse = new List<byte>();
                do
                {
                    var numberOfBytesReaded = await stream.ReadAsync(buffer, 0, buffer.Length);
                    serverResponse.AddRange(buffer.Take(numberOfBytesReaded));
                }
                while (stream.DataAvailable);

                return serverResponse.ToArray();
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
