using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type created successfully ";
                return RedirectToAction("Index");

            }

            return View(obj);
        }

        //GET
        public IActionResult Edit(int id)
        {
            var coverTypeFromDbFind = _unitOfWork.CoverType.GetFirstOrDefault(p => p.Id == id);
            /*var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id)*/
            ;

            if (coverTypeFromDbFind == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDbFind);
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type updated successfully";
                return RedirectToAction("Index");

            }

            return View(obj);
        }



        //GET
        public IActionResult Delete(int id)
        {
            var coverTypeFromDbFind = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            /*var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id)*/
            ;

            if (coverTypeFromDbFind == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDbFind);
        }



        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}


