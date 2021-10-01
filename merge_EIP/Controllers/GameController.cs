using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class GameController : Controller
    {
        FormModelEntities db = new FormModelEntities();

        // GET: Game
        public ActionResult Games()
        {
            gameScoreAll gameScoreAll = new gameScoreAll() {
                runScoreAll = db.gameRecord.Where(x => x.Type == "跑跑方塊人").OrderByDescending(x => x.Fraction).Take(5).ToList(),
                snakeScoreAll = db.gameRecord.Where(x => x.Type == "貪吃貓").OrderByDescending(x => x.Fraction).Take(5).ToList()
            };
            return View(gameScoreAll);
        }
    }
}