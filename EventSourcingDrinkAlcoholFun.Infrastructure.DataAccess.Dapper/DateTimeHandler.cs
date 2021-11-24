using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper
{
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        private readonly TimeZoneInfo databaseTimeZone = TimeZoneInfo.Local;
        public static readonly DateTimeHandler Default = new DateTimeHandler();

        public DateTimeHandler()
        {

        }

        public override DateTimeOffset Parse(object value)
        {
            DateTimeOffset storedDateTime;
            if (value == null)
                storedDateTime = DateTimeOffset.MinValue;
            else
                storedDateTime = DateTimeOffset.Parse(value.ToString());

            if (storedDateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime)
                return DateTimeOffset.MinValue;
            else
                return storedDateTime;
        }

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            DateTime paramVal = value.ToOffset(this.databaseTimeZone.BaseUtcOffset).DateTime;
            parameter.Value = paramVal;
        }
    }
}
