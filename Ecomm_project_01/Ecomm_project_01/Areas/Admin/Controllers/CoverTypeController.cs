using Dapper;
using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ecomm_project_01.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class CoverTypeController : Controller
    {
        private readonly iUnitOfWork _iUnitOfWork;
        public CoverTypeController(iUnitOfWork iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            //var CoverType_list = _iUnitOfWork.CoverType.GetAll();
            // return Json(new { data = CoverType_list });
            return Json(new { data = _iUnitOfWork.SPCall.List<CoverType>(SD.Proc_GetCoverTypes)});
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var covertypeindb = _iUnitOfWork.CoverType.Get(id);
            if (covertypeindb == null)
                return Json(new { Success = false, Message = "something went Wrong while deleting" });
            _iUnitOfWork.CoverType.Remove(covertypeindb);
            _iUnitOfWork.save();
            return Json(new { success = true, Message = "Data Deleted successfully" });
        }

        #endregion
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null) return View(coverType); //create

            //edit
           // coverType = _iUnitOfWork.CoverType.Get(id.GetValueOrDefault());
           DynamicParameters param= new DynamicParameters();
            param.Add("id", id.GetValueOrDefault());
            coverType = _iUnitOfWork.SPCall.OneRecord<CoverType>(SD.Proc_GetCoverType,param);
            if (coverType == null) return NotFound();
            return View(coverType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null) return NotFound();
            if (!ModelState.IsValid) return View(coverType);
            DynamicParameters param = new DynamicParameters();
            param.Add("Name", coverType.Name);
            if(coverType.Id == 0)
                _iUnitOfWork.SPCall.Execute(SD.Proc_CreateCoverType,param);
            else
            {
                param.Add("Id", coverType.Id);
                _iUnitOfWork.SPCall.Execute(SD.Proc_UpdateCoverType,param);
            }

            //if (coverType.Id == 0)
            //    _iUnitOfWork.CoverType.Add(coverType);
            //else
            //    _iUnitOfWork.CoverType.Update(coverType);
            //_iUnitOfWork.save();
            return RedirectToAction("Index");

        }


    }
}
