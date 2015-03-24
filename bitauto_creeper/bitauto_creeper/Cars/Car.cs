using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace bitauto_creeper.Cars
{
    public class Car
    {
        public int id, manufacturerID;
        public string name, brandName, modelYears;//brandId, 
        public int brandId;

        private List<Version> Versions;
        private string Url, Peizhi_URL;

        public Car(string name, string url, string id)
        {
            this.id = int.Parse(id);
            this.name = name;
            this.Url = url;
            this.Peizhi_URL = url + "/peizhi/";
            this.brandId = 0;
            this.brandName = this.modelYears = "";
            this.Versions = new List<Version>();

            GetModelYears();
        }

        public string GetURL()
        {
            return this.Url;
        }
        public string GetPeizhiURL()
        {
            return this.Peizhi_URL;
        }
        public void AddVersion(Version v)
        {
            this.Versions.Add(v);
        }
        public List<Version> GetVersions()
        {
            return this.Versions;
        }

        private void GetModelYears()
        {
            string temp_page = Common.GetContentbyUrl(this.Peizhi_URL);
            if (!string.IsNullOrEmpty(temp_page))
            {
                string temp_modelyears = Regex.Match(temp_page, "(?<=class=\"class\").+?(?=</div>)", RegexOptions.Singleline).Value;
                if (!string.IsNullOrEmpty(temp_modelyears))
                {
                    MatchCollection mc_years = Regex.Matches(temp_modelyears, "[12]{1}\\d{3}(?=款)");
                    if (mc_years.Count > 0)
                    {
                        this.modelYears = mc_years[0].Value;
                        for (int i = 1; i < mc_years.Count; i++)
                        {
                            this.modelYears += "," + mc_years[i].Value;
                        }
                    }
                }
            }
        }
        public void AddModelYears(string year)
        {
            this.modelYears += "," + year;
        }
    }
}
