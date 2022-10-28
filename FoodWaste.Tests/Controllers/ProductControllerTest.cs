using FakeItEasy;
using FluentAssertions;
using FoodWaste.Application.Interfaces;
using FoodWaste.Application.Services;
using FoodWaste.Application.ViewModels;
using FoodWaste.Domain;
using FoodWasteMVC.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoodWaste.Tests.Controllers
{
    public class ProductControllerTest
    {
        private ProductController _productController;
        private IProductRepo _productRepo;
        private IPhotoService _photoService;
        //private IHttpContextAccessor _httpContextAccessor;

        public ProductControllerTest()
        {
            //Dependencies
            _productRepo = A.Fake<IProductRepo>();
            _photoService = A.Fake<IPhotoService>();
            //_httpContextAccessor = A.Fake<HttpContextAccessor>();
            //SUT
            _productController = new ProductController(_productRepo, _photoService /*_httpContextAccessor*/);
        }

        [Fact]
        public void ProductController_Index_ReturnsSucces()
        {
            //Arrange - What do i need to bring in
            var products = A.Fake<IEnumerable<Product>>();
            A.CallTo(() => _productRepo.GetAll()).Returns(products);

            //Act
            var result = _productController.Index();

            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ProductController_Detail_ReturnsSucces()
        {
            //Arrange
            var id = 1;
            var product = A.Fake<Product>();
            A.CallTo(()=>_productRepo.GetByIdAsync(id)).Returns(product);
            //Act

            var result = _productController.Detail(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

       
    }
}
