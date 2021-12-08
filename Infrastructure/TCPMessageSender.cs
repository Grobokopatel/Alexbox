using Alexbox.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Alexbox.Infrastructure
{
    public class TCPMessageSender : MessageSender
    {
        private TcpListener server;
        public override int Port
        {
            get => ((IPEndPoint)server.LocalEndpoint).Port;
        }

        public IPAddress LocalIPAddress
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get;
            set;
        } = true;

        public TCPMessageSender(IFormatter formatter) : base(formatter)
        {
            LocalIPAddress = IPAddress.Any;
        }

        public override event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public override async void StartReceivingMessagesAsync()
        {
            server = new TcpListener(LocalIPAddress, 0);
            // Запуск в работу
            server.Start();

            try
            {
                // Бесконечный цикл
                while (IsRunning)
                {
                    // Подключение клиента
                    using (var client = await server.AcceptTcpClientAsync())
                    {
                        using (var stream = client.GetStream())
                        {
                            // Обмен данными
                            if (stream.CanRead && IsRunning)
                            {
                                var buffer = new byte[1024];
                                var clientMessage = new List<byte>();
                                do
                                {
                                    var numberOfBytesReaded = await stream.ReadAsync(buffer, 0, buffer.Length);
                                    clientMessage.AddRange(buffer.Take(numberOfBytesReaded));
                                }
                                while (stream.DataAvailable);

                                var args = new MessageRecievedEventArgs(clientMessage.ToArray());
                                MessageRecieved(this, args);
                                var responseData = Formatter.Serialize(args.Result);
                                await stream.WriteAsync(responseData, 0, responseData.Length);
                            }
                        }
                    }
                }
            }
            catch
            {
                IsRunning = false;
            }
            finally
            {
                server.Stop();
            }
        }

        public override async Task<byte[]> SendAsync(string address, int port, byte[] data)
        {
            // Инициализация
            using (var client = new TcpClient(address, port))
            {
                using (var stream = client.GetStream())
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
            }
        }
    }
}
