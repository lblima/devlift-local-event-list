using System;

namespace DevLiftLocalEventList.Domain
{
    public class EventType: BaseEntity
    {
        protected EventType()
        {

        }

        public EventType(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(nameof(description));

            Description = description;
        }

        public string Description { get; set; }
    }
}