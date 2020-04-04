using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WorkersApp.Models;

namespace WorkersApp.Pages.WorkerUsage
{
    public class CreateModel : PageModel
    {
        private readonly WorkersApp.Models.ApplicationDBContext _context;

        public CreateModel(WorkersApp.Models.ApplicationDBContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public Worker Worker { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var userInfo = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));
            Worker.UserId = userInfo.Id;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.Worker.AddAsync(Worker);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
