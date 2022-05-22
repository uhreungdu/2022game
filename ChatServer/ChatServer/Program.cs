using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;

using Database;

namespace Chatserver
{
    enum ChatType : byte
    {
        TEST,
        NormalChat,
        Exit,
        EnterRoom,
        RoomChat,
        ExitRoom,
        MakeRoom,
        LoginRequest,
        LoginResult,
        RoomListRequest,
        RoomListResult
    }

    public class Session
    {
        public const int bufSize = 1024;
        public byte[] buf = new byte[bufSize];
        public Socket socket = null;
        public bool is_online = false;
        public bool in_room = false;
        public string nickname;
        public string roomname;
        public string id;
    }

    public class Program
    {
        // 이벤트로 While문 제어
        // 접속 받는동안은 무한정으로 돌아갈 이유 없음
        // 그리고 요거 안걸면 미친듯이 불림
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private const int Port = 9887;
        private const int MaxConnections = 100;
        private const int BufSize = 1024;

        private static Socket? _ServerSocket;
        private static List<Session>? _ClientList;
        private static List<Session>? _NotLoginList;

        static void Main()
        {
            _ClientList = new List<Session>();
            _NotLoginList = new List<Session>();

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
            Console.WriteLine(client.RemoteEndPoint.ToString() + " Connected");

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
                                Console.WriteLine(session.nickname+": "+data);
                                foreach (Session s in _ClientList)
                                {
                                    if (s.in_room) continue;
                                    Send(s, data, session.nickname);
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
                        case (byte)ChatType.EnterRoom:
                            {
                                data = Encoding.UTF8.GetString(session.buf, 1, recvsize - 1);
                                Console.WriteLine(session.nickname + " enters Roomname " + data);
                                session.roomname = data;
                                session.in_room = true;
                                DatabaseControl.PlayerEnterRoom(session.roomname, session.nickname);
                                session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                                    new AsyncCallback(ReceiveCallback), session);
                                break;
                            }
                        case (byte)ChatType.MakeRoom:
                            {
                                string iname = Encoding.UTF8.GetString(session.buf, 4, session.buf[1]);
                                string ename = Encoding.UTF8.GetString(session.buf, 4 + session.buf[1], session.buf[2]);
                                int maxPlayerNum = session.buf[3];
                                
                                Console.WriteLine(session.nickname + " makes Roomname " + ename);
                                session.roomname = iname;
                                session.in_room = true;
                                DatabaseControl.PlayerMakeRoom(iname, ename, maxPlayerNum, session.nickname);
                                session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                                    new AsyncCallback(ReceiveCallback), session);
                                break;
                            }
                        case (byte)ChatType.LoginRequest:
                            {
                                string id = Encoding.UTF8.GetString(session.buf, 3, session.buf[1]);
                                string pw = Encoding.UTF8.GetString(session.buf, 3 + session.buf[1], session.buf[2]);
                                //Console.WriteLine(id +" "+ pw + "Access");

                                var result = DatabaseControl.LoginAccount(id, pw);

                                switch (result.code)
                                {
                                    case 0:
                                        session.is_online = true;
                                        session.nickname = result.name;
                                        session.id = result.id;
                                        _ClientList.Add(session);
                                        Console.WriteLine(session.nickname + " Login ");                                 
                                        break;
                                }
                                SendLoginResult(session, result);
                                try
                                {
                                    session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                                    new AsyncCallback(ReceiveCallback), session);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                                break;
                            }
                        case (byte)ChatType.RoomListRequest:
                            {
                               List<RoomInfo> roomData = DatabaseControl.GetRoomInfos();
                                foreach (RoomInfo roomInfo in roomData)
                                {
                                    byte[] iname = Encoding.UTF8.GetBytes(roomInfo.internal_name);
                                    byte[] ename = Encoding.UTF8.GetBytes(roomInfo.external_name);
                                    int sendSize = 1 + 6 + iname.Length + ename.Length;
                                    byte[] sendData = new byte[sendSize];
                                    sendData[0] = (byte)sendSize;
                                    sendData[1] = (byte)ChatType.RoomListResult;
                                    sendData[2] = (byte)iname.Length;
                                    sendData[3] = (byte)ename.Length;
                                    sendData[4] = (byte)roomInfo.now_playernum;
                                    sendData[5] = (byte)roomInfo.max_playernum;
                                    sendData[6] = Convert.ToByte(roomInfo.ingame);
                                    Array.Copy(iname, 0, sendData, 7, iname.Length);
                                    Array.Copy(ename, 0, sendData, 7 + iname.Length, ename.Length);
                                    //Console.WriteLine(roomInfo.external_name);
                                    SendRoomList(session, sendData);
                                }
                                
                                session.socket.BeginReceive(session.buf, 0, Session.bufSize, 0,
                                    new AsyncCallback(ReceiveCallback), session);
                                break;
                            }
                    }
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


