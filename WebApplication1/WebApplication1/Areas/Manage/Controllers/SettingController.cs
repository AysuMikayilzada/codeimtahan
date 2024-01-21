using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult>Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result=await _context.Settings.AnyAsync(X=>X.Key==setting.Key);
            if (!result) 
            {
                ModelState.AddModelError("Key", "eyni keyde yarana bimez");
                return View();  
            }
            await _context.AddAsync(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return View ();
            }
            Setting setting=await _context.Settings.FirstOrDefaultAsync(x=>x.Id==id); 
            if (setting==null)
            {
                return NotFound();  
            }
            return View(setting);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Setting OLDsetting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
