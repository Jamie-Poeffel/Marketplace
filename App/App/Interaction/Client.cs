using System;
using System.Data;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using App.Interaction.Login;

namespace App.Interaction;

public class Client
{
    private const string ServerIp = "127.0.0.1"; // Localhost
    private const int Port = 5000; // Must match the server port

    public static void StartClient()
    {
        try
        {
            // Create a TcpClient and connect to the server
            TcpClient client = new TcpClient(ServerIp, Port);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Console.WriteLine("Connected to server.");

            NetworkStream stream = client.GetStream();

            Login.CreateUser user = new Login.CreateUser();
            if (!user.createNew)
            {
                Login.Login login = new Login.Login();

                string message2 = $"LoginUser?={login.Username},{login.Password}";
                byte[] data2 = Encoding.ASCII.GetBytes(message2);
                stream.Write(data2, 0, data2.Length);
            }
            else
            {
                string message1 = $"CreateUser?={user.Username},{user.password},{user.emailAddress}";
                byte[] data1 = Encoding.ASCII.GetBytes(message1);
                stream.Write(data1, 0, data1.Length);
            }

            // Send data to the server
            string message = "GETMarkt";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            // Receive data from the server
            while (stream.DataAvailable)
            {
                byte[] responseData = new byte[1024];
                int bytes = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.ASCII.GetString(responseData, 0, bytes);
                var rep = response.Split('\n');
                foreach (var item in rep)
                {
                    switch (item.Split('?').First())
                    {
                        case "LoginResult":
                        {
                            if (!CheckForCorrectUser(item))
                            {
                                Login.Login login = new Login.Login();
                                return;
                            }
                            break;
                        }
                        case "MARKT":
                        {
                            var repo = item.Split('=').Last();
                            Program.markets.Add(repo.Split(',').First(),
                                new Markets(repo.Split(',').First(), Convert.ToDouble(repo.Split(',').Last())));
                            break;
                        }
                    }

                } 
                Thread.Sleep(20);
            }


            // Close everything
            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }

    private static bool CheckForCorrectUser(string message)
    {
        var mes = message.Split('=').Last();
        if (mes == "true")
        {
            return true;
        }
        else if (mes == "false")
        {
            return false;
        }

        return false;
    }
}