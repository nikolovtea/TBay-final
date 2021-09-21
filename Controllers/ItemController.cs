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
  
    public class ItemController : Controller
    {
        private readonly TBayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemController(TBayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string searchString)
        {         
            ViewData["CurrentFilter"] = searchString;
           
            var items = from b in _context.Item
                   select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }
          
         return View(await items.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Bags()
        {         
          return View(await _context.Item.ToListAsync());
        }
          public async Task<IActionResult> Dresses()
        {         
          return View(await _context.Item.ToListAsync());
        }
           public async Task<IActionResult> Jeans()
        {         
          return View(await _context.Item.ToListAsync());
        }
           public async Task<IActionResult> Shoes()
        {         
          return View(await _context.Item.ToListAsync());
        }
           public async Task<IActionResult> Sunglasses()
        {         
          return View(await _context.Item.ToListAsync());
        }
        

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
             return NotFound();
             }

            var item = await _context.Item
            .Include(s => s.Designer)
            .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ItemsID == id);

            if (item == null)
             {
                 return NotFound();
             }

         return View(item);
        }
        // GET: Item/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDownList();
            return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Item item = new Item
                {
                    Picture = uniqueFileName,
                    Name = model.Name,
                    Category = model.Category,
                    Designerid = model.Designerid,
                    Designer=model.Designer,
                    Price = model.Price,
                    Size = model.Size,
                    Store = model.Store
                };

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //if (ModelState.IsValid)
            //{
               
            //    _context.Add(item);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //} 
          PopulateDropDownList(model.Designerid);          
           return View();
        }


        // GET: Item/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

             ItemViewModel vm = new ItemViewModel
                {
                    ItemsID = item.ItemsID,
                    Name = item.Name,                    
                    Category = item.Category,
                    Size = item.Size,                   
                    Price= item.Price,                   
                    Store= item.Store,
                    Designer= item.Designer,
                    Designerid= item.Designerid
                };
            ViewData["Designerid"] = new SelectList(_context.Designer, "Designerid", "FullName", item.Designerid);

            return View(vm);
        }

        // POST: Item/Edit/5
      
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int? id, ItemViewModel vm)
        {
            if (id != vm.ItemsID)
            {
                return NotFound();
            }

       if (ModelState.IsValid)
       {
           try
                {
                    string uniqueFileName = UploadedFile(vm);

               Item item = new Item
                {
                    ItemsID = vm.ItemsID,
                    Name = vm.Name,                    
                    Category = vm.Category,
                    Size = vm.Size,                   
                    Picture = uniqueFileName,
                    Price=vm.Price,             
                    Store= vm.Store,
                    Designer=vm.Designer,
                    Designerid=vm.Designerid
                };

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(vm.ItemsID))
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

        

        private void PopulateDropDownList(object selecteddesigner = null)
        {
            var designerQuery = from d in _context.Designer
                                   orderby d.FirstName
                                   select d;
            ViewBag.Designerid = new SelectList(designerQuery.AsNoTracking(), "DesignerID", "FullName", selecteddesigner);
        }


        // GET: Item/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(c => c.Designer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ItemsID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
          [HttpPost]
     
        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemsID == id);
        }
          private string UploadedFile(ItemViewModel model)
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
        public IActionResult Text()
        {
            return View();
        }
        public IActionResult Order(int id)
        {
            TempData["ID"] = id;
            ViewData["ItemName"] = _context.Item.Where(t => t.ItemsID == id).Select(t => t.Name).FirstOrDefault();
            ViewData["ItemPrice"] = _context.Item.Where(t => t.ItemsID == id).Select(t => t.Price).FirstOrDefault();
            string imageURL = "~/pictures/" + _context.Item.Where(t => t.ItemsID == id).Select(t => t.Picture).FirstOrDefault();
            ViewData["Image"] = imageURL;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order([Bind("FullName, PhoneNumber, Address, City, ItemId, Item")] User order)
        {
           if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Text));
            }
            
            return View();
        }
       
            public IActionResult OrderedItems()
            {
            int id = Convert.ToInt32(TempData["ID"]);
            IQueryable<User> user = _context.User;
                return View(user);
            }        

    }
}