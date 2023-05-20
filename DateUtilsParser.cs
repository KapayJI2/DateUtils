using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateUtils
{
    internal class DateUtilsParser
    {
        public int GetMonth(string date)
        {
            return DateTime.Parse(date).Month;
        }
    }
}
