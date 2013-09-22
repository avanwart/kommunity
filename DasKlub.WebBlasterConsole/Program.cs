using System;
using System.Diagnostics;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace DasKlub.WebBlasterConsole
{
    internal class Program
    {
        private static IScheduler _scheduler;

        private static void Main(string[] args)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();
            Console.WriteLine("Starting Scheduler");
            Debug.WriteLine("hi2");

            AddJob();
        }

        public static void AddJob()
        {
            IMyJob myJob = new MyJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl("Job1", "Group1", myJob.GetType());
            var trigger = new CronTriggerImpl("Trigger1", "Group1", "0 * 8-23 * * ?");//run every minute between the hours of 8am and 11pm
            _scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null) Console.WriteLine("Next Fire Time:" + nextFireTime.Value);
        }
    }

    internal class MyJob : IMyJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("In MyJob class");
            Debug.WriteLine("hi");
            DoMoreWork();
        }

        public void DoMoreWork()
        {
            Console.WriteLine("Do More Work");
        }
    }

    internal interface IMyJob : IJob
    {
    }
}
 
