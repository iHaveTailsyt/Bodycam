using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Bodycam.Utilities
{
    public static class Logger
    {
        private static readonly string logDirectory = @"C:\Bodycam\Logs";
        private static readonly string logFilePath = Path.Combine(logDirectory, "Bodycam_log.json");

        static Logger()
        {
            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            if (!File.Exists(logFilePath))
            {
                File.WriteAllText(logFilePath, "[]"); // Start with a empty JSON array
                // ^^ Fix the bug were if the file exsits but doesnt have the empty array it wont log
            }
        }

        public static void Log(string command, string action)
        {
            var logEntry = new LogEntry
            {
                Command = command,
                Action = action,
                Timestamp = DateTime.Now
            };

            var logEntries = new List<LogEntry>();

            // Read existing logs
            try
            {
                var json = File.ReadAllText(logFilePath);
                logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log File: {ex.Message}");
            }

            // Add the new log entry
            logEntries.Add(logEntry);
            
            // Write logs back to the file
            try
            {
                var updatedJson = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
                File.WriteAllText(logFilePath, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing log file: {ex.Message}");
            }
        }

        private class LogEntry
        {
            public string Command { get; set; }
            public string Action { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}