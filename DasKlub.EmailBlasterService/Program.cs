﻿using System.ServiceProcess;

namespace DasKlub.EmailBlasterService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            var service1 = new ContactService();
            service1.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ContactService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}