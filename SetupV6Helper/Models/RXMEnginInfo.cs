using System;
using System.IO;
using System.Linq;
using System.Net;

namespace SetupV6Helper
{
    public sealed class RXMEnginInfo
    {
        private static readonly string playerpathfolder = "https://web.pcmedia.ca/apps/player/";
        private static readonly string v6_macro_version = "https://web.pcmedia.ca/apps/player/v6_macro_version.txt";


        public static string CurrentVersion { get; set; }
        public static string MD5Hash_CurrentVersion { get; set; }

        public static string ReadV6MacroVersion()
        {
            try
            {
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(v6_macro_version);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd().Trim();
            }
            catch (WebException)
            {
                Console.WriteLine("URL not found: " + v6_macro_version);
                Environment.Exit(0);
                return null;
            }
        }

        public static string ReadV6CurentVersionURL(string version)
        {
            if (ReadV6MacroVersion() != null)
            {
                string fullpath = playerpathfolder + version + "/dist.zip";
                WebRequest request = WebRequest.Create(fullpath);
                request.Timeout = 1200;
                request.Method = "HEAD";

                try
                {
                    request.GetResponse();
                }
                catch
                {
                    Console.WriteLine("URL path not found: " + fullpath);
                    Environment.Exit(0);
                }
                return fullpath;
            }
            return null;
        }

        public static string ReadMD5hashRemoteDistFile(string version)
        {
            string dist_file_md5_path = playerpathfolder + version + "/dist.zip.md5";
            try
            {
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(dist_file_md5_path);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd().Split(' ').ToList().Select(r => r.Trim()).FirstOrDefault().ToString();
            }
            catch (WebException)
            {
                Console.WriteLine("URL not found: " + dist_file_md5_path);
                Environment.Exit(0);
                return null;
            }       
        }

        public static RXMEngin GetInfo()
        {
            return new RXMEngin
            (
                CurrentVersion = ReadV6MacroVersion(),
                MD5Hash_CurrentVersion = ReadMD5hashRemoteDistFile(CurrentVersion)
            );
        }
    }
}
