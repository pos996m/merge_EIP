using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        // 登入畫面
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string account,string password)
        {
            ViewBag.mypost = "POST";
            if(account == "aaa" && password == "123")
            {
                Session["sn"] = "登入成功";
            }
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}