using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Alexbox.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Ninject;

namespace Alexbox.Infrastructure
{
    public static class Services
    {
        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            container.Bind<MessageSender>().To<TCPMessageSender>();
            container.Bind<IFormatter>().To<BinaryFormatter>();

            return container;
        }
    }
}
