using System.Data.OleDb;

namespace Server.Data;

public class DatabaseAccess
{
    private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\..\..\..\Data\Database.accdb;";
}