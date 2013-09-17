using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DasKlub.EmailBlaster
{
    /*
     * TODO:
     * -each day email people with birthdays a Happy Birthday message
     * -every week email people who have not signed in for over 2 weeks and are under 30 years old a 'you're missing out' message
     * -each day write the count of users who have logged in within the last 24 hours to a table
     * -remove beatdowns from status updates and comments
     * -add back the ability for people to view who saw them
     * 
     * 
     */ 
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
