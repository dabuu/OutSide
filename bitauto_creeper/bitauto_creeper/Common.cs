using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace bitauto_creeper
{
    public class Common
    {
        public readonly static string url = "http://car.bitauto.com/";
        public readonly static string WORK_DIR = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;

        public static string GetContentbyUrl(string url)
        {
            Stream receiveStream = null;
            StreamReader readStream = null;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                // Set some reasonable limits on resources used by this request
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                request.Timeout = 300000;
                // Set credentials to use for this request.
                request.Credentials = CredentialCache.DefaultCredentials;

                // get response from test url
                response = (HttpWebResponse)request.GetResponse();

                // Get the stream associated with the response.
                receiveStream = response.GetResponseStream();

                using (readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    return readStream.ReadToEnd();
                }
            }
            catch (System.Net.WebException webEx)
            {
                Console.WriteLine("WebException: " + webEx.Message + ";url: " + url);
                //GetContentbyUrl(url);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("WebException: " + ex.Message + ";url: " + url);
                return null;
                //throw new Exception();
            }

            finally
            {
                if (response != null)
                    response.Close();
                if (receiveStream != null)
                    receiveStream.Close();
                if (readStream != null)
                    readStream.Close();
            }

        }

        public static string GetContentbyFilePath(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception(path + " : this path is not exist!!");
            }
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetContentbyFilePath can't handle exception: " + ex.Message);
            }
        }

        public static string JsonConvert2Str(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static bool Write2Json(string json_fullname, StringBuilder content)
        {
            DirectoryInfo di = new DirectoryInfo(new FileInfo(json_fullname).Directory.FullName);
            if (!di.Exists)
            {
                di.Create();
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(json_fullname, false, Encoding.UTF8))
                {
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Write2Json " + ex.Message + ", File name is :" + json_fullname);
            }
            
        }


        public static void DownImage2LocalPathl(string img_url, string logo_fullpath)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(img_url, logo_fullpath);
                Console.WriteLine("下载 " + img_url + "的logo，保存到" + logo_fullpath);
            }
            catch (Exception ex)
            {
                throw new Exception("GetLogoByName Download Error:" + ex.Message);
            }
        }
    }
}
