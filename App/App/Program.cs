using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace App;

class Program
{
    public Dictionary<string, Markets> markets = new Dictionary<string, Markets>();
    
    static void Main(string[] args)
    {
        GetToMarket();
    }

    static void GetToMarket()
    {
        var configFilePath = "appsettings.json";
        var configService = new ConfigurationService(configFilePath);

        Console.WriteLine(configService.GetSetting("Market"));
        // Update the setting
        Console.WriteLine("Updating setting...");
        configService.SetSetting("Market", "Server");

        // Display updated setting
        Console.WriteLine($"Updated Setting: {configService.GetSetting("Market")}");

        // Keep the application running to test file changes
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        
    }

    private void AddMarkets(string Name, double price = 0.0)
    {
        markets.Add(Name, new Markets(Name, price));
    }
}
