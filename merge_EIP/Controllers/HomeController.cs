﻿using merge_EIP.Models;
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
                if (PosID == "0")
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
                int ordercnt = db.tOrder.Where(x => x.fStatus.Contains("開放")).Count();
                ViewBag.order = ordercnt;

                // 加入留言板class
                HomeClass homeClass = new HomeClass()
                {
                    lMsgBd = db.messageBoard.Where(x => x.State == "所有人" || x.assignDepartment == DepID || x.assignPerson == EID || x.employeeID == EID)
                .OrderByDescending(x => x.goTop).ThenByDescending(x => x.messageDate).ToList(),
                    leave = db.dayOff.Where(x => x.employeeID == EID && x.fcheck != true && x.State == "同意" || x.State == "退回").ToList(),
                    OT = db.workOvertime.Where(x => x.employeeID == EID && x.fcheck != true && x.State == "同意" || x.State == "退回").ToList(),
                    repunch = db.rePunchin.Where(x => x.employeeID == EID && x.fcheck != true && x.State == "同意" || x.State == "退回").ToList(),
                    budget = db.Funding.Where(x => x.employeeID == EID && x.fcheck != true && x.State == "同意" || x.State == "退回").ToList(),
                };


                return View(homeClass);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        public ActionResult check(string type, int num)
        {
            if (type == "請假")
            {
                var check = db.dayOff.Find(num);
                check.fcheck = true;
            }
            else if (type == "加班")
            {
                var check = db.workOvertime.Find(num);
                check.fcheck = true;
            }
            else if (type == "經費")
            {
                var check = db.Funding.Find(num);
                check.fcheck = true;
            }
            else if (type == "補卡")
            {
                var check = db.rePunchin.Find(num);
                check.fcheck = true;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // 修改密碼
        public ActionResult ChangePsw()
        {
            var name = Session["Name"].ToString();
            var temp = db.Employee.Where(m => m.Name == name).FirstOrDefault();
            return View(temp);
        }

        [HttpPost]
        public ActionResult ChangePsw(string Password, string newPassword, string Confirmpwd)
        {
            Employee employee = new Employee();
            string name = Session["Name"].ToString();

            var login = db.Employee.Where(m => m.Name == name).FirstOrDefault();
            if (login.Password == Password)
            {
                if (Confirmpwd == newPassword)
                {
                    login.Confirmpwd = Confirmpwd;
                    login.Password = newPassword;
                    db.SaveChanges();
                    TempData["msg"] = "<script>alert('成功修改!!');</script>";
                    ViewBag.msg = "成功";
                }
                else
                {
                    TempData["msg"] = "<script>alert('確認密碼不一致');</script>";
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('舊密碼錯誤!!');</script>";
                ViewBag.msg = "失敗";
            }
            return View();
        }

        // 聯絡人資料
        public ActionResult Contact(string depId = "A")
        {
            string PosID = Convert.ToString(Session["PosID"]);

            if (PosID == "0")
            {
                ViewBag.DepName = db.Department.Where(m => m.departmentID == depId).FirstOrDefault().departmentName + "部門";

                DepEmp de = new DepEmp()
                {
                    department = db.Department.ToList(),
                    employee = db.Employee.Where(m => m.departmentID == depId).ToList()
                };

                return View(de);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}