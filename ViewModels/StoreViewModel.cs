using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using TBay.Models;

namespace TBay.ViewModels
{
    public class StoreViewModel
    {

        public int StoreID { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        
        public string Link { get; set; }

        public string Rating { get; set; }
        [ForeignKey("ItemId")]
        public Item Item { get; set; } 
        public IFormFile Picture { get; set; }
    }
}
