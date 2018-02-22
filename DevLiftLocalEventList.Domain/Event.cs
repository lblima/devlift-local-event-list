using System;

namespace DevLiftLocalEventList.Domain
{
    public class Event : BaseEntity
    {
        protected Event()
        {

        }

        public Event(string description, string summary, DateTime date, EventType type)
        {
            if (string.IsNullOrWhiteSpace(summary))
                throw new ArgumentException("Please, enter a Summary.It´s a more detailed description about the event");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Please, enter a description");

            if (date < DateTime.Today)
                throw new ArgumentException("The event date cannot be earlier than today");

            if (type == null)
                throw new ArgumentNullException("Please, select an event type");

            Description = description;
            Summary = summary;
            Date = date;
            Type = type;
        }

        public string Description { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public EventType Type { get; set; }
        public Decimal Price { get; set; }
        public string ImageLink { get; set; }
    }
}