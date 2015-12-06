using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace AddressBook.Repository
{
    public class AccountData
    { 
        private AddressBookDbContext db = new AddressBookDbContext();


        public string SendPasswordResetEmail(string emailId,string url, string fn)
        {
            string emailStatus = "There was an error sending password reset email email to " + emailId + ".";

            try
            {
                
                MailMessage mm = new MailMessage("no-reply@jmqtnonsense.ca", emailId);

                mm.Subject = "Password Reset";
                string body = "Dear <b>"+ fn+"</b> !";
                body += "<br /><br />Please <b><a href ='" + url + "'>click here</a></b> to reset your password.";
                body += "<br /><br />Thanks.";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtpout.secureserver.net"; //"relay-hosting.secureserver.net";
                //smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("no-reply@jmqtnonsense.ca", "ZMG59k7v");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 25; //3535;// 465; // 587;
                smtp.Send(mm);
                //emailStatus = "<span class='label label-success'>Password reset email has been sent to " + emailId + ".</span>";
                emailStatus = "Password reset email has been sent to " + emailId + ".";

            }
            catch (Exception ex)
            {
                //emailStatus=ex.GetBaseException().ToString();
                emailStatus = "There was an error sending password reset email email to " + emailId + ".";
            }
            return emailStatus;
        }


        public bool IsValid(string email, string password)
        {
            return db.Users.Any(u => u.Email == email
                    && u.Password == password);
        }

        public User GetUser_By_UserName(string email)
        {
            return db.Users.Single(u => u.Email == email);            
        }

        public void CreateUser(User user)
        {            
            db.Users.Add(user);
            db.SaveChanges();
        }

        public bool checkUsernameExists(string email)
        {
            if(db.Users.Where(p => p.Email == email).Count()>0)
            {
                return true;
            }
            else
            {
                return false;
            }         
        }

        public Role GetRole_By_Name(string rolename)
        {
            return db.Roles.Where(p => p.RoleName == rolename).FirstOrDefault();
        }

    }
}