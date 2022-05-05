using System;
using System.IO;

namespace SetupV6Helper.Models
{
    internal class Report
    {
        private static readonly string v4playerfolderpath64bit = @"c:\program files (x86)\PCM\V4Player\";
        private static readonly string v4playerfolderpath32bit = @"c:\program files\PCM\V4Player\";
        private static readonly string reportname = "report.pcmstat";

        public static void WriteLine(string status)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                if (!File.Exists(v4playerfolderpath64bit + reportname))
                {
                    File.CreateText(v4playerfolderpath64bit + reportname).Dispose();
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath64bit + reportname))
                    {
                        tw.WriteLine(status);
                    }
                }
                else
                {
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath64bit + reportname, true))
                    {
                        tw.WriteLine(status);
                    }
                }
            }
            else
            {
                if (!File.Exists(v4playerfolderpath32bit + reportname))
                {
                    File.CreateText(v4playerfolderpath64bit + reportname).Dispose();
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath32bit + reportname))
                    {
                        tw.WriteLine(status);
                    }
                }
                else
                {
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath32bit + reportname, true))
                    {
                        tw.WriteLine(status);
                    }
                }
            }
        }

        public static void Write(string status)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                if (!File.Exists(v4playerfolderpath64bit + reportname))
                {
                    File.CreateText(v4playerfolderpath64bit + reportname).Dispose();
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath64bit + reportname))
                    {
                        tw.Write(status);
                    }
                }
                else
                {
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath64bit + reportname,true))
                    {
                        tw.Write(status);
                    }
                }
            }
            else
            {
                if (!File.Exists(v4playerfolderpath32bit + reportname))
                {
                    File.CreateText(v4playerfolderpath64bit + reportname).Dispose();
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath32bit + reportname))
                    {
                        tw.Write(status);
                    }
                }
                else
                {
                    using (TextWriter tw = new StreamWriter(v4playerfolderpath32bit + reportname, true))
                    {
                        tw.Write(status);
                    }
                }
            }
        }
    }
}
