using System.Runtime.InteropServices;

enum TrembleError
{
   NO_ERROR,
   UNKNOWN_ERROR,
   THIRD_PARTY_INIT_ERROR,
   THIRD_PARTY_RUNTIME_ERROR,
};

partial class TrembleInterop
{
   [LibraryImport("Tremble")]
   public static partial TrembleError engine_create(out IntPtr handle);

   [LibraryImport("Tremble")]
   public static partial void engine_destroy(IntPtr handle);

   [LibraryImport("Tremble")]
   [return: MarshalAs(UnmanagedType.Bool)]
   public static partial Boolean engine_requested_shutdown(IntPtr handle);

   [LibraryImport("Tremble")]
   public static partial void engine_update(IntPtr handle, double delta);
}
