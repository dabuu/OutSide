using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using bitauto_creeper.Cars;

namespace bitauto_creeper
{
    public class BitAuto_AllCars
    {
        private readonly static string allcars_json = "http://api.car.bitauto.com/CarInfo/getlefttreejson.ashx?tagtype=chexing";

        private static List<Brand> allcarBrands;

        public static List<Brand> AllCarsNoLogo
        {
            get
            {
                Handle_AllCarsJson(false);
                return allcarBrands;
            }
        }

        public static List<Brand> AllCarsWithLogo
        {
            get
            {
                Handle_AllCarsJson(true);
                return allcarBrands;
            }
        }

        private static void Handle_AllCarsJson(bool islogodownload)
        {
            allcarBrands = new List<Brand>();
            string json_page = Common.GetContentbyUrl(allcars_json);

            if (!string.IsNullOrEmpty(json_page))
            {
                string temp_brandlist = Regex.Match(json_page, @"(?<=brand:\{).+?(?=\}\}\))").Value;
                if (string.IsNullOrEmpty(temp_brandlist))
                {
                    Console.WriteLine("Error: 获取 temp_brandlist 出错！返回！");
                    return;
                }
                MatchCollection brands_list = Regex.Matches(temp_brandlist, "([A-Z]{1}):.+?\\]");

                foreach (Match m_brand in brands_list)
                {
                    Brand brand;
                    MatchCollection cars_name = Regex.Matches(m_brand.Value, "(?<=name:\").+?(?=\")");
                    MatchCollection cars_path = Regex.Matches(m_brand.Value, "(?<=url:\"/).+?(?=\")");
                    string id_match;
                    if (cars_name.Count == cars_path.Count)
                    {
                        for (int i = 0; i < cars_name.Count; i++)
                        {
                            id_match = Regex.Match(cars_path[i].Value, @"(?<=mb_)\d+").Value;
                            if (!string.IsNullOrEmpty(id_match))
                            {
                                // 品牌的 名字， url， id
                                brand = new Brand(cars_name[i].Value, Common.url + cars_path[i].Value, id_match);
                                
                                // 品牌的 首字母
                                brand.firstLetter = m_brand.Groups[1].Value;

                                // 下载logo 图片
                                BitAuto_BrandLogo.GetLogoByName(ref brand, islogodownload);

                                /* 保斐利 && 世爵 */
                                //if (i == 20 || i == 117) 
                                //{
                                //    Console.WriteLine(brand.Name);
                                //}

                                allcarBrands.Add(brand);
                            }
                            else
                            {
                                Console.WriteLine(cars_name[i].Value + ": 获取id 为空！无法创建品牌！");
                                Console.WriteLine("按任意键继续。。。");
                                Console.ReadKey();
                            }
                        }
                    }
                }

            }
        }
    }
}
