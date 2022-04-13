using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chatserver
{
    public class Session
    {
        public const int bufSize = 128;
        public byte[] buf = new byte[bufSize];
        public Socket socket = null;
    }
    class Program
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

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
                allDone.Reset();
                Console.WriteLine("접속 대기 중");
                _ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), _ServerSocket);
                allDone.WaitOne();
                //Socket client = _ServerSocket.Accept();
                
               // _ClientSocketList.Add(client);

                //SocketAsyncEventArgs args = new SocketAsyncEventArgs();
               // byte[] buffer = new byte[BufSize];
               // args.SetBuffer(buffer);
               // args.Completed+= new EventHandler<SocketAsyncEventArgs>()
               // client.ReceiveAsync();
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

        private static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);
            Console.WriteLine(client.RemoteEndPoint.ToString() + " 연결");

            Session session = new Session();
            session.socket = client;

        }

        private void ReceiveCallback()
        {

        }

        private void SendCallback()
        {

        }
    }
}