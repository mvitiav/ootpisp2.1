using System;

namespace ConsoleApp1
{
    public class consoleWriter : IoutWriter
    {
        public void save(string result)
        {
            Console.WriteLine(result);
        }
    }
}