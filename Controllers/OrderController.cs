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
using Microsoft.Extensions.Logging;

namespace EFTask.Controllers
{
    public class OrderController : Controller
    {


        private readonly ApplicationDbContext _dbContext;
        public ILogger<AdministratorController> _Logger { get; }
        //  IList<OrderViewModel> sa = new IList<OrderViewModel>();
        public OrderController(ApplicationDbContext dbCOntext, ILogger<AdministratorController> Logger)
        {
            _dbContext = dbCOntext;
            _Logger = Logger;   
            
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

            // Eagerloading 
            var EagerloadingOrder = await _dbContext.Orders.Include(o => o.OrderItem).ToListAsync();

            // Explicit Loading
            var OrderId = 12;
            var odr = _dbContext.Orders.FirstOrDefault(a => a.OrderId == OrderId);
            _dbContext.Entry(odr).Collection(o => o.OrderItem).Load();// load collections
         //   _dbContext.Entry(odr).Reference(s => s.OrderItem).Load(); //  load single navigation property

            // Lazy loading 
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
           
        }

        ///////////////////////////////////////////////////////////////////////
        // Add new to buy items
        [HttpGet]
        public async Task<IActionResult> BuyItems(int? id)
        {
            if (id is null)
            {
                return NotFound("Please Create Order first!!!");
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
        }
        [HttpPost]
        public async Task<IActionResult> BuyItems(OrderViewModel orderViewModels, int? id)
        {
            var orderID = orderViewModels.OrderId;
            var model = await _dbContext.Orders.FindAsync(orderID);


            if (orderViewModels.Quantity > 0 && orderViewModels.ItemId > 0 && orderID > 0)
            {
                var price = await _dbContext.Items.FindAsync(orderViewModels.ItemId);

                var orderedItem = await _dbContext.OrderedItems.FindAsync(orderID, orderViewModels.ItemId);
                if (orderedItem == null)
                {
                    var orderItem = new OrderedItem()
                    {
                        ItemId_Fk = orderViewModels.ItemId,
                        UnitId_Fk = orderViewModels.UnitId,
                        Quantity = orderViewModels.Quantity,
                        Sub_Total = price.Price * orderViewModels.Quantity,
                        OrderId_FK = orderViewModels.OrderId,
                    };
                    model.TotalPrice += orderItem.Sub_Total;
                    _dbContext.Update(model);
                    if (ModelState.IsValid)
                    {
                        await _dbContext.AddAsync(orderItem);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    // orderedItem.ItemId_Fk = orderViewModels.ItemId;
                    orderedItem.UnitId_Fk = orderViewModels.UnitId;
                    orderedItem.Quantity += orderViewModels.Quantity;
                    orderedItem.Sub_Total += price.Price * orderViewModels.Quantity;
                    model.TotalPrice += price.Price * orderViewModels.Quantity;
                    _dbContext.Update(model);
                    if (ModelState.IsValid)
                    {
                        _dbContext.Update(orderedItem);
                        await _dbContext.SaveChangesAsync();
                    }
                };
                return RedirectToRoute(orderID);

            }
            return RedirectToRoute(orderID);
        }
        [HttpGet]
        public async Task<IActionResult> EditOrderItem(int? id)
        {
            if (id is null)
            {
                return NotFound("Please Create Order first!!!");
            }
         

            var order = _dbContext.OrderedItems.ToList();
           
            List<OrderViewModel> Ordered = new List<OrderViewModel>();

            foreach (var item in order)
            {
                if(item.OrderId_FK==id)
                {
                    
                    var units = _dbContext.Units.Where(x => x.UnitId == item.UnitId_Fk).FirstOrDefault();
                    var ItemName = _dbContext.Items.Where(x => x.ItemId == item.ItemId_Fk).FirstOrDefault();

                    var x = new OrderViewModel()
                    {
                        ItemId = item.ItemId_Fk,
                        UnitId = item.UnitId_Fk,
                        UnitType = units.UnitType,
                        ItemName = ItemName.Name,
                        Price = ItemName.Price,
                        Quantity = item.Quantity,
                        Sub_Total = item.Sub_Total,
                        OrderId = id,
                    };
                    Ordered.Add(x);
                }
            }
            //var che= from order in _dbContext.OrderedItems
                //var f=  _dbContext.OrderedItems.Select(id).FirstOrDefault();
            //var items = (from item in _dbContext.OrderedItems

            //             join Item in _dbContext.Items on item.ItemId_Fk equals Item.ItemId
            //             join unit in _dbContext.Units on item.UnitId_Fk equals unit.UnitId
            //             select new OrderViewModel
            //             {
            //                 ItemId = item.ItemId_Fk,
            //                 UnitId = unit.UnitId,
            //                 UnitType = unit.UnitType,
            //                 ItemName = item.Item.Name,
            //                 Price = Item.Price,
            //                 Quantity = item.Quantity,
            //                 Sub_Total = item.Sub_Total,
            //                 OrderId = id

            //             }).ToList();

            return View(Ordered);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditOrderItem(OrderViewModel orderViewModels)
        {
            var orderID = orderViewModels.OrderId;
            var model = await _dbContext.Orders.FindAsync(orderID);


            if (orderViewModels.Quantity > 0 && orderViewModels.ItemId > 0 && orderID > 0)
            {
                var price = await _dbContext.Items.FindAsync(orderViewModels.ItemId);

                var orderedItem = await _dbContext.OrderedItems.FindAsync(orderID, orderViewModels.ItemId);
                ////  var ItemExist = await _dbContext.OrderedItems.FindAsync(orderViewModels.ItemId);
                if (orderedItem != null)
                {
                    decimal price_to_sub = orderedItem.Sub_Total;

                    orderedItem.ItemId_Fk = orderViewModels.ItemId;
                    orderedItem.UnitId_Fk = orderViewModels.UnitId;
                    orderedItem.Quantity = orderViewModels.Quantity;
                    orderedItem.Sub_Total = price.Price * orderViewModels.Quantity;
                    orderedItem.OrderId_FK = orderViewModels.OrderId;

                    model.TotalPrice -= price_to_sub;
                    model.TotalPrice += orderedItem.Sub_Total;
                    _dbContext.Update(model);
                    if (ModelState.IsValid)
                    {
                        _dbContext.Update(orderedItem);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToRoute(orderID);
                    }
                }
                return RedirectToRoute(orderID);
            }
            return RedirectToRoute(orderID);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveItem(OrderViewModel orderViewModels)
        {
            var model = await _dbContext.Orders.FindAsync(orderViewModels.OrderId);
            if ( orderViewModels.ItemId > 0 && orderViewModels.OrderId > 0)
            {
                var orderedItem = await _dbContext.OrderedItems.FindAsync(orderViewModels.OrderId, orderViewModels.ItemId);

                if (orderedItem != null)
                {
                    model.TotalPrice -= orderedItem.Sub_Total;
                    _dbContext.Update(model);
                   
                        _dbContext.Remove(orderedItem);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction("EditOrderItem",  new { @id = orderViewModels.OrderId });
                       
                }
                return RedirectToAction("EditOrderItem", new { @id = orderViewModels.OrderId });

               
            }
            return RedirectToAction("EditOrderItem", new { @id = orderViewModels.OrderId });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is not null)
            {
                var order = await _dbContext.Orders.FindAsync(Id);
                if (order != null)
                {
                    try 
                    { 

                    _dbContext.Orders.Remove(order);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("IndexOrder");
                }
                catch (DbUpdateException ex)
                {
                    //Log the exception to a file.
                    _Logger.LogError($"Exception Occured : {ex}");
                    // Pass the ErrorTitle and ErrorMessage that you want to show to
                    // the user using ViewBag. The Error view retrieves this data
                    // from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $" Order with name {order.OrderName} is in use";
                    ViewBag.ErrorMessage = $"Order cannot be deleted becaus with this Order {order.OrderName}   buy some items. If you want to delete this Order, please remove the all Items  from the Items and then try to delete";
                    return View("Error");
                }
            }
            }
            return NotFound("Record Not Found");
        }
 

    }
}
