using DatabaseConnections.Models;
using DatabaseConnections.Data;

namespace DatabaseConnections
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            DataContextDapper dapper = new DataContextDapper();

            DateTime now = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");

            Console.WriteLine(now);

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLte = false,
                ReleaseDate = DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"),
                Price = 943m,
                VideoCard = "RTX 2060"
            };

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                    Motherboard,
                    HasWifi,
                    HasLte,
                    ReleaseDate,
                    Price,
                    VideoCard
                ) VALUES ('" + myComputer.Motherboard
                        + "','" + myComputer.HasWifi
                        + "','" + myComputer.HasLte
                        + "','" + myComputer.ReleaseDate
                        + "','" + myComputer.Price
                        + "','" + myComputer.VideoCard
                + "')";

            bool result = dapper.ExecuteSql(sql);

            sql = @"
            SELECT
                    Computer.Motherboard,
                    Computer.HasWifi,
                    Computer.HasLte,
                    Computer.ReleaseDate,
                    Computer.Price,
                    Computer.VideoCard
            FROM TutorialAppSchema.Computer";

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sql);

            foreach (Computer singleComputer in computers)
            {
                Console.WriteLine(singleComputer.Motherboard);
            }
        }
    }
}