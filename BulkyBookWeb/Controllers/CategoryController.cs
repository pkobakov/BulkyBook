using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;  
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot be equal to the Name.");
            }

            if (ModelState.IsValid)
            {
              _db.Categories.Add(obj);
              _db.SaveChanges();
              TempData["success"] = "Category created successfully ";
              return RedirectToAction("Index");

            }

            return View(obj);
        }

        //GET
        public IActionResult Edit(int id)
        {
            var categoryFromDbFind = _db.Categories.Find(id);
            /*var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id)*/;

            if (categoryFromDbFind == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFind);
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot be equal to the Name.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");

            }

            return View(obj);
        }

        //GET
        public IActionResult Delete(int id)
        {
            var categoryFromDbFind = _db.Categories.Find(id);
            /*var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id)*/
            ;

            if (categoryFromDbFind == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFind);
        }



        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

           _db.Categories.Remove(obj);
           _db.SaveChanges();
            TempData["success"] = "Category deleted successfully.";  
            return RedirectToAction("Index");
        }
    }
}


