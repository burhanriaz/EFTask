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
        //public async Task<IActionResult> Index(string sortOrder, string searchString)
        //{
        //    ViewData["ItemNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "ItemName_desc" : "";
        //    ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
        //    ViewData["CurrentFilter"] = searchString;

        //    //var item = _dbContext.Items.Include(p => p.UnitItems)
        //    // //.Where(p => p.ItemId == y=>y.uni).FirstOrDefault();
        //    //var o = await _dbContext.Items.Include(x => x.UnitItems).ThenInclude(y => y.Unit).ToListAsync();
        //    //var item = await _dbContext.Items.Include(x => x.UnitItems).ThenInclude(y => y.Unit).ToListAsync();
        //    var xs = new ItemViewModel();
        //    var items = (from item in _dbContext.Items

        //                 join ItemUnit in _dbContext.UnitItems on item.ItemId equals ItemUnit.ItemId
        //                 join unit in _dbContext.Units on ItemUnit.UnitId equals unit.UnitId
        //                 select new OrderViewModel
        //                 {
        //                     ItemId = item.ItemId,
        //                     UnitId = unit.UnitId,
        //                     UnitType = unit.UnitType,
        //                     ItemName = item.Name,
        //                     Price = item.Price,
        //                 });
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        items = items.Where(s => s.ItemName.Contains(searchString)
        //                               || s.UnitType.Contains(searchString));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "ItemName_desc":
        //            items = items.OrderByDescending(s => s.ItemName);
        //            break;
        //        case "Price":
        //            items = items.OrderBy(s => s.Price);
        //            break;
        //        case "Price_desc":
        //            items = items.OrderByDescending(s => s.Price);
        //            break;
        //        default:
        //            items = items.OrderBy(s => s.ItemName);
        //            break;
        //    }
        //    return View(await items.AsNoTracking().ToListAsync());

        //}

        public async Task<IActionResult> IndexOrder(string sortOrder, string searchString)
        {
            ViewData["OrderNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "OrderName_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewData["CurrentFilter"] = searchString;

            var orders = await _dbContext.Orders.ToListAsync();


            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.OrderName.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "OrderName_desc":
                    orders = orders.OrderByDescending(s => s.OrderName).ToList();
                    break;
                case "Price":
                    orders = orders.OrderBy(s => s.TotalPrice).ToList();
                    break;
                case "Price_desc":
                    orders = orders.OrderBy(s => s.TotalPrice).ToList();
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.OrderDate).ToList();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.OrderDate).ToList();
                    break;
                default:
                    orders = orders.OrderBy(s => s.OrderName).ToList();
                    break;
            }
            return View(orders);

        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order, int? Id)
        {

            if (ModelState.IsValid)
            {

                order.OrderDate = DateTime.UtcNow;
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("IndexOrder");


            }
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int? Id)
        {
            if (Id is not null)
            {
                var order = await _dbContext.Orders.FindAsync(Id);
                if (order is null)
                {
                    return NotFound("Order does not exist");
                }
                return View("EditOrder", order);
            }
            return NotFound("Order does not exist");
        }
        [HttpPost]
        public async Task<IActionResult> EditOrder(Order order)
        {

            if (order.OrderId is not null || order.OrderId != 0)
            {
                var odr = await _dbContext.Orders.FindAsync(order.OrderId);
                if (odr is null)
                {
                    return NotFound("Record  Not found");
                }

                if (ModelState.IsValid)
                {
                    // or.OrderDate = odr.OrderDate;
                    odr.OrderName = order.OrderName;
                    _dbContext.Orders.Update(odr);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("IndexOrder");
                }
                else
                    return View(order);
            }
            return View(order);

        }

        [HttpGet]
        public async Task<IActionResult> AddOrderItems(int? id)
        {
            if (id is null)
            {
                return NotFound("Please Create ORder first!!!");
            }
            var items = (from item in _dbContext.Items

                         join ItemUnit in _dbContext.UnitItems on item.ItemId equals ItemUnit.ItemId
                         join unit in _dbContext.Units on ItemUnit.UnitId equals unit.UnitId
                         select new OrderViewModel
                         {
                             ItemId = item.ItemId,
                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,
                             ItemName = item.Name,
                             Price = item.Price,
                             OrderId = id
                         }).ToList();

            return View(items);
            //  return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> AddOrderItems(List<OrderViewModel> orderViewModels, int? id)
        {
            var orderID = orderViewModels.Select(x => x.OrderId).FirstOrDefault();
            var model = await _dbContext.Orders.FindAsync(orderID);
            //{
            //    OrderDate = DateTime.UtcNow,
            //    OrderName = "Burhan"  /*orderViewModels.Select(Order => Order.OrderName).FirstOrDefault().ToString(),*/
            //};
            var checkQuantity = orderViewModels.Select(x => x.Quantity).ToList();
            var CheckSelectedItem = orderViewModels.Select(x => x.ItemId).Count();

            if (checkQuantity is not null && CheckSelectedItem > 0 && orderID > 0)
            {
                foreach (var item in orderViewModels)
                {
                    // var UnitItemID = _dbContext.UnitItems.Where(x => x.ItemId == item.ItemId).FirstOrDefault();

                    if (item.Quantity > 0)
                    {
                        var orderItem = new OrderedItem()
                        {
                            ItemId_Fk = item.ItemId,
                            UnitId_Fk = item.UnitId,
                            Quantity = item.Quantity,
                            Sub_Total = item.Price * item.Quantity,
                            OrderId_FK = item.OrderId,
                        };
                        model.TotalPrice += item.Price * item.Quantity;
                        _dbContext.Update(model);

                        await _dbContext.AddAsync(orderItem);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(IndexOrder));
            }
            //return RedirectToAction("CreateOrder");
            return RedirectToAction(nameof(IndexOrder));

        }


        [HttpGet]
        public async Task<IActionResult> EditOrderItems( int? id)
        {

            if (id is null || id == 0)
            {
                return NotFound("Result Not found");
            }

            var items = (from item in _dbContext.Orders

                         join OrderedItem in _dbContext.OrderedItems on item.OrderId equals OrderedItem.ItemId_Fk
                         //join UnitItems in _dbContext.UnitItems on OrderedItems. equals UnitItems.ItemId
                         join unit in _dbContext.Units on OrderedItem.UnitId_Fk equals unit.UnitId
                         join Item in _dbContext.Items on OrderedItem.ItemId_Fk equals Item.ItemId
                         select new OrderViewModel
                         {
                             ItemId = Item.ItemId,
                             ItemName = Item.Name,

                             OrderId = item.OrderId,
                             OrderName = item.OrderName,

                             UnitId = unit.UnitId,
                             UnitType = unit.UnitType,

                             Sub_Total = OrderedItem.Sub_Total,
                             Price = Item.Price,
                             Quantity = OrderedItem.Quantity,
                             TotalPrice = item.TotalPrice,


                         }).ToList();

            return View(items);
        }
        [HttpPost]
        public async Task<IActionResult> EditOrderItems(List<OrderViewModel> orderViewModels, int? id)
        {

            var orderID = orderViewModels.Select(x => x.OrderId).FirstOrDefault();
            var model = await _dbContext.Orders.FindAsync(orderID);
            //{
            //    OrderDate = DateTime.UtcNow,
            //    OrderName = "Burhan"  /*orderViewModels.Select(Order => Order.OrderName).FirstOrDefault().ToString(),*/
            //};
            var checkQuantity = orderViewModels.Select(x => x.Quantity).ToList();
            var CheckSelectedItem = orderViewModels.Select(x => x.ItemId).Count();

            if (checkQuantity is not null && CheckSelectedItem > 0 && orderID > 0)
            {
                foreach (var item in orderViewModels)
                {
                    var models = _dbContext.OrderedItems.Where(x => x.ItemId_Fk == item.ItemId).FirstOrDefault();

                    if (item.Quantity > 0)
                    {
                        var orderItem = new OrderedItem()
                        {
                            ItemId_Fk = item.ItemId,
                            UnitId_Fk = item.UnitId,
                            Quantity = item.Quantity,
                            Sub_Total = item.Price * item.Quantity,
                            OrderId_FK = item.OrderId,
                        };
                        model.TotalPrice += item.Price * item.Quantity;
                        _dbContext.Update(model);

                        await _dbContext.AddAsync(orderItem);
                    }
                    else if (item.Quantity ==0)
                    {
                        //var orderItem = new OrderedItem()
                        //{
                        //    ItemId_Fk = item.ItemId,
                        //    UnitId_Fk = item.UnitId,
                        //    Quantity = item.Quantity,
                        //    Sub_Total = item.Price * item.Quantity,
                        //    OrderId_FK = item.OrderId,
                        //};
                        model.TotalPrice -= item.Price * item.Sub_Total;
                        _dbContext.Remove(model);

                        //await _dbContext.AddAsync(orderItem);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(IndexOrder));
            }
            return RedirectToAction("IndexOrder");
            return RedirectToAction(nameof(IndexOrder));

        }



            [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is not null)
            {
                var order = await _dbContext.Orders.FindAsync(Id);
                if (order != null)
                {
                    _dbContext.Orders.Remove(order);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("IndexOrder");
                }
            }
            return NotFound("Record Not Found");
        }



    }
}
