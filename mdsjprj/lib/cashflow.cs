global using static mdsj.lib.cashflow;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class cashflow
    {

        public static Dictionary<string, decimal> cash_sumByMonth(long uid, string msg2)
        {
            string[] a = msg2.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = GetElmt(a, 1);

            var month = GetElmt(a, 2);


            List<SortedList> li = Qe_qryV2<SortedList>("blshtDir", "blsht" + uid.ToString() + ".json",

               (SortedList row) =>
               {
                   if (row["month"].ToString().Equals(month))
                       return true;
                   return false;
               }, null,
               (SortedList row) =>
               {
                   return row;
               }
               , rnd4jsonFlRf());

            var rzt = SummarizeByCategory(li);
            return rzt;
        }

        public static Dictionary<string, decimal> SummarizeByCategory(List<SortedList> dataList)
        {

            const string amt = "amt";
            const string cate = "cate";
            var categoryAmountMap = new Dictionary<string, decimal>();

            foreach (var data in dataList)
            {

                if (data.ContainsKey(cate) && data.ContainsKey(amt))
                {
                    string category = data[cate].ToString();

                    decimal amount = Convert.ToDecimal(data[amt]);

                    if (categoryAmountMap.ContainsKey(category))
                    {
                        categoryAmountMap[category] += amount;
                    }
                    else
                    {
                        categoryAmountMap[category] = amount;
                    }
                }
            }

            return categoryAmountMap;
        }

        public static List<string> cash_qry(string msg2, long uid)
        {
            string[] a = msg2.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = GetElmt(a, 1);

            var month = GetElmt(a, 2);


            //   Func<SortedList, bool> whereFun = ;


            List<string> li = Qe_qryV2<string>("blshtDir", "blsht" + uid.ToString() + ".json",

                (SortedList row) =>
                {
                    if (row["month"].ToString().Equals(month))
                        return true;
                    return false;
                }, (SortedList row) =>
                {
                    return int.Parse(row["date"].ToString());
                },


                (SortedList row) =>
                {
                    return $"{row["date"]} {row["cate"]} {row["amt"]} {row["demo"]}";
                }
                , rnd4jsonFlRf());
            return li;
        }

        public static void cash_del(string msg, long uid)
        {
            string[] a = msg.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = GetElmt(a, 1);

            var id = GetElmt(a, 2);



            ormJSonFL.del(id, $"blshtDir/blsht{uid}.json");
        }
        public static string logic_addCashflow(long uid, string? text)
        {
            string[] a = text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = GetElmt(a, 1);

            var date = GetElmt(a, 2);
            var amt = toNumber(GetElmt(a, 3));
            var cate = GetElmt(a, 4);
            var demo = SubstrAfterMarker(text.Trim(), cate);
            SortedList map = new SortedList();
            map.Add("date", date);
            map.Add("amt", amt);
            map.Add("month", DateTime.Now.Year + Left(date, 2));
            map.Add("cate", cate);
            map.Add("demo", demo);
            string recID = $"{date}{cate}{new Random().Next()}";
            map.Add("id", recID);


            ormJSonFL.SaveJson(map, $"blshtDir/blsht{uid}.json");
            return recID;
        }

    }
}
