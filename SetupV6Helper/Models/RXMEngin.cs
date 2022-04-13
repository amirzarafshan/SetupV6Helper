using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SetupV6Helper
{
    public sealed class RXMEngin
    {
        public string CurrentVersion { get; set; }
        public string MD5Hash_CurrentVersion { get; set; }

        public static bool IsUrlExist(string url)
        {
            bool result = true;
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 1200;
            request.Method = "HEAD";

            try
            {
                request.GetResponse();
            }
            catch
            {
                result = false;
                Environment.Exit(0);
            }
            return result;
        } 
    }
}
