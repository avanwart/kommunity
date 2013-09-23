using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using DasKlub.Models;
using DasKlub.Models.Forum;
using DasKlub.Models.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace DasKlub.EmailBlasterService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        private static IScheduler _scheduler;


        protected override void OnStart(string[] args)
        {
           
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();
            Console.WriteLine("Starting Scheduler");
            Debug.WriteLine("windowsserv");

            AddJob();
        }


        public static void AddJob()
        {
            const string group1 = "EmailTasks";
            const string trigger1 = "EmailTasksTrigger";
            
            Service1.IMyJob myJob = new Service1.MyJob(); //This Constructor needs to be parameterless
            var jobDetail = new JobDetailImpl("BirthdayUsers", group1, myJob.GetType());
            var trigger = new CronTriggerImpl(trigger1, group1, "0 * 0-23 * * ?");//run every minute between the hours of 8am and 11pm
            _scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime != null) Console.WriteLine("Next Fire Time:" + nextFireTime.Value);
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {

        }

        internal class MyJob : IMyJob
        {
            private void WriteShit()
            {

                // Specify a name for your top-level folder. 
                string folderName = @"c:\testfiles";

                // To create a string that specifies the path to a subfolder under your  
                // top-level folder, add a name for the subfolder to folderName. 
                string pathString = System.IO.Path.Combine(folderName, "SubFolder");

                // You can write out the path name directly instead of using the Combine 
                // method. Combine just makes the process easier. 
                string pathString2 = @"c:\testfiles\SubFolder2";

                // You can extend the depth of your path if you want to. 
                //pathString = System.IO.Path.Combine(pathString, "SubSubFolder");

                // Create the subfolder. You can verify in File Explorer that you have this 
                // structure in the C: drive. 
                //    Local Disk (C:) 
                //        Top-Level Folder 
                //            SubFolder
                System.IO.Directory.CreateDirectory(pathString);

                // Create a file name for the file you want to create.  
                string fileName = System.IO.Path.GetRandomFileName();

                // This example uses a random string for the name, but you also can specify 
                // a particular name. 
                //string fileName = "MyNewFile.txt";

                // Use Combine again to add the file name to the path.
                pathString = System.IO.Path.Combine(pathString, fileName);

                // Verify the path that you have constructed.
                Console.WriteLine("Path to my file: {0}\n", pathString);

                // Check that the file doesn't already exist. If it doesn't exist, create 
                // the file and write integers 0 - 99 to it. 
                // DANGER: System.IO.File.Create will overwrite the file if it already exists. 
                // This could happen even with random file names, although it is unlikely. 
                if (!System.IO.File.Exists(pathString))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                    {
                        for (byte i = 0; i < 100; i++)
                        {
                            fs.WriteByte(i);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File \"{0}\" already exists.", fileName);
                    return;
                }

                // Read and display the data from your file. 
                try
                {
                    var readBuffer = System.IO.File.ReadAllBytes(pathString);
                    foreach (var b in readBuffer)
                    {
                        Console.Write(b + " ");
                    }
                    Console.WriteLine();
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }


                ProcessBirthDayUsers();
               


            }
            

            private static void ProcessBirthDayUsers()
            {
                using (var context = new DasKlubUserDBContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    var ua = new UserAccountEntity();

                    try
                    {
                        var datetime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                        var birthdayUsers =
                            context.UserAccountDetailEntity.Where(p => p.birthDate < DateTime.UtcNow).ToList();


                       
                    }
                    catch (Exception ex)
                    {
                    }


                    //foreach (var category in forumCategory)
                    //{
                    //    var lastPostForum = new ForumPost();

                    //    var category1 = category;
                    //    var subForums =
                    //        context.ForumSubCategory.Where(x => x.ForumCategoryID == category1.ForumCategoryID);


                    //    foreach (var forumSubCategory in subForums)
                    //    {
                    //        category.TotalPosts++;

                    //        var lastPost =
                    //            context.ForumPost.Where(x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID)
                    //                   .OrderByDescending(x => x.CreateDate)
                    //                   .FirstOrDefault();

                    //        if (lastPost == null) continue;

                    //        var forumSubPostCount =
                    //            context.ForumPost.Count(x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID);

                    //        var pageCount = (forumSubPostCount + PageSize - 1) / PageSize;

                    //        lastPost.ForumPostURL =
                    //            new Uri(forumSubCategory.SubForumURL + "/" +
                    //                    ((pageCount > 1)
                    //                         ? pageCount.ToString(CultureInfo.InvariantCulture)
                    //                         : string.Empty) + "#" +
                    //                    lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture));

                    //        lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                    //        if (lastPost.CreateDate <= lastPostForum.CreateDate) continue;

                    //        if (_mu != null)
                    //        {
                    //            var userID = Convert.ToInt32(_mu.ProviderUserKey);

                    //            var isNew =
                    //                context.ForumPostNotification.FirstOrDefault(
                    //                    x =>
                    //                    x.ForumSubCategoryID == lastPost.ForumSubCategoryID &&
                    //                    x.UserAccountID == userID);

                    //            if (isNew != null && !isNew.IsRead)
                    //            {
                    //                lastPost.IsNewPost = true;
                    //            }
                    //        }
                    //        lastPostForum = lastPost;
                    //    }

                    //    category.LatestForumPost = lastPostForum;
                }
            }


            public void Execute(IJobExecutionContext context)
            {
                ProcessBirthDayUsers();

                WriteShit();

                Console.WriteLine("In MyJob class");
                Debug.WriteLine("hitssss");
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
}
