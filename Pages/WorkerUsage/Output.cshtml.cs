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
    public class OutputModel : PageModel
    {
        private readonly ApplicationDbContext data;
        public OutputModel(ApplicationDbContext db)
        {
            data = db;
        }

        public IEnumerable<Worker> Workers { get; set; }
        public async Task OnGet()
        {
            var userInfo = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));
            Workers = await data.Worker.ToListAsync();
        }

    }
}