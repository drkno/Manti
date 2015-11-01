using System;
using CoreLoader.Core;

namespace CoreLoader
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var platform = new Platform();
            platform.Start();
            Console.ReadKey(true);
            platform.Dispose();
            Console.ReadKey(true);
        }
    }
}
