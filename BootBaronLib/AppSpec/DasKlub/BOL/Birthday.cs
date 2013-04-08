using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Birthday  
    {

        public Birthday(DataRow dr)
        {
            Get(dr);
        }


        public int UserAccountID { get; set; }

        public DateTime Birthdate { get; set; }

        public int AgeNow { get; set; }

        public int AgeAfter { get; set; }

        public  void Get(DataRow dr)
        {

            UserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountID)]);
            Birthdate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => Birthdate)]);
            AgeNow = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => AgeNow)]);
            AgeAfter = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => AgeAfter)]);
        }
    }

    public class Birhtdays : List<Birthday>
    {
          public void GetBirhtdays(int daysForward)
          {
              // get a configured DbCommand object
              var comm = DbAct.CreateCommand();
              
              // set the stored procedure name
              comm.CommandText = "up_GetBirhtdays";

              comm.AddParameter("daysForward", daysForward);

              // execute the stored procedure
              var dt = DbAct.ExecuteSelectCommand(comm);

              if (dt.Rows.Count <= 0) return;

              foreach (var pitm in from DataRow dr in dt.Rows select new Birthday(dr))
              {
                  Add(pitm);
              }
          }
    }
}
