using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Ecomm_project_01.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CategoryController : Controller
    {
          private readonly iUnitOfWork _UnitOfWork;
        public CategoryController(iUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id) 
        { 
        Category category= new Category();
            if (id == null) return View(category); //create

            //edit
            category = _UnitOfWork.Category.Get(id.GetValueOrDefault());
            if(category == null) return NotFound();
                return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Category category)
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View(category);
            if (category.Id == 0)
                _UnitOfWork.Category.Add(category);
            else
                _UnitOfWork.Category.Update(category);
            _UnitOfWork.save();
            return RedirectToAction("Index");

        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var Categorylist = _UnitOfWork.Category.GetAll();
            return Json(new { data = Categorylist });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryindb = _UnitOfWork.Category.Get(id);
            if (categoryindb == null)
                return Json(new { Success = false, Message = "something went Wrong while deleting" });
            _UnitOfWork.Category.Remove(categoryindb);
            _UnitOfWork.save();
            return Json(new { success = true, Message = "Data Deleted successfully" });
        }


        #endregion
    }
}
