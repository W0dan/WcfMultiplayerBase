using System;

namespace MultiplayerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type your (nick)name:");
            var player = Console.ReadLine();

            try
            {
                var service = new MultiplayerProxy("192.168.1.51", "8000");
                service.ChatMessageReceived += ChatMessageReceived;

                var token = service.Join(player);
                while (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("(nick)name {0} was already taken", player);
                    Console.WriteLine("pick another one:");
                    player = Console.ReadLine();
                    token = service.Join(player);
                }

                Console.WriteLine();
                Console.WriteLine("Welcome to the lobby, {0} !", player);
                Console.WriteLine();

                var input = Console.ReadLine() ?? "";
                while (input.ToLower() != "exit")
                {
                    service.SendChatMessage(token, input);

                    input = Console.ReadLine() ?? "";
                }

                service.SendChatMessage(token, "has left the building");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void ChatMessageReceived(string player, string message)
        {
            Console.WriteLine("{0}> {1}", player, message);
        }
    }
}
