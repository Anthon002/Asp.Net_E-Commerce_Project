using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class Products_ViewModel
    {
        public int Id { get; set; }
        public string Product_Name { get; set; }
        public double Product_Price { get; set; }
        public DateTime Date_added { get; set; }
        [Required]
        [Display(Name = "Add The Image of the Product")]
        [NotMapped]
        public IFormFile? Product_Image { get; set; }
        public string Image_Path { get; set; }
    }
}