using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkersApp.Models;

namespace WorkersApp.Auth
{
    public class RegistrationModel : PageModel
    {
        private readonly ApplicationDbContext data;

        public RegistrationModel(ApplicationDbContext db)
        {
            data = db;
        }
        [BindProperty]
        public User User { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public string ErrorHandler { get; set; }
        public void OnGet(string ErrorHandler)
        {
            this.ErrorHandler = ErrorHandler;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var email = (from user in data.User
                             where user.Email == User.Email
                             select user.Email).FirstOrDefault();

                if(email != null)
                {
                    return RedirectToPage(new { ErrorHandler = "Email is already in use" });
                }
                if(ConfirmPassword != User.Password)
                {
                    return RedirectToPage(new { ErrorHandler = "Password does not match" });
                }
                var crypted = Crypto.HashPassword(User.Password);
                User.Password = crypted;
                await data.User.AddAsync(User);
                await data.SaveChangesAsync();
                return RedirectToPage("Login");
            }
                return Page();
        }
    }
}
