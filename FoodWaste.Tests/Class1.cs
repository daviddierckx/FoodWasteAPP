//using FakeItEasy;
//using FluentAssertions;
//using FoodWaste.Application;
//using FoodWaste.Application.Interfaces;
//using FoodWaste.Application.ViewModels;
//using FoodWaste.Domain;
//using FoodWaste.Domain.Enums;
//using FoodWaste.Infrastructure.Data;
//using FoodWaste.Tests.Repository;
//using FoodWasteMVC.Controllers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualBasic;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace FoodWaste.Tests.Controllers
//{
//    public class PakketControllerTest
//    {

//        private ApplicationDbContext _context;
//        private Mock<IPakketRepo> _pakketRepo;
//        private Mock<IHttpContextAccessor> _httpContextAccessor;
//        private Mock<IProductRepo> _productRepo;
//        private Mock<IUserRepo> _userRepo;
//        private Mock<IMedewerkerRepo> _medewerkerRepo;
//        private PakketController _pakketController;
//        private Mock<PakketRepoTests> _pakketRepoTests;

//        public PakketControllerTest()
//        {
//            _pakketRepo = new Mock<IPakketRepo>();
//            _httpContextAccessor = new Mock<IHttpContextAccessor>();
//            _productRepo = new Mock<IProductRepo>();
//            _userRepo = new Mock<IUserRepo>();
//            _medewerkerRepo = new Mock<IMedewerkerRepo>();
//            //SUT
//            _pakketController = new PakketController(_pakketRepo.Object, _productRepo.Object, _userRepo.Object, _httpContextAccessor.Object, _medewerkerRepo.Object, _context);

//        }
//        [Fact]
//        public async void PakketController_Index_US_01_ReturnsSucces()
//        {
//            //Arrange - What do i need to bring in

//            var pakkets = A.Fake<IEnumerable<Pakket>>();


//            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//                {
//                 new Claim(ClaimTypes.Name, "username"),
//                 new Claim(ClaimTypes.NameIdentifier, "userId"),
//                 new Claim("name", "John Doe"),
//                }, "mock"));
//            var controller = _pakketController;
//            controller.ControllerContext = new ControllerContext()
//            {
//                HttpContext = new DefaultHttpContext() { User = user }
//            };
//            var packetList = new List<Pakket>
//        {
//            new()
//            {
//                      Id = 1,
//                      BeschrijvendeNaam = "Voedsel Pakket ",
//                      SelectedProductId = "1",
//                      Stad = "Breda",
//                      Kantine = "LA",
//                      TijdOphalen = DateTime.Now,
//                      TijdTotOphalen = DateTime.Now,
//                      Meerderjarig = true,
//                      Prijs = 10,
//                      TypeMaaltijd = "Brood"
//            },
//            new()
//            {
//                      Id = 2,
//                      BeschrijvendeNaam = "Voedsel Pakket ",
//                      SelectedProductId = "1",
//                      Stad = "Breda",
//                      Kantine = "LD",
//                      TijdOphalen = DateTime.Now,
//                      TijdTotOphalen = DateTime.Now,
//                      Meerderjarig = false,
//                      Prijs = 15,
//                      TypeMaaltijd = "Warm"
//            }
//        };

//            _pakketRepo.Setup(pakketRepo => pakketRepo.GetAll("date", "kantine", SortOrder.Ascending, "")).Returns(packetList.ToList()z);

//            //Act
//            var result = await _pakketController.Index();

//            //Assert - Object check actions
//            result.Should().NotBeNull();
//        }

//        //    [Theory]
//        //    [InlineData(SortOrder.Descending,"LA")]
//        //    [InlineData(SortOrder.Ascending,"LX")]
//        //    [InlineData(SortOrder.Descending, "HL")]
//        //    [InlineData(SortOrder.Ascending, "LD")]
//        //    public async void PakketController_IndexSort_US_02_ReturnsSucces(SortOrder sortOrderDate,string sortOrderKantine)
//        //    {
//        //        //Arrange - What do i need to bring in
//        //        _context = await _pakketRepoTests.GetDbContext();

//        //        var pakkets = A.Fake<IEnumerable<Pakket>>();

//        //        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//        //            {
//        //             new Claim(ClaimTypes.Name, "username"),
//        //             new Claim(ClaimTypes.NameIdentifier, "userId"),
//        //             new Claim("name", "John Doe"),
//        //            }, "mock"));
//        //        //user.IsInRole("student");
//        //        var controller = _pakketController;
//        //        controller.ControllerContext = new ControllerContext()
//        //        {
//        //            HttpContext = new DefaultHttpContext() { User = user }
//        //        };



//        //        A.CallTo(() => _pakketRepo.GetAll("date", "kantine", sortOrderDate, sortOrderKantine)).Returns(pakkets);

//        //        //Act
//        //        var result = _pakketController.Index();

//        //        //Assert - Object check actions
//        //        result.Should().BeOfType<Task<IActionResult>>();
//        //    }

//        //    [Fact]
//        //    public async void PakketController_Create_ReturnSucces()
//        //    {
//        //        //Arrange - What do i need to bring in
//        //        _context = await _pakketRepoTests.GetDbContext();

//        //        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//        //         {
//        //             new Claim(ClaimTypes.Name, "username"),
//        //             new Claim(ClaimTypes.NameIdentifier, "userId"),
//        //             new Claim("name", "John Doe"),
//        //         }, "mock"));
//        //        var controller = _pakketController;
//        //        controller.ControllerContext = new ControllerContext()
//        //        {
//        //            HttpContext = new DefaultHttpContext() { User = user }
//        //        };

//        //       var pakketVM = new CreatePakketViewModel()
//        //        { 
//        //            ProductCollectie =  new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
//        //            AppUserId = user.GetUserId(),
//        //            Kantine = Locatie.LA        
//        //        };

//        //        var pakket = new Pakket()
//        //        {
//        //            BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
//        //            SelectedProductId = "1",
//        //            Stad = pakketVM.Stad.ToString(),
//        //            Kantine = pakketVM.Kantine.ToString(),
//        //            TijdOphalen = pakketVM.TijdOphalen,
//        //            TijdTotOphalen = pakketVM.TijdTotOphalen,
//        //            Meerderjarig = true,
//        //            Prijs = pakketVM.Prijs,
//        //            TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
//        //        };


//        //        _pakketController.TempData = new TempDataDictionary(_pakketController.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


//        //        A.CallTo(() => _pakketRepo.Add(pakket)).Returns(true);

//        //        //Act
//        //        var result = _pakketController.Create(pakketVM);


//        //        //Assert - Object check actions
//        //        result.Should().BeOfType<Task<IActionResult>>();
//        //    }
//        //}

//    }
//}
