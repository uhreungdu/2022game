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
        public bool is_online = false;
    }
    class Program
    {
        // 이벤트로 While문 제어
        // 접속 받는동안은 무한정으로 돌아갈 이유 없음
        // 그리고 요거 안걸면 미친듯이 불림
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private const int Port = 9888;
        private const int MaxConnections = 100;
        private const int BufSize = 128;

        private static Socket? _ServerSocket;
        private static List<Session>? _ClientList;

        static void Main()
        {
            _ClientList = new List<Session>();
            // 서버 소켓 생성
            _ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Port);

            try
            {
                _ServerSocket.Bind(ipep);
                _ServerSocket.Listen(MaxConnections);
                Console.WriteLine("Start listen client on PORT " + Port + ".");

                while (true)
                {
                    // 이벤트 세팅
                    allDone.Reset();

                    Console.WriteLine("접속 대기 중");
                    _ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), _ServerSocket);

                    // 다 될때까지 기다림
                    allDone.WaitOne();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);
            Console.WriteLine(client.RemoteEndPoint.ToString() + " Connected");

            // 접속 됐다고 알려 줌
            allDone.Set();

            Session session = new Session();
            session.socket = client;
            session.is_online = true;
            _ClientList.Add(session);

            client.BeginReceive(session.buf, 0, Session.bufSize, 0, 
                new AsyncCallback(ReceiveCallback), session);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            String data = String.Empty;

            Session session = (Session)ar.AsyncState;

            try
            {
                int recvsize = session.socket.EndReceive(ar);

                if (recvsize > 0)
                {
                    data = Encoding.UTF8.GetString(session.buf, 0, recvsize);
                    Console.WriteLine(data);
                    foreach (Session s in _ClientList)
                    {
                        Send(s, data);
                    }
                }
                else
                {
                    if (session.is_online)
                    {
                        session.is_online = false;
                        Console.WriteLine("Client Disconnected.");
                        _ClientList.Remove(session);
                        session.socket.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (session.is_online)
                {
                    session.is_online = false;
                    Console.WriteLine("Client Disconnected.");
                    _ClientList.Remove(session);
                    session.socket.Close();
                }
            }

        }


        private static void Send(Session session, String data)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(data);
            session.socket.BeginSend(sendData, 0, sendData.Length, 0,
                new AsyncCallback(SendCallback), session);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Session session = (Session)ar.AsyncState;

                int sendsize = session.socket.EndSend(ar);
                session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                new AsyncCallback(ReceiveCallback), session);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }   
    }
}