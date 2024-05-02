using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Display(Name = "Street Address")]
        public String StreetAddress { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        [Display(Name = "Postal Code")]
        public String PostalCode { get; set; }
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }
        [Display(Name = "Is Authorised Comapy")]
        public bool IsAuthorisedCompany { get; set; }
        

    }
}
