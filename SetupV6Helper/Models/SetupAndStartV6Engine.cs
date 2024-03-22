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

        private static readonly string v6playerpath64bit = @"c:\program files (x86)\RXM\Player\";
        private static readonly string v6playerpath32bit = @"c:\program files\RXM\Player\";

        private static readonly string downloadsfolder = @"downloads";
        private static readonly string binfolder = @"bin\";

        private static readonly string rxm_engine_app = "RXMusic.Helper.App.exe";
        private static WebClient webClient; 

        public static void DownloadFile(string build_number="")
        {
            try
            {
                using (webClient = new WebClient())
                {

                    string current_version_url = V6EnginCurrentVersionURI();
                    string version_folder = V6EnginCurrentVersion();

                    Console.Write("File Downloading ..... ");
                    //webClient.DownloadFileCompleted += DownloadCompleted;
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                    if (VersionNotExist(version_folder + ".zip")) 
                    {
                        webClient.DownloadFile(new Uri(current_version_url), ReadFileNamePathLocally(version_folder + ".zip"));
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
                Console.WriteLine("Failed to download the file!");
                Console.WriteLine(e);
            }

        }

        public static void DownloadFilePCM(string build_number = "")
        {
            try
            {
                using (webClient = new WebClient())
                {

                    string current_version_url = V6EnginCurrentVersionURI();
                    string version_folder = V6EnginCurrentVersion();

                    Console.Write("File Downloading ..... ");
                    //webClient.DownloadFileCompleted += DownloadCompleted;
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                    if (VersionNotExistPCM(version_folder + ".zip"))
                    {
                        webClient.DownloadFile(new Uri(current_version_url), ReadFileNamePathLocallyPCM(version_folder + ".zip"));
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
                Console.WriteLine("Failed to download the file!");
                Console.WriteLine(e);
            }

        }

        private static void VerifyRXMTasks()
        {
            Console.Write("Setting up All RXM Helper scheduled tasks ..... ");
            Thread.Sleep(10000);
            if (TaskSchedulerInfo.ScheduledTaskCreated())
            {
                Console.WriteLine("Created");
            }
            else
            {
                Console.WriteLine("Failed.");
            }
        }

        private static void CreateLocalDirectoryToDownload()
        {

            string version_folder = V6EnginCurrentVersion();
            Console.WriteLine("V6Helper version to download : {0}", version_folder);

            if (Environment.Is64BitOperatingSystem)
            {
                if (!Directory.Exists(v6playerpath64bit + downloadsfolder))
                { 
                    Directory.CreateDirectory(v6playerpath64bit + downloadsfolder);
                }
            }
            else
            {
                if (!Directory.Exists(v6playerpath32bit + downloadsfolder ))
                {
                    Directory.CreateDirectory(v6playerpath32bit + downloadsfolder);
                }
      
            }
            Console.WriteLine("Directoy to download : " + ReadFileNamePathLocally(""));
        }

        private static void CreateLocalDirectoryToDownloadPCM()
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
                if (!Directory.Exists(v4playerpath32bit + downloadsfolder))
                {
                    Directory.CreateDirectory(v4playerpath32bit + downloadsfolder);
                }

            }
            Console.WriteLine("Directoy to download : " + ReadFileNamePathLocallyPCM(""));
        }

        private static bool VersionNotExist(string filename)
        {
            if (File.Exists(v6playerpath64bit + downloadsfolder + "/" + filename))
            {
                return false;
            }
            return true;
        }

        private static bool VersionNotExistPCM(string filename)
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
                 return Path.GetFullPath(v6playerpath64bit + downloadsfolder + "/" + filename);
            }

            else
            {
                 return Path.GetFullPath(v6playerpath32bit + downloadsfolder + "/" + filename);
            }
        }

        private static string ReadFileNamePathLocallyPCM(string filename)
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
                return Path.GetFullPath(v6playerpath64bit + binfolder + version_folder + "/" + filename);
            }
            else
            {
                return Path.GetFullPath(v6playerpath32bit + binfolder + version_folder + "/" + filename);
            }
        }

        private static string ReadPathToExtractPCM(string filename)
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

            Console.WriteLine("{0} was not downloaded!", filename);
            return false;
        }

        private static bool IsFileDownloadedPCM(string filename)
        {
            string local_downloaded_file = ReadFileNamePathLocallyPCM(filename);

            if (FileExtractor.CalculateMD5(local_downloaded_file).Equals(MD5hashRemoteDistFile()))
            {
                return true;
            }

            Console.WriteLine("{0} was not downloaded!", filename);
            return false;
        }

        private static void ExtractFile(string build_number="")
        {
            string version_folder = V6EnginCurrentVersion();
            if (IsFileDownloaded(version_folder + ".zip"))
            {
                if (!Directory.Exists(v6playerpath64bit + binfolder + version_folder))
                {
                    FileExtractor.ExtractZipfile(ReadFileNamePathLocally(version_folder + ".zip"), ReadPathToExtract(""));
                }
                else
                {
                    Console.WriteLine("File Unzipping ..... V6Helper version {0} already extracted.", version_folder);
                }
            }
        }

        private static void ExtractFilePCM(string build_number = "")
        {
            string version_folder = V6EnginCurrentVersion();
            if (IsFileDownloadedPCM(version_folder + ".zip"))
            {
                if (!Directory.Exists(v4playerpath64bit + binfolder + version_folder))
                {
                    FileExtractor.ExtractZipfile(ReadFileNamePathLocallyPCM(version_folder + ".zip"), ReadPathToExtractPCM(""));
                }
                else
                {
                    Console.WriteLine("File Unzipping ..... V6Helper version {0} already extracted.", version_folder);
                }
            }
        }

        private static void StartV6Engine(string enginefilename, string server_endpoint)
        {
            RXMEngineStarter.Start(ReadPathToExtract(enginefilename), server_endpoint);
        }

        private static void StartV6EnginePCM(string enginefilename, string server_endpoint)
        {
            RXMEngineStarter.Start(ReadPathToExtractPCM(enginefilename), server_endpoint);
        }

        public static void Run(string server_endpoint)
        {
            CreateLocalDirectoryToDownload();
            DownloadFile();
            ExtractFile();
            StartV6Engine(rxm_engine_app, server_endpoint);
            VerifyRXMTasks();
        }

        public static void RunPCM(string server_endpoint)
        {
            CreateLocalDirectoryToDownloadPCM();
            DownloadFilePCM();
            ExtractFilePCM();
            StartV6EnginePCM(rxm_engine_app, server_endpoint);
            VerifyRXMTasks();
        }

    }
}
