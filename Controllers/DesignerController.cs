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
  
    public class DesignerController : Controller
    {
        private readonly TBayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DesignerController(TBayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["Filter"] = searchString;

            var designers = from a in _context.Designer
                          select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                designers = designers.Where(s => s.FirstName.Contains(searchString)
                                          || s.LastName.Contains(searchString));
            }
            return View(await designers.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }
    
      var designer = await _context.Designer
        .FirstOrDefaultAsync(m => m.DesignerID == id);

    if (designer == null)
    {
        return NotFound();
    }

    return View(designer);
}
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
        return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DesignerViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Designer designer = new Designer
                {
                    Picture = uniqueFileName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateofBirth = model.DateofBirth,                    
                    Biography = model.Biography,  
                    Item=model.Item,
                    Items=model.Items
                };

                _context.Add(designer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // GET: Designer/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var designer = await _context.Designer.FindAsync(id);

            if (designer == null)
            {
                return NotFound();
            }

              DesignerViewModel vm = new DesignerViewModel
                {
                    DesignerID = designer.DesignerID,
                    FirstName = designer.FirstName,
                    LastName = designer.LastName,
                    DateofBirth = designer.DateofBirth,                   
                    Biography = designer.Biography,                    
                    Item=designer.Item
                };
            return View(vm);
        }

        // POST: Designer/Edit/5
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        
            public async Task<IActionResult> EditPost(int? id, DesignerViewModel vm)
        {
            if (id != vm.DesignerID)
            {
                return NotFound();
            }

       if (ModelState.IsValid)
       {
           try
                {
                    string uniqueFileName = UploadedFile(vm);

               Designer designer = new Designer
               {
                    DesignerID = vm.DesignerID,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    DateofBirth = vm.DateofBirth,                   
                    Biography = vm.Biography,
                    Picture = uniqueFileName,                    
                    Item=vm.Item
                };

                    _context.Update(designer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignerExists(vm.DesignerID))
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

        private void PopulateDropDownList(object selectedItem = null)
        {
            var itemsQuery = from d in _context.Item
                                   orderby d.Name
                                   select d;
            ViewBag.Items = new SelectList(itemsQuery.AsNoTracking(), "ItemsId", "Name", selectedItem);
        }


        // GET: Designer/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var designer = await _context.Designer
                .FirstOrDefaultAsync(m => m.DesignerID == id);
            if (designer == null)
            {
                return NotFound();
            }

            return View(designer);
        }

        // POST: Designer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var designer = await _context.Designer.FindAsync(id);
            _context.Designer.Remove(designer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
          [HttpPost]
     
        private bool DesignerExists(int id)
        {
            return _context.Designer.Any(e => e.DesignerID == id);
        }

         private string UploadedFile(DesignerViewModel model)
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
         public async Task<IActionResult> MyItems(int id)
        {
            IQueryable<Item> item = _context.Item;

           item = item.Where(s=>s.Designerid==id);
            
            ViewData["DesignersName"] = _context.Designer.Where(t => t.DesignerID == id).Select(t => t.FullName).FirstOrDefault();
            return View(item);
        }
             
    }
}