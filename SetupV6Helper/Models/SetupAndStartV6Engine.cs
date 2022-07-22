using SetupV6Helper.Models;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace SetupV6Helper
{
    public class SetupAndStartV6Engine
    {

        private static readonly string v4playerpath64bit = @"c:\program files (x86)\PCM\V4Player\";
        private static readonly string v4playerpath32bit = @"c:\program files\PCM\V4Player\";
        private static readonly string downloadsfolder = @"downloads";
        private static readonly string binfolder = @"bin\";

        private static readonly string rxm_engine_app = "RXMusic.Helper.App.exe";
        private static WebClient webClient; 

        public static void DownloadFile()
        {
            try
            {
                using (webClient = new WebClient())
                {

                    string current_version_url = V6EnginCurrentVersionURI();
                    string version_folder = V6EnginCurrentVersion();

                    //Report.Write("File Downloading ..... ");
                    Console.Write("File Downloading ..... ");
                    //webClient.DownloadFileCompleted += DownloadCompleted;
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                    if (VersionNotExist(version_folder + ".zip")) 
                    {
                        webClient.DownloadFile(new Uri(current_version_url), ReadFileNamePathLocally(version_folder + ".zip"));
                        Report.WriteLine("Downloaded");
                        Console.WriteLine("Downloaded");

                    }
                    else
                    {
                        Console.WriteLine("V6Helper version {0} already downloaded.", V6EnginCurrentVersion());                 
                    }
                }

            }
            catch (Exception e)
            {
                //Report.WriteLine("Failed to download the file!");
                //Report.WriteLine(e.ToString());
                Console.WriteLine("Failed to download the file!");
                Console.WriteLine(e);
            }

        }

        private static void VerifyRXMTasks()
        {
            //Report.Write("Setting up All RXM Helper scheduled tasks ..... ");
            Console.Write("Setting up All RXM Helper scheduled tasks ..... ");
            Thread.Sleep(10000);
            if (TaskSchedulerInfo.ScheduledTaskCreated())
            {
                //Report.WriteLine("Created");
                Console.WriteLine("Created");
            }
            else
            {
                //Report.WriteLine("Failed");
                Console.WriteLine("Failed.");
            }
        }

        private static void CreateLocalDirectoryToDownload()
        {
            string version_folder = V6EnginCurrentVersion();
            Console.WriteLine("V6Helper version to download : {0}", version_folder);

            if (Environment.Is64BitOperatingSystem)
            {
                if (!Directory.Exists(v4playerpath64bit + downloadsfolder))
                { 
                    Directory.CreateDirectory(v4playerpath64bit + downloadsfolder);
                }
            }
            else
            {
                if (!Directory.Exists(v4playerpath32bit + downloadsfolder ))
                {
                    Directory.CreateDirectory(v4playerpath32bit + downloadsfolder);
                }
      
            }

            //Report.WriteLine("Directoy to download : " + ReadFileNamePathLocally(""));
            Console.WriteLine("Directoy to download : " + ReadFileNamePathLocally(""));
        }

        private static bool VersionNotExist(string filename)
        {
            if (File.Exists(v4playerpath64bit + downloadsfolder + "/" + filename))
            {
                return false;
            }
            return true;
        }

        private static string ReadFileNamePathLocally(string filename)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                 return Path.GetFullPath(v4playerpath64bit + downloadsfolder + "/" + filename);
            }

            else
            {
                 return Path.GetFullPath(v4playerpath32bit + downloadsfolder + "/" + filename);
            }
        }

        private static string ReadPathToExtract(string filename)
        {
            string version_folder = V6EnginCurrentVersion();
            if (Environment.Is64BitOperatingSystem)
            {
                return Path.GetFullPath(v4playerpath64bit + binfolder + version_folder + "/" + filename);
            }
            else
            {
                return Path.GetFullPath(v4playerpath32bit + binfolder + version_folder + "/" + filename);
            }
        }


        private static string V6EnginCurrentVersion()
        {
            return RXMEnginInfo.GetInfo().CurrentVersion;
        }

        private static string MD5hashRemoteDistFile()
        {
            return RXMEnginInfo.GetInfo().MD5Hash_CurrentVersion;
        }

        private static string V6EnginCurrentVersionURI()
        {
            return RXMEnginInfo.ReadV6CurentVersionURL(V6EnginCurrentVersion());
        }

        private static bool IsFileDownloaded(string filename)
        {
            string local_downloaded_file = ReadFileNamePathLocally(filename);

            if (FileExtractor.CalculateMD5(local_downloaded_file).Equals(MD5hashRemoteDistFile()))
            {
                return true;
            }

            //Report.WriteLine(filename + " was not downloaded!");
            Console.WriteLine("{0} was not downloaded!", filename);
            return false;
        }

        private static void ExtractFile()
        {
            string version_folder= V6EnginCurrentVersion();
            if (IsFileDownloaded(version_folder + ".zip"))
            {
                if (!Directory.Exists(v4playerpath64bit + binfolder + version_folder))
                {
                    FileExtractor.ExtractZipfile(ReadFileNamePathLocally(version_folder + ".zip"), ReadPathToExtract(""));
                }
                else
                {
                    Console.WriteLine("File Unzipping ..... V6Helper version {0} already extracted.", V6EnginCurrentVersion());
                }
            }
        }

        private static void StartV6Engine(string enginefilename, string server_endpoint)
        {
            RXMEngineStarter.Start(ReadPathToExtract(enginefilename), server_endpoint);
        }

        public static void Run(string server_endpoint)
        {
            CreateLocalDirectoryToDownload();
            DownloadFile();
            ExtractFile();
            StartV6Engine(rxm_engine_app, server_endpoint);
            VerifyRXMTasks();
        }
    }
}
