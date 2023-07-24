using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_commerce.Models;
using E_commerce.Views.Product;

namespace E_commerce.Services.Products_ViewModelServices
{
    public interface IProducts_ViewModelServices
    {
       List<ProductDbViewModel> ViewProducts();
       Products_ViewModel AddProduct(Products_ViewModel newProduct);
       ProductDbViewModel GetProduct(Products_ViewModel theProduct);
       List<ProductDbViewModel> Add_Cart(int id, string userId);
       ProductDbViewModel Update_Item_Cart_add(int id, string userId);
       ProductDbViewModel Update_Item_Cart_remove(int id, string userId);
    }
}