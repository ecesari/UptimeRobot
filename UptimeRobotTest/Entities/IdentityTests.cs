//using System;
//using Xunit;
//using Moq;
//using Uptime_Robot.Services;
//using Uptime_Robot;
//using Uptime_Robot.Controllers;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Uptime_Robot.Data.Entities;

//namespace UptimeRobot.Test
//{
//	public class MonitorTests 
//	{
//		private readonly int _testCatalogItemId = 123;
//		private readonly decimal _testUnitPrice = 1.23m;
//		private readonly int _testQuantity = 2;
//		private readonly string _buyerId = "Test buyerId";


//        private IMonitorService Service { get; }

//        private MonitorController Controller { get; }

//        public IdentityTests()
//        {

//            var users = new List<ApplicationUser>
//            {
//                new ApplicationUser
//                {
//                    UserName = "Test",
//                    Id = Guid.NewGuid().ToString(),
//                    Email = "test@test.it"
//                }

//            }.AsQueryable();

//            var fakeUserManager = new Mock<FakeUserManager>();

//            fakeUserManager.Setup(x => x.Users)
//                .Returns(users);

//            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
//             .ReturnsAsync(IdentityResult.Success);
//            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//            .ReturnsAsync(IdentityResult.Success);
//            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
//          .ReturnsAsync(IdentityResult.Success);




//            var mapper = (IMapper)fixture.Server.Host.Services.GetService(typeof(IMapper));
//            var errorHandler = (IErrorHandler)fixture.Server.Host.Services.GetService(typeof(IErrorHandler));
//            var passwordhasher = (IPasswordHasher<AppUser>)fixture.Server.Host.Services.GetService(typeof(IPasswordHasher<AppUser>));


//            var uservalidator = new Mock<IUserValidator<AppUser>>();
//            uservalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
//             .ReturnsAsync(IdentityResult.Success);
//            var passwordvalidator = new Mock<IPasswordValidator<AppUser>>();
//            passwordvalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
//             .ReturnsAsync(IdentityResult.Success);

//            var signInManager = new Mock<FakeSignInManager>();

//            signInManager.Setup(
//                    x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
//                .ReturnsAsync(SignInResult.Success);


//            //SERVICES CONFIGURATIONS
//            Service = new UsersService(Repository, mapper, uservalidator.Object, passwordvalidator.Object, passwordhasher, signInManager.Object);
//            Controller = new UsersController(Service, errorHandler);
//        }



//        [Theory]
//        [InlineData("test@test.it", "Ciao.Ciao", "Test_user")]
//        public async Task Insert(string email, string password, string name)
//        {
//            //Arrange
//            var testUser = new CreateRequestModel
//            {
//                Email = email,
//                Name = name,
//                Password = password
//            };
//            //Act
//            var createdUser = await Controller.Create(testUser);
//            //Assert
//            Assert.Equal(email, createdUser.Email);
//        }

//        [Fact]
//        public void Get()
//        {
//            //Act
//            var result = Controller.Get();
//            // Assert
//            Assert.NotNull(result);
//        }

//        [Theory]
//        [InlineData("test@test.it")]
//        public void GetByEmail(string email)
//        {
//            //Act
//            var result = Controller.g(email);
//            // Assert
//            Assert.Equal(result.Email, email);
//        }



//        [Theory]
//        [InlineData("test@test.it", "password", "Test")]
//        public async Task Update(string email, string password, string name)
//        {
//            //Arrange
//            var testUser = new UpdateRequestModel
//            {
//                Email = email,
//                Password = password
//            };

//            //Act
//            var updated = await Controller.Edit(testUser);
//            // Assert
//            Assert.Equal(email, updated.Email);
//        }

//        [Theory]
//        [InlineData("test@test.it", "Ciao.Ciao")]
//        public async Task Delete(string email, string password)
//        {
//            //Arrange
//            var testUser = new DeleteRequestModel
//            {
//                Email = email,
//                Password = password
//            };
//            //Act
//            var deleted = await Controller.Delete(testUser);
//            //Assert
//            Assert.Equal(email, deleted.Email);
//        }

//        [Theory]
//        [InlineData("test@test.it", "Ciao.Ciao")]
//        public async Task TokenAsync(string email, string password)
//        {
//            //Arrange
//            var testUser = new TokenRequestModel
//            {
//                Username = email,
//                Password = password
//            };

//            //Act
//            var updated = await Controller.Token(testUser);
//            // Assert
//            Assert.Equal("Test", updated.Principal.Identity.Name);
//        }
//    }

//}
//}
