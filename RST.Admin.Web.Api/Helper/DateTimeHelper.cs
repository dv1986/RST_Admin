using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RST.Admin.Web.Api.Helper
{
    public class DateTimeHelper
    {
        public static int GetBussinessDays(int month, int year )
        {
            var weekends = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };



            //Fetch the amount of days in your given month.
            int daysInMonth = DateTime.DaysInMonth(year, month);

            //Here we create an enumerable from 1 to daysInMonth,
            //and ask whether the DateTime object we create belongs to a weekend day,
            //if it doesn't, add it to our IEnumerable<int> collection of days.
            IEnumerable<int> businessDaysInMonth = Enumerable.Range(1, daysInMonth)
                                                   .Where(d => !weekends.Contains(new DateTime(year, month, d).DayOfWeek));
            return businessDaysInMonth.Count();
        }
    }
}
