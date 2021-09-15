using System;
using Xunit;
using Moq;
using Uptime_Robot.Services;
using Uptime_Robot;
using Uptime_Robot.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Uptime_Robot.Data.Entities;
using Uptime_Robot.Models;

namespace UptimeRobot.Test
{
	public class MonitorServiceTests
	{
		[Theory]
		[InlineData("Google", "https://www.google.com.tr/", 2)]
		public async void Create_WithViewModel_ReturnsOk(string header, string url, int interval)
		{
			var monitorService = new Mock<IMonitorService>();
			var emailService = new Mock<IEmailSender>();
			var uptimeService = new Mock<UptimeService>();
			var service = uptimeService.Object;
			//var service = monitorService.Object;
			var id = Guid.NewGuid();
			var testMonitor = new MonitorViewModel
			{
				Id = id,
				Header = header,
				Url = url,
				Interval = interval
			};
			await service.AddMonitor(testMonitor, id.ToString());
			//var insertSuccessful = (await service.GetAllMonitors()).Any(x => x.Url == url);
			//var foo = (await service.GetAllMonitors());
			var loadedItem = await service.GetMonitor(id);


			Assert.NotNull(loadedItem);

		}
		//private readonly Mock<IMonitorService> _serviceMock;

		//public MonitorServiceTests(Mock<IMonitorService> serviceMock)
		//{
		//	_serviceMock = serviceMock;
		//}

		//[Theory]
		//[InlineData("Google", "https://www.google.com.tr/", 2)]
		//public async void Create_WithViewModel_ReturnsOk(string header, string url, int interval)
		//{
		//	var monitorService = new Mock<IMonitorService>();
		//	var service = monitorService.Object;
		//	var id = Guid.NewGuid();
		//	var testMonitor = new MonitorViewModel
		//	{
		//		Id = id,
		//		Header = header,
		//		Url = url,
		//		Interval = interval
		//	};
		//	await service.AddMonitor(testMonitor, id.ToString());
		//	//var insertSuccessful = (await service.GetAllMonitors()).Any(x => x.Url == url);
		//	//var foo = (await service.GetAllMonitors());
		//	var loadedItem = await service.GetMonitor(id);


		//	Assert.NotNull(loadedItem);

		//}


		//public IdentityTests()
		//{

		//    var users = new List<ApplicationUser>
		//    {
		//        new ApplicationUser
		//        {
		//            UserName = "Test",
		//            Id = Guid.NewGuid().ToString(),
		//            Email = "test@test.it"
		//        }

		//    }.AsQueryable();

		//    var fakeUserManager = new Mock<FakeUserManager>();

		//    fakeUserManager.Setup(x => x.Users)
		//        .Returns(users);

		//    fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
		//     .ReturnsAsync(IdentityResult.Success);
		//    fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
		//    .ReturnsAsync(IdentityResult.Success);
		//    fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
		//  .ReturnsAsync(IdentityResult.Success);




		//    var mapper = (IMapper)Services.GetService(typeof(IMapper));
		//    var errorHandler = (IErrorHandler)fixture.Server.Host.Services.GetService(typeof(IErrorHandler));
		//    var passwordhasher = (IPasswordHasher<ApplicationUser>)fixture.Server.Host.Services.GetService(typeof(IPasswordHasher<ApplicationUser>));


		//    var uservalidator = new Mock<IUserValidator<ApplicationUser>>();
		//    uservalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<ApplicationUser>()))
		//     .ReturnsAsync(IdentityResult.Success);
		//    var passwordvalidator = new Mock<IPasswordValidator<ApplicationUser>>();
		//    passwordvalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<ApplicationUser>(), It.IsAny<string>()))
		//     .ReturnsAsync(IdentityResult.Success);

		//    var signInManager = new Mock<FakeSignInManager>();

		//    signInManager.Setup(
		//            x => x.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
		//        .ReturnsAsync(SignInResult.Success);


		//    //SERVICES CONFIGURATIONS
		//    Service = new UsersService(Repository, mapper, uservalidator.Object, passwordvalidator.Object, passwordhasher, signInManager.Object);
		//    Controller = new UsersController(Service, errorHandler);
		//}


		//[Theory]
		//[InlineData("Google", "https://www.google.com.tr/", 2)]
		//public async void Create_WithViewModel_ReturnsOk(string header, string url, int interval)
		//{

		//	var testMonitor = new MonitorViewModel
		//	{
		//		Header = header,
		//		Url = url,
		//		Interval = interval
		//	};
		//	var result = await Service.AddMonitor(testMonitor);
		//	Assert.NotNull(result);
		//	Assert.Equal(200, result.GetHashCode());

		//}


		//[Theory]
		//[InlineData("Google", "https://www.google.com.tr/", 2)]
		//public void Create_WithViewModel_ReturnsOk(string header, string url, int interval)
		//{

		// var testMonitor = new MonitorViewModel
		// {
		// Header = header,
		//       Url = url,
		//       Interval = interval
		// };
		// var result = Controller.Create(testMonitor);
		// Assert.NotNull(result);
		// Assert.Equal(200, result.GetHashCode());

		//}


		//[Theory]
		//[InlineData("test@test.it", "Ciao.Ciao", "Test_user")]
		//public async Task Insert(string email, string password, string name)
		//{
		// //Arrange
		// var testUser = new CreateRequestModel
		// {
		//  Email = email,
		//  Name = name,
		//  Password = password
		// };
		// //Act
		// var createdUser = await Controller.Create(testUser);
		// //Assert
		// Assert.Equal(email, createdUser.Email);
		//}

		//[Fact]
		//public void Get()
		//{
		//	//Act
		//	var result = Controller.Get();
		//	// Assert
		//	Assert.NotNull(result);
		//}

		//[Theory]
		//[InlineData("test@test.it")]
		//public void GetByEmail(string email)
		//{
		//	//Act
		//	var result = Controller.g(email);
		//	// Assert
		//	Assert.Equal(result.Email, email);
		//}



		//[Theory]
		//[InlineData("test@test.it", "password", "Test")]
		//public async Task Update(string email, string password, string name)
		//{
		//	//Arrange
		//	var testUser = new UpdateRequestModel
		//	{
		//		Email = email,
		//		Password = password
		//	};

		//	//Act
		//	var updated = await Controller.Edit(testUser);
		//	// Assert
		//	Assert.Equal(email, updated.Email);
		//}

		//[Theory]
		//[InlineData("test@test.it", "Ciao.Ciao")]
		//public async Task Delete(string email, string password)
		//{
		//	//Arrange
		//	var testUser = new DeleteRequestModel
		//	{
		//		Email = email,
		//		Password = password
		//	};
		//	//Act
		//	var deleted = await Controller.Delete(testUser);
		//	//Assert
		//	Assert.Equal(email, deleted.Email);
		//}

		//[Theory]
		//[InlineData("test@test.it", "Ciao.Ciao")]
		//public async Task TokenAsync(string email, string password)
		//{
		//	//Arrange
		//	var testUser = new TokenRequestModel
		//	{
		//		Username = email,
		//		Password = password
		//	};

		//	//Act
		//	var updated = await Controller.Token(testUser);
		//	// Assert
		//	Assert.Equal("Test", updated.Principal.Identity.Name);
		//}
	}

}
