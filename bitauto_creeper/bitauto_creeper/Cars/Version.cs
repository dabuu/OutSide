using System;
using System.Collections.Generic;
using System.Text;

namespace bitauto_creeper.Cars
{
    public class Version
    {

        //public string Year, URL, Pailiang, Youhao_ZH, Youhao_SQ, Youhao_SJ, Ranyou, Youxiang;
        //public string Origin_Args;
        public int id, serialID, location =1 ;
        public double engineDisplacement;
        //                  名字          年款        油号              手、自动        增压方式
        public string name, modelYear, fuelLabel, transmissionType, finletWay;

        //// sallyxjchen(陈晓娟); 要加的数据
        ////              启停系统        燃油箱容积(L)    变速箱         胎压监测装置          零压续行(零胎压继续行驶)   发动机电子防盗
        //public string qitingxitong, ranyouxiang_rongji, biansuxiang, taiya_jiance_zhuangzhi, lingyaxuhang, fadongji_dianzifangdao;
        //public string URL;

        public Version(string name, string parent_url, string id, int car_id)
        { 
            this.name = name;
            //this.URL = string.Format("{0}m{1}", parent_url, id);
            this.id = int.Parse(id);
            this.serialID = car_id;

        }

    }
}
