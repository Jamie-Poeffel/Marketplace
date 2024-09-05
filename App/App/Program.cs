using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Net.Mail;

namespace App;

class Program
{
    public Dictionary<string, Markets> markets = new Dictionary<string, Markets>();

    static void Main(string[] args)
    {
        var config = GetConect();
        GetToMarket(config);
    }

    static async void GetToMarket(IConfiguration config)
    {
        string code = await Email.Email.SendEmail(config, "Jamie.poeffel@gmail.com");

        do
        {
            Console.WriteLine("Code: ");
        } while ((Console.ReadLine() != code));

        Console.WriteLine("2FA Erfolgreich");
    }

    private void AddMarkets(string Name, double price = 0.0)
    {
        markets.Add(Name, new Markets(Name, price));
    }

    private static IConfiguration GetConect()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }

   
}