using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SetupV6Helper
{
    public sealed class RXMEngineStarter
    {

        public static void Start(string filenamepath, string server_endpoint)
        {         
            string version_folder = Path.GetDirectoryName(filenamepath);
            Directory.SetCurrentDirectory(version_folder);

            IsApplicationRunning(filenamepath, true);
            Console.Write("Starting Engine [{0}] ..... " , Path.GetDirectoryName(filenamepath));

            Process proc = new Process();       
            proc.StartInfo.FileName = filenamepath;
            proc.StartInfo.Arguments = server_endpoint;
            proc.StartInfo.UseShellExecute = true; 
            proc.StartInfo.Verb = "runas";
            proc.Start();
            proc.WaitForExit(6000);
            if (IsApplicationRunning(filenamepath))
            {
                Console.WriteLine("Started");
            }
            else
                Console.WriteLine("Failed to start");
        }

        public static bool IsApplicationRunning(string filenamepath,bool killprocess=false)
        {
            foreach (Process process in Process.GetProcesses())
                if (process.ProcessName.Equals(Path.GetFileNameWithoutExtension(filenamepath)))
                {
                    
                    if (killprocess == true)
                        process.Kill();
                    Thread.Sleep(7000);
                    return true;
                }
            return false;
        }

        public static bool IsScheduledTaskCreated(string taskname)
        {
            try
            {
                using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    if (ts.GetTask(taskname).Name != null)
                    {
                        return true;
                    }
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            return false;
        }
    }
}
