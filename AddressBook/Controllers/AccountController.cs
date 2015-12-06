using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AddressBook.Repository;

namespace AddressBook.Controllers
{
    public class AccountController : Controller
    {
        AccountData myAccount;

        private AddressBookDbContext db = new AddressBookDbContext();

        public AccountController()
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Error = "";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel lvm)
        {
            ViewBag.Error = "";
            if (ModelState.IsValid)
            {
                try
                {
                    bool isAuthenticated = myAccount.IsValid(lvm.Email, lvm.Password);
                    if (isAuthenticated == true)
                    {
                        //get user details
                        var user = myAccount.GetUser_By_UserName(lvm.Email.ToLower().Trim());
                        if(user!=null)
                        {
                            //get role details
                            var userRole = user.Roles.SingleOrDefault();
                            if (userRole != null)
                            {
                                var rolename = userRole.RoleName.ToLower().Trim();

                                Session["UserName"] = user.Email;
                                Session["UserId"] = user.UserId;
                                Session["UserRole"] = rolename;

                                if (rolename == "admin")
                                {
                                    return RedirectToAction("Index", "Contacts");
                                }
                                else if (rolename == "user")
                                {
                                    return RedirectToAction("Index", "Contacts");
                                }
                                else
                                {
                                    //invalid user role
                                    ViewBag.Error="Rights to User are not Provide Contact to Admin.";  
                                }
                            }
                            else
                            {
                                //user role error
                                ViewBag.Error = "Rights to User are not Provide Contact to Admin.";
                            }
                        }
                        else
                        {
                            //user not found
                            ViewBag.Error = "Please enter valid Username and Password";
                        }                        
                    }
                    else
                    {
                        //user not authenticated
                        ViewBag.Error = "Please enter valid Username and Password";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Login error occured.";
                    //throw;
                }
            }
            else
            {
                ViewBag.Error = "Please enter Email and Password";                
            }
            return View(lvm);
        }

        //
        // POST: /Account/LogOff
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel rvm)
        {       
           if (ModelState.IsValid)
           {
                if(myAccount.checkUsernameExists(rvm.Email.ToLower().Trim())==false)
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
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    //duplicate username
                    ModelState.AddModelError("", "Email already exists.Try another.");
                }
            }
            else
            {
                    ModelState.AddModelError("", "Please enter all details");
            }
            
           return View(rvm);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotViewModel fvm)
        {           

            ViewBag.Message = "";
            if (ModelState.IsValid)
            {

                //check if this user exists in our db
                if (myAccount.checkUsernameExists(fvm.Email.ToLower().Trim()) == true)
                {
                    //get user details
                    var user = myAccount.GetUser_By_UserName(fvm.Email.ToLower().Trim());
                    if(user!=null)
                    {
                        Random rand = new Random();
                        var code = rand.Next().ToString();
                        var userId = user.UserId;
                        var url = this.Url.Action("ResetPassword", "Account", new { id =userId  , code=code}, this.Request.Url.Scheme);
                        //return RedirectToAction("ResetPassword", "Account", new { id = userId, code=code });
                        var message=myAccount.SendPasswordResetEmail(fvm.Email.ToLower().Trim(),url,user.FirstName);
                        ModelState.AddModelError("",message);
                    }
                    else
                    {
                        //invalid user
                        ModelState.AddModelError("", "This email does not match our records.");
                    }
                }
                else
                {
                    //user does not exists.
                    ModelState.AddModelError("","This email does not match our records.");
                }
            }
            return View(fvm);
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(int id, string code)
        {
            if(id==null)
            {
                return RedirectToAction("Error", "Home");
            }
            if(IsNullOrEmpty(code)==true)
            {
                return RedirectToAction("Error","Home");
            }

            ResetPasswordViewModel rpvm = new ResetPasswordViewModel();
            rpvm.UserId = id;
            rpvm.Code = code;
            return View(rpvm);
        }

        // POST: /Account/ResetPassword
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel rpvm)
        {
            if(ModelState.IsValid)
            {
                var user = db.Users.Find(rpvm.UserId);
                if (user != null)
                {
                    try
                    {
                        user.Password = rpvm.Password;
                       // db.Users.Attach(user);
                        //db.Entry(user).Property(x => x.Password).IsModified = true;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", "Home");
                        //throw;
                    }                    
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }            
        }

        private bool IsNullOrEmpty(string content)
        {
            if(content==null || content=="")
            {
               return true;
            }
            else
            {
                return false;
            }
            //throw new NotImplementedException();
        }


        public ActionResult SendEmail(int id)
        {
            var sessionData = AuthenticateUser();
            if (sessionData.userId != "" && sessionData.userName != "" && sessionData.userRole != "" && sessionData.userRole=="admin")
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    Random rand = new Random();
                    var code = rand.Next().ToString();
                    var userId = user.UserId;
                    var url = this.Url.Action("ResetPassword", "Account", new { id = userId, code = code }, this.Request.Url.Scheme);
                    //return RedirectToAction("ResetPassword", "Account", new { id = userId, code=code });
                    var message = myAccount.SendPasswordResetEmail(user.Email.ToLower().Trim(), url, user.FirstName);
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }               
        }

    }
}