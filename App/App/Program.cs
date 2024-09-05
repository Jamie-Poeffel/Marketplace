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

    static void GetToMarket(IConfiguration config)
    {
        
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

    private static void SendEmail(IConfiguration config, string to)
    {
        string smtpAddress = "smtp.example.com";  // Replace with your SMTP server address
        int portNumber = 587;                     // Typically 587 for TLS, 465 for SSL, 25 for non-secure
        bool enableSSL = true;                    // Enable SSL or TLS
        string emailFrom = config["Email:EmailFrom"];
        string password = config["Email:Password"];
        string emailTo = to;
        string subject = "Hello from C#";
        string body = "This is a test email sent from C#.";

        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailFrom, config["Email:Name"]);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = false; // If you want to send an HTML email, set this to true

            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(emailFrom, password);
                smtp.EnableSsl = enableSSL;
                
                try
                {
                    smtp.Send(mail);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}