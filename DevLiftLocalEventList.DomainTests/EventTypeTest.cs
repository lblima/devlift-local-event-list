using DevLiftLocalEventList.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevLiftLocalEventList.DomainTests
{
    [TestClass]
    public class EventTypeTest
    {
        [TestMethod]
        public void EventTypeShouldCreateWithCorrectDescription()
        {
            //Arrange
            var description = "Convention";
            var eventType = new EventType(description);

            //Assert
            Assert.AreEqual(description, eventType.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EventTypeShouldRaiseExceptionWithInvalidParams()
        {
            //Arrange
            var eventType = new EventType(null);

            //Assert
            Assert.IsNotNull(eventType);
        }
    }
}