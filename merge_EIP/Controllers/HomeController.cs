using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class HomeController : Controller
    {
        FormModelEntities db = new FormModelEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}