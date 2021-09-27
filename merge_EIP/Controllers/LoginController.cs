﻿using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class LoginController : Controller
    {
        FormModelEntities db = new FormModelEntities();
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

            var loginselect = db.Employee.Where(x => x.Account == account).FirstOrDefault();

            if(loginselect!=null && loginselect.Password == password)
            {
                //Session["sn"] = "登入成功";
                Session["ID"] = loginselect.employeeID;
                Session["Name"] = loginselect.Name;
                Session["Dep"] = loginselect.Department.departmentName; // 部門
                Session["DepID"] = loginselect.departmentID; // 部門
                Session["Pos"] = loginselect.Position.positionName; // 職位
                Session["PosID"] = loginselect.positionID;
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["ID"] = null;
            Session["Name"] = null;
            Session["Dep"] = null;
            Session["Pos"] = null;
            Session["PosID"] = null;
            return View();
        }
    }
}