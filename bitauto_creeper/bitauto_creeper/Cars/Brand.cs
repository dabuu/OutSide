using System;
using System.Collections.Generic;
using System.Text;

namespace bitauto_creeper.Cars
{
    public class Brand
    {
        
        public string  name, firstLetter, logoName;
        public int id;
        protected string URL, BigLogoUrl, BigLogoPath, SmallLogoUrl, SmallLogoPath;
        private List<Car> Cars;

        public Brand(string name, string url, string id)
        {
            this.name = name;
            this.URL = url;
            this.id = int.Parse(id);
            this.Cars = new List<Car>();
        }
        public void SetBigLogoInfo(string biglogourl, string biglogopath)
        {
            this.BigLogoUrl = biglogourl;
            this.BigLogoPath = biglogopath;
        }

        public void SetSmallLogoInfo(string smalllogourl, string smalllogopath)
        {
            this.SmallLogoUrl = smalllogourl;
            this.SmallLogoPath = smalllogopath;
        }

        public string GetURL()
        {
            return this.URL;
        }

        public void AddCar2List(Car car)
        {
            this.Cars.Add(car);
        }
        public List<Car> GetCars()
        {
            return this.Cars;
        }
    }
}
