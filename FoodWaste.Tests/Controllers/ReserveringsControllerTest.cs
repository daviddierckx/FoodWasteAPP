using FakeItEasy;
using FluentAssertions;
using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Repository;
using FoodWasteMVC.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoodWaste.Tests.Controllers
{
    public class ReserveringsControllerTest
    {
        private  IUserRepo _userRepo;
        private UserManager<AppUser> _userManager;
        private ReserveringenController _reserveringenController;

        public ReserveringsControllerTest()
        {
            //DI
            _userRepo = A.Fake<IUserRepo>();
            _userManager = A.Fake<UserManager<AppUser>>();

            //SUT
            _reserveringenController = new ReserveringenController(_userRepo, _userManager);


        }

        [Fact]
        public void ReserveringsController_Index_US_01_ReturnsSucces()
        {
            //Arrange - What do i need to bring in
            var userPakkets = A.Fake<List<Pakket>>();
            var user = A.Fake<AppUser>();
            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Name, "username"),
                 new Claim(ClaimTypes.NameIdentifier, "userId"),
                 new Claim("name", "John Doe"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            A.CallTo(() => _userRepo.GetAllStudentPakkets()).Returns(userPakkets);
            A.CallTo(()=> _userManager.GetUserAsync(claimsPrincipal)).Returns(user);
            //Act
            var result = _reserveringenController.Index();

            //Assert - Object check actions
            result.Should().NotBeNull();

        }
    }
}
