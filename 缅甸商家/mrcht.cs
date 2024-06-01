using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using prj202405;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using City = prj202405.City;

namespace prj202504
{
    internal class mrcht
    {
        //city=妙瓦底&park=世纪新城园区
        public static HashSet<City> qry4byParknameExprs2Dataobj(string uri_Query, string dbf)
        {
            HashSet<City> rowss=new HashSet<City>();
            var merchantsStr = System.IO.File.ReadAllText(dbf);
            if (!string.IsNullOrEmpty(merchantsStr))
                rowss = JsonConvert.DeserializeObject<HashSet<City>>(merchantsStr)!;
            //dataObj
            Dictionary<string, StringValues> parameters = QueryHelpers.ParseQuery(uri_Query);
            string cityName = parameters["city"]; string park = parameters["park"];
            //  Program._citys = 
            //trimOtherCity(Program._citys, city);

            // 使用LINQ查询来查找指定名称的城市
            City city1= rowss.FirstOrDefault(c => c.Name == cityName);

            Address park1 = city1.Address.FirstOrDefault(a => a.Name == park);
            city1.Address.Clear();
            city1.Address.Add(park1);
            return rowss;
        }

        //private static HashSet<City> qryByPknm(HashSet<City> citys, string? city, string? park)
        //{
        //    City city1 = FindCityByName(citys, city);
        //    Address park1 = city1.Address.FirstOrDefault(a => a.Name == park);
        //    city1.Address.Clear();
        //    city1.Address.Add(park1);
        //    return citys;
        //}

        //static City FindCityByName(HashSet<City> cities, string name)
        //{
        //    // 使用LINQ查询来查找指定名称的城市
        //    return cities.FirstOrDefault(c => c.Name == name);
        //}

        //private static HashSet<City> trimOtherCity(HashSet<City> citys, string? city)
        //{
        //    HashSet<City> rmvCitys = new HashSet<City>();
        //    foreach (City city1 in citys)
        //    {
        //        if (city1.Name != city)
        //            rmvCitys.Add(city1);
        //    }

        //    // 迭代并移除已知的值
        //    foreach (City city1 in rmvCitys)
        //    {
        //        citys.Remove(city1);
        //    }
        //    return citys;

        //}
    }
}
