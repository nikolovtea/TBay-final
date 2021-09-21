using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
  
namespace TBay.Models
{
    public class AppUser: IdentityUser
    {
        [Display(Name = "Улога")]
        public string Role { get; set; }        
        
        [Display(Name = "Корисник")]
        [ForeignKey("UserId")]
        public User User { get; set; }   
        #nullable enable        
        public string? UserId { get; set; }
       
    }
}