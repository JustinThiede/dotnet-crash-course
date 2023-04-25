using System;
using HelloWorld.Models;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
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

            Console.WriteLine(myComputer.Motherboard);
        }
    }
}