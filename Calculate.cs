using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;

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
            //try
            //{
                List<LoanList> result = new List<LoanList>();
                List<Indexes> operation_list = new List<Indexes>();
                long start_date_to_long = (long)(DateTime.Parse(startDate) - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000;
                long end_date_to_long = (long)(DateTime.Parse(endDate) - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000;
                indexes.ForEach(index =>
                {
                    //if (index.dateParse >= start_date_to_long && index.dateParse <= end_date_to_long)
                    //{
                    //    operation_list.Add(index);
                    //}
                    if ((DateTime.Parse(index.date) >= DateTime.Parse(startDate) && DateTime.Parse(index.date) <= DateTime.Parse(endDate)) || (DateTime.Parse(index.date).Year == DateTime.Parse(startDate).Year && DateTime.Parse(index.date).Month == DateTime.Parse(startDate).Month))
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
                        indexes1.nameMonth = DateTime.Parse(item.middle_date).ToString("MMMM");
                        indexes1.idx = indexes.Find((el) => el.nameMonth == indexes1.nameMonth && el.year == indexes1.year).idx;
                        indexes1.middle_loan = item.middle_summ;
                        operation_list.Add(indexes1);
                    });
                    operation_list.Sort((a, b) => a.dateParse.CompareTo(b.dateParse));
                    for (int i = 0; i < operation_list.Count; i++)
                    {
                        Indexes curr_el = operation_list[i];
                        if (i + 1 < operation_list.Count)
                        {

                            Indexes next_el = operation_list[i + 1];
                            if (i == 0)
                            {
                                LoanList loan_list = new LoanList();
                                loan_list.summ = String.Format("{0:0.00}", start_summ);
                                loan_list.from = DateTime.Parse(startDate).ToShortDateString();
                                loan_list.to = new DateTime(DateTime.Parse(loan_list.from).Year, DateTime.Parse(loan_list.from).Month, DateTime.DaysInMonth(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month)).ToShortDateString();
                            loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(startDate).Day + 1;
                                int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month);
                                loan_list.index = curr_el.idx;
                                loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                                //loan_list.formula = "000";
                                if (loan_list.index > 100)
                                {
                                    loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
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
                                continue;
                            }
                            if (curr_el.nameMonth == next_el.nameMonth && curr_el.year == next_el.year && !indexes.Contains(next_el))
                            {
                                LoanList loan_list = new LoanList();
                                loan_list.summ = String.Format("{0:0.00}", start_summ);
                            if (i - 1 < 0)
                                {
                                    loan_list.from = curr_el.date;
                                }
                                else
                                {
                                    Indexes prev_el = operation_list[i - 1];
                                    loan_list.from = DateTime.Compare(DateTime.Parse(prev_el.date), DateTime.Parse(curr_el.date)) == 0 ? new DateTime(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month, DateTime.Parse(curr_el.date).Day + 1).ToShortDateString() : curr_el.date;
                                }
                                loan_list.to = new DateTime(DateTime.Parse(next_el.date).Year, DateTime.Parse(next_el.date).Month, DateTime.Parse(loan_list.from).Day == DateTime.Parse(next_el.date).Day ? DateTime.Parse(loan_list.from).Day : DateTime.Parse(next_el.date).Day - 1).ToShortDateString();
                                loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                                int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month);
                                loan_list.index = curr_el.idx;
                                if (curr_el.middle_loan > 0)
                                {
                                loan_list.formula = String.Format("[-{0:0.00}] {1:0.00} x ({2} / {3}) x {4:0.00} / 100", curr_el.middle_loan, Convert.ToDouble(loan_list.summ) - curr_el.middle_loan, loan_list.days, days_in_current_month, loan_list.index - 100);
                                }
                                else
                                {
                                loan_list.summ = String.Format("{0:0.00}", start_summ);
                                loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                                }
                                //loan_list.formula = "111";
                                if (loan_list.index > 100)
                                {
                                    if (curr_el.middle_loan > 0)
                                    {
                                        loan_list.result = String.Format("{0:0.00}", (Convert.ToDouble(loan_list.summ) - curr_el.middle_loan) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
                                    }
                                    else
                                    {
                                        loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
                                    }
                                    if (summ_checked)
                                    {
                                        start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(loan_list.result);
                                    }
                                    else
                                    {
                                        start_summ = Convert.ToDouble(loan_list.summ);
                                    }
                                    if (curr_el.middle_loan > 0)
                                    {
                                    start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(curr_el.middle_loan);
                                    loan_list.summ = String.Format("{0:0.00}", start_summ);
                                }

                                }
                                else
                                {
                                if (curr_el.middle_loan > 0)
                                {
                                    start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(curr_el.middle_loan);
                                    loan_list.summ = String.Format("{0:0.00}", start_summ);
                                }
                                loan_list.result = "-";
                                    start_summ = Convert.ToDouble(loan_list.summ);
                                }
                                result.Add(loan_list);
                                continue;
                            }
                            else
                            {
                                LoanList loan_list = new LoanList();
                                int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month);
                                loan_list.summ = String.Format("{0:0.00}", start_summ);
                            if (i - 1 < 0)
                                {
                                    loan_list.from = curr_el.date;
                                }
                                else
                                {
                                    Indexes prev_el = operation_list[i - 1];
                                    loan_list.from = DateTime.Compare(DateTime.Parse(prev_el.date), DateTime.Parse(curr_el.date)) == 0 ? new DateTime(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month, DateTime.Parse(curr_el.date).Day + 1).ToShortDateString() : curr_el.date;
                                }
                                loan_list.to = new DateTime(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month, DateTime.Parse(next_el.date).Day > DateTime.Parse(curr_el.date).Day ? DateTime.Parse(next_el.date).Day == DateTime.Parse(curr_el.date).Day ? DateTime.Parse(next_el.date).Day : DateTime.Parse(next_el.date).Day - 1 : days_in_current_month).ToShortDateString();
                                loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                                loan_list.index = curr_el.idx;
                                if (curr_el.middle_loan > 0)
                                {
                                
                                loan_list.formula = String.Format("[-{0:0.00}] {1:0.00} x ({2} / {3}) x {4:0.00} / 100", curr_el.middle_loan, Convert.ToDouble(loan_list.summ) - curr_el.middle_loan, loan_list.days, days_in_current_month, loan_list.index - 100);
                                }
                                else
                                {
                                    loan_list.formula = String.Format("{0} x ({1} / {2}) x {3:0.00} / 100", loan_list.summ, loan_list.days, days_in_current_month, loan_list.index - 100);
                                }
                                //loan_list.formula = "222";
                                if (loan_list.index > 100)
                                {
                                    if (curr_el.middle_loan > 0)
                                    {
                                        loan_list.result = String.Format("{0:0.00}", (Convert.ToDouble(loan_list.summ) - curr_el.middle_loan) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
                                    }
                                    else
                                    {
                                        loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
                                    }

                                    if (summ_checked)
                                    {
                                        start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(loan_list.result);
                                    }
                                    else
                                    {
                                        start_summ = Convert.ToDouble(loan_list.summ);
                                    }
                                    if (curr_el.middle_loan > 0)
                                    {
                                    start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(curr_el.middle_loan);
                                    loan_list.summ = String.Format("{0:0.00}", start_summ);
                                }
                                }
                                else
                                {
                                if (curr_el.middle_loan > 0)
                                {
                                    start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(curr_el.middle_loan);
                                    loan_list.summ = String.Format("{0:0.00}", start_summ);
                                }
                                loan_list.result = "-";
                                    start_summ = Convert.ToDouble(loan_list.summ);
                                }
                                result.Add(loan_list);
                                continue;
                            }
                        }
                        else
                        {
                            LoanList loan_list = new LoanList();
                            loan_list.summ = String.Format("{0:0.00}", start_summ);
                            loan_list.from = DateTime.Parse(curr_el.date).ToShortDateString();
                            loan_list.to = new DateTime(DateTime.Parse(endDate).Year, DateTime.Parse(endDate).Month, DateTime.Parse(endDate).Day).ToShortDateString();
                            loan_list.days = DateTime.Parse(endDate).Day - DateTime.Parse(loan_list.from).Day + 1;
                            int days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(curr_el.date).Year, DateTime.Parse(curr_el.date).Month);
                            loan_list.index = curr_el.idx;
                        //loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                        if (curr_el.middle_loan > 0)
                        {
                            loan_list.formula = String.Format("[-{0:0.00}] {1:0.00} x ({2} / {3}) x {4:0.00} / 100", curr_el.middle_loan, Convert.ToDouble(loan_list.summ) - curr_el.middle_loan, loan_list.days, days_in_current_month, loan_list.index - 100);
                        }
                        else
                        {
                            loan_list.summ = String.Format("{0:0.00}", start_summ);
                            loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                        }
                        //loan_list.formula = "333";
                        if (loan_list.index > 100)
                        {
                            if (curr_el.middle_loan > 0)
                            {
                                loan_list.result = String.Format("{0:0.00}", (Convert.ToDouble(loan_list.summ) - curr_el.middle_loan) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
                            }
                            else
                            {
                                loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
                            }

                            if (summ_checked)
                            {
                                start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(loan_list.result);
                            }
                            else
                            {
                                start_summ = Convert.ToDouble(loan_list.summ);
                            }
                            if (curr_el.middle_loan > 0)
                            {
                                start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(curr_el.middle_loan);
                                loan_list.summ = String.Format("{0:0.00}", start_summ);
                            }
                        }
                        else
                        {
                            if (curr_el.middle_loan > 0)
                            {
                                start_summ = Convert.ToDouble(loan_list.summ) - Convert.ToDouble(curr_el.middle_loan);
                                loan_list.summ = String.Format("{0:0.00}", start_summ);
                            }
                            loan_list.result = "-";
                            start_summ = Convert.ToDouble(loan_list.summ);
                        }
                        result.Add(loan_list);
                            continue;
                        }
                    }
                    return result;
                }
                ////////////
                else
                {
                    operation_list.Sort((a, b) => a.dateParse.CompareTo(b.dateParse));
                    for (int i = 0; i < operation_list.Count; i++)
                    {
                        Indexes item = operation_list[i];
                        int days_in_current_month;
                        LoanList loan_list = new LoanList();
                        if (i == 0)
                        {
                            loan_list.summ = String.Format("{0:0.00}", start_summ);
                            loan_list.from = DateTime.Parse(startDate).ToShortDateString();
                            loan_list.to = new DateTime(DateTime.Parse(startDate).Year, DateTime.Parse(startDate).Month, DateTime.DaysInMonth(DateTime.Parse(startDate).Year, DateTime.Parse(startDate).Month)).ToShortDateString();
                            loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                            days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month);
                            loan_list.index = item.idx;
                            loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                            //loan_list.formula = String.Format("{0}", st_d);
                            if (loan_list.index > 100)
                            {
                                loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
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
                            continue;
                        }
                        if (i == operation_list.Count - 1)
                        {
                            loan_list.summ = String.Format("{0:0.00}", start_summ);
                            loan_list.from = DateTime.Parse(item.date).ToShortDateString();
                            loan_list.to = new DateTime(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month, DateTime.Parse(endDate).Day).ToShortDateString();
                            loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                            days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month);
                            loan_list.index = item.idx;
                            loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                            if (loan_list.index > 100)
                            {
                                loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
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
                            continue;
                        }
                        //operation_list.ForEach(item =>
                        //{
                        loan_list.summ = String.Format("{0:0.00}", start_summ);
                        loan_list.from = new DateTime(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month, DateTime.Parse(item.date).Day).ToShortDateString();
                        loan_list.to = new DateTime(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month, DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month)).ToShortDateString();
                        loan_list.days = DateTime.Parse(loan_list.to).Day - DateTime.Parse(loan_list.from).Day + 1;
                        days_in_current_month = DateTime.DaysInMonth(DateTime.Parse(item.date).Year, DateTime.Parse(item.date).Month);
                        loan_list.index = item.idx;
                        loan_list.formula = String.Format("{0:0.00} x ({1} / {2}) x {3:0.00} / 100", Convert.ToDouble(loan_list.summ), loan_list.days, days_in_current_month, loan_list.index - 100);
                        //loan_list.formula = String.Format("{0}", DateTime.Parse(startDate));
                        if (loan_list.index > 100)
                        {
                            loan_list.result = String.Format("{0:0.00}", Convert.ToDouble(loan_list.summ) * (Convert.ToDouble(loan_list.days) / Convert.ToDouble(days_in_current_month)) * (loan_list.index - 100) / 100);
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
                        //});
                    }
                    return result;
                }
            //}catch(Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
    }
}
