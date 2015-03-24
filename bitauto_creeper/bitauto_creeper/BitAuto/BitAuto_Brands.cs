using System;
using System.Collections.Generic;
using System.Text;
using bitauto_creeper.Cars;
using System.Text.RegularExpressions;

namespace bitauto_creeper
{
    public class BitAuto_Brands
    {

        public static void GetCars4Brands(Brand brand)
        {
            string brand_page;
            MatchCollection mc_name = null, mc_link= null, mc_id = null;

            brand_page = Common.GetContentbyUrl(brand.GetURL());
            //brand_page = Common.GetContentbyUrl("http://car.bitauto.com/tree_chexing/mb_26/");
           
            if (!string.IsNullOrEmpty(brand_page))
            {
                string brand_page_divCsLevel_0 = Regex.Match(brand_page, "(id=\"divCsLevel_0\").+?(id=\"divCsLevel_1\")", RegexOptions.Singleline).Value;
                if (!string.IsNullOrEmpty(brand_page_divCsLevel_0))
                {
                    mc_name = Regex.Matches(brand_page_divCsLevel_0, "(?<=<li><a href=\"/.+/\" title=\").+(?=\" target=)");
                    mc_link = Regex.Matches(brand_page_divCsLevel_0, "(?<=<li><a href=\"/).+(?=\" title=)");
                    mc_id = Regex.Matches(brand_page_divCsLevel_0, "(?<=id=\"n)\\d+");

                    if (mc_name.Count > 0 && mc_name.Count == mc_link.Count && mc_link.Count == mc_id.Count)
                    {
                        Car car_temp;
                        for (int i = 0; i < mc_name.Count; i++)
                        {
                            car_temp = new Car(mc_name[i].Value, Common.url + mc_link[i].Value, mc_id[i].Value);
                            car_temp.manufacturerID = brand.id;

                            brand.AddCar2List(car_temp);
                            //brand.Cars.Add(new Car(mc_name[i].Value, Common.url + mc_link[i].Value));
                            Console.WriteLine("获取到品牌:" + mc_name[i].Value);
                        }
                    }
                }
            }

            
        }

    }
}
