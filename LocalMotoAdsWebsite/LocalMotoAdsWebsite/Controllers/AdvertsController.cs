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
using Microsoft.AspNetCore.Authorization;
namespace LocalMotoAdsWebsite.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<IdentityUser> _userManager;
        //private readonly HostingEnvironment _hostingEnvironment;

        public AdvertsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            //_hostingEnvironment = hostingEnvironment;
            _context = context;
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        }

        // GET: Adverts
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Adverts.Include(a => a.Model);
            return View(await appDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,Name,UserId,Descritpion,VIN,Year,CarMileage,Price,ImagePath,ModelFK")] Advert advert)
        {
            if (ModelState.IsValid)
            {
                var id = _userManager.GetUserId(User);
                advert.UserId = id;
                //string wwrootPath = _hostingEnvironment.ContentRootPath;
                _context.Add(advert);
                //var files = HttpContext.Request.Form.Files;
                //if (files.Count != 0)
                //{
                //    //Extract the extension of submitted file
                //    var Extension = Path.GetExtension(files[0].FileName);

                //    //Create the relative image path to be saved in database table 
                //    var RelativeImagePath = Image.ImagePath + Advert.Id + Extension;

                //    //Create absolute image path to upload the physical file on server
                //    var AbsImagePath = Path.Combine(wwrootPath, RelativeImagePath);
                    

                //    //Upload the file on server using Absolute Path
                //    using (var filestream = new FileStream(AbsImagePath, FileMode.Create))
                //    {
                //        files[0].CopyTo(filestream);
                //    }

                //    //Set the path in database
                //    advert.ImagePath = RelativeImagePath;
                //}
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModelFK"] = new SelectList(_context.Models, "Id", "Name", advert.ModelFK);
            return View(advert);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId,Descritpion,VIN,Year,CarMileage,Price,ImagePath,ModelFK")] Advert advert)
        {
            if (id != advert.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
