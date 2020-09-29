using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    [Serializable]
    public class TraceResult
    {
        private Dictionary<int, threadRecord> threaDs;

        public List<threadRecord> threadz { get { return threaDs.Values.ToList(); } }

        // fullfills task but breaks any of xml serializer
        //public IList<threadRecord> threads { get {return threaDs.Values.ToList().AsReadOnly(); } }

        public TraceResult()
        {
            threaDs = new Dictionary<int, threadRecord>();
        }

        public void addTraceRecord(TraceRecord tr)
        {
            if (!threaDs.ContainsKey(tr.threadId)) { threaDs[tr.threadId] = new threadRecord(); threaDs[tr.threadId].id = tr.threadId; }
            threaDs[tr.threadId].addMethodRecord(tr);
        }
    }
}