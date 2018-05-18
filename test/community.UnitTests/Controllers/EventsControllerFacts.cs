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
    public class EventsControllerFacts
    {
        private readonly Event _event;
        private readonly List<Event> _events;
        private readonly Address _address;
        private readonly EventViewModel _eventViewModel;
        private readonly Mock<IManager<Event>> _eventManager;
        private readonly Mock<IManager<Address>> _addressManager;

        public EventsControllerFacts()
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
            _event = new Event
            {
                Date = DateTime.Now,
                Name = string.Empty,
                Address = _address,
                Details = string.Empty,
                UserId = string.Empty
            };
            _events = new List<Event>(1){_event};
            _eventViewModel = new EventViewModel(_event);
            _eventManager = new Mock<IManager<Event>>();
            _addressManager = new Mock<IManager<Address>>();
            SetupManagers();
        }

        [Fact]
        public async Task Index()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Index();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<EventViewModel>>(okObjectResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Details(_event.Id);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EventViewModel>(okObjectResult.Value);
            Assert.Equal(_event.Id, model.Id);
        }

        [Fact]
        public async Task Details_Bad_Id()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Details(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Create(_eventViewModel);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<EventViewModel>(okObjectResult.Value);
            Assert.Equal(_event.Id, model.Id);
        }

        [Fact]
        public async Task Create_Bad_Event()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);
            controller.ModelState.AddModelError(string.Empty, string.Empty);

            var result = await controller.Create(_eventViewModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Edit()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Edit(_event.Id, _eventViewModel);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Edit_Null_Id()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Edit(null, _eventViewModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete()
        {
            var controller = new EventsController(_eventManager.Object, _addressManager.Object);

            var result = await controller.Delete(_event.Id);

            Assert.IsType<OkResult>(result);
        }

        private void SetupManagers()
        {
            _eventManager.Setup(x => x.Index()).Returns(Task.FromResult(_events));
            _eventManager.Setup(x => x.Details(_event.Id)).Returns(Task.FromResult(_event));
            _eventManager.Setup(x => x.Create(It.IsAny<Event>())).Returns(Task.FromResult(_event));
            _eventManager.Setup(x => x.Edit(It.IsAny<Event>())).Returns(Task.CompletedTask);
            _eventManager.Setup(x => x.Delete(_event.Id)).Returns(Task.CompletedTask);
            _addressManager.Setup(x => x.Create(It.IsAny<Address>())).Returns(Task.FromResult(_address));
        }
    }
}
