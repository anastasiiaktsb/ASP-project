using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkersApp.Models;

namespace Workerapp.Pages.UserUsage
{
    public class IndexModel : PageModel
    {
        private readonly WorkersApp.Models.ApplicationDbContext _context;

        public IndexModel(WorkersApp.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {

            if (string.IsNullOrWhiteSpace((string)HttpContext.Session.GetString("SessionUser")) == true)
            {
                return RedirectToPage("/Auth/Login");
            }
            var session = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));
            if (session.Role != 0)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                User = await (from user in _context.User
                              where user.Role != 0
                              select user).ToListAsync();
            }
            return Page();
        }
    }
}
