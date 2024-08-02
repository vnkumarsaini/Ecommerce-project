using Ecomm_project_01.DataAccess.Data;
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
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly iUnitOfWork _UnitOfWork;
        public UserController(ApplicationDbContext context, iUnitOfWork unitOfWork)
        {
            _context = context;
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll() 
        {
            var UserList = _context.ApplicationUsers.ToList();
            var Roles = _context.Roles.ToList();
            var UserRole = _context.UserRoles.ToList();

            foreach (var user in UserList)
            {
                var Roleid = UserRole.FirstOrDefault(u=>u.UserId == user.Id).RoleId;
                user.Role = Roles.FirstOrDefault(r => r.Id == Roleid).Name;
                if(user.CompanyId == null) 
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
                if(user.CompanyId != null)
                {
                    user.Company = new Company()
                    {
                        Name = _UnitOfWork.Company.Get(Convert.ToInt32(user.CompanyId)).Name
                    };
                }
            }
            //remove Role_User
            var adminuser = UserList.FirstOrDefault(u=>u.Role==SD.Role_Admin);
            UserList.Remove(adminuser);
            return Json(new { data = UserList });
        }
        [HttpPost]
        public IActionResult LockUnLock([FromBody]string id)
        {
            bool islocked = false;
            var userindb = _context.ApplicationUsers.FirstOrDefault(u=>u.Id== id);
            if (userindb == null)
                return Json(new { success = false, message = "Something went wrong" });
            if(userindb != null && userindb.LockoutEnd > DateTime.Now)
            {
                userindb.LockoutEnd = DateTime.Now;
                islocked = false;
            }
            else
            {
                userindb.LockoutEnd = DateTime.Now.AddYears(100);
                islocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message= islocked == true? "User Successfully locked" : "USer is Unlocked"});

        }
        #endregion
    }
}
