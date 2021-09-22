using merge_EIP.Models;
using PagedList;
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

            if (products != null)
            {
                if (products.clockIn == null)
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

        // 幾筆資料一頁
        int pageSize = 4;

        // 打卡查詢
        public ActionResult Search(string datetime, string myname, int page = 1)
        {
            // 分出主管跟員工顯示
            string EID = Convert.ToString(Session["ID"]); // 員工ID
            string dep = Convert.ToString(Session["Dep"]); // 部門
            string pos = Convert.ToString(Session["PosID"]); // 職位
            
            // 預設為員工的顯示
            var products = db.punchIn.Where(x => x.Employee.employeeID == EID).ToList();

            if (products != null)
            {
                // 先區分主管和員工
                if (pos == "0")
                {
                    // 主管看自己部門所有員工
                    products = db.punchIn.Where(x => x.Employee.Department.departmentName == dep).ToList();
                }



                // 判斷有搜尋才做
                if (datetime != null && myname != null)
                {
                    products = products.Where(x => x.punchinDate.ToString("yyyy-MM-dd") == datetime && x.Employee.Name.Contains(myname)).ToList();
                }
                else if (datetime != null)
                {
                    products = products.Where(x => x.punchinDate.ToString("yyyy-MM-dd") == datetime).ToList();
                }
                else if (myname != null)
                {
                    products = products.Where(x => x.Employee.Name.Contains(myname)).ToList();
                }
                else
                {
                    // 預設只出現今天的打卡狀態
                    string toDay = DateTime.Now.ToString("yyyy-MM-dd");
                    products = products.Where(x => x.punchinDate.ToString("yyyy-MM-dd") == toDay).ToList();
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            int currpag = page < 1 ? 1 : page;

            // 反轉排序
            products.Reverse();

            var result = products.ToPagedList(currpag, pageSize);

            return View(result);
        }
    }
}