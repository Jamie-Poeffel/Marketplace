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
        
        string code = await SendEmail(config, "Jamie.poeffel@gmail.com");
        
        do{
            Console.WriteLine("Code: ");
        }while((Console.ReadLine() != code));

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

    private static async Task<string> SendEmail(IConfiguration config, string to)
    {
        // SMTP client settings
        string smtpAddress = "smtp.gmail.com";
        int portNumber = 587;
        bool enableSSL = true;
        string emailFrom = config["Email:EmailFrom"];
        string password = config["Email:Password"];
        string emailTo = to;
        string subject = "Ihr 2FA Code";

        // Erzeuge eine neue GUID und verwandle sie in einen alphanumerischen String ohne Bindestriche
        string guidString = Guid.NewGuid().ToString("N");

        // Filtere nur die Ziffern (0-9) aus dem GUID-String heraus
        string onlyDigits = new string(guidString.Where(char.IsDigit).ToArray());

        // Falls es weniger als 6 Ziffern gibt, wird der String gekürzt oder auf die ersten 6 Ziffern begrenzt
        string sixDigitCode = onlyDigits.Substring(0, Math.Min(6, onlyDigits.Length));
        // Platzhalter für den 2FA-Code
        string code = sixDigitCode; // Hier setzt du deinen dynamischen Code ein

        // HTML body mit Platzhalter für den 2FA-Code
        string body = $@"
        <!DOCTYPE html>
        <html lang='de'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    background-color: #007BFF;
                    color: white;
                    padding: 20px 0;
                    border-radius: 10px 10px 0 0;
                }}
                .header h1 {{
                    margin: 0;
                    font-size: 24px;
                }}
                .content {{
                    padding: 20px;
                    text-align: center;
                }}
                .content h2 {{
                    color: #333333;
                }}
                .code {{
                    display: inline-block;
                    background-color: #f8f9fa;
                    color: #007BFF;
                    font-size: 24px;
                    font-weight: bold;
                    padding: 10px 20px;
                    border: 1px solid #007BFF;
                    border-radius: 5px;
                    margin: 20px 0;
                }}
                .footer {{
                    text-align: center;
                    color: #888888;
                    font-size: 12px;
                    padding: 20px;
                }}
                .footer p {{
                    margin: 0;
                }}
                .button {{
                    background-color: #007BFF;
                    color: white;
                    padding: 10px 20px;
                    text-decoration: none;
                    border-radius: 5px;
                    font-size: 16px;
                }}
            </style>
        </head>
        <body>

            <div class='email-container'>
                <!-- Header Section -->
                <div class='header'>
                    <h1>Zwei-Faktor-Authentifizierung</h1>
                </div>

                <!-- Main Content Section -->
                <div class='content'>
                    <h2>Ihr Sicherheitscode lautet:</h2>
                    <div class='code'>{code}</div> <!-- Dynamischer Code hier -->
                    <p>Dieser Code ist nur 10 Minuten lang gültig. Geben Sie ihn ein, um den Vorgang abzuschließen.</p>
                    <p>Falls Sie diesen Code nicht angefordert haben, ignorieren Sie diese E-Mail bitte.</p>
                    <a href='#' class='button'>Support kontaktieren</a>
                </div>

                <!-- Footer Section -->
                <div class='footer'>
                    <p>© 2024 Ihr Unternehmen. Alle Rechte vorbehalten.</p>
                    <p>Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese Nachricht.</p>
                </div>
            </div>

        </body>
        </html>";

        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailFrom, config["Email:Name"]);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true; // Wichtig: Damit HTML interpretiert wird

            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(emailFrom, password);
                smtp.EnableSsl = enableSSL;
                try
                {
                    smtp.Send(mail);
                    Console.WriteLine("Email erfolgreich gesendet.");
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine("Fehler beim Senden der E-Mail: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("InnerException: " + ex.InnerException.Message);
                    }
                }
            }
        }

        return code;
    }
}