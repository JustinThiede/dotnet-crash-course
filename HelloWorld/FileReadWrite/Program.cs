using FileReadWrite.Models;
using FileReadWrite.Data;
using Microsoft.Extensions.Configuration;

namespace FileReadWrite
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLte = false,
                ReleaseDate = DateTime.Now,
                Price = 943m,
                VideoCard = "RTX 2060"
            };

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                    Motherboard,
                    CPUCores,
                    HasWifi,
                    HasLte,
                    ReleaseDate,
                    Price,
                    VideoCard
                ) VALUES (
                    @Motherboard,
                    @CPUCores,
                    @HasWifi,
                    @HasLte,
                    @ReleaseDate,
                    @Price,
                    @VideoCard
                )";

            File.WriteAllText("log.txt", sql + "\n");

            using StreamWriter openFile = new("log.txt", true);

            openFile.WriteLine(sql);
            openFile.Close();

            String fileText = File.ReadAllText("log.txt");

            Console.WriteLine(fileText);
        }
    }
}