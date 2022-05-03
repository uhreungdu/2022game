using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Specialized;

namespace Chatserver
{
    enum ChatType : byte
    {
        NormalChat,
        Exit,
        EnterRoom,
        RoomChat,
        ExitRoom
    }
    public class Session
    {
        public const int bufSize = 128;
        public byte[] buf = new byte[bufSize];
        public Socket socket = null;
        public bool is_online = false;
        public bool in_room = false;
        public string nickname;
        public string roomname;
        public string id;
    }

    class Program
    {
        // 이벤트로 While문 제어
        // 접속 받는동안은 무한정으로 돌아갈 이유 없음
        // 그리고 요거 안걸면 미친듯이 불림
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private const int Port = 9887;
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
            _ServerSocket.Close();

            /*
            Socket client = _ServerSocket.Accept();
            Console.WriteLine("A client join server");

            byte[] buffer = new byte[BufSize];
            int num = client.Receive(buffer);

            string recvdata = Encoding.UTF8.GetString(buffer, 0, num);
            Console.WriteLine(recvdata);

            client.Close();
            Console.WriteLine("A client disconnect server");
            Console.WriteLine("Server end...");
            */
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);

            // 접속 됐다고 알려 줌
            allDone.Set();

            Session session = new Session();
            session.socket = client;
            session.is_online = true;
            int num = client.Receive(session.buf);
            session.nickname = Encoding.UTF8.GetString(session.buf, 2, session.buf[0]);
            session.id = Encoding.UTF8.GetString(session.buf, 2 + session.buf[0], session.buf[1]);
            _ClientList.Add(session);
            Console.WriteLine(client.RemoteEndPoint.ToString() + " " + session.nickname + " Connected");

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
                                DisconnectClient(session);
                                return;
                            }
                        case (byte)ChatType.NormalChat:
                            {
                                data = Encoding.UTF8.GetString(session.buf, 1, recvsize - 1);
                                Console.WriteLine(session.nickname+": "+data);
                                foreach (Session s in _ClientList)
                                {
                                    Send(s, data);
                                }
                                break;
                            }
                        case (byte)ChatType.RoomChat:
                            {
                                data = Encoding.UTF8.GetString(session.buf, 1, recvsize - 1);
                                Console.WriteLine("Room | " + session.nickname + ": " + data);
                                session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                                    new AsyncCallback(ReceiveCallback), session);
                                break;
                            }
                    }
                }
                else
                {
                    DisconnectClient(session);
                }
            }
            catch (Exception)
            {
                DisconnectClient(session);
            }

        }


        private static void Send(Session session, String data)
        {
            byte[] name = Encoding.UTF8.GetBytes(session.nickname);
            byte[] chatData = Encoding.UTF8.GetBytes(data);
            byte[] sendData = new byte[3 + name.Length + chatData.Length];
            sendData[0] = (byte)ChatType.NormalChat;
            sendData[1] = (byte)name.Length;
            sendData[2] = (byte)chatData.Length;
            Array.Copy(name, 0, sendData, 3 ,name.Length);
            Array.Copy(chatData, 0, sendData, 3 + name.Length, chatData.Length);
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

        private static void DisconnectClient(Session session)
        {
            if (session.is_online)
            {
                session.is_online = false;
                Console.WriteLine(session.socket.RemoteEndPoint + " " + session.nickname + " is Disconnected.");

                var www = new WebClient();
                var data = new NameValueCollection();
                string url = "http://121.139.87.70/login/logout_account.php";
                data["id"] = "\"" + session.id + "\"";
                www.UploadValues(url, "POST", data);
                _ClientList.Remove(session);
                session.socket.Close();
            }
        }
    }
}