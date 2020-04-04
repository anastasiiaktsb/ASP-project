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
        private readonly WorkersApp.Models.ApplicationDBContext _context;

        public IndexModel(WorkersApp.Models.ApplicationDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string Search { get; set; }
        public User User { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
        public Worker Worker { get; set; }
        public string NameSort { get; set; }

        public string SurnameSort { get; set; }
        public string ExperienceSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGet(string sortOrder)
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

        public void OnPost()
        {
            User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));
            if (Search != null)
            {
                Workers = (from worker in _context.Worker
                           where worker.Name.ToLower().Contains(Search.ToLower()) ||
                           worker.Surname.ToLower().Contains(Search.ToLower()) ||
                           worker.Experience.ToString().Contains(Search) ||
                           worker.PhoneNumber.Contains(Search)
                           select worker).ToList();
            }

        }
    }
}
