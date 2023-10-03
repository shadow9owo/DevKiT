using System;
using devkit;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string identifier = GetUniqueIdentifier();

            Console.WriteLine(identifier);
        }
    }
}
