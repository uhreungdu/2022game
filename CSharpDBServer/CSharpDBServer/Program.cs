using MySql.Data.MySqlClient;

namespace DBServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, World!");
            if (ConnectDB()) { Console.WriteLine("OK"); }
            else { Console.WriteLine("F"); }
        }

        private static bool ConnectDB()
        {
            string connect = String.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};",
                "127.0.0.1", "2022project", "root", "2022project");
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
    }
}