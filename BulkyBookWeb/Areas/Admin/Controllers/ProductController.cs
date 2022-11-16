using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }



        //GET
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select( x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            if (id == null || id == 0)
            {
                //create product 

                return View(product);
            }
            else
            {
                //update product
            }


            return View(product);
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");

            }

            return View(obj);
        }



        //GET
        public IActionResult Delete(int id)
        {
            var productFromDbFind = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            /*var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id)*/
            ;

            if (productFromDbFind == null)
            {
                return NotFound();
            }

            return View(productFromDbFind);
        }



        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}


