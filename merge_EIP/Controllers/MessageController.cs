﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        int pageSize = 8;

        // GET: Message
        public ActionResult MsgIndex(int? id,string getName, string getText, DateTime? getTime, string getState, int page = 1)
        {
            string EID = Convert.ToString(Session["ID"]);
            string PosID = Convert.ToString(Session["PosID"]);
            string DepID = Convert.ToString(Session["DepID"]);

            if (id != null)
            {
                messageBoard selectMsg = db.messageBoard.Find(id);

                if(selectMsg.State == "所有人" || selectMsg.assignDepartment == DepID || selectMsg.assignPerson == EID || selectMsg.employeeID == EID)
                {
                List<messageBoard> selectMsgList = new List<messageBoard>() { selectMsg };
                int selectcurrpag = page < 1 ? 1 : page;
                var selectresult = selectMsgList.ToPagedList(selectcurrpag, pageSize);

                return View(selectresult);
                }
                else
                {
                    return Redirect("/Message/MsgIndex");
                }
            }

            Console.WriteLine(getName);

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
                showMsg = showMsg.Where(x => x.messageDate.ToString("yyyy-MM-dd") == getTimeStr || Convert.ToDateTime(x.toMsgDate).ToString("yyyy-MM-dd") == getTimeStr).ToList();
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

            int currpag = page < 1 ? 1 : page;
            var result = showMsg.ToPagedList(currpag, pageSize);

            Console.WriteLine(getName + getText + getTime + getState);

            return View(result);
        }

        // 新增頁面
        public ActionResult Fnewmsg()
        {
            string DepID = Convert.ToString(Session["DepID"]);
            ViewBag.assignPerson = new SelectList(db.Employee.Where(x => x.departmentID == DepID), "employeeID", "Name");
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

            if (messageBoard.State != "私人")
            {
                messageBoard.assignPerson = null;
            }
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


        // 編輯
        public ActionResult Setmsg(int? id)
        {
            if (Session["id"] != null)
            {

                string myEID = Convert.ToString(Session["ID"]);
                string DepID = Convert.ToString(Session["DepID"]);

                messageBoard message = db.messageBoard.Find(id);
                // 如果這筆資料不是留言主，則不能修改
                if (message.employeeID != myEID)
                {
                    return RedirectToAction("MsgIndex");
                }
                ViewBag.assignPerson = new SelectList(db.Employee.Where(x => x.departmentID == DepID), "employeeID", "Name");
                return View(message);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpPost]
        public ActionResult Setmsg(messageBoard messageBoard)
        {
            string Edep = Convert.ToString(Session["DepID"]);
            int thisnumber = messageBoard.messageboardNumber;
            var thisMsg = db.messageBoard.Find(thisnumber);

            // 匯入資料
            thisMsg.toMsgDate = messageBoard.toMsgDate;
            thisMsg.messageTitle = messageBoard.messageTitle;
            thisMsg.messageText = messageBoard.messageText;
            thisMsg.State = messageBoard.State;
            if (messageBoard.State == "部門")
            {
                thisMsg.assignDepartment = Edep;
            }
            thisMsg.assignPerson = messageBoard.assignPerson;
            thisMsg.goTop = messageBoard.goTop;
            thisMsg.toCalendar = messageBoard.toCalendar;

            // 把觀看重置
            var checkAll = db.watchCount.Where(x => x.messageboardNumber == messageBoard.messageboardNumber).ToList();

            for (int i = 0; i < checkAll.Count; i++)
            {
                checkAll[i].watchState = false;
            }

            db.SaveChanges();
            return RedirectToAction("MsgIndex");
        }

        // 確認關看
        public ActionResult Check(int num, string getName, string getText, DateTime? getTime, string getState, int? page)
        {
            string EID = Convert.ToString(Session["ID"]);
            var crin = db.watchCount.Where(x => x.messageboardNumber == num && x.employeeID == EID).FirstOrDefault();
            crin.watchState = true;
            db.SaveChanges();


            return Redirect($"/Message/MsgIndex?getName={getName}&getText={getText}&getTime={getTime}&getState={getState}&page={page}");

            //return RedirectToAction("MsgIndex",new { getName= getName, getText= getText, getTime= getTime, getState= getState, page = page });
        }

        // 刪除留言板
        public ActionResult Delmsg(int? id)
        {
            messageBoard backlog = db.messageBoard.Find(id);
            var watchCount = db.watchCount.Where(x=>x.messageboardNumber == id).ToList();

            // 先刪除所有確認觀看
            foreach (watchCount item in watchCount)
            {
                db.watchCount.Remove(item);
            }

            // 再刪除留言板
            db.messageBoard.Remove(backlog);
            db.SaveChanges();

            return RedirectToAction("MsgIndex");
            //return Redirect("/Message/MsgIndex");
        }

    }
}