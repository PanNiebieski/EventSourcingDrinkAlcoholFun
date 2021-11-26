using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces
{
    public interface IEventTableManager
    {
        Task DeleteAllRecordsInTables();
    }
}
