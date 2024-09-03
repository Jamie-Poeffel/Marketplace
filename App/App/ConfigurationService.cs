using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace App;


public class ConfigurationService
{
    private IConfiguration _configuration;
    private readonly string _filePath;
    private FileSystemWatcher _watcher;

    public ConfigurationService(string filePath)
    {
        _filePath = filePath;
        LoadConfiguration();
        WatchForChanges();
    }

    private void LoadConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(_filePath, optional: false, reloadOnChange: false)
            .Build();
    }

    private void WatchForChanges()
    {
        var directory = Path.GetDirectoryName(_filePath);
        _watcher = new FileSystemWatcher(directory, Path.GetFileName(_filePath))
        {
            NotifyFilter = NotifyFilters.LastWrite
        };
        _watcher.Changed += OnConfigurationFileChanged;
        _watcher.EnableRaisingEvents = true;
    }

    private void OnConfigurationFileChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine("Configuration file changed. Reloading...");
        LoadConfiguration();
        // Notify or trigger any necessary actions in your application
    }

    public string GetSetting(string key)
    {
        return _configuration[key];
    }

    public void SetSetting(string key, string value)
    {
        // Read the existing content of the file
        var json = File.ReadAllText(_filePath);
        var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
        
        // Update the value
        jsonObject[key] = value;
        
        // Write the updated content back to the file
        File.WriteAllText(_filePath, jsonObject.ToString());

        // Reload the configuration
        LoadConfiguration();
    }
}
