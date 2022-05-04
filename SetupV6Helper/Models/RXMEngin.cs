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

        public RXMEngin(string current_version, string md5hash_currentversion)
        {
            CurrentVersion = current_version;
            MD5Hash_CurrentVersion = md5hash_currentversion;
        }
    }
}
