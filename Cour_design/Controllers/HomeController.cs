using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cour_design.Models;
using Newtonsoft.Json;

namespace Cour_design.Controllers
{
    public class HomeController : Controller
    {
        private libraryEntities db = new libraryEntities();

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(db.Member_Message.ToList());
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(string id = null)
        {
            Member_Message member_message = db.Member_Message.Find(id);
            if (member_message == null)
            {
                return HttpNotFound();
            }
            return View(member_message);
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(Member_Message member_message)
        {
            if (ModelState.IsValid)
            {
                db.Member_Message.Add(member_message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member_message);
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(string id = null)
        {
            Member_Message member_message = db.Member_Message.Find(id);
            if (member_message == null)
            {
                return HttpNotFound();
            }
            return View(member_message);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(Member_Message member_message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member_message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member_message);
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(string id = null)
        {
            Member_Message member_message = db.Member_Message.Find(id);
            if (member_message == null)
            {
                return HttpNotFound();
            }
            return View(member_message);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Member_Message member_message = db.Member_Message.Find(id);
            db.Member_Message.Remove(member_message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult tools()
        {

            return View();
        }

        public ActionResult Users()
        {
            return View();

        }
        public ActionResult Admin()
        {
            return View();

        }

        public ActionResult add(string number, string name, string sex, string code)
        {
            Member_Message s = new Member_Message();
            s.BorBook_Cnt = Convert.ToString(8);
            s.name = name;
            s.sex = sex;
            s.code = code;
            s.number = number;
            s.Borrow_Book = null;


            db.Member_Message.Add(s);
            int v = db.SaveChanges();
            var result = new { data = v };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

       

        public JsonResult checkLogin(string number, string password)
        {
            bool result;

            libraryEntities db = new libraryEntities();
            var num1 = db.Member_Message.Where(s => s.number == number).FirstOrDefault();
            var num2 = db.Member_Message.Where(s => s.code == password).FirstOrDefault();

            if (num1 != null && num2 != null)
            {
                //  HttpSession session = Request.getSession();

                result = true;
                Session["userName"] = num1.name;
                Session["userNum"] = num1.number;
                Session["userPass"] = num2.code;
                Session["userSex"] = num2.sex;
                Session["userCnt"] = num2.BorBook_Cnt;
                Session["userBook"] = num2.Borrow_Book;
                Session.Timeout = 30;
            }
            else
            {
                result = false;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult search_book(string name)
        {

            ;
            var test = db.Book_Message.Where(s => s.name == name).FirstOrDefault();
            var test2 = db.Book_Message.Where(s => s.author == name).FirstOrDefault();
            if (test != null)
            {

                var data = JsonConvert.SerializeObject(test);
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var data = JsonConvert.SerializeObject(test2);
                return Json(data, JsonRequestBehavior.AllowGet);

            }



        }
        public JsonResult init()
        {


            //    var test = new { userName = Session["userName"], userNum = Session["userNum"],userSex= Session["userSex"], userPass = Session["userPass"], userCnt = Session["userCnt"], userBook = Session["userBook"] };
            var userName = Session["userName"];
            var test = db.Member_Message.Where(s => s.name == userName).FirstOrDefault();
            var data = JsonConvert.SerializeObject(test);
            return Json(data, JsonRequestBehavior.AllowGet);



        }



        public ActionResult addBook(string ISBN, string name, string author, string pub, string price, string content, string number)
        {
            Book_Message s = new Book_Message();
            s.ISBN = ISBN;
            s.name = name;
            s.author = author;
            s.pub = pub;
            s.price = price;
            s.content = content;
            s.number = number;

            db.Book_Message.Add(s);
            int v = db.SaveChanges();
            var result = new { data = v };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult delBook(string ISBN)
        {

            Book_Message s = db.Book_Message.Find(ISBN);
            db.Book_Message.Remove(s);

            int v = db.SaveChanges();
            var result = new { data = v };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult modBook(string ISBN, string name, string author, string pub, string price, string content, string number)
        {
            Book_Message s = new Book_Message();
            s.ISBN = ISBN;
            s.name = name;
            s.author = author;
            s.pub = pub;
            s.price = price;
            s.content = content;
            s.number = number;

            db.Entry(s).State = System.Data.EntityState.Modified;
            int v = db.SaveChanges();
            var re = new { data = v };
            return Json(re, JsonRequestBehavior.AllowGet);

        }

        public ActionResult editMem(string number, string name, string sex, string code, string borrow_book, string bor_cnt)
        {
            Member_Message s = new Member_Message();
            s.number = number;
            s.Borrow_Book = borrow_book;
            s.name = name;
            s.code = code;
            s.sex = sex;
            s.BorBook_Cnt = bor_cnt;


            db.Entry(s).State = System.Data.EntityState.Modified;
            int v = db.SaveChanges();
            var re = new { data = v };
            return Json(re, JsonRequestBehavior.AllowGet);

        }

        public JsonResult checkReader(string name)
        {

            ;
            var test = db.Member_Message.Where(s => s.name == name).FirstOrDefault();

            var data = JsonConvert.SerializeObject(test);
            return Json(data, JsonRequestBehavior.AllowGet);



        }

        public ActionResult returnbook(string number, string name, string sex, string code, string borrow_book, string bor_cnt)
        {

            Member_Message s = new Member_Message();
            s.number = number;
            s.Borrow_Book = borrow_book;
            s.name = name;
            s.code = code;
            s.sex = sex;
            s.BorBook_Cnt = bor_cnt;


            db.Entry(s).State = System.Data.EntityState.Modified;
            int v = db.SaveChanges();
            var re = new { data = v };
            return Json(re, JsonRequestBehavior.AllowGet);

        }
    }
}