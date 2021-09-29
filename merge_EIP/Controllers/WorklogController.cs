using merge_EIP.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class WorklogController : Controller
    {
        FormModelEntities db = new FormModelEntities();
        // GET: Worklog
        // 代辦事項
        // 幾筆資料一頁
        int pageSize = 9;

        public ActionResult Todolist(int page = 1)
        {
            if (Session["ID"] != null)
            {
                string EID = Convert.ToString(Session["ID"]);
                // OrderBy後面加 .ThenBy(x => x.backlogDate) 次排序 (ThenByDescending 次排序遞減)
                var crin = db.Backlog.Where(x => x.employeeID == EID).OrderByDescending(x => x.backlogDate).ToList();

                int currpag = page < 1 ? 1 : page;
                var result = crin.ToPagedList(currpag, pageSize);

                return View(result);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpPost]
        public ActionResult Todolist(string Tname, DateTime Tdate,TimeSpan? Ttime, int page = 1)
        {
            int currpag = page < 1 ? 1 : page;
            Backlog backlog = new Backlog();
            string EID = Convert.ToString(Session["ID"]);
            backlog.employeeID = EID;
            backlog.backlogTxet = Tname;
            backlog.backlogDate = Tdate;
            backlog.backlogTime = Ttime;
            backlog.checkState = false;
            db.Backlog.Add(backlog);
            db.SaveChanges();
            return RedirectToAction("Todolist",new {Page = currpag });
        }

        // 刪除代辦事項
        public ActionResult Delete(int? id)
        {
            Backlog backlog = db.Backlog.Find(id);
            db.Backlog.Remove(backlog);
            db.SaveChanges();
            return RedirectToAction("Todolist");
        }

        // 行事曆
        public ActionResult Calendar()
        {
            return View();
        }
    }
}