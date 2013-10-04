﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using DasKlub.Lib.Services;
using DasKlub.Models;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace DasKlub.EmailBlasterService
{
    public partial class ContactService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ContactService()
        {
            InitializeComponent();
        }

        private static IScheduler _scheduler;

        protected override void OnStart(string[] args)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();
            
            Log.Info("Starting Windows Service");
            Debug.WriteLine("windowsserv");


            AddJobs();
        }

        private void AddJobs()
        {
            AddBirthdayJob();
            AddInactiveReminderJob();
        }

        private void AddInactiveReminderJob()
        {
           // TODO: every week email all who are under 30 years old and have not signed in for at least 3 weeks a reminder
        }


        public static void AddBirthdayJob()
        {
            const string group1 = "EmailTasks";
            const string trigger1 = "EmailTasksTrigger";
            
            IMyJob myJob = new BirthdayJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl("BirthdayUsers", group1, myJob.GetType());
            //var trigger = new CronTriggerImpl(trigger1, group1, "0 * 0-23 * * ?");//run every minute 
            var trigger = new CronTriggerImpl(trigger1, group1, "0 0 2 * * ?") {TimeZone = TimeZoneInfo.Utc};// every day at 2 in the morning UTC
            _scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null) Console.WriteLine("Next Fire Time:{0}", nextFireTime.Value.ToString("u"));
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {

        }

        internal class BirthdayJob : IMyJob
        {
            private readonly IMailService _mail;
 
            public BirthdayJob()
            {
                 _mail = new MailService();
            }

            private void ProcessBirthDayUsers()
            {
                using (var context = new DasKlubUserDBContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    try
                    {
                        var results = from t in context.UserAccountDetailEntity
                            where t.birthDate.Month == DateTime.UtcNow.Month && t.birthDate.Day == DateTime.UtcNow.Day
                            select t;

                        var users = results.ToList();

                        foreach (var user in users.Select(birthdayUser => context != null
                            ? context.UserAccountEntity.FirstOrDefault(
                                usr => usr.userAccountID == birthdayUser.userAccountID)
                            : null).Where(user => user != null))
                        {
                            _mail.SendMail("dasklubber@gmail.com", user.eMail,
                                string.Format("Happy Birthday {0}!", user.userName), "Happy birthday!");
                            Log.Info("Sent to: " + user.eMail);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }

            public void Execute(IJobExecutionContext context)
            {
                ProcessBirthDayUsers();
            }

        }

        internal interface IMyJob : IJob
        {
        }
    }
}
