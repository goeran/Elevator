using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;

namespace Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionSet = new OptionSet();
            optionSet.Add("--h", s =>
            {
                Console.WriteLine("A" + s);
            });
            optionSet.Parse(args);
            Console.WriteLine("Hello world!");
        }
    }
}
