using System;
using System.Net;
using System.Net.Sockets;

namespace Chatserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // PORTNUM is 9888
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9888);
            server.Bind(ipep);

            // Max waiting is 100;
            server.Listen(100);
            Console.WriteLine("Start listen client on PORT 9888...");

            Socket client = server.Accept();
            Console.WriteLine("A client join server");

            client.Close();
            Console.WriteLine("A client disconnect server");
            server.Close();
            Console.WriteLine("Server end...");
        }
    }
}