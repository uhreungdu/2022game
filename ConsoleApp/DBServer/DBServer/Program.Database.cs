using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Database
{
    public struct LoginResult
    {
        public string? id;
        public string? name;
        public string? roomname;
        public int win = 0;
        public int lose = 0;
        public int costume = 0;
        public int code;

        public LoginResult(int code)
        {
            this.id = null;
            this.name = null;
            this.roomname = null;
            this.code = code;
        }
        public LoginResult(string id, string name, int win, int lose, int costume, int code)
        {
            this.id = id;
            this.name = name;
            this.roomname = null;
            this.win = win;
            this.lose = lose;
            this.costume = costume;
            this.code = code;
        }
        public LoginResult(string id, string name, int win, int lose, int costume, string roomname, int code)
        {
            this.id = id;
            this.name = name;
            this.roomname = roomname;
            this.win = win;
            this.lose = lose;
            this.costume = costume;
            this.code = code;
        }
    }

    public struct RoomInfo
    {
        public string internal_name;
        public string external_name;
        public int now_playernum;
        public int max_playernum;
        public bool ingame;

        public RoomInfo(string iname, string ename, int nPnum, int mPnum, bool ingame)
        {
            internal_name = iname;
            external_name = ename;
            now_playernum = nPnum;
            max_playernum = mPnum;
            this.ingame = ingame;
        }
    }

    public struct PlayerInfo
    {
        public string m_name;
        public int m_win;
        public int m_lose;

        public PlayerInfo(string name, int win, int lose)
        {
            m_name = name;
            m_win = win;
            m_lose = lose;
        }
    }

    public class DatabaseControl
    {
        public static string connect = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};",
    "127.0.0.1", "havocfes", "root", "2022project");
        public static MySqlConnection conn = new MySqlConnection(connect);

        public static List<RoomInfo> GetRoomInfos()
        {
            List<RoomInfo> list = new List<RoomInfo>();
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetRoomInfos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RoomInfo info = new RoomInfo();
                        info.internal_name = reader.GetString(0);
                        info.external_name = reader.GetString(1);
                        info.now_playernum = reader.GetInt32(2);
                        info.max_playernum = reader.GetInt32(3);
                        info.ingame = reader.GetBoolean(4);
                        list.Add(info);
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return list;
        }

        public static void PlayerMakeRoom(string iname, string ename, int maxPlayerNum, string Pname)
        {
            if (RoomCheck(iname))
            {
                Console.WriteLine("이미 생성된 방");
                return;
            }
            else
            {
                MakeRoom(iname, ename, maxPlayerNum);
                PlayerJoinRoom(Pname, iname);
            }
        }
        private static void MakeRoom(string iname, string ename, int maxPlayerNum)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("MakeRoom", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iname", iname);
                    cmd.Parameters.AddWithValue("@ename", ename);
                    cmd.Parameters.AddWithValue("@maxPnum", maxPlayerNum);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
        }
        public static void PlayerEnterRoom(string iname, string Pname)
        {
            if (RoomCheck(iname))
            {
                switch (RoomPlayerNumCheck(iname))
                {
                    case 0: break;
                    case 1: Console.WriteLine("ERR: MAX USER"); return;
                    case 2: Console.WriteLine("ERR: GAME ALREADY START"); return;
                };

                string[] sql = new string[2];
                sql[0] = string.Format("INSERT IGNORE INTO playingchar (character_name, room_internal_name)VALUE({0},{1})", Pname, iname);
                sql[1] = string.Format("UPDATE room SET now_playernum = now_playernum + 1 WHERE internal_name = {0}", iname);

                PlayerJoinRoom(Pname, iname);
            }
            else
            {
                Console.WriteLine("ERR: NO ROOM");
                return;
            }
        }
        public static LoginResult LoginAccount(string id, string pw)
        {
            if (CheckIDPW(id, pw))
            {
                if (CheckCharacter(id))
                {
                    if (CheckMultiLogin(id))
                    {
                        var Charname = GetCharacterName(id);
                        WriteLastLoginTime(id);
                        return GetPlayerInfo(id, CheckPlayerInGame(Charname));
                    }
                    else return new LoginResult(3); // Already Online
                }
                else return new LoginResult(2);  // Need Character
            }
            else return new LoginResult(1);  // ID or PW error
        }
        public static void LogoutAccount(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("LogoutAccount", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
        }
        private static bool CheckIDPW(string id, string pw)
        {
            var result = 0;
            using (conn)
            {
                // ID Check
                conn.Open();
                using(MySqlCommand cmd = new MySqlCommand("CheckIDPW", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@pw", pw);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                }
                conn.Close();
                conn.Dispose();
            }
            if (result == 0) return false;  // login err
            else return true;
        }
        private static bool CheckCharacter(string id)
        {
            var result = 0;
            using (conn)
            {
                // ID Check
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("CheckCharExist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                }
                conn.Close();
                conn.Dispose();
            }
            if (result == 0) return false;  // character not exist
            else return true;
        }
        private static bool CheckMultiLogin(string id)
        {
            var result = 0;
            using (conn)
            {
                // ID Check
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("CheckMultiLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                }
                conn.Close();
                conn.Dispose();
            }
            if (result == 1) return false;  // already login
            else return true;
        }
        private static bool CheckPlayerInGame(string name)
        {
            var result = 0;
            using (conn)
            {
                // ID Check
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("CheckPlayerPlayGame", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", name);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                }
                conn.Close();
                conn.Dispose();
            }
            if (result == 1) return true;  // playing game
            else return false;
        }
        public static string GetPlayerInGameRoomname(string name)
        {
            var result = new string("");
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetRoomPlayerIn", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", name);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = reader[0].ToString();
                }
                conn.Close();
                conn.Dispose();
            }
            return result;
        }
        public static string GetCharacterName(string id)
        {
            var result = new string("");
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetCharacterName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = reader[0].ToString();
                }
                conn.Close();
                conn.Dispose();
            }
            return result;
        }
        private static void WriteLastLoginTime(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("UpdateLastLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
        }
        public static LoginResult GetPlayerInfo(string id, bool alreadyPlay)
        {
            LoginResult result = new LoginResult();
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("GetPlayerInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.id = id;
                        result.name = reader[0].ToString();
                        result.costume = Convert.ToInt32(reader[2]);
                        result.win = Convert.ToInt32(reader[3]);
                        result.lose = Convert.ToInt32(reader[4]);
                        result.roomname = null;
                        if(alreadyPlay) result.code = 4;
                        else result.code = 0;
                    }
                }
                conn.Close();
                conn.Dispose();
                return result;
            }
        }
        private static int RoomPlayerNumCheck(string iname)
        {
            int nowPnum = 0;
            int maxPnum = 0;
            bool ingame = false;

            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("CheckRoomisFull", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iname", iname);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        nowPnum = (int)reader["now_playernum"];
                        maxPnum = (int)reader["max_playernum"];
                        ingame = (bool)reader["ingame"];
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            if (nowPnum >= maxPnum) return 1;
            else if (ingame) return 2;
            else return 0;
        }
        private static bool RoomCheck(string iname)
        {
            var result = 0;
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("CheckRoomExist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iname", iname);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                }
                conn.Close();
                conn.Dispose();
            }
            if (result == 0) return false;
            else return true;
        }
        private static void PlayerJoinRoom(string Pname, string iname)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("PlayerJoinRoom", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Pname", Pname);
                    cmd.Parameters.AddWithValue("@iname", iname);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// isWin 0 이면 해당 id에 win + 1
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isWin"></param>
        public static void ChangePlayerGameRecord(string id, bool isWin)
        {
            using (conn)
            {
                conn.Open();
                if (isWin)
                {
                    // player win + 1
                    using (MySqlCommand cmd = new MySqlCommand("CharacterWinPlus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account_id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (MySqlCommand cmd = new MySqlCommand("CharacterLosePlus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account_id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
