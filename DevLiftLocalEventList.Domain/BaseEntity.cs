using DevLiftLocalEventList.Domain.Interfaces;

namespace DevLiftLocalEventList.Domain
{
    public class BaseEntity: IEntity
    {
        public long Id { get; set; }
    }
}