using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlexBox
{
    public class TCPMessageSender : IMessageSender
    {
        public event EventHandler<byte[]> MessageRecieved;

        public async void StartRecievingMessagesAsync()
        {
            var localAddr = IPAddress.Parse("127.0.0.1");
            var port = 8888;
            var server = new TcpListener(localAddr, port);
            // Запуск в работу
            server.Start();
            // Бесконечный цикл
            while (true)
            {
                try
                {
                    // Подключение клиента
                    var client = await server.AcceptTcpClientAsync();
                    var stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead)
                        {
                            var myReadBuffer = new byte[1024];
                            var myCompleteMessage = new List<byte>();
                            var numberOfBytesRead = 0;
                            do
                            {
                                numberOfBytesRead = await stream.ReadAsync(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AddRange(myReadBuffer);
                            }
                            while (stream.DataAvailable);
                            MessageRecieved(this, myCompleteMessage.ToArray());
                            var responseData = Encoding.UTF8.GetBytes("УСПЕШНО!");
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
                var responseData = string.Empty;
                int numberOfBytesRead = 0;
                do
                {
                    numberOfBytesRead = await stream.ReadAsync(readingData, 0, readingData.Length);
                }
                while (stream.DataAvailable);
                return readingData;
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
