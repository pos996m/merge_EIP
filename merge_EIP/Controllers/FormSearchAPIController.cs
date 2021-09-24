using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace merge_EIP.Controllers
{
    public class FormSearchAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();
        // GET: api/FormSearchAPI
        public JsonResult<List<dayOff>> Get(string EID, string PosID)
        {
            if (PosID == "1")
            {
                var lv = db.dayOff.Where(m => m.employeeID == EID).OrderByDescending(m => m.dayoffNumber).ToList();
                var bd = db.Funding.Where(m => m.employeeID == EID).OrderByDescending(m => m.applicationNumber).ToList();
                var ot = db.workOvertime.Where(m => m.employeeID == EID).OrderByDescending(m => m.overtimeNumber).ToList();
                var re = db.rePunchin.Where(m => m.employeeID == EID).OrderByDescending(m => m.repunchID).ToList();

                Search all = new Search() { leave = lv, budget = bd, OT = ot, repunch = re };

                return Json(lv);
            }
            else
            {
                List<dayOff> eqwe = new List<dayOff>();
                return Json(eqwe);
            }
        }

        // GET: api/FormSearchAPI/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
