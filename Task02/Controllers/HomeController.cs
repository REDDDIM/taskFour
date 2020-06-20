using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Forms;
using Task02.Models;

namespace Task02.Controllers
{
    public class HomeController : Controller
    {
        UserContext db = new UserContext();
        NoteContext ndb = new NoteContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string mail, string password )
        {
            bool flag = false;
            foreach(var user in db.Users)
            {
                if(user.Mail == mail)
                {
                    if(user.Password == password)
                    {
                        if(user.Status != "Active")
                        {
                            return View("Index");
                        }
                        flag = true;
                        CurrentUser.Id = user.Id;
                        CurrentUser.Status = user.Status;
                    }
                }
            }
            if (flag)
            {
                var a = db.Database.ExecuteSqlCommand("update Users set LastLogin='" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "' where Mail='" + mail + "'");
                UserContext db1 = new UserContext();
                ViewBag.Note = ndb.Notes.FirstOrDefault(item => item.Id == CurrentUser.Id);
                return View("MainView", db1.Users);
            }
            return View("Index");
        }
        public ActionResult Reg(string name, string mail, string password)
        {
            try
            {
                foreach(var a in db.Users)
                {
                    if(a.Mail == mail)
                    {
                        throw new Exception("a user with that name exists");
                    }
                }
                User user1 = new User();
                user1.Id = db.Users.Count() + 1;
                user1.Name = name;
                user1.Mail = mail;
                user1.Password = password;
                user1.LastLogin = DateTime.Now;
                user1.DateReg = DateTime.Now;
                user1.Status = "Active";
                db.Users.Add(user1);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return View("Reg");
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult MainView(string chaction, bool[] MyCheckbox)
        {
            if(CurrentUser.Status != "Active")
            {
                return View("Index");
            }
            List<int?> AllId = new List<int?>();
            foreach (var item in db.Users)
            {
                for(int a = AllId.Count; a < MyCheckbox.Length; a++)
                {
                    if (!MyCheckbox[a])
                    {
                        AllId.Add(null);
                        if ((a - 1) >= 0 && !MyCheckbox[a - 1])
                            break;
                    }
                    else
                    {
                        AllId.Add(item.Id);
                        break;
                    }     
                }
                
            }
            if (chaction == "Lock")
            {
                
                if (MyCheckbox[0])
                {
                    var a = db.Database.ExecuteSqlCommand("update Users set Status='Banned'");
                }
                else
                {
                    for (int index = 1; index < MyCheckbox.Length; index++)
                    {
                        if (MyCheckbox[index])
                        {
                            if (CurrentUser.Id == AllId[index])
                                CurrentUser.Status = "Banned";
                            var a = db.Database.ExecuteSqlCommand("update Users set Status='Banned'" + "where Id='" + AllId[index] + "'");
                            db.SaveChanges();
                            index++;
                        }
                    }
                }
                UserContext db1 = new UserContext();
                ViewBag.Note = ndb.Notes.FirstOrDefault(item => item.Id == CurrentUser.Id);
                return View(db1.Users);
            }
            if(chaction == "Unlock")
            {
                if (MyCheckbox[0])
                {
                    var a = db.Database.ExecuteSqlCommand("update Users set Status='Active'");
                }
                else
                {
                    for (int index = 1; index < MyCheckbox.Length; index++)
                    {
                        if (MyCheckbox[index])
                        {
                            if (CurrentUser.Id == AllId[index])
                                CurrentUser.Status = "Banned";
                            var a = db.Database.ExecuteSqlCommand("update Users set Status='Active'" + "where Id='" + AllId[index] + "'");
                            db.SaveChanges();
                            index++;
                        }
                    }
                }
                UserContext db1 = new UserContext();
                ViewBag.Note = ndb.Notes.FirstOrDefault(item => item.Id == CurrentUser.Id);
                return View(db1.Users);
            }
            if(chaction == "Delete")
            {
                if (MyCheckbox[0])
                {
                    var a = db.Database.ExecuteSqlCommand("delete from Users");
                }
                else
                {
                    for (int index = 1; index < AllId.Count; index++)
                    {
                        if (MyCheckbox[index])
                        {
                            var a = db.Database.ExecuteSqlCommand("delete from Users where Id='" + AllId[index] + "'");
                            AllId.Remove(AllId[index]);
                            db.SaveChanges();
                            index++;
                            if (CurrentUser.Id == AllId[index])
                                return View("Index");
                        }
                    }
                }
                UserContext db1 = new UserContext();
                ViewBag.Note = ndb.Notes.FirstOrDefault(item => item.Id == CurrentUser.Id);
                return View(db1.Users);
            }
            ViewBag.Note = ndb.Notes.FirstOrDefault(item => item.Id == CurrentUser.Id);
            return View(db.Users);
        }
    }
}