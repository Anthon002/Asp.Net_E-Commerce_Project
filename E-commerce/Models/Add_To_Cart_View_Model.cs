using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class Add_To_Cart_View_Model
    {
        public int Id { get; set; }
        public string Product_Name { get; set; }
        public double Product_Price { get; set; }
        public DateTime Date_added { get; set; }
        public string Image_Path { get; set; } 
        public string UserID {get; set;}
        public int Number_of_items { get; set; }= 1;
        public double Total_Price {get; set; } = 0;
    }
}