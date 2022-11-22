using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }



        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(coverType => new SelectListItem 
                { 
                    Text= coverType.Name,
                    Value = coverType.Id.ToString()
                })

            };

            if (id == null || id == 0)
            {
                //create product 
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;

                return View(productVM);
            }
            else
            {
                //update product

                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id); 
                return View(productVM);
            }


        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {


            if (ModelState.IsValid)
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension), FileMode.Create)) 
                    { 
                      file.CopyTo(fileStreams);
                    }

                    obj.Product.ImageURL = @"\images\products\" + fileName + extension;
                }
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
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

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
         var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }


        #endregion
    }
}


