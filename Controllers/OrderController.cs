using EFTask.Data;
using EFTask.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using static EFTask.Models.ViewModel.OrderViewModel;
using System;
using EFTask.Models;

namespace EFTask.Controllers
{
    public class OrderController : Controller
    {


        private readonly ApplicationDbContext _dbContext;
        //  IList<OrderViewModel> sa = new IList<OrderViewModel>();
        public OrderController(ApplicationDbContext dbCOntext)
        {
            _dbContext = dbCOntext;
        }
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
                         select new OrderViewModel
                         {
                             ItemId = item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = item.Name,
                             Description = item.Description,
                             Price = item.Price,
                             Imgurl = item.Imgurl,
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
            var items = (from item in _dbContext.Items

                         join ItemUnit in _dbContext.UnitItems on item.ItemId equals ItemUnit.ItemId
                         join unit in _dbContext.Units on ItemUnit.UnitId equals unit.UnitId
                         select new OrderViewModel
                         {
                             ItemId = item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = item.Name,
                             Description = item.Description,
                             Price = item.Price,
                             Imgurl = item.Imgurl,
                         }).ToList();

            return View(items);
            //  return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Create(List<OrderViewModel> orderViewModels)
        {

            var newOrder = new Order()
            {
                OrderDate = DateTime.UtcNow,
                OrderName = "Burhan"  /*orderViewModels.Select(Order => Order.OrderName).FirstOrDefault().ToString(),*/
            };
            var checkQuantity = orderViewModels.Select(x=>x.Quantity).ToList();
            var CheckSelectedItem = orderViewModels.Select(x => x.ItemId).Count();

            if (checkQuantity is not null && CheckSelectedItem > 0)
            {
                foreach (var item in orderViewModels)
                {
                    var UnitItemID = _dbContext.UnitItems.Where(x => x.ItemId ==item.ItemId).FirstOrDefault();
                
                    newOrder.OrderItem.Add(new OrderedItem()
                    {
                        UnitItemIdFK = UnitItemID.UnitItemId,
                        Quantity = item.Quantity,
                        Sub_Total = item.Price * item.Quantity,
                        OrderId_FK = newOrder.OrderId
                    });
                    
                   await  _dbContext.AddAsync(newOrder);
                    newOrder.TotalPrice += item.Price * item.Quantity;
                }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }


        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var items = (from item in _dbContext.Orders
                         
                         join OrderedItem in _dbContext.OrderedItems on item.OrderId equals OrderedItem.OrderId_FK
                         join UnitItems in _dbContext.UnitItems on OrderedItem.UnitItemIdFK equals UnitItems.ItemId
                         join unit in _dbContext.Units on UnitItems.UnitId equals unit.UnitId
                         join Item in _dbContext.Items on UnitItems.ItemId equals Item.ItemId
                         select new OrderViewModel
                         {
                             ItemId = Item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = Item.Name,
                             Description = Item.Description,
                             Price = Item.Price,
                             Imgurl = Item.Imgurl,
                             Quantity=OrderedItem.Quantity,
                             OrderId= item.OrderId,
                             TotalPrice= item.TotalPrice,

                         }).ToList();

            return View(items);
            //  return RedirectToAction("Index");
        }
    }
}
