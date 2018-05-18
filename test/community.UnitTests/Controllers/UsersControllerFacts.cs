using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community.Api.v1.Controllers;
using community.Api.v1.ViewModels;
using community.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace community.UnitTests.Controllers
{
    public class UsersControllerFacts
    {
        private readonly User _user;
        private readonly IQueryable<User> _users;
        private readonly Mock<UserManager<User>> _userManager;

        public UsersControllerFacts()
        {
            _user = new User();
            _users = new List<User>(1){_user}.AsQueryable();
            _userManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null);
            SetupManager();
        }

        [Fact]
        public void Index()
        {
            var controller = new UsersController(_userManager.Object);

            var result =  controller.Index();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserViewModel>>(okObjectResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details()
        {
            var controller = new UsersController(_userManager.Object);

            var result = await controller.Details(_user.Id);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<UserViewModel>(okObjectResult.Value);
            Assert.Equal(_user.Id, model.Id);
        }

        [Fact]
        public async Task Details_Bad_Id()
        {
            var controller = new UsersController(_userManager.Object);

            var result = await controller.Details(string.Empty);

            Assert.IsType<NotFoundResult>(result);
        }

        private void SetupManager()
        {
            _userManager.Setup(x => x.Users).Returns(_users);
            _userManager.Setup(x => x.FindByIdAsync(_user.Id)).Returns(Task.FromResult(_user));
        }
    }
}
