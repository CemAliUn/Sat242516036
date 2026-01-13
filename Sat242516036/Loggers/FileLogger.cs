namespace Sat242516036.Loggers;

public class FileLogger(IWebHostEnvironment env)
{
    public void Log(string message)
    {
        try
        {
            // Proje ana dizininde 'Logs' klasörüne yazar
            string path = Path.Combine(env.ContentRootPath, "Logs");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, $"log_{DateTime.Now:yyyy-MM-dd}.txt");
            string logLine = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";

            File.AppendAllText(filePath, logLine);
        }
        catch { }
    }
}