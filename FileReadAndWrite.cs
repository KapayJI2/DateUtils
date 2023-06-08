using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public double middle_loan { get; set; }
    }
    public class FileReadAndWrite
    {
        private List<Indexes> indexes;
        readonly string path = Directory.GetCurrentDirectory() + "\\indexes.json";
        readonly string path_table = Directory.GetCurrentDirectory() + "\\table.htm";

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
        public List<Indexes> SaveChanges(Indexes ch_value, int index = 0)
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
        public List<Indexes> AddIndex(Indexes ch_value)
        {
            indexes.Clear();
            indexes = this.CreateList();

            indexes.Add(ch_value);

            StreamWriter sw = new StreamWriter(path);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string s = jss.Serialize(indexes.ToArray());
            sw.WriteLine(s);
            sw.Close();
            indexes.Clear();
            indexes = this.CreateList();
            return indexes;
        }
        public List<Indexes> DelItem(long dateParse)
        {
            List<Indexes> indexes = this.CreateList();
            indexes.ForEach(item =>
            {
                if(item.dateParse == dateParse)
                {
                    indexes.Remove(item);
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
        public void WriteClipboard(string str) {
            try
            {
                string hash = DateTime.Now.ToString().GetHashCode().ToString();
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\table" + hash + ".htm");
                sw.WriteLine(str);
                sw.Close();
                ProcessStartInfo info = new ProcessStartInfo("winword.exe");
                info.Arguments = Directory.GetCurrentDirectory() + "\\table" + hash + ".htm";
                Process.Start(info);
                
            }catch (Exception ex)
            {
                
            }
        }
    }
}
