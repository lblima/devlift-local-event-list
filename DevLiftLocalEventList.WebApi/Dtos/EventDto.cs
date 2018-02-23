using System;

namespace DevLiftLocalEventList.WebApi.Dto
{
    public class EventDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public long TypeId { get; set; }
        public string TypeDescription { get; set; }
        public Decimal Price { get; set; }
        public string ImageLink { get; set; }
    }
}