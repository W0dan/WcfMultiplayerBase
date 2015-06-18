using System;
using MultiplayerClient.Contracts;
using MultiplayerClient.Extensions;
using MultiplayerClient.Hosting;
using MultiplayerClient.Proxy;

namespace MultiplayerClient
{
    class Program
    {
        private static Shell _shell;

        static void Main(string[] args)
        {
            _shell = new Shell();
            var host = new Host();

            int intPort;

            _shell.WriteLine("Type your (nick)name:");
            var player = _shell.ReadLine();

            _shell.WriteLine("Will you be hosting ? (y/n)");
            var iAmHost = _shell.ReadKey();
            _shell.WriteLine();

            if (iAmHost.Key == ConsoleKey.Y)
            {
                _shell.WriteLine("enter a port number to host (empty will default to 8000):");
                intPort = _shell.ReadInt(8000);

                host.Start(player, intPort);

                _shell.WriteLine("hosting on port {0} by player {1}", intPort, player);
            }
            else
            {
                _shell.WriteLine("enter a port number to join (empty will default to 8000):");
                intPort = _shell.ReadInt(8000);
            }

            try
            {
                var service = new MultiplayerProxy("192.168.1.51", intPort.ToString("0"));
                service.ChatMessageReceived += ChatMessageReceived;

                var token = TryJoin(service, player);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _shell.WriteLine();
                    _shell.WriteLine("Welcome to the lobby, {0} !", player);
                    _shell.WriteLine();

                    var input = _shell.ReadLine() ?? "";
                    while (input.ToLower() != "exit")
                    {
                        service.SendChatMessage(token, input);

                        input = _shell.ReadLine() ?? "";
                    }

                    service.SendChatMessage(token, "has left the building");
                }
            }
            catch (Exception ex)
            {
                _shell.WriteLine(ex.ToString());
            }
            finally
            {
                if (iAmHost.Key == ConsoleKey.Y)
                {
                    try
                    {
                        host.Stop();
                    }
                    catch (Exception ex)
                    {
                        _shell.WriteLine("An error occured: {0}", ex);
                    }
                }
            }
            _shell.WriteLine("Press any key to exit");
            _shell.ReadLine();
        }

        private static string TryJoin(IMultiplayerService service, string player)
        {
            try
            {
                var token = service.Join(player);
                while (string.IsNullOrEmpty(token))
                {
                    _shell.WriteLine("(nick)name {0} was already taken", player);
                    _shell.WriteLine("pick another one:");
                    player = _shell.ReadLine();
                    token = service.Join(player);
                }
                return token;
            }
            catch (Exception)
            {
                _shell.WriteLine("it appears that no service is listening on the port you provided");
                return null;
            }
        }

        static void ChatMessageReceived(string player, string message)
        {
            _shell.WriteLine("{0}> {1}", player, message);
        }
    }
}
