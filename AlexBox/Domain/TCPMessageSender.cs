using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AlexBox.Domain
{
    public class TCPMessageSender : IMessageSender
    {
        public event EventHandler<byte[]> MessageRecieved;

        public void StartRecievingMessages()
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
                    var client = server.AcceptTcpClient();
                    var stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            var myCompleteMessage = new List<byte>();
                            int numberOfBytesRead = 0;
                            do
                            {
                                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AddRange(myReadBuffer);
                            }
                            while (stream.DataAvailable);
                            MessageRecieved(this, myCompleteMessage.ToArray());
                            var responseData = Encoding.UTF8.GetBytes("УСПЕШНО!");
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
        }

        public byte[] Send(string address, int port, byte[] data)
        {
            // Инициализация
            var client = new TcpClient(address, port);
            var stream = client.GetStream();
            try
            {
                // Отправка сообщения
                stream.Write(data, 0, data.Length);
                // Получение ответа
                var readingData = new Byte[256];
                var responseData = String.Empty;
                int numberOfBytesRead = 0;
                do
                {
                    numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
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
