using ConsoleApp1;
using ConsoleApp1.serializers;
using System;
using System.Threading;

namespace TestBuild
{
    class Program
    {
        static ITracer tracer;
        static ISerializer serializer;
        static IoutWriter writer;
        string input;
        static Thread tr1;
        static Thread tr2;
        static Random rnd = new Random();

        static void Main(string[] args)
        {

            string input;
            tracer = new ITracerImplementation();
            Console.WriteLine("1 - json other - xml");
            input = Console.ReadLine();
            if (input == "1") {
                serializer = new jsonSer();
            } else {
                serializer = new xmlSer();
            }

            tracer.StartTrace();
            tr1 = new Thread(threadProc1);
            tr2 = new Thread(threadProc2);

            tr1.Start();
            tr2.Start();

            new cl2(tracer).m1();

            tr1.Join();
            tr2.Join();

           // Thread.Sleep(500);


            tracer.StopTrace();

            TraceResult tr = tracer.GetTraceResult();

            writer = new consoleWriter();
            writer.save(serializer.serialize(tr));
            writer = new fileWriter("myfile.txt");
            writer.save(serializer.serialize(tracer.GetTraceResult()));
            
            


            Console.WriteLine("Hello World!");
        }

        static void threadProc1() {
            tracer.StartTrace();

            for (int i = 0; i < 300; i++) {
                Console.WriteLine("t1");
                Thread.Sleep(rnd.Next(1,5));
            }

            tracer.StopTrace();
        }

        static void threadProc2(){



            tracer.StartTrace();
            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine("t2!");
                Thread.Sleep(rnd.Next(1, 5));
            }
            threadProc1();
            tracer.StopTrace();


        }


    }

    public class cl2
    {

        ITracer _tracer;

        public cl2(ITracer tracer) { this._tracer = tracer; }

        public void m1() {
            _tracer.StartTrace();
            m2();
            _tracer.StopTrace();
        }

        private void m2()
        {
            m3();
        }
        private void m3()
        {
            _tracer.StartTrace();
            Console.WriteLine("m3");
            _tracer.StopTrace();
        }

    }
}
