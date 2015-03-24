using System;
using System.Collections.Generic;
using System.Text;
using bitauto_creeper.Cars;
using System.Text.RegularExpressions;
namespace bitauto_creeper
{
    public class BitAuto_Peizhi
    {
        private static string car_page;
        //public static string extra_years;
        public static void GetVersion4Car(Car car)
        {
            car_page = Common.GetContentbyUrl(car.GetPeizhiURL());
            //car_page = Common.GetContentbyUrl("http://car.bitauto.com/feiyatecmedium/peizhi/");
            //extra_years = "";

            GetVersions4CarOnSale(car);
            GetVersion4CarOutSale(car);
            Console.WriteLine(car.name + ":品牌下面所有车型获取完成！");
        }

        private static Cars.Version GetCarVersionByContent(string content, string parent_car_url, ref Car car)
        {
            MatchCollection mc_carinfo_groups = Regex.Matches(content, "\\[.+?\\]");
            if (mc_carinfo_groups.Count > 0)
            {
                Cars.Version version;
                // group 0
                MatchCollection mc_temp = Regex.Matches(mc_carinfo_groups[0].Value, "(?<=[\\[,]\").*?(?=\"[,\\]])");
                if (mc_temp.Count > 8)
                {
                    version = new Cars.Version(mc_temp[1].Value, //string.Format("{0} {1}", mc_temp[7].Value, mc_temp[1].Value),
                                                                    parent_car_url,
                                                                    mc_temp[0].Value, car.id);
                    version.modelYear = mc_temp[7].Value; //年款

                    if (!string.IsNullOrEmpty(car.modelYears) && car.modelYears.IndexOf(version.modelYear) < 0)//&& extra_years.IndexOf(version.modelYear) < 0)
                    {
                        car.AddModelYears(version.modelYear);
                        //extra_years += "," + version.modelYear;
                    }
                }
                else
                {
                    return null;
                }

                // group 1 
                mc_temp = Regex.Matches(mc_carinfo_groups[1].Value, "(?<=[\\[,]\").*?(?=\"[,\\]])");
                if (mc_temp.Count > 8)
                {
                    try
                    {
                        version.engineDisplacement = double.Parse(mc_temp[5].Value); // 排量
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(parent_car_url + " -- Error: engineDisplacement value exchage :" + ex.Message + "value is : " + mc_temp[5].Value);
                        Console.WriteLine("按任意键继续。。。");
                        //Console.ReadKey();
                        return null;
                    }
                    version.transmissionType = mc_temp[7].Value; // 手、自动模式 <- todo
                }
                else
                {
                    return null;
                }

                // group 3 
                mc_temp = Regex.Matches(mc_carinfo_groups[3].Value, "(?<=[\\[,]\").*?(?=\"[,\\]])");
                if (mc_temp.Count > 21)
                {
                    if (mc_temp[5].Value == "无")
                    {
                        version.finletWay = mc_temp[4].Value; // 增压方式    
                    }
                    else
                    {
                        version.finletWay = mc_temp[5].Value; // 增压方式    
                    }
                    if (mc_temp[20].Value == "")
                    {
                        version.fuelLabel = mc_temp[19].Value; // 油号
                    }
                    else
                    {
                        version.fuelLabel = mc_temp[20].Value; // 油号
                    }
                }
                else
                {
                    return null;
                }

                //mc_temp = Regex.Matches(content, "(?<=[\\[,]\").*?(?=\"[,\\]])");
                //Cars.Version version = new Cars.Version(mc_temp[1].Value, //string.Format("{0} {1}", mc_temp[7].Value, mc_temp[1].Value),
                //                                                    parent_car_url,
                //                                                    mc_temp[0].Value, car_id);

                //try
                //{
                //    version.engineDisplacement = double.Parse(mc_temp[19].Value); // 排量
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(parent_car_url + " -- Error: engineDisplacement value exchage :" + ex.Message + "value is : " + mc_temp[19].Value);
                //    Console.WriteLine("按任意键继续。。。");
                //    Console.ReadKey();
                //    return null;
                //}

                //version.modelYear = mc_temp[7].Value;
                //version.fuelLabel = mc_temp[76].Value; // 油号
                //version.transmissionType = mc_temp[21].Value; // 手、自动模式 <- todo
                //version.finletWay = mc_temp[61].Value; // 增压方式

                //version.Pailiang = mc_temp[19].Value;
                //version.Youxiang = mc_temp[78].Value;
                //version.Youhao_ZH = mc_temp[24].Value;
                //version.Youhao_SQ = mc_temp[22].Value;
                //version.Youhao_SJ = mc_temp[23].Value;
                //version.Ranyou = mc_temp[76].Value;
                ////version.Origin_Args = Regex.Replace(serial.Value, @"[\[\]]", "");

                return version;
            }
            return null;
        }

