using SetupV6Helper.Models;
using System;
using System.IO;
using System.IO.Compression;

namespace SetupV6Helper
{
    public sealed class FileExtractor
    {
        public static void ExtractZipfile(string source, string destination)
        {
            Report.Write("File Unzipping ..... ");
            Console.Write("File Unzipping ..... ");
            ZipFile.ExtractToDirectory(source, destination);
            if (!IsFileLocked("dist.zip"))
            {
                Report.WriteLine("Unzipped");
                Console.WriteLine("Unzipped");
            }
        }

        public static string CalculateMD5(string filenamepath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filenamepath))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private static bool IsFileLocked(string filePath)
        {
            FileStream stream;
            try
            {
                using (stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
