using System;
using System.Collections.Generic;
using System.Text;
using bitauto_creeper.Cars;
using System.IO;
using System.Text.RegularExpressions;
namespace bitauto_creeper
{
    class Program
    {
        static void Main(string[] args)
        {

            //Car car1 = new Car("feiyatecmedium", "http://car.bitauto.com/feiyatecmedium/", "12");
            //BitAuto_Peizhi.GetVersion4Car(car1);

            //StringBuilder sb = new StringBuilder();
            //sb.Append("test, 1, \"\"2012,2011\"\", 2, adasd" + System.Environment.NewLine);
            //sb.Append("test, 1, \"\"2012,2011\"\", 2, adasd" + System.Environment.NewLine);
            //string aa = "3011, 3012";
            //sb.Append(string.Format("test,1,\"2012,2011\",\"\"\"{0}\"\"\",2, adasd" + System.Environment.NewLine,aa));
            
            ////sb.Append("\"\"   \"2,3\"     \"\",\"\"      2013,  \"\"\"2,3\"\"\"" + System.Environment.NewLine);

            //using (StreamWriter sww = new StreamWriter(@"D:\12312310.csv", false, Encoding.UTF8))
            //{
            //    sww.Write(sb);
            //    sww.Flush();
            //}

            /*  
             * 
             * Program start below .........      
             * 
             */

            //DateTime log_time = DateTime.Now;
            string log_time_str = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            // 下载logo
            //List<Brand> Brands = BitAuto_AllCars.AllCarsWithLogo;
            // *不*下载logo
            List<Brand> Brands = BitAuto_AllCars.AllCarsNoLogo;
           Console.WriteLine("所有汽车品牌 获取完毕！");

           StringBuilder sb_pinpai = new StringBuilder();
           StringBuilder sb_chexi = new StringBuilder();
           StringBuilder sb_chexing = new StringBuilder();
           sb_pinpai.Append("[");
           sb_chexi.Append("[");
           sb_chexing.Append("[");

           StringBuilder csv_pinpai = new StringBuilder();
           StringBuilder csv_chexi = new StringBuilder();
           StringBuilder csv_chexing = new StringBuilder();
           csv_pinpai.Append("id, name, firstletter, logoname" + System.Environment.NewLine);
           csv_chexi.Append("id, name, manufacturerID, modelYears, brandId, brandName" + System.Environment.NewLine);
           csv_chexing.Append("id, name, location, modelYear, serialID, transmissionType, engineDisplacement, finletWay, fuelLabel" + System.Environment.NewLine);

            //StringBuilder csv_201478 = new StringBuilder();
            //csv_201478.Append("id, carname, versionname, modelYear, 胎压监测装置, 启停系统, 发动机电子防盗, 零压续行, 燃油箱容积(L), 变速箱, 配置URL" + System.Environment.NewLine);

            string nextYear = (DateTime.Now.Year+1).ToString();

            foreach (Brand brand in Brands)
            {
                sb_pinpai.Append(Common.JsonConvert2Str(brand) + ",");
                csv_pinpai.Append(string.Format("{0},{1},{2},{3}" + System.Environment.NewLine, brand.id, brand.name, brand.firstLetter, brand.logoName));

                BitAuto_Brands.GetCars4Brands(brand);

                if (brand.GetCars().Count > 0)
                {
                    foreach (Car car in brand.GetCars())
                    {
                        BitAuto_Peizhi.GetVersion4Car(car);
                        if (car.modelYears.Contains(nextYear))
                        {
                            car.modelYears = car.modelYears.Replace(nextYear, "").Trim(new char[] { ',' });
                        }
                        sb_chexi.Append(Common.JsonConvert2Str(car) + ",");
                        csv_chexi.Append(string.Format("{0},{1},{2},\"\"\"{3}\"\"\",{4},{5}" + System.Environment.NewLine, car.id, car.name, car.manufacturerID, car.modelYears, car.brandId, car.brandName));


                        if (car.GetVersions().Count > 0)
                        {
                            foreach (Cars.Version v in car.GetVersions())
                            {
                                sb_chexing.Append(Common.JsonConvert2Str(v) + ",");
                                csv_chexing.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}" + System.Environment.NewLine, v.id, v.name, v.location, v.modelYear, v.serialID, v.transmissionType, v.engineDisplacement, v.finletWay, v.fuelLabel));

                                ////"id, carname, versionname, modelYear, 胎压监测装置, 启停系统, 发动机电子防盗, 零压续行, 燃油箱容积(L), 变速箱, 配置URL"
                                //csv_201478.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}/peizhi/" + System.Environment.NewLine, 
                                //    v.id, car.name,v.name, v.modelYear, v.taiya_jiance_zhuangzhi,
                                //    v.qitingxitong,v.fadongji_dianzifangdao, v.lingyaxuhang, v.ranyouxiang_rongji, v.biansuxiang,v.URL));

                            }
                        }
                    }
                }
            }
            //string csv_201478_file = string.Format(@"{0}\pinpai_{1}.csv", Common.WORK_DIR, log_time_str);
            //Common.Write2Json(csv_201478_file, csv_201478);
            //Console.WriteLine("csv 文件生成 成功：" + csv_201478_file);

            sb_pinpai.Remove(sb_pinpai.Length - 1, 1);
            sb_pinpai.Append("]");

            sb_chexi.Remove(sb_chexi.Length - 1, 1);
            sb_chexi.Append("]");

            sb_chexing.Remove(sb_chexing.Length - 1, 1);
            sb_chexing.Append("]");

            string pinpai_json_file = string.Format(@"{0}\pinpai_{1}.json", Common.WORK_DIR, log_time_str);
            string chexi_json_file = string.Format(@"{0}\chexi_{1}.json", Common.WORK_DIR, log_time_str);
            string chexing_json_file = string.Format(@"{0}\chexing_{1}.json", Common.WORK_DIR, log_time_str);

            string pinpai_csv_file = string.Format(@"{0}\pinpai_{1}.csv", Common.WORK_DIR, log_time_str);
            string chexi_csv_file = string.Format(@"{0}\chexi_{1}.csv", Common.WORK_DIR, log_time_str);
            string chexing_csv_file = string.Format(@"{0}\chexing_{1}.csv", Common.WORK_DIR, log_time_str);

            Common.Write2Json(pinpai_json_file, sb_pinpai);
            Console.WriteLine("Json 文件生成 成功：" + pinpai_json_file);

            Common.Write2Json(chexi_json_file, sb_chexi);
            Console.WriteLine("Json 文件生成 成功：" + chexi_json_file);

            Common.Write2Json(chexing_json_file, sb_chexing);
            Console.WriteLine("Json 文件生成 成功：" + chexing_json_file);

            Common.Write2Json(pinpai_csv_file, csv_pinpai);
            Console.WriteLine("csv 文件生成 成功：" + pinpai_csv_file);

            Common.Write2Json(chexi_csv_file, csv_chexi);
            Console.WriteLine("csv 文件生成 成功：" + chexi_csv_file);

            Common.Write2Json(chexing_csv_file, csv_chexing);
            Console.WriteLine("csv 文件生成 成功：" + chexing_csv_file);
        }
    }
}
