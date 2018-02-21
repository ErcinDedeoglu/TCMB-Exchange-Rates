using System;
using System.Threading;

namespace TCMB.Helper
{
    class ThreadHelper
    {
        public static void ExecuteThread(Action action, bool join = false)
        {
            try
            {
                Thread bigStackThread = new Thread(() => action(), 1024 * 1024);
                bigStackThread.Start();

                if (join)
                {
                    bigStackThread.Join();
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}