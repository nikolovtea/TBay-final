using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBay.Models
{
    public class Designer
    {

        [Display(Name = "Слика")]
        public string Picture { get; set; }
        [Required]
        [Key]
        public int DesignerID { get; set; }
        [Display(Name = "Име")]
        public string FirstName { get; set; }
        [Display(Name = "Презиме")]
        public string LastName { get; set; }
        [Display(Name = "Дата на раѓање")]
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }
       
        
        [Display(Name = "Биографија")]
        public string Biography { get; set; }
       
        [Display(Name = "Парче облека")]
        public string Items {get; set;}
        
        [Display(Name = "Име и презиме")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
       
       public ICollection<Item> Item {get; set;}
    }
}