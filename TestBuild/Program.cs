using ConsoleApp1;
using ConsoleApp1.serializers;
using System;
using System.Threading;

namespace TestBuild
{
    class Program
    {
        static ITracer tracer;
        static ISerializer serializer=null;
        static IoutWriter writer;
        static ConsoleKeyInfo pressedKey;
        static Thread tr1;
        static Thread tr2;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            tracer = new ITracerImplementation();

            while (serializer == null)
            {
                Console.WriteLine("Hello! please, selecct serialization method:\n J) json\nX) xml");
                //input = Console.ReadLine();
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.J:
                        serializer = new jsonSer();
                        break;
                    case ConsoleKey.X:
                        serializer = new xmlSer();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("ONLY J AND X ALLOWED!!!");
                        pressedKey = Console.ReadKey(true);
                        break;
                }
                Console.Clear();
            }

            tracer.StartTrace();
            tr1 = new Thread(threadProc1);
            tr2 = new Thread(threadProc2);

            tr1.Start();
            tr2.Start();

            new cl2(tracer).m1();

            tr1.Join();
            tr2.Join();
            tracer.StopTrace();

            TraceResult tr = tracer.GetTraceResult();

            writer = new consoleWriter();
            writer.save(serializer.serialize(tr));
            writer = new fileWriter("myfile.txt");
            writer.save(serializer.serialize(tracer.GetTraceResult()));
        }

        static void threadProc1() {
            tracer.StartTrace();

            for (int i = 0; i < 300; i++) {
                textWriter("t1");
                Thread.Sleep(rnd.Next(1,5));
            }

            tracer.StopTrace();
        }

        static void threadProc2(){

            tracer.StartTrace();
            threadProc1();
            for (int i = 0; i < 300; i++)
            {
                textWriter("t2");
                Thread.Sleep(rnd.Next(1, 5));
            }
            threadProc1();
            tracer.StopTrace();

        }


        static void textWriter(string s)
        {
            tracer.StartTrace();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString()+":"+s);
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
