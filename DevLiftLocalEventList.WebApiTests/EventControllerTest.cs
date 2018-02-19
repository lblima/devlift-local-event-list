using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using DevLiftLocalEventList.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DevLiftLocalEventList.WebApiTests
{
    [TestClass]
    public class EventControllerTest
    {
        private IEventRepository _eventRepository;
        private IEventTypeRepository _eventTypeRepository;
        private EventController controller;

        [TestInitialize]
        public void SetupTests()
        {
            var eventRepository = new Mock<IEventRepository>();
            var eventTypeRepository = new Mock<IEventTypeRepository>();

            var eventType = new EventType("Celebration") { Id = 1 };
            var newEvent1 = new Event("My celebration to get hired at DevLift", new DateTime(2018, 3, 15), eventType) { Id = 1 };
            var newEvent2 = new Event("Reception party at Toronto airport", new DateTime(2018, 4, 1), eventType) { Id = 2 };
            var newEvent3 = new Event("Birthday party", new DateTime(2018, 5, 15), eventType) { Id = 3 };

            var mockEventList = new List<Event>
            {
                newEvent1,
                newEvent2,
                newEvent3
            };

            // Setup Event Repo
            eventRepository
                .Setup(repo => repo.GetAll())
                .Returns(Task.FromResult(mockEventList));

            eventRepository
                .Setup(repo => repo.GetOne(1))
                .Returns(Task.FromResult(newEvent1));

            // Setup EventType Repo
            _eventRepository = eventRepository.Object;

            eventTypeRepository
                .Setup(repo => repo.GetOne(1))
                .Returns(Task.FromResult(eventType));

            _eventTypeRepository = eventTypeRepository.Object;
        }

        [TestMethod]
        public void EventControllerGetAllTest()
        {
            // Arrange
            controller = new EventController(_eventRepository, _eventTypeRepository);

            // Act
            var result = controller.Get();

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void EventControllerGetById()
        {
            // Arrange
            controller = new EventController(_eventRepository, _eventTypeRepository);

            // Act
            var result = controller.Get(1) as ObjectResult;

            // Assert
            Assert.AreEqual(1, ((Event)result.Value).Id);
        }
    }
}