using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;

namespace RaceDay.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var request = new RegisterDto { Email = "test@test.com", Password = "Password123!", Role = "Participant" };

            // Act
            var result = await controller.Register(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Single(context.Users);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Users.Add(new User { Email = "test@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword") });
            context.SaveChanges();

            var controller = new AuthController(context);
            // We need to mock HttpContext.Session to avoid a null error
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new Moq<ISession>().Object;
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var request = new LoginDto { Email = "test@test.com", Password = "WrongPassword" };

            // Act
            var result = controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Users.Add(new User { Email = "test@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"), Role = "Organiser" });
            context.SaveChanges();

            var controller = new AuthController(context);
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new Moq<ISession>().Object;
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var request = new LoginDto { Email = "test@test.com", Password = "CorrectPassword" };

            // Act
            var result = controller.Login(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}