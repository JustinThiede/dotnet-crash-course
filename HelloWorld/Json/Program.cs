using Json.Models;
using Json.Data;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Json
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            DataContextDapper dapper = new DataContextDapper(config);

            string computersJson = File.ReadAllText("Computers.json");

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            IEnumerable<Computer>? computersNewtonsoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            if (computersNewtonsoft != null)
            {
                foreach (Computer computer in computersNewtonsoft)
                {
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

                    dapper.ExecuteSql(sql, computer);
                }
            }

            string computersCopyNewtonsoft = JsonConvert.SerializeObject(computersNewtonsoft, settings);
            string computersCopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);

            File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);
            File.WriteAllText("computersCopySystem.txt", computersCopySystem);
        }
    }
}