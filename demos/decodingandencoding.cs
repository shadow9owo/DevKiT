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
                Console.WriteLine("do you want to decode or encode a string in base64?\n\n1 - encode\n2 - decode");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    string stringtoencode = Console.ReadLine();
                    Console.WriteLine(EncodeString64(stringtoencode));
                }
                else if (input == "2")
                {
                    string stringtodecode = Console.ReadLine();
                    Console.WriteLine(DecodeString64(stringtodecode));
                }
                else
                {
                    Console.WriteLine("choose from avalible options");
                }
            }
        }
    }
}
