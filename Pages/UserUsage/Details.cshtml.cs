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
    public class DetailsModel : PageModel
    {
        private readonly WorkersApp.Models.ApplicationDbContext _context;

        public DetailsModel(WorkersApp.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        public User User { get; set; }
        public IEnumerable<Worker> Workers { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            
            if (string.IsNullOrWhiteSpace((string)HttpContext.Session.GetString("SessionUser")) == true)
            {
                return RedirectToPage("/Auth/Login");
            }
            if (id == null)
            {
                return NotFound();
            }
            User = await _context.User.FindAsync(id);
            if (User.Role == User.RoleBar.Seller)
            {
                Workers = await (from workers in _context.Worker
                                 where workers.UserId == id
                                 select workers).ToListAsync();
            }
            return Page();
        }
    }
}
