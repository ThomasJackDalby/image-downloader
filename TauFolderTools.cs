using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownloaderConsole
{
    public static class FolderTools
    {
        public static string GetFileName(string url)
        {
            int lastindex = url.LastIndexOf('/');
            return url.Substring(lastindex);
        }

        public static string GetDirectoryName(string url)
        {
            StringBuilder directoryname = new StringBuilder();
            directoryname.Append(url);
            for (int index = 0; index < url.Length; index++)
            {
                if (string.Equals(url[index], ':') || string.Equals(url[index], '.'))
                {
                    directoryname[index] = '_';
                }
            }
            return directoryname.ToString();
        }

    }
}
