using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.DataAccess.Repository.iRepository
{
    public interface iUnitOfWork
    {
        iCategoryRepository Category { get; }
        iCoverTypeRepository CoverType { get; }
        IproductRepository Product { get; }
        ISPCall SPCall { get; }
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        
        void save();
    }
}
