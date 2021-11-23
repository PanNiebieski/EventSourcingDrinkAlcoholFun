namespace EventSourcingDrinkAlcoholFun.Domain.Events
{
    //public abstract record DomainEvent
    //    (AggregateKey? Key_StreamId,
    //    )
    //{
    //    public AggregateKey? Key_StreamId { get; set; }
    //    public int Version_SerialNumber { get; set; } = 0;
    //    public DateTimeOffset TimeStamp { get; set; }

    //    protected DomainEvent()
    //    {
    //        TimeStamp = DateTimeOffset.Now;
    //    }
    //}


    public abstract class DomainEvent
    {
        //public DomainEventData Data { get; init; } 
        //    = new DomainEventData();

        public AggregateKey? Key_StreamId { get; set; }
        public int Version_SerialNumber { get; set; } = 0;
        public DateTimeOffset TimeStamp { get; set; }

        protected DomainEvent()
        {
            TimeStamp = DateTimeOffset.UtcNow;
        }

        public abstract int Type { get; }
    }

    public class DomainEventData
    {

    }


}