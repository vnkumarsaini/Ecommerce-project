using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using Ecomm_project_01.Models.ViewModels;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Stripe;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Ecomm_project_01.DataAccess.Repository;
using Address = Ecomm_project_01.Models.Address;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecomm_project_01.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly iUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private static bool IsEmailConfirm = false;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(iUnitOfWork unitOfWork, IEmailSender emailSender,UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claims == null)
            {
                ShoppingCartVM = new ShoppingCartVM()
                {
                    ListCart= new List<ShoppingCart>()
                };
                return View(ShoppingCartVM);
            }

            // ....
            ShoppingCartVM = new ShoppingCartVM()
            {
               ListCart = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claims.Value,
               includeProperties: "Product"),OrderHeader= new OrderHeader()
            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.
                FirstOrDeafault(au=>au.Id==claims.Value);

            foreach (var List in ShoppingCartVM.ListCart)
            {
                List.Price = SD.GetPriceBasedOnQuantity(List.Count,List.Product.Price,
                    List.Product.Price50,List.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (List.Count * List.Price);
                if(List.Product.Description.Length > 100)
                {
                    List.Product.Description = List.Product.Description.Substring(0,99) + "....";
                }
            }
            if (!IsEmailConfirm)
            {
                ViewBag.EmailMessage = "Email has been Sent kindly verify your email !!";
                ViewBag.EmailCSS = "text-success";
                IsEmailConfirm = false;
            }
            else
            {
                ViewBag.EmailMessage = "Email Must Be Confirmed for Authorise Customer";
                ViewBag.EmailCSS = "text-danger";
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {

            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = _unitOfWork.ApplicationUser.FirstOrDeafault(au => au.Id == claims.Value);
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Email is Empty");
            }
            else
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code},
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Plus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDeafault(sc=> sc.Id == id);
            cart.Count += 1;
            _unitOfWork.save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int id) 
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDeafault(sc => sc.Id == id);
           if(cart.Count ==1)
                cart.Count = 1;
           else
            cart.Count -= 1;
            _unitOfWork.save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id) 
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDeafault(sc => sc.Id == id);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.save();

            // session
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
           
            var count = _unitOfWork.ShoppingCart.GetAll( sc=>sc.ApplicationUserId == claims.Value).ToList().Count;
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            return RedirectToAction("Index");
        }

        //public IActionResult Summary() 
        //{
        //    var claimsIdentity = (ClaimsIdentity)(User.Identity);
        //    var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //    ShoppingCartVM = new ShoppingCartVM()
        //    {
        //        ListCart = _unitOfWork.ShoppingCart.GetAll(lc => lc.ApplicationUserId == claims.Value, includeProperties: "Product"),
        //        OrderHeader = new OrderHeader()
        //    };
        //    ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDeafault(au=>au.Id == claims.Value);

        //    foreach (var list in ShoppingCartVM.ListCart)
        //    {
        //        list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
        //        ShoppingCartVM.OrderHeader.OrderTotal = list.Price * list.Count;
        //        if (list.Product.Description.Length > 100)
        //        {
        //            list.Product.Description = list.Product.Description.Substring(0, 99) + "....";
        //        }
        //    }
        //    ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
        //    ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
        //    ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
        //    ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
        //    ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
        //    ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
        //    return View(ShoppingCartVM);
        //}
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
             var ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(lc => lc.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader(),
                //UserAddresses = _unitOfWork.Address.GetAll(a => a.ApplicationUserId == userId).ToList()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDeafault(au => au.Id == claims.Value);

            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal = list.Price * list.Count;
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "....";
                }
            }
            var addresses = _unitOfWork.OrderHeader.GetAll(o => o.ApplicationUserId == claims.Value)
                 .Select(o => new
                 {
                     o.Name,
                     o.PhoneNumber,
                     o.StreetAddress,
                     o.City,
                     o.State,
                     o.PostalCode,
                     FullAddress = $"{o.Name}, {o.PhoneNumber}, {o.StreetAddress}, {o.City}, {o.State}, {o.PostalCode}"
                 })
                 .Distinct()
                 .ToList();

            ViewBag.UserAddresses = new SelectList(addresses, "FullAddress", "FullAddress");

            return View(ShoppingCartVM);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Summary")]

        public async Task<IActionResult> SummaryPostAsync(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDeafault(au => au.Id == claims.Value);
            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(sc=>sc.ApplicationUserId == claims.Value, includeProperties:"Product");

            ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId= claims.Value;
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.save();

            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId= list.ProductId,
                    OrderHeaderId=ShoppingCartVM.OrderHeader.Id,
                    Price=list.Price,
                    Count=list.Count,
                };

                ShoppingCartVM.OrderHeader.OrderTotal = (list.Price * list.Count);
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.save();
            }
            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.save();
            //session
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, 0);
            #region stripe
            if (stripeToken == null)
            {
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusShipped;
            }
            else
            {
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "USD",
                    Description = "Order Id:" + ShoppingCartVM.OrderHeader.Id.ToString(),
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.BalanceTransactionId == null)
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    ShoppingCartVM.OrderHeader.TranscationId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                }
                _unitOfWork.save();
                //phoneno.
                if (charge.BalanceTransactionId != null)
                {
                    string SID = "AC968602e86ba5355e96985e3195b55bc3";
                    string AuthToken = "2973933c23d6612f340abf13ae4798df";
                    string fromPhoneNumber = "12569643444";

                    TwilioClient.Init(SID, AuthToken);
                    var message = MessageResource.Create(
                        body: "Product Ordered Succesfully",
                        from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(ShoppingCartVM.OrderHeader.PhoneNumber)
                        );
                    Console.WriteLine(message.ErrorCode);

                    //email
                    try
                    {
                        string subject = "Order Confirmation - Your Order with Us";
                        string amessage = $"Hello {ShoppingCartVM.OrderHeader.ApplicationUser.Name},<br/>Thank you for your order. Your order ID is {ShoppingCartVM.OrderHeader.Id}.";
                        await _emailSender.SendEmailAsync(ShoppingCartVM.OrderHeader.ApplicationUser.Email, subject, amessage);
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }
                }
                return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });              
            }
            #endregion
            return RedirectToAction("OrderConfirmation", "Cart", new {id=ShoppingCartVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}
