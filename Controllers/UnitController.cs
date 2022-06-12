using EFTask.Data;
using EFTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFTask.Controllers
{
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitController(ApplicationDbContext dbCOntext)
        {
            _dbContext = dbCOntext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Units.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        // [Route]
        public async Task<IActionResult> Create(Unit units, int? Id)
        {

            if (units.UnitId == null || units.UnitId == 0)
            {
                await _dbContext.AddAsync(units);
            }
            else
            {
                var model = await _dbContext.Units.FindAsync(Id);
                if (model == null)
                {
                    return NotFound("Record does not exist againt Give Id");
                }
                model.UnitType = units.UnitType;
                _dbContext.Units.Update(model);
            }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound("Record does not exist againt Give Id");

            }
            return View(await _dbContext.Units.FindAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Unit units, int? Id)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Units.Update(units);
                await _dbContext.SaveChangesAsync();
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
            var model = await _dbContext.Units.FindAsync(Id);
            if (model != null)
            {
                _dbContext.Units.Remove(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            else
                return NotFound("Record does not exist againt Give Id");
            return RedirectToAction("Index");
        }


    }
}
