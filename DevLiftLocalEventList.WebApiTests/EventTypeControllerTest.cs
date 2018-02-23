using AutoMapper;
using AutoMapper.Configuration;
using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using DevLiftLocalEventList.WebApi.AutoMapperProfiles;
using DevLiftLocalEventList.WebApi.Controllers;
using DevLiftLocalEventList.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevLiftLocalEventList.WebApiTests
{
    [TestClass]
    public class EventTypeControllerTest
    {
        private IEventTypeRepository _eventTypeRepository;
        private EventTypeController controller;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            var mappings = new MapperConfigurationExpression();
            mappings.AddProfile<DomainProfile>();
            Mapper.Initialize(mappings);
        }

        [TestInitialize]
        public void SetupTests()
        {
            var eventTypeRepository = new Mock<IEventTypeRepository>();

            var eventType1 = new EventType("Celebration") { Id = 1 };
            var eventType2 = new EventType("Meeting") { Id = 2 };

            var mockEventTypeList = new List<EventType>
            {
                eventType1,
                eventType2
            };

            // Setup EventType Repo
            eventTypeRepository
                .Setup(repo => repo.GetAll())
                .Returns(Task.FromResult(mockEventTypeList));

            eventTypeRepository
                .Setup(repo => repo.GetOne(1))
                .Returns(Task.FromResult(eventType1));

            _eventTypeRepository = eventTypeRepository.Object;
        }

        [TestMethod]
        public void EventTypeControllerGetAllTest()
        {
            // Arrange
            controller = new EventTypeController(_eventTypeRepository);

            // Act
            var result = controller.Get();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void EventTypeControllerGetByIdTest()
        {
            // Arrange
            controller = new EventTypeController(_eventTypeRepository);

            // Act
            var result = controller.Get(1) as ObjectResult;

            // Assert
            Assert.AreEqual(1, ((EventTypeDto)result.Value).Id);
        }
    }
}