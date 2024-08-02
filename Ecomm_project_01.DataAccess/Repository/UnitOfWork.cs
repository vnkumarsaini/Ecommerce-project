using Ecomm_project_01.DataAccess.Data;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.DataAccess.Repository
{
    public class UnitOfWork : iUnitOfWork
    {
        private readonly ApplicationDbContext _Context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _Context = context;
            Category = new CategoryRepository(_Context);
            CoverType= new CoverTypeRepository(_Context);
            Product= new ProductRepository(_Context);
            Company= new CompanyRepository(_Context);
            ApplicationUser= new ApplicationUserRepository(_Context);
            ShoppingCart= new ShoppingCartRepository(_Context);
            OrderHeader= new OrderHeaderRepository(_Context);
            OrderDetails= new OrderDetailsRepository(_Context);
            Address= new AddressRepository(_Context);

            SPCall = new SPCall (_Context);
        }

        public iCategoryRepository Category { private set; get; }
                 
        public iCoverTypeRepository CoverType { private set; get; }
        public ISPCall SPCall { private set; get; }

        public IproductRepository Product { private set; get; }
        public ICompanyRepository Company { private set; get; }
        public IApplicationUserRepository ApplicationUser { private set; get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailsRepository OrderDetails { get; }
        public IAddressRepository Address { get; }

        public void save()
        {
           _Context.SaveChanges();
        }
    }
}