        private static void Send(Session session, string data, string sender)
        {
            byte[] name = Encoding.UTF8.GetBytes(sender);
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

        private static void SendLoginResult(Session session, LoginResult result)
        {
            switch (result.code)
            {
                case 0:
                    {
                        byte[] id = Encoding.UTF8.GetBytes(result.id);
                        byte[] name = Encoding.UTF8.GetBytes(result.name);
                        int sendSize = 1 + 7 + id.Length + name.Length;
                        byte[] sendData = new byte[sendSize];
                        sendData[0] = (byte)sendSize;
                        sendData[1] = (byte)ChatType.LoginResult;
                        sendData[2] = (byte)result.code;
                        sendData[3] = (byte)id.Length;
                        sendData[4] = (byte)name.Length;
                        sendData[5] = (byte)result.win;
                        sendData[6] = (byte)result.lose;
                        sendData[7] = (byte)result.costume;
                        Array.Copy(id, 0, sendData, 8, id.Length);
                        Array.Copy(name, 0, sendData, 8 + id.Length, name.Length);
                        session.socket.BeginSend(sendData, 0, sendData.Length, 0,
                            new AsyncCallback(SendCallback), session);
                        break;
                    }
                case 4:
                    {
                        byte[] id = Encoding.UTF8.GetBytes(result.id);
                        byte[] name = Encoding.UTF8.GetBytes(result.name);
                        byte[] roomname = Encoding.UTF8.GetBytes(result.roomname);
                        int sendSize = 1 + 8 + id.Length + name.Length + roomname.Length;
                        byte[] sendData = new byte[sendSize];
                        sendData[0] = (byte)sendSize;
                        sendData[1] = (byte)ChatType.LoginResult;
                        sendData[2] = (byte)result.code;
                        sendData[3] = (byte)id.Length;
                        sendData[4] = (byte)name.Length;
                        sendData[5] = (byte)roomname.Length;
                        sendData[6] = (byte)result.win;
                        sendData[7] = (byte)result.lose;
                        sendData[8] = (byte)result.costume;
                        Array.Copy(id, 0, sendData, 9, id.Length);
                        Array.Copy(name, 0, sendData, 9 + id.Length, name.Length);
                        Array.Copy(roomname, 0, sendData, 9 + id.Length + name.Length, roomname.Length);
                        session.socket.BeginSend(sendData, 0, sendData.Length, 0,
                            new AsyncCallback(SendCallback), session);
                        break;
                    }
                default:
                    {
                        int sendSize = 3;
                        byte[] sendData = new byte[sendSize];
                        sendData[0] = (byte)sendSize;
                        sendData[1] = (byte)ChatType.LoginResult;
                        sendData[2] = (byte)result.code;
                        session.socket.BeginSend(sendData, 0, sendData.Length, 0,
                            new AsyncCallback(SendCallback), session);
                        break;
                    }
            }
        }

        private static void SendRoomList(Session session, byte[] sendData)
        {
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
                DatabaseControl.LogoutAccount(session.id);
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

        private static byte[] ConvertRoomlistToByte(List<RoomInfo> roomlist)
        {
            string sendData = "";
            foreach (RoomInfo room in roomlist)
            {
                string data =   "iname:" + room.internal_name + "|ename:" + room.external_name +
                                "|nowPnum:" + room.now_playernum + "|maxPnum:" + room.max_playernum + "|ingame:" + room.ingame + ";";
                sendData = sendData + data;
            }
            
            return Encoding.UTF8.GetBytes(sendData);
        }
    }
}