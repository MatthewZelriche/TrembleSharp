using System.Diagnostics;

namespace TR;

public class Engine
{
   IntPtr handle;
   Stopwatch deltaTimeCounter = new();
   double delta = 1 / 60.0f;

   public Engine()
   {
      TrembleError errorCode;
      if ((errorCode = TrembleInterop.engine_create(out handle)) != TrembleError.NO_ERROR)
      {
         Log.Error($"Failed to init TrembleCPP with error: {errorCode}. Shutting down.");
         Environment.Exit((int)errorCode);
      }
   }

   ~Engine()
   {
      // TODO: Destructors apparently don't work the way they do in c++
      TrembleInterop.engine_destroy(handle);
   }

   public void Start()
   {
      // TODO: Eventually we will be running all of our user c# scripts here
      while (!TrembleInterop.engine_requested_shutdown(handle))
      {
         deltaTimeCounter.Start();

         TrembleInterop.engine_update(handle, delta);

         deltaTimeCounter.Stop();
         delta = deltaTimeCounter.Elapsed.TotalSeconds;
         deltaTimeCounter.Reset();
      }
   }
}
