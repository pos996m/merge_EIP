using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using merge_EIP.Models;
using PagedList;

namespace merge_EIP.Controllers
{
    public class MessageController : Controller
    {
        FormModelEntities db = new FormModelEntities();

        // 幾筆資料一頁
        int pageSize = 3;

        // GET: Message
        public ActionResult MsgIndex(int page = 1)
        {
            string EID = Convert.ToString(Session["ID"]);
            string PosID = Convert.ToString(Session["PosID"]);
            string DepID = Convert.ToString(Session["DepID"]);

            // 先撈所有可看見的
            var showMsg = db.messageBoard.Where(x => x.State == "所有人" || x.assignDepartment == DepID || x.assignPerson == EID)
                .OrderBy(x => x.goTop).ThenByDescending(x => x.messageDate).ToList();

            int currpag = page < 1 ? 1 : page;
            var result = showMsg.ToPagedList(currpag, pageSize);

            return View(result);
        }

        // 新增頁面
        public ActionResult Fnewmsg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Fnewmsg(messageBoard messageBoard)
        {
            string EID = Convert.ToString(Session["ID"]);
            string Ename = Convert.ToString(Session["Name"]);
            string Edep = Convert.ToString(Session["DepID"]);

            messageBoard.employeeID = EID;
            messageBoard.employeeName = Ename;
            messageBoard.messageDate = DateTime.Now;

            if (messageBoard.State == "部門")
            {
                messageBoard.assignDepartment = Edep;
            }
            db.messageBoard.Add(messageBoard);
            db.SaveChanges();

            var mesNum = db.messageBoard.OrderByDescending(x=>x.messageboardNumber).FirstOrDefault().messageboardNumber;

            for (int i = 0; i < db.Employee.ToList().Count; i++)
            {
                watchCount checkAll = new watchCount();
                checkAll.employeeID = db.Employee.ToList()[i].employeeID;
                checkAll.messageboardNumber = mesNum;
                checkAll.watchState = false;

                db.watchCount.Add(checkAll);
            }
            db.SaveChanges();

            return RedirectToAction("MsgIndex");
        }
    }
}