        /// <summary>
        /// 为 sallyxjchen 抓取数据
        /// ========================
        /// 启停系统	group 3
        /// 燃油箱容积(L)	group 3
        /// 变速箱	group 4
        /// 胎压监测装置 group 6
        /// 零压续行(零胎压继续行驶) group 6
        /// 发动机电子防盗	group 6
        /// </summary>
        /// <param name="content"></param>
        /// <param name="parent_car_url"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        //private static Cars.Version GetCarVersionByContent4sallyxjchen(string content, string parent_car_url, ref Car car)
        //{
        //    MatchCollection mc_carinfo_groups = Regex.Matches(content, "\\[.+?\\]");
        //    Regex reg_carinfo_value = new Regex("(?<=[\\[,]\").*?(?=\"[,\\]])");

        //    if (mc_carinfo_groups.Count > 0)
        //    {
        //        Cars.Version version;
        //        // group 0 && 初始化  具体的车型 Version
        //        MatchCollection mc_temp = reg_carinfo_value.Matches(mc_carinfo_groups[0].Value);
        //        if (mc_temp.Count > 8)
        //        {
        //            version = new Cars.Version(mc_temp[1].Value, //string.Format("{0} {1}", mc_temp[7].Value, mc_temp[1].Value),
        //                                                            parent_car_url,
        //                                                            mc_temp[0].Value, car.id);
        //            version.modelYear = mc_temp[7].Value; //年款

        //            if (!string.IsNullOrEmpty(car.modelYears) && car.modelYears.IndexOf(version.modelYear) < 0)
        //            {
        //                car.AddModelYears(version.modelYear);
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }


        //        // group 3
        //        mc_temp = reg_carinfo_value.Matches(mc_carinfo_groups[3].Value);
        //        if (mc_temp.Count > 27)
        //        {
        //            version.qitingxitong = mc_temp[26].Value; //启停系统	
        //            version.ranyouxiang_rongji = mc_temp[22].Value; // 燃油箱容积(L) 

        //        }
        //        else
        //        {
        //            return null;
        //        }

        //        // group 4
        //        mc_temp = reg_carinfo_value.Matches(mc_carinfo_groups[4].Value);
        //        if (mc_temp.Count > 2)
        //        {
        //            version.biansuxiang = (mc_temp[0].Value == "手动") ? string.Format("{0}档{1}", mc_temp[1].Value, mc_temp[0].Value) : mc_temp[0].Value;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //        // group 6
        //        mc_temp = reg_carinfo_value.Matches(mc_carinfo_groups[6].Value);
        //        if (mc_temp.Count > 25)
        //        {
        //            version.taiya_jiance_zhuangzhi = mc_temp[15].Value;
        //            version.lingyaxuhang = mc_temp[16].Value;
        //            version.fadongji_dianzifangdao = mc_temp[25].Value;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //        //// group 1 
        //        //mc_temp = Regex.Matches(mc_carinfo_groups[1].Value, "(?<=[\\[,]\").*?(?=\"[,\\]])");
        //        //if (mc_temp.Count > 8)
        //        //{
        //        //    try
        //        //    {
        //        //        version.engineDisplacement = double.Parse(mc_temp[5].Value); // 排量
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        Console.WriteLine(parent_car_url + " -- Error: engineDisplacement value exchage :" + ex.Message + "value is : " + mc_temp[5].Value);
        //        //        Console.WriteLine("按任意键继续。。。");
        //        //        //Console.ReadKey();
        //        //        return null;
        //        //    }
        //        //    version.transmissionType = mc_temp[7].Value; // 手、自动模式 <- todo
        //        //}
        //        //else
        //        //{
        //        //    return null;
        //        //}

