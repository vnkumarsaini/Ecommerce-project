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
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
