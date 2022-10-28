using FoodWaste.Application.Interfaces;
using FoodWaste.Infrastructure.Data;
using FoodWasteMVC.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FoodWaste.Domain;
using FoodWaste.Domain.Enums;
using FoodWaste.Application.ViewModels;
using FoodWaste.Application;
using FluentAssertions;

namespace FoodWaste.Tests.Controllers
{

    public class PakketControllerTest
    {
        private readonly PakketController _sut;
        private readonly ApplicationDbContext _context;
        private readonly Mock<IPakketRepo> _pakketRepo = new Mock<IPakketRepo>();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new Mock<IHttpContextAccessor>();
        private readonly Mock<IProductRepo> _productRepo = new Mock<IProductRepo>();
        private readonly Mock<IUserRepo> _userRepo = new Mock<IUserRepo>();
        private readonly Mock<IMedewerkerRepo> _medewerkerRepo = new Mock<IMedewerkerRepo>();

        public PakketControllerTest()
        {
            _sut = new PakketController(_pakketRepo.Object, _productRepo.Object, _userRepo.Object, _httpContextAccessor.Object, _medewerkerRepo.Object, _context);
        }


        [Fact]
        public async void PakketController_Index_US_01_ReturnsSucces()
        {
            // mock HttpContext
            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Email, "foo")
                    }
                    ))
                }
            };
            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            _pakketRepo.Setup(pakketRepo => pakketRepo.GetAll("date", "kantine", SortOrder.Ascending, "")).Returns(
            new List<Pakket>
            {
                new()
                {
                      Id = 1,
                      BeschrijvendeNaam = "Voedsel Pakket 1",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "LA",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"

                },
                new()
                {
                      Id = 2,
                      BeschrijvendeNaam = "Voedsel Pakket 2",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "LD",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"
                }
            });

            // Act
            var result = await _sut.Index() as ViewResult;

            //Assert
            var model = result.Model as IEnumerable<Pakket>;
            Assert.Equal(2, model?.Count());


        }
        [Theory]
        [InlineData(SortOrder.Descending, "LA")]
        [InlineData(SortOrder.Ascending, "LX")]
        [InlineData(SortOrder.Descending, "HL")]
        [InlineData(SortOrder.Ascending, "LD")]
        public async void PakketController_IndexSort_US_02_ReturnsSucces(SortOrder sortOrderDate, string sortOrderKantine)
        {
            var _sortExpression = "";
            if (sortOrderDate == SortOrder.Descending)
            {
                _sortExpression = "date_desc";
            }
            // mock HttpContext
            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Email, "foo")
                    }
                    ))
                }
            };
            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());
            var pakketList = new List<Pakket>
            {
                new()
                {
                      Id = 1,
                      BeschrijvendeNaam = "Voedsel Pakket 1",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "LA",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"

                },
                new()
                {
                      Id = 2,
                      BeschrijvendeNaam = "Voedsel Pakket 2",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "LX",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"
                },
                new()
                {
                      Id = 3,
                      BeschrijvendeNaam = "Voedsel Pakket 2",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "HL",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"
                }
                ,
                new()
                {
                      Id = 4,
                      BeschrijvendeNaam = "Voedsel Pakket 2",
                      SelectedProductId = "1",
                      Stad = "Breda",
                      Kantine = "LD",
                      TijdOphalen = DateTime.Now,
                      TijdTotOphalen = DateTime.Now,
                      Meerderjarig = true,
                      Prijs = 10,
                      TypeMaaltijd = "Brood"
                } };
            _pakketRepo.Setup(pakketRepo => pakketRepo.GetAll("date","kantine", sortOrderDate, sortOrderKantine)).Returns(pakketList);

            // Act
            var result = await _sut.Index(_sortExpression,sortOrderKantine) as ViewResult;

            //Assert
            var model = result.Model as IEnumerable<Pakket>;
            Assert.NotNull(model);
        }


        [Fact]
        public async void PakketController_Create_ReturnErrorNoProductSelected()
        {    
            
            //Arrange

            // mock HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
           }, "mock"));
            var controller = _sut ;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            var pakketVM = new CreatePakketViewModel()
            {
                Id = 1,
                ProductCollectie = new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
                AppUserId = user.GetUserId(),
                Kantine = Locatie.LA
            };

            var pakket = new Pakket()
            {
                BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                SelectedProductId = "1",
                Stad = pakketVM.Stad.ToString(),
                Kantine = pakketVM.Kantine.ToString(),
                TijdOphalen = pakketVM.TijdOphalen,
                TijdTotOphalen = pakketVM.TijdTotOphalen,
                Meerderjarig = true,
                Prijs = pakketVM.Prijs,
                TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
            };


            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


            //Act
            var result = await _sut.Create(pakketVM) as ViewResult;


            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ViewResult));
            result.Equals(true);
            result.Model.Should().BeSameAs(pakketVM);
            result.TempData["Error"].Should().Subject.Should().Be("No products selected");
        }


        [Fact]
        public async void PakketController_Create_Returns_18_Plus_If_Containing_Alcohol()
        {

            //Arrange

            // mock HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
           }, "mock"));
            var controller = _sut;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            var pakketVM = new CreatePakketViewModel()
            {
                Id = 1,
                ProductCollectie = new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
                SelectIDArray = new string[] { "1" },
                AppUserId = user.GetUserId(),
                Kantine = Locatie.LA
            };

            var pakket = new Pakket()
            {
                BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                SelectedProductId = "1",
                Stad = pakketVM.Stad.ToString(),
                Kantine = pakketVM.Kantine.ToString(),
                TijdOphalen = pakketVM.TijdOphalen,
                TijdTotOphalen = pakketVM.TijdTotOphalen,
                Meerderjarig = true,
                Prijs = pakketVM.Prijs,
                TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
            };


            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


            _pakketRepo.Setup(pakketRepo => pakketRepo.GetAllProductsFromPakket("1")).Returns(
                 new List<Product>
                {
                     new()
                    {
                         Id = 1,
                         Alcohol = true,
                         BeschrijvendeNaam = "Grey Goose",
                         Foto = "www.image.nl"
                     } 
                 }
                );

            //Act
            var result = await _sut.Create(pakketVM) as RedirectToActionResult;


            //Assert
            result.Should().BeOfType(typeof(RedirectToActionResult));
            result.Equals(true);
            result.ActionName.Equals("Index");
        }

        [Theory]
        [InlineData(1)]
        public async void PakketController_Reserve_ReturnsSuccess(int id)
        {

            //Arrange

            // mock HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
           }, "mock"));
            var controller = _sut;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            var pakketVM = new CreatePakketViewModel()
            {
                Id = 1,
                ProductCollectie = new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
                SelectIDArray = new string[] { "1" },
                AppUserId = user.GetUserId(),
                Kantine = Locatie.LA
            };

            var pakket = new Pakket()
            {
                BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                SelectedProductId = "1",
                Stad = pakketVM.Stad.ToString(),
                Kantine = pakketVM.Kantine.ToString(),
                TijdOphalen = pakketVM.TijdOphalen,
                TijdTotOphalen = pakketVM.TijdTotOphalen,
                Meerderjarig = true,
                Prijs = pakketVM.Prijs,
                TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
            };


            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


            _pakketRepo.Setup(pakketRepo => pakketRepo.GetByIdAsyncNoTracking(1)).Returns(
                 new Pakket()
                    {
                         Id= 1,
                         BeschrijvendeNaam = "Voedsel Pakket ",
                        SelectedProductId = "1",
                        Stad = "Breda",
                         Kantine = "LA",
                        TijdOphalen = DateTime.Now,
                        TijdTotOphalen = DateTime.Now,
                         Meerderjarig = true,
                         Prijs = 10,
                         TypeMaaltijd = "Brood",                    
                 }
                );

            _userRepo.Setup(userRepo => userRepo.GetStudentByAppuserId()).Returns(
                new Student()
                {
                    Id = 1,
                    Naam = "David Dierckx",
                    Geboortedatum = DateTime.Now.AddYears(-30),
                    EmailAdress = "dierckx3434@gmail.com",
                    TelefoonNummer = "0488367478",
                    Studentnummer = "1",
                    StudieStad = "Breda",
                    AppUserId = user.GetUserId()
                }
                ) ;
            _pakketRepo.Setup(pakket => pakket.GetPakketsEqualCurUserID(user.GetUserId())).Returns(new List<Pakket>()
            {

            new()
            {
                Id = 1,
                BeschrijvendeNaam = "Voedsel Pakket ",
                SelectedProductId = "1",
                Stad = "Breda",
                Kantine = "LA",
                TijdOphalen = DateTime.Now,
                TijdTotOphalen = DateTime.Now,
                Meerderjarig = true,
                Prijs = 10,
                TypeMaaltijd = "Brood",
            }
            });
            //Act
            var result = await _sut.Reserve(id,pakketVM) as RedirectToActionResult;


            //Assert
            result.Should().BeOfType(typeof(RedirectToActionResult));
            result.Equals(true);
            result.ActionName.Equals("Reserve");
        }





        [Theory]
        [InlineData(1)]
        public async void PakketController_Reserve_Return_Gebruiker_Is_Minderjarig_Alcohol_Pakket(int id)
        {

            //Arrange

            // mock HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
           }, "mock"));
            var controller = _sut;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            var pakketVM = new CreatePakketViewModel()
            {
                Id = 1,
                ProductCollectie = new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
                SelectIDArray = new string[] { "1" },
                AppUserId = user.GetUserId(),
                Kantine = Locatie.LA,
                Meerderjarig = true
                
            };

            var pakket = new Pakket()
            {
                BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                SelectedProductId = "1",
                Stad = pakketVM.Stad.ToString(),
                Kantine = pakketVM.Kantine.ToString(),
                TijdOphalen = pakketVM.TijdOphalen,
                TijdTotOphalen = pakketVM.TijdTotOphalen,
                Meerderjarig = true,
                Prijs = pakketVM.Prijs,
                TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
            };


            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


            _pakketRepo.Setup(pakketRepo => pakketRepo.GetByIdAsyncNoTracking(1)).Returns(
                 new Pakket()
                 {
                     Id = 1,
                     BeschrijvendeNaam = "Voedsel Pakket ",
                     SelectedProductId = "1",
                     Stad = "Breda",
                     Kantine = "LA",
                     TijdOphalen = DateTime.Now,
                     TijdTotOphalen = DateTime.Now,
                     Meerderjarig = true,
                     Prijs = 10,
                     TypeMaaltijd = "Brood",
                 }
                );

            _userRepo.Setup(userRepo => userRepo.GetStudentByAppuserId()).Returns(
                new Student()
                {
                    Id = 1,
                    Naam = "David Dierckx",
                    Geboortedatum = DateTime.Now.AddYears(-10),
                    EmailAdress = "dierckx3434@gmail.com",
                    TelefoonNummer = "0488367478",
                    Studentnummer = "1",
                    StudieStad = "Breda",
                    AppUserId = user.GetUserId()
                }
                );
            _pakketRepo.Setup(pakket => pakket.GetPakketsEqualCurUserID(user.GetUserId())).Returns(new List<Pakket>()
            {

            new()
            {
                Id = 1,
                BeschrijvendeNaam = "Voedsel Pakket ",
                SelectedProductId = "1",
                Stad = "Breda",
                Kantine = "LA",
                TijdOphalen = DateTime.Now,
                TijdTotOphalen = DateTime.Now,
                Meerderjarig = true,
                Prijs = 10,
                TypeMaaltijd = "Brood",
            }
            });
            //Act
            var result = await _sut.Reserve(id, pakketVM) as RedirectToActionResult;


            //Assert
            result.Should().BeOfType(typeof(RedirectToActionResult));
            result.Equals(true);
            result.ActionName.Equals("Index");
        }
        [Theory]
        [InlineData(1)]
        public async void PakketController_Reserve_Return_Maximaal_1_Pakket_Per_Afhaaldag(int id)
        {

            //Arrange

            // mock HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
           }, "mock"));
            var controller = _sut;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            var pakketVM = new CreatePakketViewModel()
            {
                Id = 1,
                ProductCollectie = new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
                SelectIDArray = new string[] { "1" },
                AppUserId = user.GetUserId(),
                Kantine = Locatie.LA,
                TijdOphalen = DateTime.Now,

            };

            var pakket = new Pakket()
            {
                BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                SelectedProductId = "1",
                Stad = pakketVM.Stad.ToString(),
                Kantine = pakketVM.Kantine.ToString(),
                TijdOphalen = pakketVM.TijdOphalen,
                TijdTotOphalen = pakketVM.TijdTotOphalen,
                Meerderjarig = true,
                Prijs = pakketVM.Prijs,
                TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
            };


            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


            _pakketRepo.Setup(pakketRepo => pakketRepo.GetByIdAsyncNoTracking(1)).Returns(
                 new Pakket()
                 {
                     Id = 1,
                     BeschrijvendeNaam = "Voedsel Pakket ",
                     SelectedProductId = "1",
                     Stad = "Breda",
                     Kantine = "LA",
                     TijdOphalen = DateTime.Now,
                     TijdTotOphalen = DateTime.Now,
                     Meerderjarig = true,
                     Prijs = 10,
                     TypeMaaltijd = "Brood",
                 }
                );

            _userRepo.Setup(userRepo => userRepo.GetStudentByAppuserId()).Returns(
                new Student()
                {
                    Id = 1,
                    Naam = "David Dierckx",
                    Geboortedatum = DateTime.Now.AddYears(-30),
                    EmailAdress = "dierckx3434@gmail.com",
                    TelefoonNummer = "0488367478",
                    Studentnummer = "1",
                    StudieStad = "Breda",
                    AppUserId = user.GetUserId()
                }
                );
            _pakketRepo.Setup(pakket => pakket.GetPakketsEqualCurUserID(user.GetUserId())).Returns(new List<Pakket>()
            {

            new()
            {
                Id = 1,
                BeschrijvendeNaam = "Voedsel Pakket ",
                SelectedProductId = "1",
                Stad = "Breda",
                Kantine = "LA",
                TijdOphalen = DateTime.Now,
                TijdTotOphalen = DateTime.Now,
                Meerderjarig = true,
                Prijs = 10,
                TypeMaaltijd = "Brood",
            }
            });
            //Act
            var result = await _sut.Reserve(id, pakketVM) as RedirectToActionResult;


            //Assert
            result.Should().BeOfType(typeof(RedirectToActionResult));
            result.Equals(true);
            result.ActionName.Equals("Index");
        }
        [Theory]
        [InlineData(1)]
        public async void PakketController_Reserve_Return_Is_Al_Gereserveerd(int id)
        {

            //Arrange

            // mock HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
           }, "mock"));
            var controller = _sut;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            var pakketVM = new CreatePakketViewModel()
            {
                Id = 1,
                ProductCollectie = new Product[] { new() { BeschrijvendeNaam = "Product 1", Foto = "www.img.com", Alcohol = true, Id = 1 } },
                SelectIDArray = new string[] { "1" },
                AppUserId = user.GetUserId(),
                Kantine = Locatie.LA,
                TijdOphalen = DateTime.Now,

            };

            var pakket = new Pakket()
            {
                BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                SelectedProductId = "1",
                Stad = pakketVM.Stad.ToString(),
                Kantine = pakketVM.Kantine.ToString(),
                TijdOphalen = pakketVM.TijdOphalen,
                TijdTotOphalen = pakketVM.TijdTotOphalen,
                Meerderjarig = true,
                Prijs = pakketVM.Prijs,
                TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
            };


            // create TempData, otherwise crash
            _sut.TempData = new TempDataDictionary(_sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());


            _pakketRepo.Setup(pakketRepo => pakketRepo.GetByIdAsyncNoTracking(1)).Returns(
                 new Pakket()
                 {
                     Id = 1,
                     BeschrijvendeNaam = "Voedsel Pakket ",
                     SelectedProductId = "1",
                     Stad = "Breda",
                     Kantine = "LA",
                     TijdOphalen = DateTime.Now,
                     TijdTotOphalen = DateTime.Now,
                     Meerderjarig = true,
                     Prijs = 10,
                     TypeMaaltijd = "Brood",
                     StudentenIds = "1"
                 }
                );

            _userRepo.Setup(userRepo => userRepo.GetStudentByAppuserId()).Returns(
                new Student()
                {
                    Id = 1,
                    Naam = "David Dierckx",
                    Geboortedatum = DateTime.Now.AddYears(-30),
                    EmailAdress = "dierckx3434@gmail.com",
                    TelefoonNummer = "0488367478",
                    Studentnummer = "1",
                    StudieStad = "Breda",
                    AppUserId = user.GetUserId()
                }
                );
            _pakketRepo.Setup(pakket => pakket.GetPakketsEqualCurUserID(user.GetUserId())).Returns(new List<Pakket>()
            {

            new()
            {
                Id = 1,
                BeschrijvendeNaam = "Voedsel Pakket ",
                SelectedProductId = "1",
                Stad = "Breda",
                Kantine = "LA",
                TijdOphalen = DateTime.Now,
                TijdTotOphalen = DateTime.Now,
                Meerderjarig = true,
                Prijs = 10,
                TypeMaaltijd = "Brood",
            }
            });
            //Act
            var result = await _sut.Reserve(id, pakketVM) as RedirectToActionResult;


            //Assert
            result.Should().BeOfType(typeof(RedirectToActionResult));
            result.Equals(true);
            result.ActionName.Equals("Index");
        }
    }
}
