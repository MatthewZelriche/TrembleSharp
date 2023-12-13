using System.Runtime.CompilerServices;

internal class ProjectSourceRoot
{
   static readonly string sourceRoot = CachePath();

   /// <summary>
   /// Returns the full filesystem path to the source directory containing the
   /// application's csproj file, including a trailing slash
   /// </summary>
   public static string Get()
   {
      return sourceRoot;
   }

   private static string CachePath([CallerFilePath] string sourceRoot = "")
   {
      string? path = Path.GetDirectoryName(sourceRoot);
      return (path is not null) ? path + Path.DirectorySeparatorChar : "";
   }
}
