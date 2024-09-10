namespace App.Interaction.Login;

public class Login
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public Login()
    {
        Start();
    }

    void Start()
    {
        Console.Clear();

        Console.Write("Username: ");
        Username = Console.ReadLine();
        
        Console.Write("\nPassword: ");
        var password = GetPassword();
        
        Console.Write("\nIst ihr eingegebenes login korrekt (y\\n)");
        switch (Console.ReadLine().ToLower())
        {
            case "n":
            {
                var login = new Login();
                return;
            }
            case "y":
            {
                break;
            }
        }
        
        Password = password;
        
    }
    
    private string GetPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Überprüfe, ob die Eingabe eine normale Taste ist
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");  // Zeige ein "*" für jedes eingegebene Zeichen
            }
            // Wenn die Eingabe Backspace ist, entferne das letzte Zeichen
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");  // Lösche das letzte Sternchen
            }
        }
        // Beende die Eingabe, wenn Enter gedrückt wird
        while (key.Key != ConsoleKey.Enter);

        return password;
    }
}