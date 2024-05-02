using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.Utility
{
    public interface ISMSSender
    {

        Task SendSMSAsync(string toPhone, string message);
    }
}
