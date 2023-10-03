using System;
using devkit;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("yourkey1", "yourvalue1");
            postData.Add("yourkey2", "yourvalue2");

            string url = "https://example.com/api/endpoint";

            string response = main.HttpPost(url, postData);

            Console.WriteLine("Response: " + response);
        }
    }
}
