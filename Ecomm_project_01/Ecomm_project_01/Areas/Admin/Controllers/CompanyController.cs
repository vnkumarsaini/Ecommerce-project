using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ecomm_project_01.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly iUnitOfWork _unitOfWork;
        public CompanyController(iUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null) return View(company);
                company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if(company==null) return NotFound();
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if(company==null) return BadRequest();
            if(!ModelState.IsValid) return View(company);
            if (company.Id == 0)
                _unitOfWork.Company.Add(company);
            else
                _unitOfWork.Company.Update(company);
            _unitOfWork.save();
            return RedirectToAction("Index");
        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll() 
        {
            return Json(new {data = _unitOfWork.Company.GetAll()});
        }
        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var companyindb = _unitOfWork.Company.Get(id);
            if (companyindb == null)
            return Json(new {success=false, message="Something went Wrong"});
            else
               _unitOfWork.Company.Remove(companyindb);
            _unitOfWork.save();
            return Json(new { success = true, message = "data delete successfully" });
        }

        #endregion
    }
}
