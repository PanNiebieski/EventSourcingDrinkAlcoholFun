using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core
{
    public interface ITableManager
    {
        Task DeleteAllRecordsInTables();
    }


}
