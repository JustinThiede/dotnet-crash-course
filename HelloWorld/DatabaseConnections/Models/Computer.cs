using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace DatabaseConnections.Models
{
    public class Computer
    {
        // public string _motherboard; This would create a field which goes against C# best practices
        public int ComputerId { get; set; }
        public string Motherboard { get; set; } = ""; // This creates a property "Motherboard" that has getter and setter methods and creates a private field _motherboard
        public int CPUCores { get; set; }
        public bool HasWifi { get; set; }
        public bool HasLte { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string VideoCard { get; set; } = ""; // String is not nullable thats why we need to set a default value with = "", alternatively we could create a constructor with the default vals
    }
}