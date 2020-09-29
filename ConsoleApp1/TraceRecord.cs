using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace ConsoleApp1
{
    [Serializable]
    public class TraceRecord : IComparable<TraceRecord>
    {
        private long avgTime;

        private long avgTimes;

        public int count { get; private set; }

        private long sum = 0;

        private long sums = 0;

        private Thread thread;    

        public int threadId { get { return thread.ManagedThreadId; } }

        public string fullname { get; set; }

        private Type clazz;

        public string className { get { return clazz.ToString(); } }

        public TraceRecord(Thread thread, string fullname, Type clazz)
        {
            this.thread = thread;
            this.fullname = fullname;
            this.clazz = clazz;
        }

        public long getAvg()
        {
            return avgTime;
        }

        public long getAvgs()
        {
            return avgTimes;
        }

        public void addValue(Stopwatch watch)
        {
            count++;
            sum += watch.Elapsed.Ticks;
            sums += watch.ElapsedMilliseconds;
            this.avgTime = sum / count;
            this.avgTimes = sums / count;
        }

        public int CompareTo([AllowNull] TraceRecord other)
        {
            if (other == null)
                return 1;
            else
                return this.fullname.Length.CompareTo(other.fullname.Length);
        }

        public TraceRecord()
        {
        }
    }
}