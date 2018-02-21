using DevLiftLocalEventList.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevLiftLocalEventList.DomainTests
{
    [TestClass]
    public class EventTest
    {
        [TestMethod]
        public void EventShouldCreateWithCorrectParams()
        {
            //Arrange
            var eventType = new EventType("Celebration");
            var description = "My celebration to get hired at DevLift";
            var date = new DateTime(2018, 03, 15);
            var eventSummary = "This event is a party to celebrate this achievement and all my friends are invited to participate. Bring only your good vibes and happiness";

            var newEvent = new Event(description, eventSummary, date, eventType);

            //Assert
            Assert.AreEqual(description, newEvent.Description);
            Assert.AreEqual(date, newEvent.Date);
            Assert.IsNotNull(newEvent.Type);
            Assert.AreEqual(eventType.Description, newEvent.Type.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EventShouldRaiseExceptionWithInvalidDescription()
        {
            //Arrange
            var eventType = new EventType("Celebration");
            var eventSummary = "This event is a party to celebrate this achievement and all my friends are invited to participate. Bring only your good vibes and happiness";
            var newEvent = new Event(null, eventSummary, new DateTime(2018, 03, 15), eventType);

            //Assert
            Assert.IsNotNull(newEvent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EventShouldRaiseExceptionWithInvalidSummary()
        {
            //Arrange
            var eventType = new EventType("Celebration");
            var description = "My celebration to get hired at DevLift";
            var newEvent = new Event(description, null, new DateTime(2018, 03, 15), eventType);

            //Assert
            Assert.IsNotNull(newEvent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EventShouldRaiseExceptionWithInvalidDate()
        {
            //Arrange
            var eventType = new EventType("Celebration");
            var description = "My celebration to get hired at DevLift";
            var eventSummary = "This event is a party to celebrate this achievement and all my friends are invited to participate. Bring only your good vibes and happiness";
            var newEvent = new Event(description, eventSummary, new DateTime(2018, 1, 1), eventType);

            //Assert
            Assert.IsNotNull(newEvent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EventShouldRaiseExceptionWithInvalidType()
        {
            //Arrange
            var description = "My celebration to get hired at DevLift";
            var eventSummary = "This event is a party to celebrate this achievement and all my friends are invited to participate. Bring only your good vibes and happiness";
            var newEvent = new Event(description, eventSummary, new DateTime(2018, 3, 15), null);

            //Assert
            Assert.IsNotNull(newEvent);
        }
    }
}