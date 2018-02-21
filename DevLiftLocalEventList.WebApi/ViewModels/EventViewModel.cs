using System;

namespace DevLiftLocalEventList.WebApi.ViewModels
{
    public class EventViewModel
    {
        public string Description { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public EventTypeViewModel Type { get; set; }
        public Decimal Price { get; set; }
        public string ImageLink { get; set; }
    }
}
