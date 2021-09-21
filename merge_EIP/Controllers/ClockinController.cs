using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class ClockinController : Controller
    {
        FormModelEntities db = new FormModelEntities();

        // GET: Clockin
        // 打卡主畫面
        public ActionResult Index()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.test = date;
            string EID = Convert.ToString(Session["ID"]);
            var products = db.punchIn.Where(x => x.punchinDate.ToString() == date && x.employeeID == EID).FirstOrDefault();

            if(products != null)
            {
                if(products.clockIn == null)
                {
                    ViewBag.clockinstr = "上班打卡";
                }
                else
                {
                    ViewBag.clockinstr = "下班打卡";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        // 打卡查詢
        public ActionResult Search()
        {
            return View();
        }
    }
}