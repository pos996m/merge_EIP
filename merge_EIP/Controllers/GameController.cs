using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Games()
        {
            return View();
        }
    }
}