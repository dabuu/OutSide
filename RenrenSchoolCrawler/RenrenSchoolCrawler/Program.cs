using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft;

namespace RenrenSchoolCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawl = new Crawler();
            crawl.Crawl(SchoolType.HIGHSCHOOL);
            string highschool = Newtonsoft.Json.JsonConvert.SerializeObject(crawl.provsList);
            string highschoolLog = Path.Combine(Common.CurDir, "highschool_"+ DateTime.Now.ToString("yyyy-MM-dd_HHmmss")+".txt");
            WriteFile(highschoolLog, highschool);

            crawl.Crawl(SchoolType.JUNIORSCHOOL);
            string juniorschool = Newtonsoft.Json.JsonConvert.SerializeObject(crawl.provsList);
            string juniorschoolLog = Path.Combine(Common.CurDir, "juniorschool_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt");
            WriteFile(juniorschoolLog, juniorschool);
        }

        static void WriteFile(string fileName, string content)
        {
            using ( StreamWriter sw = new StreamWriter(fileName,false, Encoding.UTF8))
            {
                sw.Write(content);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
