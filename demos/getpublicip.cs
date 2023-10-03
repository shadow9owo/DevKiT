using System;
using devkit;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            //works only when connected to wifi

            string ip = GetPublicIP();

            if (ip != "fail")
            {
                Console.WriteLine(ip);
            }else
            {
                Console.WriteLine("please connect to an wifi network");
            }
        }
    }
}
