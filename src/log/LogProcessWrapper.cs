using System.Diagnostics;
using System.Text.RegularExpressions;

/// <summary>
/// Wraps the engine in a Logger process in order to capture both managed and unmanaged
/// standard output
/// </summary>
///
/// TrembleSharp uses an unmanaged C++ DLL that regularly prints log data to stdout.
/// While these DLL stdout logs print to the console just fine, it is difficult to capture
/// this stdout data and redirect it to a file from within the process.
/// Instead, our engine becomes a child of a small wrapper parent process so that we can
/// redirect the child process stdout and capture all data being written to it, from both
/// the managed and unmanaged code
internal partial class LogProcessWrapper()
{
   Process wrappedProcess = new();

   // Constructor is guarunteed to succeed with logFile non-null, sowe use null-forgiving
   // operator to suppress warning
   StreamWriter logFile = null!;

   public LogProcessWrapper(CommandLineOptions options, string[] args)
      : this()
   {
      // We pass --no-log-wrapper to the child process to avoid being stuck in
      // an infinite loop of creating Wrapper processes
      var startInfo = new ProcessStartInfo(
         "TrembleSharp",
         args.Append("--no-log-wrapper")
      );
      startInfo.UseShellExecute = false;
      startInfo.RedirectStandardOutput = true;

      wrappedProcess.StartInfo = startInfo;
      wrappedProcess.EnableRaisingEvents = true;

      // Set up new timestamped log file, as well as erase old logs
      Directory.CreateDirectory("logs/");
      DeleteOldLogs();
      var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
      logFile = new StreamWriter($"logs/log_{timestamp}.txt");
      logFile.AutoFlush = true;

      wrappedProcess.OutputDataReceived += RedirectStdOut;
   }

   /// <summary>
   /// Starts the engine as a child process, blocking execution of the parent until
   /// the child process exits
   /// </summary>
   public void StartChild()
   {
      wrappedProcess.Start();
      wrappedProcess.BeginOutputReadLine();
      wrappedProcess.WaitForExit();
   }

   private void DeleteOldLogs()
   {
      var files = Directory.GetFiles("logs/");

      var filesToDelete = files
         .Where(file => OldLogsRegex().IsMatch(file))
         .Select(file => new FileInfo(file))
         .Where(info => info.CreationTime < DateTime.Now.AddDays(-7));

      foreach (FileInfo info in filesToDelete)
      {
         info.Delete();
      }
   }

   /// <summary>
   /// Called every time our child process receives a new line of data to its standard
   /// output stream
   /// </summary>
   private void RedirectStdOut(object sender, DataReceivedEventArgs e)
   {
      if (e.Data is null)
      {
         // For some reason, our callback fired but we have no data, so just bail early
         return;
      }

      // Split our redirected standard out to both log file and console
      logFile.WriteLine(StripAnsiRegex().Replace(e.Data, ""));
      Console.WriteLine(e.Data);
   }

   [GeneratedRegex("\x1b(.*?)m")]
   private static partial Regex StripAnsiRegex();

   [GeneratedRegex("log_[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}.[0-9]{2}.[0-9]{2}.txt")]
   private static partial Regex OldLogsRegex();
}
