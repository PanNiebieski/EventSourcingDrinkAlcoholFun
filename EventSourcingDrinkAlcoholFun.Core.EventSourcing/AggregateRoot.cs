using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using EventSourcingDrinkAlcoholFun.DomainEvents.Drinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    public abstract class AggregateRoot
    {
        private readonly List<DomainEvent> _changes = new List<DomainEvent>();
        private readonly List<DomainEvent> _uncommitedChanges = new List<DomainEvent>();

        public AggregateKey Key { get; protected set; }
        public int Version_SerialNumber { get; protected set; }
        public int LoadedVersion_SerialNumber { get; protected set; }

        public List<DomainEvent> Changes
        {
            get
            {
                return _changes;
            }
        }

        public List<DomainEvent> UncommitedChanges
        {
            get
            {
                return _uncommitedChanges;
            }
        }

        public void LoadFromHistory(IEnumerable<DomainEvent> history)
        {
            //Drink drink = null;

            foreach (var @event in history)
            {
                if (@event.Version_SerialNumber != Version_SerialNumber)
                    throw new EventsOutOfOrderException(@event.Version_SerialNumber);

                
                ApplyChangeBase(@event, false);

            }
        }

        public IEnumerable<DomainEvent> GetUncommittedChanges()
        {
            lock (_uncommitedChanges)
            {
                return _uncommitedChanges.ToArray();
            }
        }

        public void MarkChangesAsCommitted()
        {
            lock (_changes)
            {
                LoadedVersion_SerialNumber = Version_SerialNumber;
                _uncommitedChanges.Clear();
            }
        }

        public void ReverseEventTimeTravel(int step)
        {
            lock (_changes)
            {
                if (_changes.Count > step)
                {
                    List<DomainEvent> reverseEvents = new List<DomainEvent>();

                    var f = _changes.Count  - step;

                    for (int i = _changes.Count - 1; i >= f; i--)
                    {
                        var @event = _changes[i];
                        //ReverseChange(@event);
                        reverseEvents.Add(@event);
                    }

                    ApplyChangeBase(
                        new ReverseEvent
                        (reverseEvents,
                        Key.Id, 
                        Version_SerialNumber++), true);
                }
            }
        }

        protected void ApplyChangeBase(DomainEvent @event, bool isNew)
        {
            lock (_changes)
            {
                //@event.Version_SerialNumber = Version_SerialNumber + 1;

                ApplyChange(@event);
                _changes.Add(@event);

                if (isNew)
                {
                    _uncommitedChanges.Add(@event);
                }
                else
                {
                    LoadedVersion_SerialNumber++;
                }

                Key = @event.Key_StreamId;
                Version_SerialNumber++;

            }
        }

        protected abstract void ApplyChange(DomainEvent @event);

        //protected abstract void ReverseChange(DomainEvent @event);
    }
}
