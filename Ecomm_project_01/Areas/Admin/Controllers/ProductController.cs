using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using Ecomm_project_01.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Ecomm_project_01.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class ProductController : Controller
    {
        private readonly iUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public ProductController(iUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment= webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id) 
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text= cl.Name,
                    Value = cl.Id.ToString(),   
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(ct => new SelectListItem()
                {
                    Text = ct.Name,
                    Value = ct.Id.ToString(),
                })
            };
           if(id==null) return View(productVM);
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null) return BadRequest();
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var uploads = Path.Combine(webRootPath,@"Images\Product");
                    if(productVM.Product.Id != 0)
                    {
                        var imageExists = _unitOfWork.Product.Get(productVM.Product.Id).ImageUrl;
                        productVM.Product.ImageUrl = imageExists;
                    }
                    if(productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath,productVM.Product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath); 
                        }
                    }
                    using(var fileStream = new FileStream(Path.Combine(uploads,fileName+extension),
                        FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName + extension;
                }
                else
                {
                    if(productVM.Product.Id != 0)
                    {
                        var imageExists = _unitOfWork.Product.Get(productVM.Product.Id).ImageUrl;
                        productVM.Product.ImageUrl = imageExists;
                    }
                }
                if(productVM.Product.Id == 0)
                    _unitOfWork.Product.Add(productVM.Product);
                else
                    _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            else
            {
                var productin = productVM.Product.Id;
                productVM = new ProductVM()
                {
                    Product = new Product(),
                    CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString(),
                    }),
                    CoverTypeList = _unitOfWork.CoverType.GetAll().Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString(),
                    })
                };
                if(productin!= 0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productin);
                }
                return View(productVM);
            }
        }


        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
           return Json(new { data = _unitOfWork.Product.GetAll() });
           // return Json(new { data = _unitOfWork.SPCall.List<Product>(SD.getProducts) });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productindb = _unitOfWork.Product.Get(id);
            if (productindb == null)
                return Json(new { Success = false, Message = "something went Wrong while deleting" });
            var Imageurl = _unitOfWork.Product.Get(id).ImageUrl;
            var webRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, Imageurl.Trim('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.Product.Remove(productindb);
            _unitOfWork.save();
            return Json(new { success = true, Message = "Data Deleted successfully" });
        }
        #endregion
    }
}
