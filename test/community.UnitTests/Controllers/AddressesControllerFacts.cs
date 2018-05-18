using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using community.Api.v1.Controllers;
using community.Api.v1.ViewModels;
using community.Core.Interfaces;
using community.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace community.UnitTests.Controllers
{
    public class AddressesControllerFacts
    {
        private readonly Address _address;
        private readonly List<Address> _addresses;
        private readonly AddressViewModel _addressViewModel;
        private readonly Mock<IManager<Address>> _addressManager;

        public AddressesControllerFacts()
        {
            _address = new Address
            {
                Street = string.Empty,
                Street2 = string.Empty,
                City = string.Empty,
                State = string.Empty,
                ZipCode = string.Empty,
                UserId = string.Empty
            };
            _addresses = new List<Address>(1){_address};
            _addressViewModel = new AddressViewModel(_address);
            _addressManager = new Mock<IManager<Address>>();
            SetupManager();
        }

        [Fact]
        public async Task Index()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Index();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AddressViewModel>>(okObjectResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Details(_address.Id);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<AddressViewModel>(okObjectResult.Value);
            Assert.Equal(_address.Id, model.Id);
        }

        [Fact]
        public async Task Details_Bad_Id()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Details(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Create(_addressViewModel);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<AddressViewModel>(okObjectResult.Value);
            Assert.Equal(_address.Id, model.Id);
        }

        [Fact]
        public async Task Create_Bad_Recipe()
        {
            var controller = new AddressesController(_addressManager.Object);
            controller.ModelState.AddModelError(string.Empty, string.Empty);

            var result = await controller.Create(_addressViewModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Edit()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Edit(_address.Id, _addressViewModel);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Edit_Null_Id()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Edit(null, _addressViewModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete()
        {
            var controller = new AddressesController(_addressManager.Object);

            var result = await controller.Delete(_address.Id);

            Assert.IsType<OkResult>(result);
        }

        private void SetupManager()
        {
            _addressManager.Setup(x => x.Index()).Returns(Task.FromResult(_addresses));
            _addressManager.Setup(x => x.Details(_address.Id)).Returns(Task.FromResult(_address));
            _addressManager.Setup(x => x.Create(It.IsAny<Address>())).Returns(Task.FromResult(_address));
            _addressManager.Setup(x => x.Edit(It.IsAny<Address>())).Returns(Task.CompletedTask);
            _addressManager.Setup(x => x.Delete(_address.Id)).Returns(Task.CompletedTask);
        }
    }
}
