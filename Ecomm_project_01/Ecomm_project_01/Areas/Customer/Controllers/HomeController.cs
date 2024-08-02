using Ecomm_project_01.DataAccess.Data;
using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using Ecomm_project_01.Models.ViewModels;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecomm_project_01.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly iUnitOfWork _UnitOfWork;

        public HomeController(ILogger<HomeController> logger,iUnitOfWork UnitOfWork)
        {
            _logger = logger;
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                var count = _UnitOfWork.ShoppingCart.GetAll(sc => 
                sc.ApplicationUserId == claims.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }

            var productlist = _UnitOfWork.Product.GetAll(includeProperties: "CoverType,Category");
            var productsWithCartCount = productlist.Select(product => {
                var cartCount = _UnitOfWork.OrderDetails
                    .GetAll(od => od.ProductId == product.Id)
                    .Sum(od => od.Count);  // Summing up all quantities from OrderDetails

                product.SoldCount = cartCount; // Setting the SoldCount property to sum of quantities added to cart
                return product;
            }).OrderByDescending(p => p.SoldCount).ToList(); // Order by SoldCount in descending order

            return View(productsWithCartCount);
            //return View(productlist);

        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Details(int id) 
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                var count = _UnitOfWork.ShoppingCart.GetAll(sc =>
                sc.ApplicationUserId == claims.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            var productindb = _UnitOfWork.Product.FirstOrDeafault(p => p.Id == id,includeProperties: "Category,CoverType");
            if(productindb == null) return NotFound();
            var ShoppingCart = new ShoppingCart()
            {
                Product = productindb,
                ProductId = productindb.Id
            };
            return View(ShoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
                if(ModelState.IsValid) 
            {
                var claimidentity = (ClaimsIdentity)User.Identity;
                var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
                if(claims == null) return NotFound();
                shoppingCart.ApplicationUserId = claims.Value;
                var shoppingCartInDb = _UnitOfWork.ShoppingCart.FirstOrDeafault
                    (sc => sc.ApplicationUserId == claims.Value && sc.ProductId == shoppingCart.ProductId);
                if (shoppingCartInDb == null)
                    _UnitOfWork.ShoppingCart.Add(shoppingCart);
                else
                    shoppingCartInDb.Count += shoppingCart.Count;
                _UnitOfWork.save();
                return RedirectToAction("Index");
            }
            else
            {
                var productindb = _UnitOfWork.Product.FirstOrDeafault(p => p.Id == shoppingCart.Id);
                if (productindb == null) return NotFound();
                var ShoppingCart = new ShoppingCart()
                {
                    Product = productindb,
                    ProductId = productindb.Id
                };
                return View(ShoppingCart);
            }
                
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}