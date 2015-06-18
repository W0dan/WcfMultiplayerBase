using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using MultiplayerClient.Contracts;
using MultiplayerClient.Extensions;
using MultiplayerClient.Server;

namespace MultiplayerClient.Hosting
{
    public class Host
    {
        private readonly Shell _shell;

        public Host(Shell shell)
        {
            _shell = shell;
        }

        private ServiceHost _serviceHost;

        public int Start(string player)
        {
            _shell.WriteLine("enter a port number to host (empty will default to 8000):");
            var intPort = _shell.ReadInt(8000);

            _shell.WriteLine("hosting on port {0} by player {1}", intPort, player);

            var serviceRunning = false;
            Task.Run(() =>
            {
                var uriString = string.Format("net.tcp://localhost:{0}/MultiplayerServer", intPort);
                var baseAddress = new Uri(uriString);
                _serviceHost = new ServiceHost(typeof(MultiplayerService), baseAddress);
                var tcpBinding = new NetTcpBinding(SecurityMode.None);
                _serviceHost.AddServiceEndpoint(typeof(IMultiplayerService), tcpBinding, baseAddress);

                _serviceHost.Open();

                serviceRunning = true;
            });

            while (!serviceRunning)
            {
                Thread.Sleep(10);
            }

            return intPort;
        }

        public void Stop()
        {
            try
            {
                _serviceHost.Close();
            }
            catch (Exception ex)
            {
                _shell.WriteLine("An error occured: {0}", ex);
            }
        }
    }
}