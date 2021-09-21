using merge_EIP.Models;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace merge_EIP.Controllers
{
    public class ClockinAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();

        // POST: api/Clockin?分數=100&用戶=小明
        public string Post(string EID, string day, string clockin, string bodyTemp)
        {
            var crin = db.punchIn.Where(x => x.punchinDate.ToString() == day && x.employeeID == EID).FirstOrDefault();
            int num = 0;

            if (crin != null)
            {
                crin.employeeID = EID;
                crin.clockIn = TimeSpan.Parse(clockin);
                crin.bodyTemperature = Convert.ToDecimal(bodyTemp);
                num = db.SaveChanges();
            }


            if (num != 0)
            {
                return "打卡成功";
            }
            else
            {
                return "打卡失敗";
            }
        }

        // PUT: api/Clockin/5
        public string Put(string EID, string day, string clockout)
        {
            var crin = db.punchIn.Where(x => x.punchinDate.ToString() == day && x.employeeID == EID).FirstOrDefault();
            int num = 0;

            // 時間轉型後相減
            TimeSpan timeout = TimeSpan.Parse(clockout);
            string clockin = Convert.ToString(crin.clockIn);
            TimeSpan ts = TimeSpan.Parse(clockout) - TimeSpan.Parse(clockin);

            decimal timecot = Convert.ToDecimal(ts.TotalHours);

            if (crin != null)
            {
                crin.clockOut = TimeSpan.Parse(clockout);
                crin.totalHours = timecot;
                num = db.SaveChanges();
            }


            if (num != 0)
            {
                return "打卡成功";
            }
            else
            {
                return "打卡失敗";
            }
        }
    }
}
