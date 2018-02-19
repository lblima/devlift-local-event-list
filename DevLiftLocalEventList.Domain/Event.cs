using System;

namespace DevLiftLocalEventList.Domain
{
    public class Event : BaseEntity
    {
        protected Event()
        {

        }

        public Event(string description, DateTime date, EventType type)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(nameof(description));

            if (date < DateTime.Today)
                throw new ArgumentException(nameof(date));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Description = description;
            Date = date;
            Type = type;
        }

        public string Description { get; set; }
        public DateTime Date { get; set; }
        public EventType Type { get; set; }
    }
}