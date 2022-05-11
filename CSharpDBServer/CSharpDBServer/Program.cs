using MySql.Data.MySqlClient;

namespace DBServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, World!");
            SelectDB();
        }

        private static bool ConnectDB()
        {
            string connect = String.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};",
                "127.0.0.1", "havocfes", "root", "2022project");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connect))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void SelectDB()
        {
            string connect = String.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};",
                "127.0.0.1", "havocfes", "root", "2022project");
            string sql = "select * from account";
            using(MySqlConnection conn = new MySqlConnection(connect))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                conn.Close();
            }
        }
    }
}