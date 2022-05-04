using SetupV6Helper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace SetupV6Helper
{
    public class SetupAndStartV6Engin
    {
        private static readonly string versionsfolderpath64bit = @"c:\program files (x86)\PCM\V4Player\versions\";
        private static readonly string versionsfolderpath32bit = @"c:\program files\PCM\V4Player\versions\";
        private static readonly string versions_folder_path_local = "";
        private static readonly string rxm_engine_app = "RXMusic.Engine.App.exe";
        private static readonly string file_to_unzip = "dist.zip";
        private static WebClient webClient;

        public static void DownloadFile()
        {
            try
            {
                using (webClient = new WebClient())
                {

                    string current_version_url = V6EnginCurrentVersionURI();
                    Console.Write("File Downloading ..... ");
                    //webClient.DownloadFileCompleted += DownloadCompleted;
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                    webClient.DownloadFile(new Uri(current_version_url), ReadFileNamePathLocally("dist.zip"));
                    Console.WriteLine("Downloaded");
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
                Console.WriteLine("Created");

            else
                Console.WriteLine("Failed.");
        }

        private static void CreateLocalDirectoryToDownload()
        {
            string version_folder = V6EnginCurrentVersion();


            if (Environment.Is64BitOperatingSystem)
            {
                if (Directory.Exists(versionsfolderpath64bit + version_folder))
                {
                    Directory.Delete(versionsfolderpath64bit + version_folder, true);  
                }
                Directory.CreateDirectory(versionsfolderpath64bit + version_folder);
            }
            else
            {
                if (Directory.Exists(versionsfolderpath32bit + version_folder))
                {
                    Directory.Delete(versionsfolderpath32bit + version_folder, true);
                }
                Directory.CreateDirectory(versionsfolderpath32bit + version_folder);
            }
            Console.WriteLine("Directoy to download : " + ReadFileNamePathLocally(""));
        }

        private static string ReadFileNamePathLocally(string filename)
        {
            string version_folder = V6EnginCurrentVersion();
            if (Environment.Is64BitOperatingSystem)
            {
                return Path.GetFullPath(versionsfolderpath64bit + version_folder + "/" + filename);
            }
            else
            {
                return Path.GetFullPath(versionsfolderpath32bit + version_folder + "/" + filename);
            }
        }

        private static string V6EnginCurrentVersion()
        {
            return RXMEnginInfo.CurrentVersion;
        }

        private static string MD5hashRemoteDistFile(string version)
        {
            return RXMEnginInfo.MD5Hash_CurrentVersion;
        }

        private static string V6EnginCurrentVersionURI()
        {
            return RXMEnginInfo.ReadV6CurentVersionURL(V6EnginCurrentVersion());
        }

        private static bool IsFileDownloaded(string filename)
        {
            string local_downloaded_file = ReadFileNamePathLocally(filename);

            if (FileExtractor.CalculateMD5(local_downloaded_file).Equals(MD5hashRemoteDistFile(V6EnginCurrentVersion())))
            {
                return true;
            }
            Console.WriteLine("{0} was not downloaded!", filename);
            return false;
        }

        private static void ExtractFile(string filename)
        {
            if (IsFileDownloaded(filename))
                FileExtractor.ExtractZipfile(ReadFileNamePathLocally(filename), ReadFileNamePathLocally(""));
        }

        private static void StartV6Engine(string enginefilename)
        {
            RXMEngineStarter.Start(ReadFileNamePathLocally(enginefilename));
        }

        public static void Run()
        {
            CreateLocalDirectoryToDownload();
            DownloadFile();
            ExtractFile(file_to_unzip);
            StartV6Engine(rxm_engine_app);
            VerifyRXMTasks();
        }
    }
}
