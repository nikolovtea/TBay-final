using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace TBay.Models
{
    public class Item
    {
        
        [Display(Name = "Слика")]
        public string Picture { get; set; }
        [Required]
        [Key]
        public int ItemsID { get; set; }
        public int Designerid { get; set; }
        [Display(Name = "Име")]
        public string Name { get; set; }
        
        [Display(Name = "Категорија")]
        public string Category { get; set; }
        [Display(Name = "Величина")]
        public string Size { get; set; }
       
        [Display(Name = "Цена")]
        public int Price { get; set; }

        [ForeignKey("Designerid")]
        [Display(Name = "Дизајнер")]
        public Designer Designer {get; set;}


        public ICollection<Store> Store { get; set; }
        public ICollection<User> Users { get; set; }
       
    }
}