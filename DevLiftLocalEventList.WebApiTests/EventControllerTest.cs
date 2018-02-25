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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevLiftLocalEventList.WebApiTests
{
    [TestClass]
    public class EventControllerTest
    {
        private IEventRepository _eventRepository;
        private IEventTypeRepository _eventTypeRepository;
        private EventController controller;

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
            var eventRepository = new Mock<IEventRepository>();
            var eventTypeRepository = new Mock<IEventTypeRepository>();

            var eventType = new EventType("Celebration") { Id = 1 };
            var newEvent1 = new Event("My celebration to get hired at DevLift", "This event is a party to celebrate this achievement and all my friends are invited to participate. Bring only your good vibes and happiness", new DateTime(2018, 3, 15), eventType) { Id = 1 };
            var newEvent2 = new Event("Reception party at Toronto airport", "This event is reception meeting to celebrate my arrive in Canada", new DateTime(2018, 4, 1), eventType) { Id = 2 };
            var newEvent3 = new Event("Birthday party", "This event is my birthday party, all my friends are invited", new DateTime(2018, 5, 15), eventType) { Id = 3 };

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
                .Setup(repo => repo.GetAllUpcoming())
                .Returns(Task.FromResult(mockEventList));            

            eventRepository
                .Setup(repo => repo.GetOne(1))
                .Returns(Task.FromResult(newEvent1));
            
            _eventRepository = eventRepository.Object;

            // Setup EventType Repo
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
        public void EventControllerGetByIdTest()
        {
            // Arrange
            controller = new EventController(_eventRepository, _eventTypeRepository);

            // Act
            var result = controller.Get(1) as ObjectResult;

            // Assert
            Assert.AreEqual(1, ((EventDto)result.Value).Id);
        }
    }
}