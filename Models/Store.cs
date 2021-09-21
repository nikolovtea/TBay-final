using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBay.Models
{
    public class Store
    {

        [Display(Name = "Слика")]
        public string Picture { get; set; }
        [Required]
        [Key]
        public int StoreID { get; set; }
        [Display(Name = "Име")]
        public string Name { get; set; }
        public int ItemId { get; set; }
   
        public string Link { get; set; }
        [Display(Name = "Оцена")]
        public string Rating { get; set; }
        [ForeignKey("ItemId")]
        [Display(Name = "Парче облека")]
        public Item Item { get; set; }
        
    }
}