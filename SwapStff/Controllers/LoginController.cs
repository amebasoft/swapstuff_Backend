using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwapStff.Models;
using SwapStff.Service;

namespace SwapStff.Controllers
{
    public class LoginController : Controller
    {
        
        public IUserService UserService { get; set; }
       
        public LoginController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        // GET: /Login/
        public ActionResult Index()
        {

            return View();
        }

        public string LoginMethod(string Email, string Password, string LoggedIn)
        {
            //Get Data from user table
            var objUsers = UserService.GetAll().Where(x => x.EmailID == Email && x.Password == Password ).OrderByDescending(x => x.UserID).ToList();  

            if (objUsers.Count() > 0) //User Found
            {
                Session["UserName"] = objUsers.FirstOrDefault().UserName;
                if (LoggedIn == "on")
                {
                    Response.Cookies["EmailID"].Value = Email;
                    Response.Cookies["EmailID"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Password"].Value = Password;
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["EmailID"].Value = " ";
                    Response.Cookies["Password"].Value = " ";
                }
                
                return "OK";
            }
            else
            {
                return "";
            }
        }
    }
}