using System;
using System.Collections.Generic;
using System.IO;
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
        public event EventHandler<byte[]> MessageRecieved;

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
                            var myReadBuffer = new byte[1024];
                            var myCompleteMessage = new List<byte>();
                            do
                            {
                                await stream.ReadAsync(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AddRange(myReadBuffer);
                            }
                            while (stream.DataAvailable);
                            MessageRecieved(this, myCompleteMessage.ToArray());
                            var responseData = new BinaryFormatter().Serialize("УСПЕШНО!");
                            await stream.WriteAsync(responseData, 0, responseData.Length);
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
                var readingData = new byte[256];
                var completeMessage = new List<byte>();
                var responseData = string.Empty;
                do
                {
                    await stream.ReadAsync(readingData, 0, readingData.Length);
                    completeMessage.AddRange(readingData);
                }
                while (stream.DataAvailable);

                return completeMessage.ToArray();
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
