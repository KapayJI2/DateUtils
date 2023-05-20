using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateUtils
{
    public class DateUtilsParser
    {
        public int GetMonth(string date)
        {
            //123 222
            return DateTime.Parse(date).Month;
        }
    }
}
