using Microsoft.Extensions.Configuration;

namespace App;

class Program
{
    
    static void Main(string[] args)
    {
        var config = getConfig();
        
        Console.WriteLine($"Time: {config["MySettings:Time"].ToString()}");
        Console.ReadKey(true);
    }

    private static dynamic getConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        return config;
    }
}