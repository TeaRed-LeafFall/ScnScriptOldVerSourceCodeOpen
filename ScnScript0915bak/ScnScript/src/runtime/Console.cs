using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime;
/// <summary>
/// 控制台
/// </summary>
public static class Console
{
    // 写委托与事件
    public delegate void WriteHandler(string message);
    public static event WriteHandler? WriteOutput;

    // 写方法
    public static void Write(string message)
    {
        WriteOutput?.Invoke(message);
    }
    public static void WriteLine(string? message=null)
    {
        WriteOutput?.Invoke(message + Environment.NewLine);
    }

    // 读委托与事件
    public delegate string? ReadHandler(ReadMode mode);
    public static event ReadHandler? ReadInput;
    public enum ReadMode
    {
        Line,
        Key
    }

    // 读方法
    public static string? Read()
    {
        return ReadInput?.Invoke(ReadMode.Key);
    }
    public static string? ReadLine()
    {
        return ReadInput?.Invoke(ReadMode.Line);
    }

    // 日志委托与事件
    public delegate void LogHandler(string message, MessageType type);
    public static event LogHandler? LogOutput;
    public enum MessageType
    {
        Info,
        Warning,
        Error,
        Fatal,
        Debug
    }

    // 日志方法
    public static void LogFatal(string message)
    {
        LogOutput?.Invoke(message, MessageType.Fatal);
    }
    public static void LogError(string message)
    {
        LogOutput?.Invoke(message, MessageType.Error);
    }
    public static void LogWarning(string message)
    {
        LogOutput?.Invoke(message, MessageType.Warning);
    }
    public static void LogInfo(string message)
    {
        LogOutput?.Invoke(message, MessageType.Info);
    }
    [Conditional("DEBUG")]
    public static void LogDebug(string message)
    {
        LogOutput?.Invoke(message, MessageType.Debug);
    }
    
}
