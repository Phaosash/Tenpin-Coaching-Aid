using Microsoft.Extensions.Logging;

namespace ErrorLogging;

public sealed class LoggingManager {
    private const int DefaultRetentionPeriod = 7;
    private static readonly Lazy<LoggingManager> _instance = new(() => new LoggingManager(DefaultRetentionPeriod));
    private readonly FileManager _fileManager;
    private readonly int _fileRetentionPeriod;

    //  The constructor initializes the logging manager with a file retention period, defaulting if a negative value is provided.
    private LoggingManager (int fileRetentionPeriod){
        _fileRetentionPeriod = fileRetentionPeriod < 0 ? DefaultRetentionPeriod : fileRetentionPeriod;
        _fileManager = new FileManager(_fileRetentionPeriod);
    }

    //  The Instance property provides a singleton instance of the LoggingManager.
    public static LoggingManager Instance => _instance.Value;

    //  This method is used to record an informational message using the file manager.
    public void LogInformation (string message){
        _fileManager.Log(LogLevel.Information, message, null, (state, exception) => $"Message: {state}");
    }

    //  This method is used to record an error message along with the exception details.
    public void LogError (Exception ex, string message){
        _fileManager.Log(LogLevel.Error, message, ex, (state, exception) => $"{state}: {exception?.Message}\nStackTrace: {exception?.StackTrace}");
    }

    //  This method is used to record a warning message via the file manager.
    public void LogWarning (string message){
        _fileManager.Log(LogLevel.Warning, message, null, (state, exception) => $"Warning: {state}");
    }
}