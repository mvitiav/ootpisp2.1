using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TraceLib
{
    public class ITracerImplementation : ITracer
    {
        private Dictionary<string, TraceRecord> recordDictionary = new Dictionary<string, TraceRecord>();
        private Dictionary<string, Stopwatch> watchDictionary = new Dictionary<string, Stopwatch>();

        public ITracerImplementation() {}

        public TraceResult GetTraceResult()
        {
            List<TraceRecord> temopRecs = recordDictionary.Values.ToList();
            temopRecs.Sort();
            TraceResult traceResult = new TraceResult();
            foreach (TraceRecord tr in temopRecs)
            {
                traceResult.addTraceRecord(tr);
            }
            return traceResult;        
        }

        public void StartTrace()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            StackTrace stackTrace = new StackTrace();

            lock (watchDictionary)
            {      
                watchDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)] = watch;
            }
        }

        public void StopTrace()
        {
        
            StackTrace stackTrace = new StackTrace();
            Stopwatch watch;
            lock (watchDictionary)
            {
                watch = watchDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)];
            }
            watch.Stop();

            lock (recordDictionary)
            {
                if (!recordDictionary.ContainsKey(Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)))
                {
                    recordDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)]=new TraceRecord(Thread.CurrentThread, getFullName(stackTrace), getMethodClazz(stackTrace));
                }
               
                recordDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)].addValue(watch);
            }

        }

        private Type getMethodClazz(StackTrace stackTrace)
        {
            return stackTrace.GetFrame(1).GetMethod().ReflectedType;
        }

        private string getClassString(StackTrace stackTrace)
        {
            return stackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
        }

        private string getMethodsString(StackTrace stackTrace)
        {
            string ret = "";

            for (int i = stackTrace.GetFrames().Length - 1; i > 0; i--)
            {
                ret += '.' + stackTrace.GetFrame(i).GetMethod().Name;
            }

            return ret;
        }

        private string getFullName(StackTrace stackTrace)
        {
            return getClassString(stackTrace) + getMethodsString(stackTrace);
        }
    }
}