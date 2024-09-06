namespace App.Interaction.Login;

public class CreateUser
{
    public bool createNew { get; private set; }
    public string emailAddress { get; private set; }
    public string Username { get; private set; }
    public string password { get; private set; }

    public CreateUser()
    {
        Start();
    }

    private void Start()
    {
        Console.Clear();

        Console.Write("Do you want to create a new user? (y\\n) ");
        switch (Console.ReadLine().ToLower())
        {
            case "n":
                createNew = false;
                return;
            case "y":
                createNew = true;
                break;
        }
        
        Console.Clear();
        
        Console.Write("Email: ");
        emailAddress = Console.ReadLine();
        
        Console.Write("\nUsername: ");
        Username = Console.ReadLine();
        
        Console.Write("\nPassword: ");
        password = GetPassword();

        var code = Email.Email.SendEmail(Program.config, emailAddress);
        do
        {
            Console.Clear();
            Console.Write("Code: ");
        } while (Console.ReadLine() == code.ToString());
        
        Console.WriteLine("Try erfolgreich");
        
        Thread.Sleep(1000);
        
        Console.Clear();
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
                Console.Write("*"); // Zeige ein "*" für jedes eingegebene Zeichen
            }
            // Wenn die Eingabe Backspace ist, entferne das letzte Zeichen
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Lösche das letzte Sternchen
            }
        }
        // Beende die Eingabe, wenn Enter gedrückt wird
        while (key.Key != ConsoleKey.Enter);

        return password;
    }
}