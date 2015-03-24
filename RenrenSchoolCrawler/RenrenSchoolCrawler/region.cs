using System;
using System.Collections.Generic;
using System.Text;

namespace RenrenSchoolCrawler
{
    public class Region
    {
        public string name;
        public int rid;
        public List<string> schList;
    }

    public enum SchoolType
    {
        JUNIORSCHOOL, //http://support.renren.com/juniorschool/1101.html
        HIGHSCHOOL //http://support.renren.com/highschool/3502.html
    }
}
