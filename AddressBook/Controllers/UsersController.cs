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
    public class UsersController : Controller
    {
        AccountData myAccount;

        private AddressBookDbContext db = new AddressBookDbContext();

        public UsersController()
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

        // GET: Users/Details/5
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
                User user = db.Users.Find(id);
                if (user == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Error", "Home");
                }
                return View(user);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }

        }

        // GET: Users/Edit/5
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
                User user = db.Users.Find(id);
                if (user == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Error", "Home");
                }
                return View(user);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
            
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Email,Password")] User user)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Users.Attach(user);
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        if (sessionData.userRole == "admin")
                        {                            
                            return RedirectToAction("Index", "Users");
                        }
                        else if (sessionData.userRole == "user")
                        {
                            //ask user to login with new credentials
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            //invalid user role
                            return RedirectToAction("Logout", "Account");
                        }
                    }
                    catch (Exception ex)
                    {
                        //duplicate username/email
                        ModelState.AddModelError("", "Email already exists.Try another.");
                        //throw;
                        //return RedirectToAction("Error", "Home");
                    }
                }
                return View(user);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }           

        }
        
              
        #region 

        // GET: Users
        public ActionResult Index()
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "")
            {
                //authenticated user
                if(sessionData.userRole == "admin")
                {
                    return View(db.Users.ToList());
                }
                else if(sessionData.userRole == "user")
                {
                    return RedirectToAction("Index", "Contacts");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }                
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }                
        }
        
        // GET: Users/Create
        public ActionResult Create()
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "" && sessionData.userRole == "admin")
            {
                //authenticated admin
                return View();
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }        

        
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel rvm)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "" && sessionData.userRole == "admin")
            {
                //authenticated admin
                if (ModelState.IsValid)
                {
                    if (myAccount.checkUsernameExists(rvm.Email.ToLower().Trim()) == false)
                    {
                        //unique username                                        
                        User myUser = new User();
                        myUser.FirstName = rvm.Firstname;
                        myUser.LastName = rvm.Lastname;
                        myUser.Email = rvm.Email.ToLower().Trim();
                        myUser.Password = rvm.Password;

                        //add role to the user
                        Role role = myAccount.GetRole_By_Name("user");
                        myUser.Roles.Add(role);

                        //add user 
                        myAccount.CreateUser(myUser);
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        //duplicate username
                        ModelState.AddModelError("", "Email already exists.Try another.");
                    }
                }
                return View(rvm);
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "" && sessionData.userRole=="admin")
            {
                //authenticated admin
                if (id == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return RedirectToAction("Error", "Home");
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    var myRoles = user.Roles.ToList<Role>();
                    var myContacts = user.Contacts.ToList<Contact>();
                    foreach(var role in myRoles)
                    {
                        user.Roles.Remove(role);
                    }
                    foreach(var contact in myContacts)
                    {
                        user.Contacts.Remove(contact);
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();                    
                }
                return RedirectToAction("Index");
            }
            else
            {
                //invalid user, return to landing page
                return RedirectToAction("Logout", "Account");
            }
        }

        #endregion


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
