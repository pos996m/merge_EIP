using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class ClockinController : Controller
    {
        // GET: Clockin
        // 打卡主畫面
        public ActionResult Index()
        {
            return View();
        }

        // 打卡查詢
        public ActionResult Search()
        {
            return View();
        }
    }
}