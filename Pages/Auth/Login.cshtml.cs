using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WorkersApp.Models;

namespace WorkersApp.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext data;

        public LoginModel(ApplicationDbContext db)
        {
            data = db;
        }
        public IEnumerable<User> Users { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public string Error_Hadnler { get; set; }
        public void OnGet(string Error_Hadnler)
        {
            this.Error_Hadnler = Error_Hadnler;
        }

        public IActionResult OnPost()
        {
            if (Input.Email == null || Input.Password == null)
            {
                return RedirectToPage(new { Error_Hadnler = "Fields cannot be empty" });
            }
            var user_check = (from user in data.User
                              where user.Email == Input.Email
                              select user).FirstOrDefault();
            if (user_check == null)
            {
                return RedirectToPage(new { Error_Hadnler = "User is not found" });
            }
            bool verification = Crypto.VerifyHashedPassword(user_check.Password, Input.Password);
            if (verification && user_check.IsActive)
            {
                HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(user_check));
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage(new { Error_Hadnler = "Password or email is incorrect or user is unactive" });
            }

        }
    }
}
