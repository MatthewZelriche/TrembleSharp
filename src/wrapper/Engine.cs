using System.Diagnostics;

namespace TR;

public class Engine
{
   IntPtr handle;
   Stopwatch deltaTimeCounter = new Stopwatch();
   double delta = 1 / 60.0f;

   public Engine()
   {
      handle = TrembleInterop.engine_create();
   }

   public void Start()
   {
      // TODO: Hook up to DLL exit request
      // TODO: Eventually we will be running all of our user c# scripts here
      while (true)
      {
         deltaTimeCounter.Start();

         TrembleInterop.engine_update(handle, delta);

         deltaTimeCounter.Stop();
         delta = deltaTimeCounter.Elapsed.TotalSeconds;
         deltaTimeCounter.Reset();
      }
   }
}
