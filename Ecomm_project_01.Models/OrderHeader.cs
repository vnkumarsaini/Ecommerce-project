using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime ShippingDate { get; set; }
        [Required]
        public double OrderTotal { get; set; }
        public string TreckingNumber { get; set; }
        public string Carrier { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string TranscationId { get; set; }
        [Required]
        public String Name { get; set; }
        public string SelectedAddress { get; set; }
        [Required]
        [Display(Name = "Street Address")]
        public String StreetAddress { get; set; }
        [Required]
        public String City { get; set; }
        [Required]
        public String State { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public String PostalCode { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }

    }
}
