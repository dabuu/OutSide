using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JsonNetTest
{
    public class Product
    {
        public Product(string n)
        {
            this.Name = n;
        }
        //public string Name { get; set; }
        //public DateTime Expiry { get; set; }
        //public string[] Sizes { get; set; }
        //public int index { get; set; }

        private string Name;
        public string P_name
        {
            get
            {
                return this.Name;
            }
        }
        public DateTime Expiry;
        public string[] Sizes;
        public int index;
    }
    
    class Program
    {
        public void max_grade(int[] grades)
        {
            // Write your code here
            // To print results to the standard output you can use Console.WriteLine()
            // Example: Console.WriteLine("Hello world!");
            int temp;
            for (int i = 0; i < grades.Length; i++)
            {
                for (int j = i + 1; j < grades.Length; j++)
                {
                    if (grades[i] <= grades[j])
                    {
                        temp = grades[i];
                        grades[i] = grades[j];
                        grades[j] = temp;
                    }
                }
            }

            Console.WriteLine(grades[0]);
        }


        static void Main(string[] args)
        {

            Product product = new Product("Apple");
            //product.Name = "Apple";
            product.Expiry = new DateTime(2008, 12, 28);
            product.Sizes = new string[] { "2014", "2013" };
            JsonConvert.DeserializeObject("");
            string json = JsonConvert.SerializeObject(product);
            Product p = JsonConvert.DeserializeObject<Product>(json);
            


            Console.ReadKey();
            //{
            //  "Name": "Apple",
            //  "Expiry": "2008-12-28T00:00:00",
            //  "Sizes": [
            //    "Small"
            //  ]
            //}


//            string json = @"{
//                                      'Name': 'Bad Boys',
//                                      'ReleaseDate': '1995-4-7T00:00:00',
//                                      'Genres': [
//                                        'Action',
//                                        'Comedy'
//                                      ]
//                                    }";

        }
    }
}
