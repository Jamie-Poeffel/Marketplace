using System.Data;
using System.Data.OleDb;

namespace Server.Data;

public class DatabaseAccess
{
    private static readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\..\..\..\Data\Database.accdb;";
    
    public static bool CheckUser(string Username, string Password)
    {
        var users = GetAllUsers();
        var fint = new KeyValuePair<string, string>(Username, Password);

        foreach (var user in users)
        {
            if (user.Value.Equals(fint.Value) && user.Key.Equals(fint.Key))
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }

    private static List<KeyValuePair<string, string>> GetAllUsers()
    { 
        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
        
        string query = "SELECT * FROM [User]";

        try
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new KeyValuePair<string, string>(reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("Password"))));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return list;
    }


    public static void addUser(string username, string password, string email)
    {
        string query = "INSERT INTO [User] ([Email], [Username], [Password]) VALUES (@Email, @Username, @Password)])";
        try
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}