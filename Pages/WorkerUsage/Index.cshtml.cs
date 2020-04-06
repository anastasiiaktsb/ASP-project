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

namespace WorkersApp.Pages.WorkerUsage
{
    public class IndexModel : PageModel
    {
        private readonly WorkersApp.Models.ApplicationDbContext _context;

        public IndexModel(WorkersApp.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        public string CurrentFilter { get; set; }
        [BindProperty]
        public string Search { get; set; }
        public User User { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
        public Worker Worker { get; set; }
        public string NameSort { get; set; }

        public string SurnameSort { get; set; }
        public string ExperienceSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGet(string sortOrder, string searchString)
        {
            if (string.IsNullOrWhiteSpace((string)HttpContext.Session.GetString("SessionUser")) == true)
            {
                return RedirectToPage("/Auth/Login");
            }
            User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));

            if (User.IsSeller)
            {
                Workers = await (from worker in _context.Worker
                                 where worker.UserId == User.Id
                                 select worker).ToListAsync();
            }
            else
            {
                Workers = await _context.Worker.ToListAsync();
            }

            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            SurnameSort = sortOrder == "Surname" ? "surname_desc" : "Surname";
            ExperienceSort = sortOrder == "Experience" ? "experience_desc" : "Experience";
            CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                    Workers = (from worker in Workers
                               where worker.Name.ToLower().Contains(searchString.ToLower()) ||
                               worker.Surname.ToLower().Contains(searchString.ToLower()) ||
                               worker.Experience.ToString().Contains(searchString) ||
                               worker.PhoneNumber.Contains(searchString)
                               select worker);
                
            }

            if (SurnameSort != null || NameSort != null || ExperienceSort != null)
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        Workers = Workers.OrderByDescending(s => s.Name);
                        break;
                    case "Surname":
                        Workers = Workers.OrderBy(s => s.Surname);
                        break;
                    case "surname_desc":
                        Workers = Workers.OrderByDescending(s => s.Surname);
                        break;
                    case "Experience":
                        Workers = Workers.OrderBy(s => s.Experience);
                        break;
                    case "experience_desc":
                        Workers = Workers.OrderByDescending(s => s.Experience);
                        break;
                    default:
                        Workers = Workers.OrderBy(s => s.Name);
                        break;
                }

            }
            return Page();

        }

    }
}
