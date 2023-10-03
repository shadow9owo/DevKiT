using System;
using devkit;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://example.com/api/endpoint";

            string response = main.HttpGet(url);

            Console.WriteLine("Response: " + response);
        }
    }
}
