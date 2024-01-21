using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Manage.ViewModels;
using WebApplication1.DAL;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class FeautureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env ;

        public FeautureController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task< IActionResult> Index(int page=1)
        {
            int take = 3;
            decimal count=await _context.Feautures.CountAsync();    
            List<Feauture> feautures=await _context.Feautures.Skip((page-1)*take).Take(take).ToListAsync();
            PaginateVm<Feauture> paginateVm = new PaginateVm<Feauture>()
            {
                Currentpage = page,
                Totalpage = Math.Ceiling(count / take),
                Take=take,
                items = feautures
            };
            return View(paginateVm);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(Feauture feauture)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(feauture.ImageFile == null) 
            {
                ModelState.AddModelError("ImageFile", "Imagefile null ola bilmez");
                return View();  
            }
            if(feauture.ImageFile.CheckFileLength(300))
            {
                ModelState.AddModelError("ImageFile", "ImageFile 300 kb dan cox olmalidir");
                return View();
            }
            if(!feauture.ImageFile.CheckFileType("image/"))
            {
                ModelState.AddModelError("ImageFile", "ImageFile image type da olmalidir");
                return View();
            }
            feauture.Image = feauture.ImageFile.CreateFile(_env.WebRootPath, "uploads/feauture");
            await _context.Feautures.AddAsync(feauture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
     
        public async Task< IActionResult> Update(int?id)
        {
            if(id == null) 
            {
                return NotFound();
            }
            Feauture oldfeauture=await _context.Feautures.FirstOrDefaultAsync(x=>x.Id==id); 
            if(oldfeauture == null)
            {
                return BadRequest();
            }
            return View(oldfeauture);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,Feauture newfeauture)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if (id == null)
            {
                return NotFound();
            }
            Feauture oldfeauture = await _context.Feautures.FirstOrDefaultAsync(x => x.Id == id);
            if (oldfeauture == null)
            {
                return BadRequest();
            }
            if (newfeauture.ImageFile != null)
            {
                if (newfeauture.ImageFile.CheckFileLength(300))
                {
                    ModelState.AddModelError("ImageFile", "ImageFile 300 kb dan cox olmalidir");
                    return View();
                }
                if (!newfeauture.ImageFile.CheckFileType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "ImageFile image type da olmalidir");
                    return View();
                }
                oldfeauture.Image.DeleteFile(_env.WebRootPath, "uploads/feauture");
                oldfeauture.Image=newfeauture.ImageFile.CreateFile(_env.WebRootPath, "uploads/feauture");
            }
            oldfeauture.Name = newfeauture.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Feauture oldfeauture = await _context.Feautures.FirstOrDefaultAsync(x => x.Id == id);
            if (oldfeauture == null)
            {
                return BadRequest();
            }
            oldfeauture.Image.DeleteFile(_env.WebRootPath, "uploads/feauture");
            _context.Feautures.Remove(oldfeauture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }

    }
}
