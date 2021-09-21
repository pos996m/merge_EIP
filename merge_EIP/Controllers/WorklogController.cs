using merge_EIP.Models;
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
        public ActionResult Todolist()
        {
            if (Session["ID"] != null)
            {
                string EID = Convert.ToString(Session["ID"]);
                var crin = db.Backlog.Where(x => x.employeeID == EID).OrderBy(x => x.checkState).ToList();
                return View(crin);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpPost]
        public ActionResult Todolist(string Tname)
        {
            Backlog backlog = new Backlog();
            string EID = Convert.ToString(Session["ID"]);
            backlog.employeeID = EID;
            backlog.backlogTxet = Tname;
            backlog.backlogDate = DateTime.Now;
            backlog.checkState = false;
            db.Backlog.Add(backlog);
            db.SaveChanges();
            return RedirectToAction("Todolist");
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