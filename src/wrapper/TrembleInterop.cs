using System.Runtime.InteropServices;

partial class TrembleInterop
{
   [LibraryImport("Tremble")]
   public static partial IntPtr engine_create();

   [LibraryImport("Tremble")]
   public static partial IntPtr engine_update(IntPtr handle, double delta);
}
