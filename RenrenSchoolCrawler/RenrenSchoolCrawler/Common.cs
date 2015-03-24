using System;
using System.IO;

namespace RenrenSchoolCrawler
{
    public static class Common
    {
        public static string CurDir = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
    }
}
