using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using TBay.Models;

namespace TBay.ViewModels
{
    public class ItemViewModel
    {

          public int ItemsID { get; set; }
        public int Designerid { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        
       
        public string Category { get; set; }
        public string Size { get; set; }

        public int Price { get; set; }
       
        
        [ForeignKey("Designerid")]
        public Designer Designer  {get; set;}
        public ICollection<Store> Store { get; set; }
        public ICollection<Designer> Designers {get; set;}      
        public IFormFile Picture { get; set; }
    }
}
