using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using bitauto_creeper.Cars;
namespace bitauto_creeper
{
    public static class BitAuto_BrandLogo
    {
        private readonly static string brandlogo_url = "http://car.bitauto.com/qichepinpai/",
                 big_brandlogo_download_targetpath = string.Format(@"{0}\Logo_100", Common.WORK_DIR),
                 small_brandlogo_download_targetpath = string.Format(@"{0}\Logo_30", Common.WORK_DIR),
                 big_logo_url_format = "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_{0}_100.png",
                 small_logo_url_format = "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/30/m_{0}_30.png";


        private static string brandlogo_page;//= Common.GetContentbyUrl(brandlogo_url);
        //private static int index = 1;

        public static void GetLogoByName(ref Brand brand, bool isdownload)
        {
            
            if (!Directory.Exists(big_brandlogo_download_targetpath))
            {
                Directory.CreateDirectory(big_brandlogo_download_targetpath);
            }
            if (!Directory.Exists(small_brandlogo_download_targetpath))
            {
                Directory.CreateDirectory(small_brandlogo_download_targetpath);
            }

            if (string.IsNullOrEmpty(brandlogo_page))
            {
                brandlogo_page = Common.GetContentbyUrl(brandlogo_url);
            }
            brand.logoName = Regex.Match(brandlogo_page, "(?<=<a href=\"http://car.bitauto.com/)[\\w\\d-]+?(?=/\" title=\"" + brand.name + "\" target=\"_blank)").Value;


            //<img width="100px" height="100px" alt="奥迪" src="http://image.bitautoimg.com/bt/car/default/images/carimage/m_9_100.jpg">
            string big_logo_url = Regex.Match(brandlogo_page, "(?<=height=\"100px\" alt=\"" + brand.name+ "\" src=\").+?(?=\")").Value;
            //string big_logo_url = string.Format(big_logo_url_format, brand.ID);
            string big_logo_fullname = string.Format(@"{0}\{1}.jpg", big_brandlogo_download_targetpath, brand.logoName);
            brand.SetBigLogoInfo(big_logo_url, big_logo_fullname);

            string small_logo_url = string.Format(small_logo_url_format, brand.id);
            string small_logo_fullname = string.Format(@"{0}\{1}.jpg", small_brandlogo_download_targetpath, brand.logoName);
            brand.SetSmallLogoInfo(small_logo_url, small_logo_fullname);

            if (isdownload)
            {
                if (!string.IsNullOrEmpty(big_logo_url))
                {
                    Common.DownImage2LocalPathl(big_logo_url, big_logo_fullname);
                }
                Common.DownImage2LocalPathl(small_logo_url, small_logo_fullname);
            }

            //if (!string.IsNullOrEmpty(img_url))
            //{
            //    //<a href="http://car.bitauto.com/audi/" title="奥迪" target="_blank">
            //    brand.logoName = Regex.Match(brandlogo_page, "(?<=<a href=\"http://car.bitauto.com/).+?(?=/\" title=\"" + brand.Name + "\" target=\"_blank)").Value;

            //    //string logo_fullname = string.Format(@"{0}\{2}_{1}.jpg", brandlogo_download_targetpath, brand.Name, index);
            //    string logo_fullname = string.Format(@"{0}\{1}.jpg", brandlogo_download_targetpath, brand.logoName);

            //    //brand.LogoUrl = img_url;
            //    //brand.LogoPath = logo_fullname;
            //    brand.SetLogoInfo(img_url, logo_fullname);

            //    if (isdownload)
            //    {
            //        try
            //        {
            //            WebClient client = new WebClient();
            //            client.DownloadFile(img_url, logo_fullname);
            //            Console.WriteLine("下载 " + brand.Name + "的logo，保存到" + logo_fullname);
            //        }
            //        catch (Exception ex)
            //        {
            //            throw new Exception("GetLogoByName Download Error:" + ex.Message);
            //        }
            //    }

            //}
            //index++;
        }


        
    }
}
