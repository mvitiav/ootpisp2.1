using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    [Serializable]
    public class threadRecord
    {
        public threadRecord()
        {
            methodz = new Dictionary<string, methodRecord>();
        }

        public int id { get; set; }

        private Dictionary<string, methodRecord> methodz;
        public List<methodRecord> methods { get { return methodz.Values.ToList(); } }

        public void addMethodRecord(TraceRecord tr)
        {
            string temp = "";
            foreach (string s in methodz.Keys.ToList())
            {
                if (tr.fullname.Substring(0, s.Length) == s)
                {
                    temp = s;
                 //   tr.fullname = tr.fullname.Substring(s.Length+1);
                    break;
                }
            }

            if ((temp.Length > 0)&& (tr.fullname.Length > temp.Length))
            {
                tr.fullname = tr.fullname.Substring(temp.Length + 1);
                methodz[temp].addMethodRecord(tr);
            }
            else
            {
                if (!methodz.ContainsKey(tr.fullname)) { methodz[tr.fullname] = new methodRecord(); }
                methodz[tr.fullname].fullName = tr.fullname;
                methodz[tr.fullname].classname = tr.className;
                methodz[tr.fullname].executionTimes = tr.count;
                methodz[tr.fullname].avgTimeTicks = tr.getAvg();
                methodz[tr.fullname].avgTimeMilis = tr.getAvgs();
            }
        }
    }
}