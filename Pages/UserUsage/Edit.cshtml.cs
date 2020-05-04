using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkersApp.Models;

namespace Workerapp.Pages.UserUsage
{
    public class EditModel : PageModel
    {
        private readonly WorkersApp.Models.ApplicationDbContext _context;

        public EditModel(WorkersApp.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            User = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            User.IsActive = !User.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");

        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
