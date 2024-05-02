using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.Utility
{
    public static class SD
    {
        public const String Proc_GetCoverTypes = "GetCoverTypes";
        public const String Proc_GetCoverType = "GetCoverType";
        public const String Proc_CreateCoverType = "CreateCoverType";
        public const String Proc_UpdateCoverType = "UpdateCoverType";
        public const String Proc_DeleteCoverType = "DeleteCoverType";



        public const String createProduct = "createProduct";
        public const String updateProduct = "updateProduct";
        public const String getProducts = "getProducts";
        public const String getProduct = "getProduct";
        public const String deleteProduct = "deleteProduct";

        //Roles
        public const String Role_Admin = "Admin";
        public const String Role_Employee = "Employee User";
        public const String Role_Company = "Company User";
        public const String Role_Individual = "Individual User";

        // Session 
        public const String Ss_CartSessionCount = "Cart Count Session";

       
        public static double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50) return price;
            else if(quantity < 100) return price50;
            else return price100;
        }

        //Order status 
        public const String OrderStatusPending = "Pending";
        public const String OrderStatusApproved = "Approved";
        public const String OrderStatusInProgress = "Processing";
        public const String OrderStatusShipped = "Shipped";
        public const String OrderStatusCancelled = "Cancelled";
        public const String OrderStatusRefunded = "Refunded";

        // Payment Status
        public const String PaymentStatusPending = "Pending";
        public const String PaymentStatusApproved = "Approved";
        public const String PaymentStatusDelayPayment = "Delay Payment";
        public const String PaymentStatusRejected = "Rejected";
    }
}
