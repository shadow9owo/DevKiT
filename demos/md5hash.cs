using System;
using devkit;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string md5hashed = hashmd5("hashme");

            Console.WriteLine("hashed version: " + md5hashed);
        }
    }
}
