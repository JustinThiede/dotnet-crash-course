using DatabaseConnections.Models;
using DatabaseConnections.Data;

namespace DatabaseConnections
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // DataContextDapper dapper = new DataContextDapper();
            DataContextEF entityFramework = new DataContextEF();

            // DateTime now = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");

            // Console.WriteLine(now);

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLte = false,
                ReleaseDate = DateTime.Now,
                Price = 943m,
                VideoCard = "RTX 2060"
            };

            entityFramework.Add(myComputer);
            entityFramework.SaveChanges();

            // string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //         Motherboard,
            //         HasWifi,
            //         HasLte,
            //         ReleaseDate,
            //         Price,
            //         VideoCard
            //     ) VALUES ('" + myComputer.Motherboard
            //             + "','" + myComputer.HasWifi
            //             + "','" + myComputer.HasLte
            //             + "','" + myComputer.ReleaseDate
            //             + "','" + myComputer.Price
            //             + "','" + myComputer.VideoCard
            //     + "')";

            // bool result = dapper.ExecuteSql(sql);

            // sql = @"
            // SELECT
            //         Computer.ComputerId,
            //         Computer.Motherboard,
            //         Computer.HasWifi,
            //         Computer.HasLte,
            //         Computer.ReleaseDate,
            //         Computer.Price,
            //         Computer.VideoCard
            // FROM TutorialAppSchema.Computer";

            // IEnumerable<Computer> computers = dapper.LoadData<Computer>(sql);
            IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();

            // foreach (Computer singleComputer in computers)
            // {
            //     Console.WriteLine(singleComputer.ComputerId);
            // }


            if (computersEf != null)
            {
                foreach (Computer singleComputer in computersEf)
                {
                    Console.WriteLine(singleComputer.ComputerId);
                }
            }
        }
    }
}