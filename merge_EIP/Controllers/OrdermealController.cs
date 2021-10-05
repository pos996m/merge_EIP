using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace merge_EIP.Controllers
{
    public class OrdermealController : Controller
    {
        // GET: Ordermeal
        FormModelEntities db = new FormModelEntities();

        // 開團首頁
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var temp = db.tOrder.Where(m => m.fStatus.Contains("結束")).ToList();
                var list = db.tOrder.Where(m => m.fStartDate <= DateTime.Today || DateTime.Today <= m.fEndDate).OrderByDescending(x => x.fStartDate).ToList();
                db.tOrder.Where(m => m.fWeek == DateTime.Today.DayOfWeek.ToString());
                ViewBag.test = DateTime.Today.DayOfWeek.ToString();
                return View(list);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        // 新增開團
        public ActionResult Open(string errtext)
        {
            // 如果餐廳重覆就會到這裡
            ViewBag.errtext = errtext;

            // 繫結選擇餐廳，要在顯示tOrder資料表出現 fStoreID ，並且將fStoreID 轉為 tShop資料表的 fStore，拿fStoreID做綁定。
            ViewBag.fStoreID = new SelectList(db.tShop, "fStoreID", "fStore");

            tOrder tOrder = new tOrder();
            // 為了要綁定 tOrder 這邊必須回傳 tOrder
            return View(tOrder);

            //return View(db.tShop.ToList());
        }

        [HttpPost]
        public ActionResult Open(tOrder myOrder)
        {
            tShop shop = new tShop();

            // 原本是舊有餐廳沒有值
            // 如果餐廳選擇新建餐廳有值
            if (myOrder.fNewStore != null)
            {
                // 先看有沒有重複
                var twoname = db.tShop.Where(x => x.fStore == myOrder.fNewStore).FirstOrDefault();
                if (twoname != null)
                {
                    // 不是null就是有重複的
                    return RedirectToAction("Open", new { errtext = "新建餐廳已存在" });
                }

                // 要先新建餐廳
                if (myOrder.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(myOrder.ImageFile.FileName);
                    string extension = Path.GetExtension(myOrder.ImageFile.FileName);
                    fileName = DateTime.Now.ToString("yymmssfff") + extension;
                    myOrder.fImagePath = "~/Menu/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Menu"), fileName);
                    myOrder.ImageFile.SaveAs(fileName);
                }

                db.tShop.Add(new tShop { fStore = myOrder.fNewStore, fImagePath = myOrder.fImagePath });
                db.SaveChanges();

                // 如果是新建團的話，就找到剛剛新建的那一團ID存進去，或是說 更改 繫結的原設定
                myOrder.fStoreID = db.tShop.Where(x => x.fStore == myOrder.fNewStore).FirstOrDefault().fStoreID;
            }

            // 新開團
            // 因為是用 tOrder 收回來，所以會直接回收 myOrder.fStoreID，如果沒輸入的話也會
            myOrder.employeeID = Session["ID"].ToString();
            myOrder.fStartDate = DateTime.Today;
            myOrder.fStartTime = DateTime.Now.TimeOfDay;
            myOrder.fStatus = "開放中";

            db.tOrder.Add(myOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // 新增餐點
        public ActionResult Add(int? id)
        {
            //tOrderDetail detail = new tOrderDetail();
            //detail.fOrderId = id;
            if (Session["ID"] != null)
            {
                List<tOrderDetail> od = new List<tOrderDetail> { new tOrderDetail { fOrderId = (int)id, DetailEID = Session["ID"].ToString(), fFood = "", fPrice = 0, fQty = 0, fNote = "" } };

                // 找出相對的餐廳圖片
                var temp = db.tOrder.Where(m => m.fOrderId == id).FirstOrDefault();
                // 商店名
                ViewBag.temp = temp.tShop.fStore;
                // 商店照片路徑
                ViewBag.pic = temp.tShop.fImagePath;

                return View(od);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(List<tOrderDetail> od)
        {
            if (ModelState.IsValid)
            {
                foreach (tOrderDetail i in od)
                {
                    i.DetailEID = Session["ID"].ToString();
                    db.tOrderDetail.Add(i);
                }

                db.SaveChanges();
                ModelState.Clear();

                //od = new List<tOrderDetail> { new tOrderDetail { fId = 0, fName = Session["Name"].ToString(), fFood = "", fPrice = 0, fQty = 0, fNote = "" } };

            }
            return RedirectToAction("Index");
        }

        // 查看訂單
        public ActionResult Check(int? id)
        {
            tOrderDetail od = new tOrderDetail();

            // 撈同商店的便當(list)
            var list = db.tOrderDetail.Where(m => m.fOrderId == id).ToList();

            // 撈訂購人不重複
            var nameList = list.Select(p => p.Employee.Name).Distinct().ToList();

            // 總金額
            var total = list.Sum(x => x.fPrice * x.fQty);

            ViewBag.nameList = nameList;
            ViewBag.total = total;
            return View(list);
        }

        // 關閉開團
        public ActionResult Off(int id)
        {
            var temp = db.tOrder.Where(m => m.fOrderId == id).FirstOrDefault();
            temp.fStatus = "已結束";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // 管理訂單
        public ActionResult MyOrder()
        {
            if (Session["ID"] != null)
            {

                var EID = Session["ID"].ToString();

                // 取得tOrderDetail 是開放中的
                //var openOrder = db.tOrderDetail.Where(x => x.tOrder.fStatus.Contains("開放") && x.DetailEID == EID).ToList();
                var openOrder = db.tOrderDetail.Where(x => x.DetailEID == EID).OrderByDescending(x=>x.tOrder.fStartDate).ToList();


                ////所有訂單開放中的編號
                //var temp = db.tOrder.Where(m => m.fStatus.Contains("開放")).ToList();

                //// 細節
                //List<tOrderDetail> test = new List<tOrderDetail>();
                //foreach (var item in temp)
                //{
                //    // 取得Order的fId
                //    test.AddRange(db.tOrderDetail.Where(x => x.fOrderId == item.fOrderId && x.DetailEID == EID).ToList());

                //}
                //return View(test);


                return View(openOrder);
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

        }

        // 編輯訂單
        public ActionResult Edit(int? id)
        {
            var edit = db.tOrderDetail.Where(m => m.OrderDetailId == id).FirstOrDefault();
            return View(edit);
        }

        [HttpPost]
        public ActionResult Edit(tOrderDetail od)
        {
            //ViewBag.Error = false;
            var temp = db.tOrderDetail.Where(m => m.OrderDetailId == od.OrderDetailId).FirstOrDefault();
            temp.DetailEID = Session["ID"].ToString();
            temp.fFood = od.fFood;
            temp.fPrice = od.fPrice;
            temp.fQty = od.fQty;
            temp.fNote = od.fNote;

            db.SaveChanges();
            return RedirectToAction("MyOrder");
        }

        // 刪除訂單
        public ActionResult Delete(int id)
        {
            var temp = db.tOrderDetail.Where(m => m.OrderDetailId == id).FirstOrDefault();

            db.tOrderDetail.Remove(temp);
            db.SaveChanges();
            return RedirectToAction("MyOrder");
        }
    }
}