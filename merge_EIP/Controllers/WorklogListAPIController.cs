using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace merge_EIP.Controllers
{
    public class WorklogListAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();

        // POST: api/WorklogListAPI
        public string Post(int Num, int clock, string Text)
        {
            var cain = db.Backlog.Where(x => x.backlogNumber == Num).FirstOrDefault();
            try
            {
                if (Text == null)
                {
                    cain.checkState = Convert.ToBoolean(clock);
                    db.SaveChanges();
                    return "clock存檔成功";
                }
                else
                {
                    cain.backlogTxet = Text;
                    db.SaveChanges();
                    return "編輯成功";
                }
            }
            catch (Exception)
            {

                return "存檔失敗";
            }
        }
    }
}
