using System;
using devkit;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("do you want to encrypt a string or decrypt a string\n\n1 is encrypt\n2 is decrypt");
                string action = Console.ReadLine();
                if (action == "1")
                {
                    Console.WriteLine("input a string");
                    string stringinput = Console.ReadLine();
                    Console.WriteLine("input a password");
                    string passwordinput = Console.ReadLine();

                    Console.WriteLine(EncryptString(stringinput, passwordinput));
                }
                else if (action == "2")
                {
                    Console.WriteLine("input a string");
                    string stringinput = Console.ReadLine();
                    Console.WriteLine("input a password");
                    string passwordinput = Console.ReadLine();

                    Console.WriteLine(DecryptString(stringinput, passwordinput));
                }
                else
                {
                    Console.WriteLine("select from 1(encrypt) or 2(decrypt)");
                }
            }
        }
    }
}
