using System;
using System.ServiceModel;
using MultiplayerClient.Extensions;
using MultiplayerContracts;
using MultiplayerServer;

namespace ConsoleHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            var shell = new Shell();

            Console.WriteLine("enter a port number to host (empty will default to 8000):");
            var port = Console.ReadLine();

            int intPort;
            if (!int.TryParse(port, out intPort))
            {
                intPort = 8000;
            }

            Console.WriteLine("hosting on port {0}", intPort);

            var uriString = string.Format("net.tcp://localhost:{0}/MultiplayerServer", intPort);
            var baseAddress = new Uri(uriString);
            var serviceHost = new ServiceHost(typeof(MultiplayerService), baseAddress);
            var tcpBinding = new NetTcpBinding(SecurityMode.None);
            serviceHost.AddServiceEndpoint(typeof (IMultiplayerService), tcpBinding, baseAddress);

            serviceHost.Open();

            Console.WriteLine("Service running. Please 'Enter' to exit...");
            Console.ReadLine();

            serviceHost.Close();
        }
    }
}
