using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chatserver
{
    class Program
    {
        private const int Port = 9888;
        private const int MaxConnections = 100;
        private const int BufSize = 128;
        static void Main(string[] args)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Port);
            server.Bind(ipep);

            server.Listen(MaxConnections);
            Console.WriteLine("Start listen client on PORT 9888...");

            Socket client = server.Accept();
            Console.WriteLine("A client join server");

            byte[] buffer = new byte[BufSize];
            int num = client.Receive(buffer);

            string recvdata = Encoding.UTF8.GetString(buffer, 0, num);
            Console.WriteLine(recvdata);

            client.Close();
            Console.WriteLine("A client disconnect server");
            server.Close();
            Console.WriteLine("Server end...");
        }
    }
}