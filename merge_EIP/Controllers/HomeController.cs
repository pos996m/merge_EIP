using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class HomeController : Controller
    {
        FormModelEntities db = new FormModelEntities();

        // GET: Home
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                string EID = Convert.ToString(Session["ID"]);
                string PosID = Convert.ToString(Session["PosID"]);
                string DepID = Convert.ToString(Session["DepID"]);


                // 員工顯示今天打卡沒
                DateTime today = DateTime.Today;
                var punchIntemp = db.punchIn.Where(x => x.punchinDate == today && x.employeeID == EID).FirstOrDefault();
                //預設未打卡
                ViewBag.clockin = "未打卡";
                if (punchIntemp.clockIn != null)
                {
                    TimeSpan punchIntime = TimeSpan.Parse(punchIntemp.clockIn.ToString());
                    ViewBag.clockin = punchIntime.ToString(@"hh\:mm");
                }
                // 主管顯示今天還有多少人沒打卡

                // 員工顯示還有幾筆審核中
                int dayOffCnt = db.dayOff.Where(x => x.State == "審核中" && x.employeeID == EID).Count();
                int workOvertimeCnt = db.workOvertime.Where(x => x.State == "審核中" && x.employeeID == EID).Count();
                int FundingCnt = db.Funding.Where(x => x.State == "審核中" && x.employeeID == EID).Count();
                int rePunchCnt = db.rePunchin.Where(x => x.State == "審核中" && x.employeeID == EID).Count();
                // 主管顯示還有幾筆待審核
                if(PosID == "0")
                {
                    dayOffCnt = db.dayOff.Where(x => x.State == "審核中" && x.Employee.departmentID == DepID).Count();
                    workOvertimeCnt = db.workOvertime.Where(x => x.State == "審核中" && x.Employee.departmentID == DepID).Count();
                    FundingCnt = db.Funding.Where(x => x.State == "審核中" && x.Employee.departmentID == DepID).Count();
                    rePunchCnt = db.rePunchin.Where(x => x.State == "審核中" && x.Employee.departmentID == DepID).Count();
                }
                ViewBag.form = dayOffCnt + workOvertimeCnt + FundingCnt + rePunchCnt;

                // 待辦事項員工主管都一樣
                int todolist = db.Backlog.Where(x => x.backlogDate == today && x.employeeID == EID && x.checkState == false).Count();
                ViewBag.todolist = todolist;

                // 訂餐員工主管都一樣
                ViewBag.order = "";
                return View();
            }
            else
            {
                return RedirectToAction("Logout","Login");
            }
        }
    }
}