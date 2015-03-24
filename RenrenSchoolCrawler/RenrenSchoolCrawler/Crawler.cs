using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace RenrenSchoolCrawler
{
    public class Crawler
    {
        public List<Prov> provsList;
        private string cityArrayPage;
        private Dictionary<int, int> dgcityMap = new Dictionary<int, int> { { 1, 1101 }, { 2, 3101 }, { 3, 1201 }, { 4, 5001 }, { 33, 8101 } }; //directly governed city region
        private string crawlURLTemplate;// "http://support.renren.com/{0}/{1}.html";

        public void Crawl(SchoolType st)
        {
            this.crawlURLTemplate = string.Format("http://support.renren.com/{0}/{1}.html", st.ToString().ToLower(), "{0}");

            GetProvList();
            if (this.provsList.Count > 0)
            {
                GetCityArrayPageInfo();
                foreach (Prov prov in this.provsList)
                {
                    GetCityList(prov);
                }
            }

        }

        private void GetProvList()
        {
            string[] provsArray = File.ReadAllLines(Path.Combine(Common.CurDir, "provList.txt"));

            string[] infoTempArray;
            this.provsList = new List<Prov>();
            foreach (string provInfo in provsArray)
            {
                infoTempArray = provInfo.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (infoTempArray.Length == 2)
                {
                    provsList.Add(new Prov { pid = Convert.ToInt32(infoTempArray[0]), name = infoTempArray[1], cList = new List<City>() });
                }
            }
        }

        private void GetCityList(Prov prov)
        {
            List<Region> regionList;
            if (dgcityMap.ContainsKey(prov.pid))
            {
                regionList = GetRegionList(dgcityMap[prov.pid]);
                prov.cList.Add(new City { cid = dgcityMap[prov.pid], name = prov.name, rList = regionList});
            }
            else
            {
                string tempCityArray = Regex.Match(cityArrayPage, string.Format("_city_{0}.+?];", prov.pid)).Value;

                if (!string.IsNullOrEmpty(tempCityArray))
                {
                    string[] cityInfoTempArray;
                    int cityID;
                    foreach (Match cityMatch in Regex.Matches(tempCityArray, "(?<=\")\\d+:.+?(?=\")"))
                    {
                        cityInfoTempArray = cityMatch.Value.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            cityID = Convert.ToInt32(cityInfoTempArray[0]);
                        }
                        catch
                        {
                            Console.WriteLine("Error: cityMatch's value is " + cityMatch.Value);
                            continue;
                        }
                        regionList = GetRegionList(cityID);
                        prov.cList.Add(new City { cid = cityID, name = cityInfoTempArray[1], rList = regionList});
                    }
                }
            }
        }

        private List<Region> GetRegionList(int cityID)
        {
            List<Region> regionTempList = new List<Region>();

            string url = string.Format(this.crawlURLTemplate, cityID);
            string pageContent = GetCityDetailsInfo(url);

            Regex regionContent = new Regex("schoolCityQuList.+?</ul>");
            string tempRegion = regionContent.Match(pageContent).Value;

            Regex regionInfo = new Regex("qu_(\\d+)'\\)\">(.+?)</a>");
            Region region = new Region();
            List<string> schoolList;

            foreach (Match regionMatch in regionInfo.Matches(tempRegion))
            {
                schoolList = new List<string>();
                int regionID = Convert.ToInt32(regionMatch.Groups[1].Value);

                string schoolsRegionTempContent = Regex.Match(pageContent, "qu_" + regionID + "\".+?</ul>", RegexOptions.Singleline).Value;
                schoolList = new List<string>();
                foreach (Match school in Regex.Matches(schoolsRegionTempContent, "(?<=(\">)).+?(?=(</a>))"))
                {
                    schoolList.Add(school.Value);
                }

                regionTempList.Add(new Region
                {
                    rid = regionID,//id;
                    name = regionMatch.Groups[2].Value, //name;
                    schList = schoolList
                });
            }

            return regionTempList;
        }

        private void GetCityArrayPageInfo()
        {
            

            cityArrayPage = GetPageContent("http://s.xnimg.cn/js/cityArray.js" );
        }

        private string GetCityDetailsInfo(string url)
        {
            return System.Web.HttpUtility.HtmlDecode(GetPageContent(url));
        }

        private string GetPageContent(string url)
        {
            Random r = new Random();
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.GetEncoding("UTF-8");
                return wc.DownloadString(url+"?"+ r.Next());
            }
        }


    }
}
