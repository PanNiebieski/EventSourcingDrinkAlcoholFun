using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    public static class AggregateFactory
    {
        public static T CreateAggregate<T>()
        {
            try
            {
                return (T)Activator.CreateInstance
                    (typeof(T), true);
            }
            catch (MissingMethodException)
            {
                throw new 
                    MissingParameterLessConstructorException
                    (typeof(T));
            }
        }
    }
}
