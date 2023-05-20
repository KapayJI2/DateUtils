using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace DateUtils
{
    public class Indexes
    {
        public string date { get; set; }
        public long dateParse { get; set; }
        public string nameMonth { get; set; }
        public string year { get; set; }
        public double idx { get; set; }
    }
    public class FileReadAndWrite
    {
        private List<Indexes> indexes;
        readonly string path = Directory.GetCurrentDirectory() + "\\indexes.json";

        public List<Indexes> CreateList()
        {
            
            StreamReader sr = new StreamReader(path);
            string jstr = sr.ReadToEnd();
            sr.Close();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            indexes = jss.Deserialize<List<Indexes>>(jstr);
            indexes.Sort((a, b) => b.dateParse.CompareTo(a.dateParse));
            return indexes;
        }
        public List<Indexes> SaveChanges(Indexes ch_value, int index)
        {
            List<Indexes> indexes = this.CreateList();


            indexes.ForEach(a =>
            {
                if(a.dateParse == ch_value.dateParse)
                {
                    a.date = ch_value.date;
                    a.nameMonth = ch_value.nameMonth;
                    a.year = ch_value.year;
                    a.idx = ch_value.idx;
                }
            });
           
            StreamWriter sw = new StreamWriter(path);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string s = jss.Serialize(indexes.ToArray());
            sw.WriteLine(s);
            sw.Close();
            indexes.Clear();
            indexes = this.CreateList();
            return indexes;
        }
    }
}
