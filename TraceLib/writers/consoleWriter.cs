using System;

namespace TraceLib
{
    public class consoleWriter : IoutWriter
    {
        public void save(string result)
        {
            Console.WriteLine(result);
        }
    }
}