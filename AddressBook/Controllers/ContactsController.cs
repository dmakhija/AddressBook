using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AddressBook.Models;
using AddressBook.Repository;

namespace AddressBook.Controllers
{
    public class ContactsController : Controller
    {
        AccountData myAccount;

        private AddressBookDbContext db = new AddressBookDbContext();

        public ContactsController()
        {
            myAccount = new AccountData();
        }

        public SessionDataModel AuthenticateUser()
        {
            var sessionData = new SessionDataModel();
            var userName = "";
            var userId = "";
            var userRole = "";
            if (Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                userName = Session["UserName"].ToString().ToLower().Trim();
            }
            if (Session["UserId"] != null && Session["UserId"].ToString() != "")
            {
                userId = Session["UserId"].ToString().ToLower().Trim();
            }
            if (Session["UserRole"] != null && Session["UserRole"].ToString() != "")
            {
                userRole = Session["UserRole"].ToString().ToLower().Trim();
            }

            sessionData.userId = userId;
            sessionData.userName = userName;
            sessionData.userRole = userRole;
            return sessionData;
        }
               

        // GET: Contacts
        public ActionResult Index()
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user,
                //check user role
                if(sessionData.userRole == "admin")
                {
                    //display all contacts
                    return View(db.Contacts.ToList());
                }
                else if (sessionData.userRole == "user")
                {
                    //display user's contact list
                    //get user details
                    var user = myAccount.GetUser_By_UserName(sessionData.userName);
                    if (user != null)
                    {
                        //get list of user's contacts
                        List<Contact> myContacts = new List<Contact>();
                        myContacts = user.Contacts.ToList<Contact>();
                        return View(myContacts);
                    }
                    else
                    {
                        //invalid user, 
                        //return HttpNotFound();
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    //user not authenticated, return to landing page
                    return RedirectToAction("Logout", "Account");
                }
            }
            else
            {
                //user not authenticated, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                if (id == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return RedirectToAction("Error", "Home");
                }
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Error", "Home");
                }
                return View(contact);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                return View();
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactId,FirstName,LastName,Phone,Email,Street,City,Province,Postal,Country,Notes")] Contact contact)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                if (ModelState.IsValid)
                {
                    if(sessionData.userRole == "admin")
                    {
                        db.Contacts.Add(contact);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else if (sessionData.userRole == "user")
                    {
                        //get the user details
                        var user = db.Users.Find(Convert.ToInt16(sessionData.userId));
                        if (user != null)
                        {
                            //user found, add his/her contacts
                            contact.Users.Add(user);
                            db.Contacts.Add(contact);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            //invalid user, return to landing page
                            return RedirectToAction("Logout", "Account");
                        }
                    }
                    else
                    {
                        //return HttpNotFound();
                        return RedirectToAction("Error", "Home");
                    }                     
                }
                return View(contact);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
            
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                if (id == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return RedirectToAction("Error", "Home");

                }
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Error", "Home");
                }
                return View(contact);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }            
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactId,FirstName,LastName,Phone,Email,Street,City,Province,Postal,Country, Notes")] Contact contact)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(contact);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }                
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                if (id == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return RedirectToAction("Error", "Home");
                }
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    if(sessionData.userRole == "admin")
                    {
                        List<User> users = contact.Users.ToList<User>();
                        foreach(var u in users)
                        {
                            u.Contacts.Remove(contact);
                        }
                        db.Contacts.Remove(contact);
                        db.SaveChanges();
                    }
                    else if (sessionData.userRole == "user")
                    {
                        var user = db.Users.Find(Convert.ToInt16(sessionData.userId));
                        if (user != null)
                        {
                            user.Contacts.Remove(contact);
                            db.Contacts.Remove(contact);
                            db.SaveChanges();
                        }
                        else
                        {
                            //invalid user, return to landing page
                            return RedirectToAction("Logout", "Account");
                        }
                    }
                    else
                    {
                        //return HttpNotFound();
                        return RedirectToAction("Error", "Home");
                    } 
                }
                return RedirectToAction("Index");
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
