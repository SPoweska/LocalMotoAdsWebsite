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
using Microsoft.AspNetCore.Authorization;

namespace LocalMotoAdsWebsite.Controllers
{
    [Authorize(Roles = "User,Admin")]

    public class MyAdvertsController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public MyAdvertsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        }

        // GET: MyAdverts
        public async Task<IActionResult> Index()
        {
            //TODO: Take adverts which current user id == UserID            
            var id = _userManager.GetUserId(User);
            var appDbContext = _context.Adverts.Include(a => a.Model);
            return View(await appDbContext.Where(x=>x.UserId.Equals(id)).ToListAsync());
        }

        // GET: MyAdverts/Details/5        
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

        // GET: MyAdverts/Edit/5        
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

        // POST: MyAdverts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
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

        // GET: MyAdverts/Delete/5        
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

        // POST: MyAdverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
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
