using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LocalMotoAdsWebsite.Data;
using LocalMotoAdsWebsite.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using LocalMotoAdsWebsite.Models.ViewModels;

namespace LocalMotoAdsWebsite.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<IdentityUser> _userManager;

        private readonly IWebHostEnvironment WebHostEnvironment;

        public AdvertsController(AppDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;            
            _context = context;
            WebHostEnvironment = webHostEnvironment;
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;            
        }

        // GET: Adverts
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            var adverts = from a in _context.Adverts
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                adverts = adverts.Where(s => s.Name.Contains(searchString)
                                       || s.Descritpion.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "price_desc":
                    adverts = adverts.OrderByDescending(s => s.Price);
                    break;
                case "price_asc":
                    adverts = adverts.OrderBy(s => s.Price);
                    break;
                case "name_desc":
                    adverts = adverts.OrderByDescending(s => s.Name);
                    break;
                default:
                    adverts = adverts.OrderBy(s => s.Name);
                    break;
            }

           //var appDbContext = _context.Adverts.Include(a => a.Model);
            return View(await adverts.ToListAsync());
        }

        // GET: Adverts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advert = await _context.Adverts
                .Include(a => a.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advert == null)
            {
                return NotFound();
            }

            return View(advert);
        }        
        // GET: Adverts/Create
        [Authorize(Roles = "User,Admin")]
        public IActionResult Create()
        {
            ViewData["ModelFK"] = new SelectList(_context.Models, "Id", "Name");
            return View();
        }

        // POST: Adverts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create(AdvertViewModel vm, Advert advert)
        {
            string stringFileName = UploadFile(vm);
            if (ModelState.IsValid)
            {
                var id = _userManager.GetUserId(User);
                advert.UserId = id;
                advert.ImagePath = stringFileName;
                
                _context.Add(advert);
                
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModelFK"] = new SelectList(_context.Models, "Id", "Name", advert.ModelFK);
            return View(advert);
        }

        private string UploadFile(AdvertViewModel vm)
        {
            string file = null;
            if (vm.ImagePath != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                file = Guid.NewGuid().ToString() + "-" + vm.ImagePath.FileName;
                string filePath = Path.Combine(uploadDir, file);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.ImagePath.CopyTo(fileStream);
                }

            }
            return file;
        }


        // GET: Adverts/Edit/5
        [Authorize(Roles = "User,Admin")]    

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advert = await _context.Adverts.FindAsync(id);
            if (advert == null)
            {
                return NotFound();
            }
            ViewData["ModelFK"] = new SelectList(_context.Models, "Id", "Name", advert.ModelFK);
            return View(advert);
        }

        // POST: Adverts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Edit(int id, AdvertViewModel vm, Advert advert)
        {
            if (id != advert.Id)
            {
                return NotFound();
            }
            string stringFileName = UploadFile(vm);
            if (ModelState.IsValid)
            {
                try
                {
                    advert.ImagePath = stringFileName;
                    _context.Update(advert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertExists(advert.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModelFK"] = new SelectList(_context.Models, "Id", "Name", advert.ModelFK);
            return View(advert);
        }

        // GET: Adverts/Delete/5
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advert = await _context.Adverts
                .Include(a => a.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advert == null)
            {
                return NotFound();
            }

            return View(advert);
        }

        // POST: Adverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advert = await _context.Adverts.FindAsync(id);
            _context.Adverts.Remove(advert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertExists(int id)
        {
            return _context.Adverts.Any(e => e.Id == id);
        }
    }
}
