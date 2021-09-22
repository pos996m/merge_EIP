using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class FormController : Controller
    {
        FormModelEntities db = new FormModelEntities();

        // GET - 表單申請
        public ActionResult Leave()
        {
            return View();
        }

        // POST - 表單申請
        [HttpPost]
        public ActionResult Leave(dayOff tdayOff)
        {
            tdayOff.State = "審核中";
            tdayOff.employeeID = Session["ID"].ToString();
            tdayOff.employeeName = Session["Name"].ToString();

            var start = Convert.ToDateTime(tdayOff.startDate);
            var end = Convert.ToDateTime(tdayOff.endDate);

            var temp = Convert.ToInt32(new TimeSpan(end.Ticks - start.Ticks).TotalHours);

            var d = (temp / 24).ToString();
            var h = (temp % 24).ToString();
            tdayOff.totalDay = $"{d}日 {h}時";

            // 上傳檔案
            if (tdayOff.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(tdayOff.ImageFile.FileName);
                string extension = Path.GetExtension(tdayOff.ImageFile.FileName);
                fileName = DateTime.Now.ToString("yymmssfff") + extension;
                tdayOff.filePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image"), fileName);
                tdayOff.ImageFile.SaveAs(fileName);
            }

            db.dayOff.Add(tdayOff);
            db.SaveChanges();

            Session["mypost"] = "FOK";

            return RedirectToAction("Index", "Clockin");
        }

        // 附件檔案
        public ActionResult Img(int? fId)
        {
            if (Session["ID"] != null)
            {
                dayOff image = new dayOff();
                image = db.dayOff.Where(m => m.dayoffNumber == fId).FirstOrDefault();

                return View(image);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        // GET - 加班單申請
        public ActionResult Overtime()
        {
            return View();
        }

        //POST - 加班單申請
        [HttpPost]
        public ActionResult Overtime(workOvertime tworkOvertime)
        {
            tworkOvertime.State = "審核中";
            tworkOvertime.employeeID = Session["ID"].ToString();
            tworkOvertime.employeeName = Session["Name"].ToString();

            var start = tworkOvertime.startTime.ToString();
            var end = tworkOvertime.expectTime.ToString();

            var int_time = Convert.ToDecimal((DateTime.Parse(end) - DateTime.Parse(start)).TotalHours);

            var temp = int_time % 100;
            tworkOvertime.overtimeHours = $"{temp.ToString("F2")} 小時";

            db.workOvertime.Add(tworkOvertime);
            db.SaveChanges();
            Session["mypost"] = "FOK";
            return RedirectToAction("Index", "Clockin");
        }

        // GET - 經費申請
        public ActionResult Funding()
        {
            return View();
        }

        // POST - 經費申請
        [HttpPost]
        public ActionResult Funding(Funding tfunding)
        {
            tfunding.State = "審核中";
            tfunding.employeeID = Session["ID"].ToString();
            tfunding.employeeName = Session["Name"].ToString();

            db.Funding.Add(tfunding);
            db.SaveChanges();
            Session["mypost"] = "FOK";
            return RedirectToAction("Index", "Clockin");
        }

        // GET - 補打卡
        public ActionResult rePunch()
        {
            return View();
        }

        // POST - 補打卡
        [HttpPost]
        public ActionResult rePunch(rePunchin trePunchin)
        {
            trePunchin.State = "審核中";
            trePunchin.employeeID = Session["ID"].ToString();
            trePunchin.employeeName = Session["Name"].ToString();
            if (trePunchin.Reason == null)
            {
                trePunchin.Reason = "無備註";
            }

            db.rePunchin.Add(trePunchin);
            db.SaveChanges();
            Session["mypost"] = "FOK";
            return RedirectToAction("Index", "Clockin");
        }

        // 員工查詢
        public ActionResult Search()
        {
            // 判斷是否有登入
            if (Session["ID"] != null)
            {
                string posID = Convert.ToString(Session["PosID"]);
                if (posID == "1")
                {
                    var id = Convert.ToString(Session["ID"]);
                    var lv = db.dayOff.Where(m => m.employeeID == id).OrderByDescending(m => m.dayoffNumber).ToList();
                    var bd = db.Funding.Where(m => m.employeeID == id).OrderByDescending(m => m.applicationNumber).ToList();
                    var ot = db.workOvertime.Where(m => m.employeeID == id).OrderByDescending(m => m.overtimeNumber).ToList();
                    var re = db.rePunchin.Where(m => m.employeeID == id).OrderByDescending(m => m.repunchID).ToList();

                    Search all = new Search() { leave = lv, budget = bd, OT = ot, repunch = re };
                    return View(all);
                }
                else
                {
                    return RedirectToAction("Approve");
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        // 老闆審核
        public ActionResult Approve()
        {
            if (Session["ID"] != null)
            {
                string posID = Convert.ToString(Session["PosID"]);
                string Dep = Convert.ToString(Session["Dep"]);

                if (posID == "0")
                {
                    // 判斷同部門的上司才可以看得到自己的審核
                    var lv = db.dayOff.Where(m => m.State.Contains("審核") && m.Employee.Department.departmentName == Dep).ToList();
                    var bd = db.Funding.Where(m => m.State.Contains("審核") && m.Employee.Department.departmentName == Dep).ToList();
                    var ot = db.workOvertime.Where(m => m.State.Contains("審核") && m.Employee.Department.departmentName == Dep).ToList();
                    var re = db.rePunchin.Where(m => m.State.Contains("審核") && m.Employee.Department.departmentName == Dep).ToList();

                    int lv_qty = lv.Count;
                    int bd_qty = bd.Count;
                    int ot_qty = ot.Count;
                    int re_qty = re.Count;

                    foreach (var item in lv)
                    {
                        // 上傳檔案
                        if (item.filePath == null)
                        {
                            item.filePath = "";
                        }
                        else
                        {
                            item.filePath = "查看附件";
                        }
                    }

                    ViewBag.lv_qty = lv_qty;
                    ViewBag.bd_qty = bd_qty;
                    ViewBag.ot_qty = ot_qty;
                    ViewBag.re_qty = re_qty;

                    Search all = new Search() { leave = lv, budget = bd, OT = ot, repunch = re };
                    return View(all);
                }
                else
                {
                    return RedirectToAction("Search");
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        // 假單同意
        public ActionResult Yes(int fId)
        {
            var temp = db.dayOff.Where(m => m.dayoffNumber == fId).FirstOrDefault();
            temp.State = "同意";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 假單退回
        public ActionResult No(int fId)
        {
            var temp = db.dayOff.Where(m => m.dayoffNumber == fId).FirstOrDefault();
            temp.State = "退回";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 加班單同意
        public ActionResult OTYes(int fId)
        {
            var temp = db.workOvertime.Where(m => m.overtimeNumber == fId).FirstOrDefault();
            temp.State = "同意";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 加班單退回
        public ActionResult OTNo(int fId)
        {
            var temp = db.workOvertime.Where(m => m.overtimeNumber == fId).FirstOrDefault();
            temp.State = "退回";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 經費同意
        public ActionResult bdYes(int fId)
        {
            var temp = db.Funding.Where(m => m.applicationNumber == fId).FirstOrDefault();
            temp.State = "同意";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 經費退回
        public ActionResult bdNo(int fId)
        {
            var temp = db.Funding.Where(m => m.applicationNumber == fId).FirstOrDefault();
            temp.State = "退回";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 補打卡同意
        public ActionResult reYes(int fId)
        {
            var temp = db.rePunchin.Where(m => m.repunchID == fId).FirstOrDefault();
            temp.State = "同意";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }

        // 補打卡退回
        public ActionResult reNo(int fId)
        {
            var temp = db.rePunchin.Where(m => m.repunchID == fId).FirstOrDefault();
            temp.State = "退回";

            db.SaveChanges();
            return RedirectToAction("Approve", "Form");
        }
    }
}