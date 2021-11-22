﻿namespace EventSourcingDrinkAlcoholFun.Domain.Events
{
    public abstract class DomainEvent
    {
        public AggregateKey? Key_StreamId { get; set; }
        public int Version_SerialNumber { get; set; } = 0;
        public DateTimeOffset TimeStamp { get; set; }

        protected DomainEvent()
        {
            TimeStamp = DateTimeOffset.Now;
        }
    }
}