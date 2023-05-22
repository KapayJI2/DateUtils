using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateUtils
{
    public class MiddleLoan
    {
        public string middle_date { get; set; }
        public double middle_summ { get; set; }
    }
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
        public List<LoanList> Result_List(List<Indexes> indexes,bool summ_checked,double start_summ, string startDate, string endDate, List<MiddleLoan> middle_list = null)
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
            if (middle_list != null)
            {
                middle_list.ForEach(item =>
                {
                    long middle_dateParse = (long)(DateTime.Parse(item.middle_date) - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000;
                    Indexes indexes1 = new Indexes();
                    indexes1.dateParse = middle_dateParse;
                    indexes1.date = item.middle_date;
                    indexes1.year = DateTime.Parse(item.middle_date).Year.ToString();
                    indexes1.nameMonth = DateTime.Parse(item.middle_date).Month.ToString();
                    indexes1.idx = indexes.Find((el) => el.nameMonth == indexes1.nameMonth && el.year == indexes1.year).idx;
                    indexes1.middle_loan = item.middle_summ;
                    operation_list.Add(indexes1);
                });
                operation_list.Sort((a, b) => a.dateParse.CompareTo(b.dateParse));
                operation_list.ForEach(item =>
                {
                    LoanList loan_list = new LoanList();
                    loan_list.summ = String.Format("{0:0.00}", start_summ);
                    loan_list.from = item.date;
                    loan_list.to = new DateTime(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month, DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month)).ToShortDateString();
                    loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                    int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month);
                    loan_list.index = item.idx;
                    loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                    if (loan_list.index > 100)
                    {
                        loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (double)(loan_list.days / days_in_current_month) * (loan_list.index - 100) / 100);
                        if (summ_checked)
                        {
                            start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(loan_list.result);
                        }
                        else
                        {
                            start_summ = Convert.ToDouble(loan_list.summ);
                        }
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
            ////////////
            else
            {
                operation_list.ForEach(item =>
                {
                    LoanList loan_list = new LoanList();
                    loan_list.summ = String.Format("{0:0.00}", start_summ);
                    loan_list.from = item.date;
                    loan_list.to = new DateTime(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month, DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month)).ToShortDateString();
                    loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                    int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month);
                    loan_list.index = item.idx;
                    loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                    if (loan_list.index > 100)
                    {
                        loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (double)(loan_list.days / days_in_current_month) * (loan_list.index - 100) / 100);
                        if (summ_checked)
                        {
                            start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(loan_list.result);
                        }
                        else
                        {
                            start_summ = Convert.ToDouble(loan_list.summ);
                        }
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
}
