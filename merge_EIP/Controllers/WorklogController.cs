using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class WorklogController : Controller
    {
        // GET: Worklog
        // 代辦事項
        public ActionResult Todolist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Todolist(int? id)
        {
            return View();
        }

        // 刪除代辦事項
        public ActionResult Delete()
        {
            return View();
        }

        // 行事曆
        public ActionResult Calendar()
        {
            return View();
        }
    }
}