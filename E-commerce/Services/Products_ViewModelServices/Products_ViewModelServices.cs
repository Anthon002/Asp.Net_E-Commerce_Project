using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Views.Product;
using E_commerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace E_commerce.Services.Products_ViewModelServices
{
    public class Product_ViewModelServices : IProducts_ViewModelServices
    {
        private readonly AppDbContext _dbContext;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<E_commerceUser> _usermanager;
        public ProductDbViewModel newProduct = new ProductDbViewModel();
        public Product_ViewModelServices(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, UserManager<E_commerceUser> usermanager)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            this._usermanager = usermanager;
        }
        public List<ProductDbViewModel> ViewProducts()
        {
            var products = _dbContext.ProductDb_Ecommerce.ToList();
           // products.ForEach(p => p.Image_Path = Path.Combine("Images/ProductImages", p.Image_Path));
           var view_product = new List<ProductDbViewModel>();
           foreach(var i in products)
           {
            var product = new ProductDbViewModel();
            i.Image_Path = Path.Combine("Images/ProductImages", i.Image_Path);
            product.Id = i.Id;
            product.Product_Name = i.Product_Name;
            product.Product_Price = i.Product_Price;
            product.Image_Path = i.Image_Path;
            view_product.Add(product);
           }
           return (view_product);
        }

        public Products_ViewModel AddProduct(Products_ViewModel newProduct)
        {
            if (newProduct.Product_Image != null)
            {
                /**
                 * first get the serverfolder from iwebhosting.webrouting
                 * then get the imagefile name and add a Guid.Newguid() in front for uniqueness
                 * then combine the serverfolder and the imgfile name to get the complete path to img
                 * then if (Directory.Exists(serverfolder) == false){CreateDirectory(serverfolder)}
                 * then newProduct.Product_Image.CopyTo(FileStream(serverfolder,Filemode.create())) to save the actual img to file
                 * then newProduct.Image_Path = imagefile name to save the image file name (not the path) the the Image_Path
                 * you can then retrieve the img from database in the view action or view page by combining serverfolder and imagename then returning it
                 **/
                 var serverfolder = Path.Combine(_webHostEnvironment.WebRootPath,"Images/ProductImages");
                 string imgfilename = Guid.NewGuid().ToString().Replace("-","") + "_" + newProduct.Product_Image.FileName;
                 string completePath = Path.Combine(serverfolder, imgfilename);
                 if (!Directory.Exists(serverfolder))
                 {
                    Directory.CreateDirectory(serverfolder);
                 }
                 newProduct.Product_Image.CopyTo(new FileStream(completePath,FileMode.Create));
                 newProduct.Image_Path = imgfilename;
            }
            var productObj = new Products_ViewModel
            {
                Id = newProduct.Id,
                Product_Name = newProduct.Product_Name,
                Product_Price = newProduct.Product_Price,
                Date_added = DateTime.Now,
                Product_Image = newProduct.Product_Image,
                Image_Path = newProduct.Image_Path,
            };
            _dbContext.ProductDb_Ecommerce.Add(productObj);
            _dbContext.SaveChanges();
            return (productObj);
        }

        public ProductDbViewModel GetProduct(Products_ViewModel theProduct)
        {
            // var theProductVm = new ProductDbViewModel();
            //     theProductVm.Product_Name = theProduct.Product_Name;
            //     theProductVm.Product_Price = theProduct.Product_Price;
            //     theProductVm.Image_Path = theProduct.Image_Path;
            
            var theProductVm = new ProductDbViewModel();
            try{
            theProduct.Image_Path = Path.Combine("Images/ProductImages",theProduct.Image_Path);
            theProductVm.Id = theProduct.Id;
            theProductVm.Product_Name = theProduct.Product_Name;
            theProductVm.Product_Price =theProduct.Product_Price;
            theProductVm.Image_Path = theProduct.Image_Path;

            return (theProductVm);
            }
            catch(Exception error)
            {
                
            };
            return (theProductVm);
            
        }

        public List<ProductDbViewModel> Add_Cart(int id, string userId)
        {
            var Products_in_cart = new List<ProductDbViewModel>();
            var theProduct = _dbContext.ProductDb_Ecommerce.FirstOrDefault(x => x.Id == id);
            if (theProduct != null)
            { 
                _dbContext.CartDb_Ecommerce.Add(
                    new Add_To_Cart_View_Model{
                        Product_Name = theProduct.Product_Name,
                        Product_Price = theProduct.Product_Price,
                        Date_added = DateTime.Now,
                        Image_Path = theProduct.Image_Path,
                        UserID = userId,
                        Total_Price = theProduct.Product_Price,
                    }
                );
               
            }
            var Cart_Db = _dbContext.CartDb_Ecommerce.ToList();
            foreach (var i in Cart_Db)
            {
                //i.Total_Price += theProduct.Product_Price;
                Products_in_cart.Add(
                    new ProductDbViewModel{
                        Id = i.Id,
                        Product_Name = i.Product_Name,
                        Product_Price = i.Product_Price,
                        Image_Path = i.Image_Path,
                        Number_of_items = i.Number_of_items,
                        UserId = userId,
                    }
                );
            }

            _dbContext.SaveChanges();
            return(Products_in_cart);
        }

       public ProductDbViewModel Update_Item_Cart_add(int id, string userId)
        {
            //This section of the function adds an extra quantity to the selected item
            var theCartProduct = _dbContext.CartDb_Ecommerce.FirstOrDefault(x=> x.Id == id);
            var theCartList = _dbContext.CartDb_Ecommerce.ToList();
            theCartProduct.Number_of_items += 1;

            //This section of the function calculates the total price of all the items in the cart(Item_price * quantity_of_each_item* number of items in cart)
            double totalPrice = 0;
            // foreach(var item in theCartList)
            // {
            //     if( item.UserID == userId )
            //     {
            //     // double pricePerItem = item.Product_Price * item.Number_of_items;//This is the price per item instance e.g how much is (pizza x 5)
            //     // totalPrice += pricePerItem;// Gets the price of that instance + all other instances to get total price of all items * quantity of items in the cart
            //     item.Total_Price += theCartProduct.Product_Price;
            //     }

            // }
            // foreach(var item in theCartList)
            // {
            //     item.Total_Price += totalPrice;
            // }
          //  theCartProduct.Total_Price += totalPrice;

            theCartProduct.Total_Price += theCartProduct.Product_Price;
            _dbContext.SaveChanges();

            var theProductVm = new ProductDbViewModel();
            if (theCartProduct != null)
            {
                theProductVm = new ProductDbViewModel(){
                    Id = id,
                    Number_of_items = theCartProduct.Number_of_items,
                    Total_Price = theCartProduct.Total_Price,
                };
            }
            return (theProductVm);
        }
        
        public ProductDbViewModel Update_Item_Cart_remove(int id, string userId)
        {
            // First of all remove a quantity (total quantity - 1) from the cart
            var theCartProduct = _dbContext.CartDb_Ecommerce.FirstOrDefault(x => x.Id == id);
            var theCartList = _dbContext.CartDb_Ecommerce.ToList();
            var theProductVm = new ProductDbViewModel();

            if(theCartProduct != null)
            {
                theCartProduct.Number_of_items -= 1;
                theCartProduct.Total_Price -= theCartProduct.Product_Price;
                if (theCartProduct.Number_of_items <= 0 || theCartProduct.Total_Price <=0)
                {
                    _dbContext.CartDb_Ecommerce.Remove(theCartProduct);
                }
                
            }
            //reduct the total cost of items in cart
            // foreach(var item in theCartList)
            // {
            //     if (item.UserID == userId)
            //     {
            //         item.Total_Price -= theCartProduct.Product_Price;
            //     }
            // }
            _dbContext.SaveChanges();
            
            return(theProductVm);

        }

    }
}