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

        private static Socket? _ServerSocket;
        private static List<Socket>? _ClientSocketList;

        static void Main()
        {
            _ClientSocketList = new List<Socket>();
            _ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Port);
            _ServerSocket.Bind(ipep);

            _ServerSocket.Listen(MaxConnections);
            Console.WriteLine("Start listen client on PORT " + Port + ".");

            while (true)
            {
                Console.WriteLine("접속 대기 중");
                Socket client = _ServerSocket.Accept();
                Console.WriteLine(client.RemoteEndPoint.ToString() + " 연결");
                _ClientSocketList.Add(client);
            }

            /*
            Socket client = _ServerSocket.Accept();
            Console.WriteLine("A client join server");

            byte[] buffer = new byte[BufSize];
            int num = client.Receive(buffer);

            string recvdata = Encoding.UTF8.GetString(buffer, 0, num);
            Console.WriteLine(recvdata);

            client.Close();
            Console.WriteLine("A client disconnect server");
            _ServerSocket.Close();
            Console.WriteLine("Server end...");
            */
        }

        private static void AcceptCallback(object? sender, SocketAsyncEventArgs ev)
        {
            Socket? socket = ev.AcceptSocket;
            if (socket != null)
            {
                _ClientSocketList.Add(socket);
                Console.WriteLine(socket.RemoteEndPoint.ToString()+" 연결");
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                byte[] buffer = new byte[BufSize];
                args.SetBuffer(buffer, 0, buffer.Length);
                socket.ReceiveAsync(args);
            }
            ev.AcceptSocket = null;
            _ServerSocket.AcceptAsync(ev);
        }

        private void ReceiveCallback()
        {

        }

        private void SendCallback()
        {

        }
    }
}