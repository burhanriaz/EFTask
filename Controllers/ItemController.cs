using EFTask.Data;
using EFTask.Extentions;
using EFTask.Models;
using EFTask.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTask.Controllers
{
    // [Authorize]
     [Authorize]

   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ILogger<AdministratorController> _Logger { get; }

        public ItemController(ApplicationDbContext dbCOntext, ILogger<AdministratorController> logger)
        {
            _dbContext = dbCOntext;
            _Logger = logger;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["ItemNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "ItemName_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

           

            //var item = _dbContext.Items.Include(p => p.UnitItems)
            // //.Where(p => p.ItemId == y=>y.uni).FirstOrDefault();
            //var o = await _dbContext.Items.Include(x => x.UnitItems).ThenInclude(y => y.Unit).ToListAsync();
            //var item = await _dbContext.Items.Include(x => x.UnitItems).ThenInclude(y => y.Unit).ToListAsync();
            var xs = new ItemViewModel();
            var items = (from item in _dbContext.Items

                         join ItemUnit in _dbContext.UnitItems on item.ItemId equals ItemUnit.ItemId
                         join unit in _dbContext.Units on ItemUnit.UnitId equals unit.UnitId
                         select new ItemViewModel
                         {
                             ItemId = item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = item.Name,
                             Price = item.Price,
                         });
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ItemName.Contains(searchString)
                                       || s.UnitType.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "ItemName_desc":
                    items = items.OrderByDescending(s => s.ItemName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                default:
                    items = items.OrderBy(s => s.ItemName);
                    break;
            }
            return View(await items.AsNoTracking().ToListAsync());
           
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {


            ItemUnitVM itemUnitVM = new ItemUnitVM();
            //var unit = _dbContext.Units.ToList();
            //itemUnitVM.Unit= unit.ToSelectListItem();
            var unit = _dbContext.Units.Select(x => new SelectListItem()
            {
                Text = x.UnitType,
                Value = x.UnitId.ToString(),
            }).ToList();
            itemUnitVM.Unit = unit;

            return View(itemUnitVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemUnitVM model)
        {
            //ModelState.Remove("model.Unit");

                var item = new Item
                {
                    Name = model.Item.Name,
                    Price = model.Item.Price,
                };
                var unit = model.Unit.Where(x => x.Selected).Select(y => y.Value).ToList();
                foreach (var i in unit)
                {
                    item.UnitItems.Add(new UnitItem()
                    {
                        UnitId = int.Parse(i)
                    });


                }
        
            if (ModelState.IsValid)
            {
                await _dbContext.AddAsync(item);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is not null)
            {
                var item = _dbContext.Items.Include(p => p.UnitItems).Where(p => p.ItemId == id).FirstOrDefault();

                if (item == null) return
                        NotFound("Record Not found");

                // item = await _dbContext.Items.Include(m => m.UnitItems).Where(m => m.ItemId == id).FirstOrDefaultAsync();
                //var Itemunitvm = new ItemUnitVM()
                //{
                //    Item = item,
                //    Unit = item.UnitItems.Select(p => p.UnitId == id).FirstOrDefault()
                //};
                var selectedunit = item.UnitItems.Select(x => x.UnitId).ToList();
                var unit = _dbContext.Units.Select(x => new SelectListItem()
                {
                    Text = x.UnitType,
                    Value = x.UnitId.ToString(),
                    Selected = selectedunit.Contains(x.UnitId)
                }).ToList();
                ItemUnitVM itemUnitVM = new ItemUnitVM();
                itemUnitVM.Unit = unit;
                itemUnitVM.Item = item;

                return View(itemUnitVM);
            }
            else
            {

            }
            return NotFound("Record Not found");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(ItemUnitVM model, int? Id)
        {
            if (Id == null || model.Item.ItemId == null)
                return NotFound();

            var item = _dbContext.Items.Include(p => p.UnitItems).Where(p => p.ItemId == Id).FirstOrDefault();

            if (item == null)
                return NotFound();
           // item.ItemId = model.Item.ItemId;
            item.Name = model.Item.Name;
            item.Price = model.Item.Price;


            var existingunitID = item.UnitItems.Select(x => x.UnitId).ToList();


            var selectedunitID = model.Unit.Where(x => x.Selected).Select(y => y.Value).Select(int.Parse).ToList();

            var ToAdd = selectedunitID.Except(existingunitID);
            var ToRemove = existingunitID.Except(selectedunitID);
            item.UnitItems = item.UnitItems.Where(x => !ToRemove.Contains(x.UnitId)).ToList();

            foreach (var itm in ToAdd)
            {
                item.UnitItems.Add(new UnitItem()
                {
                    UnitId = itm
                });
            }
            if (ModelState.IsValid)
            {
                _dbContext.Items.Update(item);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null)
            {
                return NotFound("Record does not exist againt Give Id");
            }
            var model = await _dbContext.Items.FindAsync(Id);
            if (model != null)
            {
                try
                {
                    _dbContext.Items.Remove(model);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    //Log the exception to a file.
                    _Logger.LogError($"Exception Occured : {ex}");
                    // Pass the ErrorTitle and ErrorMessage that you want to show to
                    // the user using ViewBag. The Error view retrieves this data
                    // from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $"{model.Name} Item is in use";
                    ViewBag.ErrorMessage = $"{model.Name} item cannot be deleted as there are units in this item. If you want to delete this item, please remove the Unit type from the items and then try to delete";
                    return View("Error");
                }
            }
            else
                return NotFound("Record does not exist againt Give Id");
            return RedirectToAction("Index");
        }

    }
}
