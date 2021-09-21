using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TBay.Models
{
    public class User
    {
        [Key]
        public string UserID {get; set;}
        
        [Display(Name = "Корисничко име")]
        public string Name { get; set; }                
        
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        [Display(Name = "Телефонски Број")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Адреса на живеење")]
        public string Address { get; set; }
        
        [Display(Name = "Град")]
        public string City { get; set; }      
       
        [Display(Name = "Име и презиме")]
        public string FullName {get; set;}
    }
}