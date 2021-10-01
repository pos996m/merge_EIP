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
        int pageSize = 7;

        public ActionResult Todolist(int? id,int page = 1)
        {
            string EID = Convert.ToString(Session["ID"]);


            if (Session["ID"] != null)
            {
                if(id != null)
                {
                    Backlog selectList = db.Backlog.Find(id);

                    if (selectList.employeeID == EID)
                    {
                        List<Backlog> selectListAll = new List<Backlog>() { selectList };
                        int selectcurrpag = page < 1 ? 1 : page;
                        var selectresult = selectListAll.ToPagedList(selectcurrpag, pageSize);

                        return View(selectresult);
                    }
                    else
                    {
                        return Redirect("/Worklog/Todolist");
                    }
                }

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
            return Redirect("/Worklog/Todolist");
            //return RedirectToAction("Todolist",new {Page = currpag });
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