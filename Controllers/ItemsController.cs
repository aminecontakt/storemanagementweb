using Microsoft.AspNetCore.Mvc;
using StoreManagementWeb.Models;
using StoreManagementWeb.Helpers;
using System.Collections.Generic;

namespace StoreManagementWeb.Controllers
{
    public class ItemsController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public ItemsController()
        {
            // Connection string for local PostgreSQL database
            string connectionString = "Host=localhost;Username=postgres;Password=Admin@123;Database=storemanagement";
            _dbHelper = new DatabaseHelper(connectionString);
            _dbHelper.CreateTableIfNotExists();
        }

        // GET: Items
        public IActionResult Index()
        {
            var items = _dbHelper.GetAllItems();
            return View(items);
        }

        // GET: Items/Details/5
        public IActionResult Details(int id)
        {
            var item = _dbHelper.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,Price,Quantity")] Item item)
        {
            if (ModelState.IsValid)
            {
                _dbHelper.AddItem(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _dbHelper.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,Price,Quantity")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbHelper.UpdateItem(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _dbHelper.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dbHelper.DeleteItem(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
