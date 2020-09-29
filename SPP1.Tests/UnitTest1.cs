//using ConsoleApp1;
//using ConsoleApp1.serializers

using TraceLib;
using TraceLib.serializers;

using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SPP1.Tests
{
    public class Tests
    {
        int _DELTA = 25;
        private string path;
        private string tempTxt;
        private ITracer tracer;
        private IoutWriter writer;
        private ISerializer serializer;
        private Random rnd;
        TraceRecord tr1;
        TraceRecord tr2;
        Stopwatch st1;
        Stopwatch st2;
        [SetUp]
        public void Setup()
        {
            path = "myfile.test.txt";
            rnd = new Random();
            tracer = new ITracerImplementation();
            tempTxt = "This is my string! there are many like this but this is mine!...";
            if (File.Exists(path)) { File.Delete(path); }

            tr1 = new TraceRecord(Thread.CurrentThread, "method1.method2", tempTxt.GetType());
            tr2 = new TraceRecord(Thread.CurrentThread, "method1.method2.method3", rnd.GetType());

            st1 = System.Diagnostics.Stopwatch.StartNew();
             st2 = System.Diagnostics.Stopwatch.StartNew();
            Thread.Sleep(rnd.Next(1, 100));
            st1.Stop();
            Thread.Sleep(rnd.Next(1, 100));
            st2.Stop();
            tr1.addValue(st1);
            tr1.addValue(st2);

        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        [Test]
        public void VisualStudioIsOkTest()
        {
            Assert.Pass();
        }

        [Test]
        public void JSONSerializerTest()
        {
            TraceResult traceResult = new TraceResult();
            serializer = new jsonSer();
            tempTxt = serializer.serialize(traceResult);

            Assert.AreEqual(tempTxt, "{\r\n  \"threadz\": []\r\n}");
        }

        [Test]
        public void XMLSerializerTest()
        {
            TraceResult traceResult = new TraceResult();
            serializer = new xmlSer();
            tempTxt = serializer.serialize(traceResult);

            Assert.AreEqual(tempTxt, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<TraceResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <threadz />\r\n</TraceResult>");
        }

        [Test]
        public void ConsoleWriterTest()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                writer = new consoleWriter();
                writer.save(tempTxt);
                tempTxt = string.Format(tempTxt + "{0}", Environment.NewLine);
                Assert.AreEqual(tempTxt, sw.ToString());
            }
        }

        [Test]
        public void FileWriterTest()
        {
            using (StringWriter sw = new StringWriter())
            {
                writer = new fileWriter(path);

                writer.save(tempTxt);

                if (File.Exists(path))
                {
                    Assert.AreEqual(tempTxt, File.ReadAllText(path));
                }
                else
                {
                    Assert.Fail("file does not exist");
                }

                tempTxt = string.Format(tempTxt + "{0}", Environment.NewLine);
            }
        }

        [Test]
        public void ThreadRecordTest()
        {
            threadRecord thr = new threadRecord();
            Assert.AreEqual(thr.methods.Count(), 0);
            thr.addMethodRecord(tr1);
            Assert.AreEqual(tr1.count, thr.methods[0].executionTimes);
            Assert.AreEqual(tr1.getAvgs(), thr.methods[0].avgTimeMilis);

            Assert.AreEqual(tr1.getAvg(), thr.methods[0].avgTimeTicks);

            Assert.AreEqual(thr.methods.Count(), 1);
            Assert.AreEqual(0, thr.methods[0].methods.Count());

            thr.addMethodRecord(tr2);
            Assert.AreEqual(1, thr.methods.Count());
            Assert.AreEqual(1, thr.methods[0].methods.Count());
            Assert.AreEqual("method3", thr.methods[0].methods[0].fullName);

            Assert.AreEqual(rnd.GetType().ToString(), thr.methods[0].methods[0].classname);
        }

        [Test]
        public void TraceRecordCounterTest()
        {           
            tr1.addValue(st1);
            Assert.AreEqual(3, tr1.count);
            // Assert.Fail("WIP");
        }

        [Test]
        public void TraceRecordThreadIdTest()
        {
            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, tr1.threadId);         
        }

        [Test]
        public void TraceRecordTypeTest()
        {
            Assert.AreEqual(tempTxt.GetType().ToString(), tr1.className);
        }

        [Test]
        public void TraceRecordFullNameTest()
        {
            Assert.AreEqual("method1.method2", tr1.fullname);
          
        }

        [Test]
        public void TraceRecordCompareToTest()
        {
            string st1 = RandomString(rnd.Next(1, 50));
            string st2 = RandomString(rnd.Next(1, 50)); ;
            TraceRecord tr1 = new TraceRecord(null, st1, null);
            TraceRecord tr2 = new TraceRecord(null, st2, null);

            Assert.AreEqual((tr1.Equals(tr2)), (st1.Equals(st2)));
            Assert.AreEqual((tr1.CompareTo(tr2)), (st1.Length.CompareTo(st2.Length)));
            
        }

        [Test]
        public void TraceRecordMathTest()
        {
            
            Assert.AreEqual(((st1.ElapsedTicks + st2.ElapsedTicks) / 2),(tr1.getAvg()));
            Assert.AreEqual(((st1.ElapsedMilliseconds + st2.ElapsedMilliseconds) / 2),(tr1.getAvgs()));

        }

        [Test]
        public void MethodRecordTest()
        {
            methodRecord mr = new methodRecord();
            Assert.AreEqual(mr.methods.Count(), 0);
            mr.addMethodRecord(tr1);
            Assert.AreEqual(tr1.count,mr.methods[0].executionTimes);
            Assert.AreEqual(tr1.getAvgs(),mr.methods[0].avgTimeMilis);

            Assert.AreEqual(tr1.getAvg(),mr.methods[0].avgTimeTicks);

            Assert.AreEqual(mr.methods.Count(), 1);
            Assert.AreEqual(0, mr.methods[0].methods.Count());

            mr.addMethodRecord(tr2);
            Assert.AreEqual(1, mr.methods.Count());
            Assert.AreEqual(1, mr.methods[0].methods.Count());
            Assert.AreEqual("method3",mr.methods[0].methods[0].fullName);

            Assert.AreEqual(rnd.GetType().ToString(), mr.methods[0].methods[0].classname);
        }

        [Test]
        public void TraceResultTest()
        {
            //tracer.GetTraceResult();

            TraceResult tresRez = new TraceResult();

            Assert.AreEqual(0, tresRez.threadz.Count);

            tresRez.addTraceRecord(tr1);

            Assert.AreEqual(1, tresRez.threadz.Count);
          


            //Assert.Fail("WIP");
        }      

        [Test]
        public void TracerTest()
        {
            int await = rnd.Next(100, 1000);
            Stopwatch st3 = System.Diagnostics.Stopwatch.StartNew();
            tracer.StartTrace();
            Thread.Sleep(await);
            tracer.StopTrace();
            st3.Stop();

            Assert.AreEqual(await, tracer.GetTraceResult().threadz[0].methods[0].avgTimeMilis,_DELTA);
            // Assert.Fail("WIP");
        }

        //todo: finish!!!
        [Test]
        public void SetupTest()
        {
            Assert.NotNull(tracer);
            Assert.Pass();
        }

     
    }
}