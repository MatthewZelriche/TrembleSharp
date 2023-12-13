using System.Runtime.CompilerServices;
using Pastel;

namespace TR;

public enum LogLevel
{
   Trace,
   Info,
   Warn,
   Error,
}

// TODO: Consider a way of stripping out the filepath/line number in Release Mode
/// <summary>
/// Prints formatted log messages to standard out stream.
/// </summary>
///
/// If the engine is being optionally run through LogProcessWrapper, then all log messages
/// through this class will be written to log files as well.
public static class Log
{
   public static ConsoleColor TraceColor { get; set; } = ConsoleColor.White;
   public static ConsoleColor InfoColor { get; set; } = ConsoleColor.White;
   public static ConsoleColor WarnColor { get; set; } = ConsoleColor.Yellow;
   public static ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
   public static LogLevel LogLevel { get; set; } = LogLevel.Info;

   private static readonly string dateFormat = "yyyy/MM/dd HH:mm:ss.fff";

   public static void Trace(
      string message,
      [CallerFilePath] string path = "",
      [CallerLineNumber] int line = 0
   )
   {
      PrintLog(LogLevel.Trace, TraceColor, message, path, line);
   }

   public static void Info(
      string message,
      [CallerFilePath] string path = "",
      [CallerLineNumber] int line = 0
   )
   {
      PrintLog(LogLevel.Info, InfoColor, message, path, line);
   }

   public static void Warn(
      string message,
      [CallerFilePath] string path = "",
      [CallerLineNumber] int line = 0
   )
   {
      PrintLog(LogLevel.Warn, WarnColor, message, path, line);
   }

   public static void Error(
      string message,
      [CallerFilePath] string path = "",
      [CallerLineNumber] int line = 0
   )
   {
      PrintLog(LogLevel.Error, ErrorColor, message, path, line);
   }

   private static void PrintLog(
      LogLevel level,
      ConsoleColor color,
      string message,
      string path,
      int line
   )
   {
      if (LogLevel <= level)
      {
         path = path.Replace(ProjectSourceRoot.Get(), "");
         string formatted =
            $"[{level.ToString().PadRight(5)}] [{DateTime.Now.ToString(dateFormat)}]"
            + $" [{path}:{line}]: {message}";
         Console.WriteLine(formatted.Pastel(color));
      }
   }
}
