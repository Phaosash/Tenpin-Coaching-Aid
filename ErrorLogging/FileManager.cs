using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ErrorLogging;

internal class FileManager {
        private readonly Lock _fileLock = new();
    private readonly int _retentionPeriod;
    private readonly string _logsDirectory;

    //  The constructor sets the log retention period, initializes the logs directory path, and triggers cleanup of old log files.
    public FileManager (int logRetentionPeriod){
        _retentionPeriod = logRetentionPeriod;

        _logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        CleanUpOldLogs(_retentionPeriod);
    }

    //  This method formats a log message with context details and writes it asynchronously to a log file.
    public void Log<TState> (LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter,
                            [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0){
        ArgumentNullException.ThrowIfNull(formatter);

        var logMessage = BuildLogMessage(logLevel, state, exception, formatter, memberName, filePath, lineNumber);

        WriteLogToFileAsync(logMessage);
    }

    //  This method constructs a detailed log message string including the timestamp, log level, formatted message, and source code location.
    private static string BuildLogMessage<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter, string memberName, string filePath, int lineNumber){
        return $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} [{logLevel}] {formatter(state, exception)}" + $"\nSource: {Path.GetFileName(filePath)}.{memberName} Line: {lineNumber}";
    }

    //  This asynchronous method writes the log message to a file in a thread-safe manner and handles any write errors gracefully.
    private async void WriteLogToFileAsync (string logMessage){
        try {
            await Task.Yield();

            lock (_fileLock){
                LogToFile(logMessage);
            }
        } catch (Exception ex){
            Debug.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    //  This method ensures the logs directory exists and appends the log message to a daily log file, handling any file write exceptions gracefully.
    private void LogToFile (string logMessage){
        try {
            if (!Directory.Exists(_logsDirectory)){
                Directory.CreateDirectory(_logsDirectory);
            }

            string filePath = Path.Combine(_logsDirectory, $"ApplicationLogs_{DateTime.Now:dd-MM-yyyy}.txt");
            File.AppendAllText(filePath, logMessage + Environment.NewLine);
        } catch (Exception ex){
            Debug.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    //  This method deletes log files older than the specified retention period to maintain storage hygiene, handling any errors during cleanup.
    private void CleanUpOldLogs (int retentionDays){
        try {
            if (Directory.Exists(_logsDirectory)){
                var logFiles = Directory.GetFiles(_logsDirectory, "ApplicationLogs_*.txt");

                foreach (var logFile in logFiles){
                    DateTime creationDate = File.GetCreationTime(logFile);

                    if (creationDate < DateTime.Now.AddDays(-retentionDays)){
                        File.Delete(logFile);
                    }
                }
            }
        } catch (Exception ex){
            Debug.WriteLine($"Error cleaning up old log files: {ex.Message}");
        }
    }
}