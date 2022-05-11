using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Database
{
    public class DatabaseControl
    {
        public static string connect = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};",
    "127.0.0.1", "havocfes", "root", "2022project");
        public static MySqlConnection conn = new MySqlConnection(connect);

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
                PlusPlayerNumInRoom(iname);
            }
            else
            {
                Console.WriteLine("ERR: NO ROOM");
                return;
            }
        }

        private static int RoomPlayerNumCheck(string iname)
        {
            string sql = 
                string.Format("SELECT now_playernum, max_playernum, ingame FROM room WHERE internal_name={0}", iname);
            using (conn)
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                conn.Close();

                var nowPnum = (int)reader["now_playernum"];
                var maxPnum = (int)reader["max_playernum"];
                var ingame = (bool)reader["ingame"];

                if (nowPnum >= maxPnum)
                {
                    return 1;
                }
                else if (ingame)
                {
                    return 2;
                }
                else return 0;
            }
        }

        private static bool RoomCheck(string iname)
        {
            string sql = string.Format("SELECT COUNT(*) FROM room WHERE internal_name={0}", iname);

            using (conn)
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                conn.Close();

                var result = (int)reader[0];

                if (result == 0) return false;
                else return true;
            }
        }

        private static void PlayerJoinRoom(string Pname, string iname) {
            string sql = string.Format("INSERT IGNORE INTO playingchar (character_name, room_internal_name)VALUE({0},{1})", Pname, iname);

            using (conn)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteReader();
                conn.Close();
            }
        }

        private static void PlusPlayerNumInRoom(string iname)
        {
            string sql = string.Format("UPDATE room SET now_playernum = now_playernum + 1 WHERE internal_name = {0}", iname);

            using (conn)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteReader();
                conn.Close();
            }
        }
        
    }
}
