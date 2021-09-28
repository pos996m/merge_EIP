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
        int pageSize = 9;

        // GET: Message
        public ActionResult MsgIndex(string getName, string getText, DateTime? getTime, string getState, int page = 1)
        {
            string EID = Convert.ToString(Session["ID"]);
            string PosID = Convert.ToString(Session["PosID"]);
            string DepID = Convert.ToString(Session["DepID"]);

            // 先撈所有可看見的
            var showMsg = db.messageBoard.Where(x => x.State == "所有人" || x.assignDepartment == DepID || x.assignPerson == EID || x.employeeID == EID)
                .OrderByDescending(x => x.goTop).ThenByDescending(x => x.messageDate).ToList();

            // 再撈指定搜尋的
            if (getName != null)
            {
                showMsg = showMsg.Where(x => x.employeeName.Contains(getName)).ToList();
            }
            if (getText != null)
            {
                showMsg = showMsg.Where(x => x.messageText.Contains(getText)).ToList();
            }
            if (getTime != null)
            {
                string getTimeStr = Convert.ToDateTime(getTime).ToString("yyyy-MM-dd");
                showMsg = showMsg.Where(x => x.messageDate.ToString("yyyy-MM-dd") == getTimeStr).ToList();
            }
            if (getState != null && getState != "全部")
            {
                if (getState == "主管")
                {
                    showMsg = showMsg.Where(x => x.Employee.positionID == "0").ToList();
                }
                else
                {
                    showMsg = showMsg.Where(x => x.State == getState).ToList();
                }
            }
            //var requestType = Request.RequestType;
            //if(requestType == "POST")
            //{
            //    page = 1;
            //}

            int currpag = page < 1 ? 1 : page;

            var result = showMsg.ToPagedList(currpag, pageSize);

            Console.WriteLine(getName + getText + getTime + getState);

            return View(result);
        }

        // 新增頁面
        public ActionResult Fnewmsg()
        {
            string DepID = Convert.ToString(Session["DepID"]);
            ViewBag.assignPerson = new SelectList(db.Employee.Where(x=>x.departmentID == DepID), "employeeID", "Name");
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

            var mesNum = db.messageBoard.OrderByDescending(x => x.messageboardNumber).FirstOrDefault().messageboardNumber;

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

        public ActionResult Setmsg(int? id)
        {
            string myEID = Convert.ToString(Session["ID"]);
            string DepID = Convert.ToString(Session["DepID"]);

            messageBoard message = db.messageBoard.Find(id);
            // 如果這筆資料不是留言主，則不能修改
            if(message.employeeID != myEID)
            {
                return RedirectToAction("MsgIndex");
            }

            ViewBag.assignPerson = new SelectList(db.Employee.Where(x => x.departmentID == DepID), "employeeID", "Name");
            return View(message);
        }
    }
}