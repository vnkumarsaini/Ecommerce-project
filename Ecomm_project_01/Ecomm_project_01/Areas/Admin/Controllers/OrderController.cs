using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models.ViewModels;
using Ecomm_project_01.Models;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Ecomm_project_01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly iUnitOfWork _unitOfWork;
        public OrderController(iUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //ViewBag.Id = id;
            //ViewBag.items = new List<SelectListItem> { 
            // new SelectListItem{Value=SD.OrderStatusApproved,Text=SD.OrderStatusApproved},
            // new SelectListItem{Value=SD.OrderStatusPending,Text=SD.OrderStatusPending},
            // new SelectListItem{Value=SD.PaymentStatusRejected,Text=SD.PaymentStatusRejected},
            // new SelectListItem{Value=SD.OrderStatusRefunded,Text=SD.OrderStatusRefunded},
            // new SelectListItem{Value=SD.OrderStatusCancelled,Text=SD.OrderStatusCancelled},
            //};
            return View();
        }
        public IActionResult Details(int id)
        {
            var userDetails = _unitOfWork.OrderHeader
                .FirstOrDeafault(
                    filter: o => o.Id == id,
                    includeProperties: "OrderDetails.Product"
                );

            if (userDetails == null)
            {
                return NotFound();
            }

            return View(userDetails);
        }
        #region MyRegion            
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser") });
        }
        #endregion
            
    }
}
