using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Domain
{
    public class AuditableEntity
    {
        public DateTimeOffset CreatedAt { get; init; } 
            = DateTimeOffset.Now;
    }

    //public record AuditableEntity(DateTimeOffset CreatedAt);


    //public class AuditableEntity
    //{
    //    //public string CreatedBy { get; set; }

    //    public DateTime CreatedAt { get; set; }

    //    //public string LastModifiedBy { get; set; }

    //    //public DateTime? LastModifiedDate { get; set; }
    //}
}
