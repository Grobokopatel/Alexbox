using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alexbox.Infrastructure
{
    public class SignalRServices
    {
        public static void StartServer(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static async Task TryConnect(string[] args)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:9153/notification")
                .Build();

            hubConnection.On<string>("Send", message => Console.WriteLine($"Message from server: {message}"));

            await hubConnection.StartAsync();

            bool isExit = false;

            while (!isExit)
            {
                var message = Console.ReadLine();

                if (message != "exit")
                    await hubConnection.SendAsync("SendMessage", message);
                else
                    isExit = true;
            }

            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
