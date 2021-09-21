using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TBay.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using TBay.ViewModels;

namespace TBay.Controllers
{
  
    public class StoreController : Controller
    {
       private readonly TBayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoreController(TBayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["Filter1"] = searchString;

            var stores = from m in _context.Store
                        select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(s => s.Name.Contains(searchString));
            }
            return View(await stores.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var store = await _context.Store
        .Include(s => s.Item)
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.StoreID == id);

    if (store == null)
    {
        return NotFound();
    }

    return View(store);
}
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDownList();
            return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Store store = new Store
                {
                    Picture = uniqueFileName,
                    Name = model.Name,
                    StoreID = model.StoreID,
                    ItemId = model.ItemId,
                    Link = model.Link,
                    Rating = model.Rating,
                    Item = model.Item
                };

                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownList(model.ItemId);
            return View();
        }


        // GET: Store/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

              StoreViewModel vm = new StoreViewModel
                {
                    StoreID = store.StoreID,
                    Name = store.Name,                    
                    ItemId= store.ItemId,                    
                    Link = store.Link,
                    Rating= store.Rating,
                    Item= store.Item
                };

            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "Name", store.ItemId);
            return View(vm);
        }

        // POST: Store/Edit/5
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, StoreViewModel vm)
        {
            if (id != vm.StoreID)
            {
                return NotFound();
            }

       if (ModelState.IsValid)
       {
           try
                {
                    string uniqueFileName = UploadedFile(vm);

                Store store = new Store
                {
                    StoreID = vm.StoreID,
                    Name = vm.Name,                    
                    ItemId=vm.ItemId,
                    Picture = uniqueFileName,                    
                    Link = vm.Link,
                    Rating= vm.Rating,
                    Item=vm.Item
                };

                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(vm.StoreID))
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
    return View(vm);
          }

        private void PopulateDropDownList(object selecteditem = null)
        {
            var itemQuery = from d in _context.Item
                                   orderby d.Name
                                   select d;
            ViewBag.ItemId = new SelectList(itemQuery.AsNoTracking(), "ItemsID", "Name", selecteditem);
        }


        // GET: Store/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(c => c.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StoreID == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Store.FindAsync(id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
          [HttpPost]
     
        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.StoreID == id);
        }
         private string UploadedFile(StoreViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}