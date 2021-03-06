﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Services;
using DasKlub.Models;
using DasKlub.Models.Forum;
using DasKlub.Models.Models;
using DasKlubModel.Models;
using log4net;
using log4net.Config;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace DasKlub.EmailBlasterService
{
    public partial class ContactService : ServiceBase
    {
        private const string Group1 = "BusinessTasks";
        private const string Job = "Job";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const int MaxEmailsPerSecond = 5;
        private static IScheduler _scheduler;

        public ContactService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            XmlConfigurator.Configure();

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();

            Log.Info(string.Format("Starting Windows Service: {0}", GeneralConfigs.SiteName));

            AddJobs();
        }

        private void AddJobs()
        {
            AddHealthMonitoringJob();
            AddBirthdayJob();
            AddTopForumThreadsJob();
        }

        private void AddTopForumThreadsJob()
        {
            const string trigger1 = "TopForumThreads";
            const string timeToRun = "0 0 1 ? * SAT";

            IMyJob myJob = new TopForumThreadsJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl(trigger1 + Job, Group1, myJob.GetType());
            var trigger = new CronTriggerImpl(trigger1, Group1, timeToRun)
            {TimeZone = TimeZoneInfo.Utc};
            _scheduler.ScheduleJob(jobDetail, trigger);
            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null)
                Log.Info(string.Format("{0}+{1}", Group1, trigger1), new Exception(nextFireTime.Value.ToString("u")));
        }

        public static void AddHealthMonitoringJob()
        {
            const string trigger1 = "HealthMonitoring";
            const string timeToRun = "0 0/10 * * * ?";

            IMyJob myJob = new HealthMonitiorJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl(trigger1 + Job, Group1, myJob.GetType());
            var trigger = new CronTriggerImpl(trigger1, Group1, timeToRun)
            {TimeZone = TimeZoneInfo.Utc};
            _scheduler.ScheduleJob(jobDetail, trigger);
            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null)
                Log.Info(string.Format("{0}+{1}", Group1, trigger1), new Exception(nextFireTime.Value.ToString("u")));
        }

        public static void AddBirthdayJob()
        {
            const string trigger1 = "EmailTasksTrigger";
            const string jobName = trigger1 + Job;
            const string timeToRun = "0 0 2 * * ?";

            IMyJob myJob = new BirthdayJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl(jobName, Group1, myJob.GetType());
            var trigger = new CronTriggerImpl(trigger1, Group1, timeToRun)
            {TimeZone = TimeZoneInfo.Utc};
            _scheduler.ScheduleJob(jobDetail, trigger);
            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null)
                Log.Debug(string.Format("{0}+{1}", Group1, trigger1), new Exception(nextFireTime.Value.ToString("u")));
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            Log.Info("Stopping Windows Service: " + GeneralConfigs.SiteName);
        }

        internal class BirthdayJob : IMyJob
        {
            private readonly IMailService _mail;

            public BirthdayJob()
            {
                _mail = new MailService();
            }

            public void Execute(IJobExecutionContext context)
            {
                ProcessBirthDayUsers();
            }

            public void ProcessBirthDayUsers()
            {
                using (var context = new DasKlubUserDBContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    try
                    {
                        IQueryable<UserAccountDetailEntity> results = from t in context.UserAccountDetailEntity
                            where
                                t.birthDate.Month == DateTime.UtcNow.Month &&
                                t.birthDate.Day == DateTime.UtcNow.Day &&
                                t.emailMessages
                            select t;

                        List<UserAccountDetailEntity> users = results.ToList();

                        foreach (UserAccountEntity user in users.Select(
                            birthdayUser => context != null
                                ? context.UserAccountEntity
                                    .FirstOrDefault(usr => usr.userAccountID == birthdayUser.userAccountID)
                                : null)
                            .Where(user => user != null))
                        {
                            if (user.createDate == null) continue;

                            string signUpDate = string.Format("{0} {1}, {2}",
                                user.createDate.Value.ToString("MMM"),
                                user.createDate.Value.Day,
                                user.createDate.Value.Year);

                            System.Threading.Thread.Sleep(1000 / MaxEmailsPerSecond);

                            _mail.SendMail(AmazonCloudConfigs.SendFromEmail, user.eMail,
                                string.Format("Happy Birthday {0}!", user.userName),
                                string.Format(
                                    "Happy birthday from Das Klub! {1}{1} Visit: {0} {1}{1} Membership Sign Up Date: {1}{1}{2}",
                                    GeneralConfigs.SiteDomain, Environment.NewLine, signUpDate));

                            Log.Info(string.Format("Sent to: {0}", user.eMail));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }
        }

        public class HealthMonitiorJob : IMyJob
        {
            public void Execute(IJobExecutionContext context)
            {
                Log.Info(DateTime.UtcNow);
            }
        }

        internal interface IMyJob : IJob
        {
        }

        public class TopForumThreadsJob : IMyJob
        {
            private readonly IMailService _mail;

            public TopForumThreadsJob()
            {
                _mail = new MailService();
            }

            public void Execute(IJobExecutionContext context)
            {
                using (var contextDb = new DasKlubDbContext())
                {
                    var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

                    const int totalTopAmount = 3;

                    var mostPopularForumPosts =
                        contextDb.ForumPost
                            .Where(x => x.CreateDate > oneWeekAgo)
                            .GroupBy(x => x.ForumSubCategoryID)
                            .OrderByDescending(y => y.Count())
                            .Take(totalTopAmount)
                            .ToList();

                    if (mostPopularForumPosts.Count == totalTopAmount)
                    {
                        var threads = new StringBuilder();

                        foreach (var item in mostPopularForumPosts)
                        {
                            ForumSubCategory forumThread =
                                contextDb.ForumSubCategory
                                    .FirstOrDefault(x => x.ForumSubCategoryID == item.Key);

                            ForumCategory forum =
                                contextDb.ForumCategory
                                    .FirstOrDefault(x => x.ForumCategoryID == forumThread.ForumCategoryID);

                            threads.Append(forumThread.Title);
                            threads.AppendLine();
                            threads.AppendFormat("{0}/forum/{1}/{2}", GeneralConfigs.SiteDomain, forum.Key,
                                forumThread.Key);
                            threads.AppendLine();
                            threads.Append("------------------------------");
                            threads.AppendLine();
                            threads.AppendLine();
                        }

                        string top3Threads = threads.ToString();

                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        Calendar cal = dfi.Calendar;
                        int weekNumber = cal.GetWeekOfYear(DateTime.UtcNow, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

                        string title = string.Format("TOP {0} Forum Threads [Week: {1}, {2}]",
                            totalTopAmount,
                            weekNumber,
                            DateTime.UtcNow.Year);

                        var uas = new UserAccounts();
                        uas.GetAll();

                        foreach (UserAccount user in uas)
                        {
                            var uad = new UserAccountDetail();
                            uad.GetUserAccountDeailForUser(user.UserAccountID);

                            if (!uad.EmailMessages || uad.UserAccountDetailID == 0) continue;

                            string message = string.Format(
                                                "Hello {0}! {1}{1}{2}", 
                                                user.UserName,
                                                Environment.NewLine,
                                                top3Threads);

                            System.Threading.Thread.Sleep(1000 / MaxEmailsPerSecond);

                            _mail.SendMail(AmazonCloudConfigs.SendFromEmail,
                                user.EMail,
                                title,
                                message);

                            Log.Info(string.Format("Sent top 3 to: {0}", user.EMail));
                        }
                    }
                    else
                    {
                        Log.Info("Not enough forum activity to mail users");
                    }
                }
            }
        }
    }
}