        //        //// group 3 
        //        //mc_temp = Regex.Matches(mc_carinfo_groups[3].Value, "(?<=[\\[,]\").*?(?=\"[,\\]])");
        //        //if (mc_temp.Count > 21)
        //        //{
        //        //    if (mc_temp[5].Value == "无")
        //        //    {
        //        //        version.finletWay = mc_temp[4].Value; // 增压方式    
        //        //    }
        //        //    else
        //        //    {
        //        //        version.finletWay = mc_temp[5].Value; // 增压方式    
        //        //    }
        //        //    if (mc_temp[20].Value == "")
        //        //    {
        //        //        version.fuelLabel = mc_temp[19].Value; // 油号
        //        //    }
        //        //    else
        //        //    {
        //        //        version.fuelLabel = mc_temp[20].Value; // 油号
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    return null;
        //        //}


        //        return version;
        //    }
        //    return null;
        //}

        private static void GetVersions4CarOnSale(Car car)
        {
            if (!string.IsNullOrEmpty(car_page))
            {
                MatchCollection mc_serials;

                string serials_json = Regex.Match(car_page, "(?<=carCompareJson = ).+?(?=</script>)", RegexOptions.Singleline).Value;

                if (!string.IsNullOrEmpty(serials_json))
                {
                    mc_serials = Regex.Matches(serials_json, @"\[\[.+?\]\](?=[,\]])");
                    //Cars.Version version;
                    foreach (Match serial in mc_serials)
                    {
                        Cars.Version v = GetCarVersionByContent(serial.Value, car.GetURL(), ref car);
                        //Cars.Version v = GetCarVersionByContent4sallyxjchen(serial.Value, car.GetURL(), ref car);
                        if (v != null)
                        {
                            car.AddVersion(v);
                        }

                        //car.Versions.Add(GetCarVersionByContent(serial.Value, car.GetURL()));
                    }

                }
            }

        }

        private static void GetVersion4CarOutSale(Car car)
        {
            if (!string.IsNullOrEmpty(car_page))
            {
                /*<dl id="bt_car_spcar" class=""><dt>停售年款<em></em></dt><dd style="display: none;"><a target="_self" href="/quanxinaodia4l/2011">2011款</a><a target="_self" href="/quanxinaodia4l/2010">2010款</a><a class="last_a" target="_self" href="/quanxinaodia4l/2009">2009款</a></dd></dl>*/
                string outsale_dl = Regex.Match(car_page, "(?<=dl id=\"bt_car_spcar\").+?(?=</dl>)", RegexOptions.Singleline).Value;

                MatchCollection mc_outsale_serials_urls = Regex.Matches(outsale_dl, "(?<=href=\").+?(?=\">)");
                string outsale_url;
                foreach (Match m in mc_outsale_serials_urls)
                {
                    outsale_url = m.Value.Insert(m.Value.IndexOf("/", 1), "/peizhi");
                    GetVersions4CarByUrl(outsale_url, ref car);
                }

            }
        }

        private static void GetVersions4CarByUrl(string url, ref Car car)
        {
            //string page_content = Common.GetContentbyUrl("http://car.bitauto.com" + url);
            string page_content = Common.GetContentbyUrl(Common.url + url);
            if (!string.IsNullOrEmpty(page_content))
            {
                MatchCollection mc_serials;

                string serials_json = Regex.Match(page_content, "(?<=carCompareJson = ).+?(?=</script>)", RegexOptions.Singleline).Value;

                if (!string.IsNullOrEmpty(serials_json))
                {
                    mc_serials = Regex.Matches(serials_json, @"\[\[.+?\]\](?=[,\]])");

                    foreach (Match serial in mc_serials)
                    {
                        Cars.Version v = GetCarVersionByContent(serial.Value, car.GetURL(), ref car);
                        //Cars.Version v = GetCarVersionByContent4sallyxjchen(serial.Value, car.GetURL(), ref car);

                        if (v != null)
                        {
                            car.AddVersion(v);
                        }
                        //car.AddVersion(GetCarVersionByContent(serial.Value, car.GetURL(), car.ID));
                        //car.Versions.Add(GetCarVersionByContent(serial.Value, car.GetURL()));

                    }
                }
            }
        }



    }
}
