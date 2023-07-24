using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_commerce.Models;

namespace E_commerce.Views.Product
{
    public class ProductDbViewModel
    {
        public int Id {get; set;}
        public string? Product_Name{get; set;}
        public double Product_Price{get; set;}
        public string Image_Path{get; set;}
        public string UserId {get; set;}
        public int Number_of_items {get; set;} = 1;
        public double Total_Price {get; set;} = 0;
    }
}