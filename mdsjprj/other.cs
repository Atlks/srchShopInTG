using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using prj202405;
using prj202405.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace mdsj
{
    internal class other
    {
        //设置用户限制
        public static async Task<Operas> _SetUserOperas(long userId)
        {
            //操作计数
            var operas = new Operas();

            var member =Program. _users[userId];
            //有此用户
            if (member == null)
            {
                Program._users.Add(userId, new prj202405.User { ViewTimes = [DateTime.Now] });
            }
            //无此用户
            else
            {
                member.ViewTimes.Add(DateTime.Now);
                operas.Todays = member.ViewTimes.Count(time => (DateTime.Now - time).TotalHours <= 24);
                operas.Weeks = member.ViewTimes.Count(time => (DateTime.Now - time).TotalDays <= 7);
                operas.Months = member.ViewTimes.Count(time => (DateTime.Now - time).TotalDays <= 30);
                operas.Totals = member.ViewTimes.Count;
            }

            await _SaveConfig();
            return operas;
        }



        public static async Task _SaveConfig()
        {
        writeUser:
            try
            {
                await System.IO.File.WriteAllTextAsync("Users.json", JsonConvert.SerializeObject(Program._users));
            }
            catch (Exception e)
            {
                Console.WriteLine("向本地写入限制用户时出错：" + e.Message);
                goto writeUser;
            }

        writeMerchant:
            try
            {
                await System.IO.File.WriteAllTextAsync(_shangjiaFL(Program.groupId.ToString() ), JsonConvert.SerializeObject(getCitysObj()));
            }
            catch (Exception e)
            {
                Console.WriteLine("向本地写入商家时出错：" + e.Message);
                goto writeMerchant;
            }
        }

        public static HashSet<prj202405.City>   getCitysObj()
        {
            //联系商家城市
            HashSet<prj202405.City> _citys = [];
            var merchants = System.IO.File.ReadAllText(_shangjiaFL( Program.groupId.ToString()));
          //  if (!string.IsNullOrEmpty(merchants))
                _citys = JsonConvert.DeserializeObject<HashSet<prj202405.City>>(merchants)!;
            return _citys;
        }

        public const string botname = Program.botname;


        //是否在营业时间内
        public static string _IsBusinessHours(TimeSpan startTime, TimeSpan endTime)
        {
            var currentDayTime = DateTime.Now.TimeOfDay;

            // 如果结束时间小于开始时间，说明跨越了午夜
            if (endTime < startTime)
            {
                if (currentDayTime >= startTime || currentDayTime <= endTime)
                {
                    return "(营业中)";
                }
                else
                {
                    return "(已打烊)";
                }
            }
            else
            {
                // 正常情况下比较开始时间和结束时间
                if (currentDayTime >= startTime && currentDayTime <= endTime)
                {
                    return "(营业中)";
                }
                else
                {
                    return "(已打烊)";
                }
            }

        }

        public static string _shangjiaFL(string groupId)
        {
            List<Dictionary<string, object>> lst = (List<Dictionary<string, object>>)ormSqlt._qry($"select * from grp_loc_tb where grpid='{groupId}'", "grp_loc.db");
            if (lst.Count > 0)
            {
                Dictionary<string, object> d = lst[0];
                if (d["shangjiaFL"] == null)
                    return "Merchant.json";
                return (string)d["shangjiaFL"];
            }

            return "Merchant.json";
        }

        public static async Task _readMerInfo()
        {
          Program.  chatIds = [.. System.IO.File.ReadAllLines("chatIds.txt")];

            if (System.IO.File.Exists("Users.json"))
            {
                var users = await System.IO.File.ReadAllTextAsync("Users.json");
                if (!string.IsNullOrEmpty(users))
                    Program._users = JsonConvert.DeserializeObject<Dictionary<long, prj202405.User>>(users)!;
            }

            var merchants = await System.IO.File.ReadAllTextAsync(_shangjiaFL((string)Program.groupId.ToString()));
            //if (!string.IsNullOrEmpty(merchants))
            //    Program._citys = JsonConvert.DeserializeObject<HashSet<prj202405.City>>(merchants)!;
            ////ini（）  finish
        }

        //获取枚举描述
        public static string _GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }

        public static string getTrgwdHash(string filePath)
        {
            HashSet<string> hs = getTrgwdHashProcessFile(filePath);

            return string.Join(" ", hs);
        }

        public static HashSet<string> getTrgwdHashProcessFile(string filePath)
        {
            // 创建一个 HashSet 来存储处理后的行
            HashSet<string> processedLines = new HashSet<string>();

            // 读取文件并逐行处理
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // 替换连字符和双引号，并进行 Trim()
                    line = line.Replace("-", "").Replace("\"", "").Trim();

                    // 将处理后的行添加到 HashSet 中
                    if (!string.IsNullOrEmpty(line)) // 可选：跳过空行
                    {
                        processedLines.Add(line);
                    }
                }
            }

            return processedLines;
        }


    }
}
