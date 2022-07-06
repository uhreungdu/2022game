using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;

namespace LobbyChatserver
{
    enum ChatType : byte
    {
        NormalChat,
        Exit,
        Login
    }

    public class Session
    {
        public const int bufSize = 256;
        public byte[] buf = new byte[bufSize];
        public Socket socket = null;
        public bool is_online = false;
        public string id;
    }

    public class Program
    {
        // 이벤트로 While문 제어
        // 접속 받는동안은 무한정으로 돌아갈 이유 없음
        // 그리고 요거 안걸면 미친듯이 불림
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private const int Port = 15000;
        private const int MaxConnections = 100;
        private const int BufSize = 256;

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
            _ServerSocket.Close();
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);

            // 접속 됐다고 알려 줌
            allDone.Set();

            Session session = new Session();
            session.socket = client;
            Console.WriteLine(client.RemoteEndPoint.ToString() + " Connected");
            _ClientList.Add(session);

            client.BeginReceive(session.buf, 0, Session.bufSize, 0,
                new AsyncCallback(ReceiveCallback), session);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            string data;

            Session session = (Session)ar.AsyncState;
            try
            {
                int recvsize = session.socket.EndReceive(ar);

                if (recvsize > 0)
                {
                    switch (session.buf[0])
                    {
                        case (byte)ChatType.Exit:
                            {
                                if (!session.is_online) return;
                                DisconnectClient(session);
                                return;
                            }
                        case (byte)ChatType.NormalChat:
                            {
                                data = Encoding.UTF8.GetString(session.buf, 1, recvsize - 1);
                                Console.WriteLine(session.id + ": " + data);
                                foreach (Session s in _ClientList)
                                    SendChat(s, data, session.id);
                                break;
                            }
                        case (byte)ChatType.Login:
                            {
                                session.is_online = true;
                                session.id = Encoding.UTF8.GetString(session.buf, 2, session.buf[1]);
                                break;
                            }
                    }

                    session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                                    new AsyncCallback(ReceiveCallback), session);
                }
                else
                {
                    if (!session.is_online) return;
                    DisconnectClient(session);
                }
            }
            catch (Exception)
            {
                if (!session.is_online) return;
                DisconnectClient(session);
            }

        }


        private static void SendChat(Session session, string data, string sender)
        {
            byte[] name = Encoding.UTF8.GetBytes(sender);
            byte[] chatData = Encoding.UTF8.GetBytes(data);
            byte[] sendData = new byte[1 + 3 + name.Length + chatData.Length];
            sendData[0] = (byte)(1 + 3 + name.Length + chatData.Length);
            sendData[1] = (byte)ChatType.NormalChat;
            sendData[2] = (byte)name.Length;
            sendData[3] = (byte)chatData.Length;
            Array.Copy(name, 0, sendData, 4, name.Length);
            Array.Copy(chatData, 0, sendData, 4 + name.Length, chatData.Length);
            session.socket.BeginSend(sendData, 0, sendData.Length, 0,
                new AsyncCallback(SendCallback), session);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Session session = (Session)ar.AsyncState;
                /*
                int sendsize = session.socket.EndSend(ar);
                session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                new AsyncCallback(ReceiveCallback), session);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void DisconnectClient(Session session)
        {
            if (session.is_online)
            {
                session.is_online = false;
                Console.WriteLine(session.socket.RemoteEndPoint + " " + session.id + " is Disconnected.");
                _ClientList.Remove(session);
                session.socket.Close();
            }
            else
            {
                try
                {
                    Console.WriteLine(session.socket.RemoteEndPoint + " is Disconnected.");
                    session.socket.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}