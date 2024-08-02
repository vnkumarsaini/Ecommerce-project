using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_project_01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDateController : Controller
    {
        private readonly iUnitOfWork _iUnitOfWork;
        public OrderDateController(iUnitOfWork iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        public IActionResult Index(DateTime? fromDate, DateTime? toDate, string orderStatus)
        {
            var query = _iUnitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");

            // Apply filtering based on selected dates
            if (fromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= fromDate);
            }

            if (toDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= toDate);
            }

            // Apply filtering based on order status if provided
            if (!string.IsNullOrEmpty(orderStatus))
            {
                query = query.Where(o => o.OrderStatus == orderStatus);
            }

            var OrderList = query.ToList();
            return View(OrderList);
        }
    }          
}
    
