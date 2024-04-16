using System.Text.Json.Serialization;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
public class FileLogger : ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(LogRecord log)
    {
        var jsonString = JsonSerializer.Serialize(log);
        File.AppendAllText(_filePath, jsonString + Environment.NewLine);
    }
}