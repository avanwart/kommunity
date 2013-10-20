using System;
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
        private const string Group1 = "BusinessTasks";
        private const string Job = "Job";
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

            Log.Info("Starting Windows Service: " + Lib.Configs.GeneralConfigs.SiteName);

            AddJobs();
        }

        private void AddJobs()
        {
            AddHealthMonitoringJob();
            AddBirthdayJob();
            AddInactiveReminderJob();
        }

        private void AddInactiveReminderJob()
        {
            // TODO: every week email all who are under 30 years old and have not signed in for at least 3 weeks a reminder
        }

        public static void AddHealthMonitoringJob()
        {
            const string trigger1 = "HealthMonitoring";

            IMyJob myJob = new HealthMonitiorJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl(trigger1 + Job, Group1, myJob.GetType());
            // run every 5 minutes
            var trigger = new CronTriggerImpl(trigger1, Group1, "0 0/10 * * * ?" /* every 10 minutes */) {TimeZone = TimeZoneInfo.Utc}; 
            _scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null)
                Log.Info(Group1 + "+" + trigger1, new Exception(nextFireTime.Value.ToString("u")));
        }

        public class HealthMonitiorJob : IMyJob
        {
            public void Execute(IJobExecutionContext context)
            {
                Log.Info(DateTime.UtcNow);
            }
        }

        public static void AddBirthdayJob( )
        {
            const string trigger1 = "EmailTasksTrigger";
            const string jobName = trigger1 + Job;
            IMyJob myJob = new BirthdayJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl(jobName, Group1, myJob.GetType());
            var trigger = new CronTriggerImpl(trigger1, Group1, "0 0 2 * * ?"  /* run every day at 2:00 UTC */ ) {TimeZone = TimeZoneInfo.Utc}; 
            _scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null)
                Log.Debug(Group1 + "+" + trigger1, new Exception(nextFireTime.Value.ToString("u")));
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            Log.Info("Stopping Windows Service: " + Lib.Configs.GeneralConfigs.SiteName);
        }

        internal class BirthdayJob : IMyJob
        {
            private readonly IMailService _mail;

            public BirthdayJob()
            {
                _mail = new MailService();
            }

            public void ProcessBirthDayUsers()
            {
                using (var context = new DasKlubUserDBContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    try
                    {
                        var results = from t in context.UserAccountDetailEntity
                            where
                                t.birthDate.Month == DateTime.UtcNow.Month && t.birthDate.Day == DateTime.UtcNow.Day  &&
                                t.emailMessages == true
                            select t;

                        var users = results.ToList();

                        foreach (var user in users.Select(birthdayUser => context != null
                            ? context.UserAccountEntity.FirstOrDefault(
                                usr => usr.userAccountID == birthdayUser.userAccountID)
                            : null).Where(user => user != null))
                        {
                            var signUpDate = string.Format("{0} {1}, {2}", 
                                                user.createDate.Value.ToString("MMM"), 
                                                user.createDate.Value.Day, 
                                                user.createDate.Value.Year);

                            _mail.SendMail(Lib.Configs.AmazonCloudConfigs.SendFromEmail, user.eMail,
                                string.Format("Happy Birthday {0}!", user.userName),
                                string.Format("Happy birthday from Das Klub! {1}{1} Visit: {0} {1}{1} Membership Sign Up Date: {1}{1}{2}",
                                    Lib.Configs.GeneralConfigs.SiteDomain, Environment.NewLine, signUpDate));
                            Log.Info("Sent to: " + user.eMail);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
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