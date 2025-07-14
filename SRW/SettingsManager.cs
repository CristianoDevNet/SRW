using System;
using System.IO;
using System.Text.Json;

namespace SRW;

public static class SettingsManager
{
    private static readonly string settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

    public static AppSettings Load()
    {
        if (!File.Exists(settingsFile))
            return new AppSettings();

        try
        {
            var json = File.ReadAllText(settingsFile);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(settingsFile, json);
    }
}