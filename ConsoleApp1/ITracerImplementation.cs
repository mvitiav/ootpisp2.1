using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    public class ITracerImplementation : ITracer
    {
        //private Stopwatch watch;

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
            //throw new NotImplementedException();
        }

        public void StartTrace(
            //[CallerMemberName]string memberName = ""
            //,
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            StackTrace stackTrace = new StackTrace();

            //Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

            //Console.WriteLine(stackTrace.GetFrame(1).GetMethod().ReflectedType.Name + "." + stackTrace.GetFrame(1).GetMethod().Name);
            //Console.WriteLine( "tracing of "+ Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace) + " started");
            // watch = System.Diagnostics.Stopwatch.StartNew();

            //if (!watchDictionary.ContainsKey(getFullName(stackTrace)))
            //{
            //    watchDictionary.Remove(getFullName(stackTrace));
            //}

            lock (watchDictionary)
            {
                //Console.WriteLine("yay"+Thread.CurrentThread.ManagedThreadId.ToString());
                watchDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)] = watch;
                //Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString()+ "yay");
            }

            //Console.WriteLine("traceStarted");
            //Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            //Console.WriteLine(memberName);
        }

        public void StopTrace()
        {
            //   watch.Stop();
            StackTrace stackTrace = new StackTrace();
            Stopwatch watch;
            lock (watchDictionary)
            {
                watch = watchDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)];
            }
            watch.Stop();

            if (!recordDictionary.ContainsKey(Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace))) { recordDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)] = new TraceRecord(Thread.CurrentThread, getFullName(stackTrace), getMethodClazz(stackTrace)); }
            //recordDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)].addValue(watch.Elapsed.Ticks);
            recordDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)].addValue(watch);

            //recordDictionary[getFullName(stackTrace)].addValue(watch.ElapsedMilliseconds);
            //var elapsedMs = watch.ElapsedMilliseconds;
            //todo: add nice time format
            //Console.WriteLine("trace of " + stackTrace.GetFrame(1).GetMethod().ReflectedType.Name + "." + stackTrace.GetFrame(1).GetMethod().Name + " ended in " + watch.Elapsed);

            //Console.WriteLine("thread                           "+Thread.CurrentThread.ManagedThreadId);
            //Console.WriteLine(Task.CurrentId.ToString());

            //Console.WriteLine("trace of "+ getFullName(stackTrace) + " ended in " + watch.Elapsed+" avg: " + recordDictionary[Thread.CurrentThread.ManagedThreadId.ToString() + ":" + getFullName(stackTrace)].getAvg());

            //Console.WriteLine(stackTrace.GetFrame(2).GetMethod().Name);
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