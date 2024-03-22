using System;

namespace SetupV6Helper
{
    internal class Program
    {
        private static readonly string server_endpoint = @"--RXM_ENDPOINT_URI=https://deva1.rxmusic.com/api/app/endpoints";
        public static string v6_version;

        //public const string v6_macro_version = "https://web.rxmusic.com/apps/player/v6_macro_version.txt";
        public const string v6_macro_version = "https://web.rxmusic.com/apps/player/current.txt";
        //public static string build_number;
        //public static readonly string v6_version = "22.8.31.911";


        public static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine("Installing V6Helper: {0}", dt);
            Console.WriteLine("==========================================");

            if (args.Length > 0 && args[0].Equals("-deva1"))
            {
                Console.WriteLine("Connecting to deva1");
                try
                {
                    if (args[1].Length > 0)
                    {
                        if (args[1].StartsWith("--v="))
                        {
                            v6_version = args[1].Remove(0, 4);
                            SetupAndStartV6Engine.Run(server_endpoint);
                        }
                        else
                        {
                            Console.WriteLine("version number is incorrect [--v=xx.xx.xx.xxx]");
                        }
                    }
                }

                catch
                {
                    v6_version = v6_macro_version;
                    SetupAndStartV6Engine.Run(server_endpoint);
                }

            }

            else if (args.Length > 0 && args[0].Equals("-a1"))
            {
                Console.WriteLine("Connecting to a1");
                try
                {
                    if (args[1].Length > 0)
                    {
                        if (args[1].StartsWith("--v=") && args[2].StartsWith("-path=PCM"))
                        {
                            v6_version = args[1].Remove(0, 4);
                            SetupAndStartV6Engine.RunPCM("");
                        }

                        else if (args[1].StartsWith("--v="))
                        {
                            v6_version = args[1].Remove(0, 4);
                            SetupAndStartV6Engine.Run("");
                        }

                        else if (args[1].StartsWith("-path=PCM"))
                        {
                            v6_version = v6_macro_version;
                            SetupAndStartV6Engine.RunPCM("");
                        }

                        else
                        {
                            Console.WriteLine("version number is incorrect [--v=xx.xx.xx.xxx]");
                        }
                    }
                }

                catch
                {
                    v6_version = v6_macro_version;
                    SetupAndStartV6Engine.Run("");
                }

            }

            else if (args.Length > 0 && args[0].StartsWith("--v="))
            {
                Console.WriteLine("Connecting to a1");
                v6_version = args[0].Remove(0, 4);
                SetupAndStartV6Engine.Run("");
            }

            else if (args.Length==0)
            {
                Console.WriteLine("Connecting to a1");
                v6_version = v6_macro_version;
                SetupAndStartV6Engine.Run("");
            }
       
            else
            {
                Console.WriteLine("Incorrect parameters");
                Environment.Exit(0);
            }
        }       
    }
}
