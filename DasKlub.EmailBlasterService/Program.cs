using System;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace DasKlub.EmailBlasterService
{
    internal class Program
    {
      
        private static void Main(string[] args)
        {
#if DEBUG
            var service1 = new Service1();
            service1.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            

#else
     ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }


    }

   
}
