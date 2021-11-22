﻿namespace EventSourcingDrinkAlcoholFun.Domain.Events
{
    public class AggregateKey
    {
        public Guid Id { get; set; }

        public static AggregateKey FromGuid
            (Guid id)
        {
            return new AggregateKey() { Id = id};
        }


    }
}