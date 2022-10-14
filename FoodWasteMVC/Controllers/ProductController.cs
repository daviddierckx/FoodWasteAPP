using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using FoodWaste.Infrastructure.Repository;
using FoodWasteMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodWasteMVC.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductRepo _productRepo ;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productRepo.GetAll();
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            //Lazy loading is a way to conserve data (Include => like a join in sql)
            //Product product = _context.Products.Include(a => a.Address).FirstOrDefault(c => c.Id == id);

            Product product = await _productRepo.GetByIdAsync(id);
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _productRepo.Add(product);
            return RedirectToAction("Index");
        }
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
        public async Task<IActionResult> Edit(int id, EditProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit product");
                    return View("Edit", productVM);
            }
            var userProduct = await _productRepo.GetByIdAsyncNoTracking(id);
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
    }
}
