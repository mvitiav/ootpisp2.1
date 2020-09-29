using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class methodRecord
    {
        public string classname { get; set; }
        public int executionTimes { get; set; }
        public string fullName { get; set; }
        public long avgTimeTicks { get; set; }
        public long avgTimeMilis { get; set; }

        private Dictionary<string, methodRecord> methodz;
        public List<methodRecord> methods { get { return methodz.Values.ToList(); } }

        public methodRecord()
        {
            methodz = new Dictionary<string, methodRecord>();
        }

        public void addMethodRecord(TraceRecord tr)
        {
            string temp = "";
            foreach (string s in methodz.Keys.ToList())
            {
                if (tr.fullname.Substring(0, s.Length) == s)
                {
                    temp = s;
                    tr.fullname = tr.fullname.Substring(s.Length+1);
                    break;
                }
            }

            if (temp.Length > 0)
            {
                methodz[temp].addMethodRecord(tr);
            }
            else
            {
                //todo: change condition
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