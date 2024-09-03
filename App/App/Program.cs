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
        var config = getConfig();
        
        config.GetSection("Appsettings").Bind("Marketplace", "Local");
        
        
    }
    
    private static dynamic getConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        return config;
    }

    private void AddMarkets(string Name, double price = 0.0)
    {
        markets.Add(Name, new Markets(Name, price));
    }
}
