using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces
{
    public class EventTemp
    {

        public int Id { get; set; }
        public string Key_StreamId { get; set; }
        public string Value_Data { get; set; }
        public string AssemblyQualifiedName_Type { get; set; }

        public string TimeStamp { get; set; }

        public string Version_SerialNumber { get; set; }
    }
}
