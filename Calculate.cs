using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateUtils
{
    public class LoanList
    {
        public string summ { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int days { get; set; }
        public double index { get; set; }
        public string formula { get; set; }
        public string result { get; set; }
    }
    public class Calculate
    {
        public List<LoanList> Result_List(List<Indexes> indexes,double start_summ, string startDate, string endDate, List<Indexes> middle_list = null)
        {
            List<LoanList> result = new List<LoanList>();
            List<Indexes> operation_list = new List<Indexes>();
            long start_date_to_long = (long)(DateTime.Parse(startDate) - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000;
            long end_date_to_long = (long)(DateTime.Parse(endDate) - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000;
            indexes.ForEach(index =>
            {
                if (index.dateParse >= start_date_to_long && index.dateParse <= end_date_to_long)
                {
                    operation_list.Add(index);
                }
            });
            operation_list.ForEach(item =>
            {
                LoanList loan_list = new LoanList();
                loan_list.summ = String.Format("{0:0.00}",start_summ);
                loan_list.from = item.date;
                loan_list.to = new DateTime(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month, DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month)).ToString();
                loan_list.days = (DateTime.Parse(loan_list.to) - DateTime.Parse(loan_list.from)).Days + 1;
                int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month);
                loan_list.index = item.idx;
                loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), DateTime.Parse(loan_list.from).Day, DateTime.Parse(loan_list.to).Day, loan_list.index - 100);
                if (loan_list.index > 100) {
                    loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (double)(loan_list.days / days_in_current_month) * (loan_list.index - 100) / 100);
                    start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(loan_list.result);
                }
                else
                {
                    loan_list.result = "-";
                    start_summ = Convert.ToDouble(loan_list.summ);
                }
                result.Add(loan_list);
            });
            return result;
        }
    }
}
