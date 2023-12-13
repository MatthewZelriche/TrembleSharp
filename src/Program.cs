using CommandLine;

internal class CommandLineOptions
{
   [Option(
      "no-log-wrapper",
      Required = false,
      HelpText = "Bypass the LogProcessWrapper process and run the engine directly."
   )]
   public bool NoLogWrapper { get; set; }
}

internal partial class Program
{
   private static void Main(string[] args)
   {
      Parser
         .Default
         .ParseArguments<CommandLineOptions>(args)
         .WithParsed<CommandLineOptions>(o =>
         {
            if (o.NoLogWrapper)
            {
               // TODO: Main program execution goes here
               Console.ForegroundColor = ConsoleColor.Yellow;
               Console.WriteLine("Hello world!");
               TrembleInterop.Test();
            }
            else
            {
               var wrapper = new LogProcessWrapper(o, args);
               wrapper.StartChild();
            }
         });
   }
}
