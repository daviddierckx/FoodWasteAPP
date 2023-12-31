﻿using FoodWaste.Application.Interfaces;
using FoodWaste.Application.ViewModels;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using FoodWaste.Infrastructure.Repository;
using FoodWasteMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodWasteMVC.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductRepo _productRepo ;
        private readonly IPhotoService _photoService;

        public ProductController(IProductRepo productRepo, IPhotoService photoService)
        {
            _productRepo = productRepo;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products =  _productRepo.GetAll();
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewData["Products"] = _productRepo.GetAll(); 
            Product product = await _productRepo.GetByIdAsync(id);
            return View(product);
        }
        [Authorize(Roles = "kantineMedewerker")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Create(CreatProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(productVM.Foto);

                var product = new Product
                {
                    BeschrijvendeNaam = productVM.BeschrijvendeNaam,
                    Alcohol = productVM.Alcohol,
                    Foto = result.Url.ToString()
                };

                _productRepo.Add(product);
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(productVM);
        }
        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return View("Error");
            var productVM = new EditProductViewModel
            {
                BeschrijvendeNaam = product.BeschrijvendeNaam,
                Alcohol = product.Alcohol,
                Foto = product.Foto,
            };
            return View(productVM);
        }
        [HttpPost]
        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Edit(int id, EditProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit product");
                    return View("Edit", productVM);
            }
            var userProduct =  _productRepo.GetByIdAsyncNoTracking(id);
            if(userProduct != null)
            {
                var product = new Product
                {
                    Id = id,
                    BeschrijvendeNaam = productVM.BeschrijvendeNaam,
                    Alcohol = productVM.Alcohol,
                    Foto = productVM.Foto
                };
                _productRepo.Update(product);
                return RedirectToAction("Index");

            }
            else
            {
                return View(productVM);
            }
        }

        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Delete(int id)
        {
            var productDetails = _productRepo.GetByIdAsyncNoTracking(id);
            if (productDetails == null) return View("Error");
            return View(productDetails);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeletePakket(int id)
        {
            var productDetails =  _productRepo.GetByIdAsyncNoTracking(id);
            if (productDetails == null) return View("Error");

            _productRepo.Delete(productDetails);
            return RedirectToAction("Index");
        }
    }
 }

