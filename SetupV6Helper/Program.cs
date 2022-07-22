using System;

namespace SetupV6Helper
{
    internal class Program
    {
        private static readonly string server_endpoint = @"--RXM_ENDPOINT_URI=https://deva1.rxmusic.com/api/app/endpoints";

        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("-deva1"))
            {
                Console.WriteLine("Connecting to deva1");
                SetupAndStartV6Engine.Run(server_endpoint);
            }
            else if (args.Length > 0 && !args[0].Equals("-deva1"))
            {
                Console.WriteLine("Enter correct value");
                Environment.Exit(0);
            }

            else if (args.Length == 0)
            {
                Console.WriteLine("Connecting to a1");
                SetupAndStartV6Engine.Run("");
            }

            else
            {
                Environment.Exit(0);
            }
        }       
    }
}
