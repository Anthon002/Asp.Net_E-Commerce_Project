using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using E_commerce.Areas.Identity.Data;
using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Views.Product;
using E_commerce.Services.Products_ViewModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProducts_ViewModelServices _IproductService;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<E_commerceUser> _userManager;

        public ProductController(ILogger<ProductController> logger, IProducts_ViewModelServices IproductService, AppDbContext dbContext, UserManager<E_commerceUser> usermanager)
        {
            _logger = logger;
            _IproductService = IproductService;
            _dbContext = dbContext;
            this._userManager = usermanager;
        }

        public ActionResult<List<Products_ViewModel>> ViewProducts()
        {
            var username =  _userManager.Users.FirstOrDefault(x => x.Id == _userManager.GetUserId(User));
            if(User.Identity.IsAuthenticated)
            {
                ViewData["FirstName"] = username.FirstName;
            }
            else
            {
                ViewData["FristName"] = "Anonymous";
            }
            return View(_IproductService.ViewProducts());
            
        }
        public ActionResult<Products_ViewModel> AddProduct()
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
        [Authorize]
        [HttpPost("addProduct")]
        public ActionResult<Products_ViewModel> AddProduct(Products_ViewModel newProduct)
        {
            _IproductService.AddProduct(newProduct);
            ViewData["userId"] = _userManager.GetUserId(this.User);
            return RedirectToAction(nameof(ViewProducts));
        }

        [Authorize]
        [HttpGet]
        public ActionResult<ProductDbViewModel> GetProduct(int id)
        {
            //var theProduct = _IproductService.GetProduct(id);
            // var theProductVm = new ProductDbViewModel();
            // theProductVm.Product_Name = theProduct.Product_Name;
            // theProductVm.Product_Price = theProduct.Product_Price;
            // theProductVm.Image_Path = theProduct.Image_Path;
            var theProduct = _dbContext.ProductDb_Ecommerce.FirstOrDefault(x => x.Id == id);
            var theProductVm = _IproductService.GetProduct(theProduct);

            return View(theProductVm);
        }

        [Authorize]
        public ActionResult<List<ProductDbViewModel>> Add_Cart(int id)
        {
            var userId = _userManager.GetUserId(User);
            var theProduct = _IproductService.Add_Cart(id, userId);

            return RedirectToAction(nameof(ViewCart));

        }



        [Authorize]
        [HttpPost]
        public ActionResult Update_Item_Cart_add(int id)
        {
            string userId = _userManager.GetUserId(User);   
          //  var theUpdatedProduct = _IproductService.Update_Item_Cart_add(id, userId);
          _IproductService.Update_Item_Cart_add(id, userId);
            return RedirectToAction(nameof(ViewCart));
        }
        [Authorize]
        [HttpPost]
        public ActionResult Update_Item_Cart_remove(int id)
        {
            string userId = _userManager.GetUserId(User);
          //  var theUpdatedProduct = _IproductService.Update_Item_Cart_remove(id, userId);
          _IproductService.Update_Item_Cart_remove(id, userId);
            return RedirectToAction(nameof(ViewCart));
        }

        public ActionResult<List<ProductDbViewModel>> ViewCart()
        {
            var userId = _userManager.GetUserId(User);
            var cartItemId = _dbContext.CartDb_Ecommerce.ToList();
            var theCartVm = new List<ProductDbViewModel>();
            foreach (var i in cartItemId)
            {
                if (i.UserID == userId)
                {
                    theCartVm.Add(
                        new ProductDbViewModel
                        {
                            Id = i.Id,
                            Product_Name = i.Product_Name,
                            Product_Price = i.Product_Price,
                            Image_Path = i.Image_Path,
                            UserId = i.UserID,
                            Number_of_items = i.Number_of_items,
                            Total_Price = i.Total_Price,
                        }
                    );
                }
            }
            return View(theCartVm);
        }

        [Authorize]
        public ActionResult<Products_ViewModel> GetProduct(ProductDbViewModel theProductVm)
        {

            return View(theProductVm);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


    }
}