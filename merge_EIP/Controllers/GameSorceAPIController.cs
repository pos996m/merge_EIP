using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace merge_EIP.Controllers
{
    public class GameSorceAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();
        // POST: api/GameSorceAPI
        public string Post(string EID, int sorce,string type)
        {
            gameRecord gameRecord = new gameRecord()
            {
                employeeID = EID,
                employeeName = db.Employee.Where(x=>x.employeeID == EID).FirstOrDefault().Name,
                Fraction = sorce,
                Type = type
            };

            db.gameRecord.Add(gameRecord);
            db.SaveChanges();
            return "分數儲存成功";
        }

    }
}
