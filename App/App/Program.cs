using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Net.Mail;

namespace App;

class Program
{
    public static Dictionary<string, Markets> markets = new Dictionary<string, Markets>();

    static void Main(string[] args)
    {
        var config = GetConect();
        // Get2FA(config);
        
        Interaction.Client.StartClient();
        
        GetToMarket(config);
    }

    static void GetToMarket(IConfiguration config)
    {
        Print(markets);
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

    private static async void Get2FA(IConfiguration config)
    {
        Console.Write("Enter email: ");
        string code = await Email.Email.SendEmail(config, Console.ReadLine());

        do
        {
            Console.Write("\nCode: ");
        } while ((Console.ReadLine() != code));

        Console.WriteLine("2FA Erfolgreich");
    }

    private static void Print(Dictionary<string, Markets> markets)
    {
        Console.Clear();
        // Sortiere das Dictionary nach dem Key (string)
        var sortedMarkets = markets.OrderBy(m => m.Key);

        // Überschriften der Tabelle ausgeben
        Console.WriteLine($"{"MarketName",-20} {"CurrentPrice",-20}");

        // Werte ausgeben
        foreach (var market in sortedMarkets)
        {
            // Jede Zeile besteht aus dem MarketName und dem CurrentPrice
            Console.WriteLine($"{market.Value.Marketname,-20} {market.Value.CurrontPrice,-20}");
        }
    }

}