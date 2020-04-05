using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkersApp.Models;

namespace WorkersApp.Pages.WorkerUsage
{
    public class DetailsModel : PageModel
    {
        private readonly WorkersApp.Models.ApplicationDbContext _context;

        public DetailsModel(WorkersApp.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public Worker Worker { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Worker = await _context.Worker.FirstOrDefaultAsync(m => m.Id == id);

            if (Worker == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
