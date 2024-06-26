using Microsoft.Playwright;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime;

namespace Telegram助手
{
    internal class Helper
    {
        const string _domain = "http://156.247.9.173/";

        public static async Task<List<AccountUser>> GetServiceAccounts()
        {
            List<AccountUser> accounts = new();
            HttpResponseMessage? result = null;
            HttpClient http = new();
            try
            {
                result = await http.GetAsync(_domain + "Accounts");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("获取云端账号出错:" + ex.Message);
            }

            string readStr = string.Empty;
            if (result != null && result.IsSuccessStatusCode)            
                readStr = await result.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(readStr))
            {
                Debug.WriteLine("获取云端账号ReadAsStringAsync()后是空字符串");
            }
            else
            {
                var netAccounts = JsonConvert.DeserializeObject<List<NetAccountUser>>(readStr);
                if (netAccounts != null && netAccounts.Any())
                {
                    foreach (var account in netAccounts)
                    {
                        var au = account as AccountUser;
                        if (au != null && !string.IsNullOrEmpty(account.ContextOptions))
                        {
                            var deserObj = JsonConvert.DeserializeObject<BrowserNewContextOptions>(account.ContextOptions);
                            if (deserObj == null)
                            {
                                Debug.WriteLine("循环云端账号时在把ContextOptions字符串映射为对象时为null");
                            }
                            else
                            {
                                au.ContextOptions = deserObj;
                            }

                            accounts.Add(au);
                        }
                    }
                }
            }

            foreach (var item in accounts)
            {
                item.AddMembers = 0;
                item.TodayAddedUsers = 0;
                item.LastAddedUserTime = null;

                if (item != null && item.DeadlineLimitedTime < DateTime.Now)
                    item.DeadlineLimitedTime = null;
            }

            return accounts;
        }

        //网卡Mac
        public static string GetMacByNetworkInterface()
        {
            var mac = string.Empty;
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                mac += ni.GetPhysicalAddress().ToString();

            return mac;
        }

        //是否已开机自启动
        public static bool IsStartupSet()
        {
            // 获取当前用户的注册表键
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                // 检查是否存在自启动项
                return key.GetValue(Application.ProductName) != null;
            }
        }

        //设置为开机启动
        public static void SetStartup()
        {
            // 获取当前用户的注册表键
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                // 添加自启动项
                key.SetValue(Application.ProductName, Application.ExecutablePath);
            }
        }

        public static void RemoveStartup()
        {
            // 获取当前用户的注册表键
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                // 移除自启动项
                key.DeleteValue(Application.ProductName, false);
            }
        }

        //字节格式
        public static string FormatBytes(long bytes)
        {
            if (bytes <= 0)
            {
                return "0B";
            }

            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixIndex = 0;

            double byteCount = bytes;

            while (byteCount >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                byteCount /= 1024;
                suffixIndex++;
            }

            return $"{byteCount:0.#}{suffixes[suffixIndex]}";
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {

            if (timeSpan.TotalSeconds<=0)
            {
                return string.Empty;
            }
            else if (timeSpan.TotalMinutes < 1)
            {
                return string.Format("{0}秒", timeSpan.Seconds);
            }
            else if (timeSpan.TotalHours < 1)
            {
                return string.Format("{0}分{1}秒", timeSpan.Minutes, timeSpan.Seconds);
            }
            else if (timeSpan.TotalDays < 1)
            {
                return string.Format("{0}时{1}分{2}秒", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                int days = (int)timeSpan.TotalDays;
                int hours = timeSpan.Hours;
                int minutes = timeSpan.Minutes;
                int seconds = timeSpan.Seconds;

                return string.Format("{0}天{1}时{2}分{3}秒", days, hours, minutes, seconds);
            }
        }

        /// <summary>
        /// 距离目标时间
        /// </summary>
        /// <param name="targetDateTime"></param>
        public static string CalculateTimeDifference(DateTime? targetDateTime)
        {
            var timeSpan = Convert.ToDateTime(targetDateTime) - DateTime.Now;

            if (timeSpan.TotalSeconds <= 0)
            {
                return string.Empty;
            }
            else if (timeSpan.TotalMinutes < 1)
            {
                return string.Format("{0}秒", timeSpan.Seconds);
            }
            else if (timeSpan.TotalHours < 1)
            {
                return string.Format("{0}分{1}秒", timeSpan.Minutes, timeSpan.Seconds);
            }
            else if (timeSpan.TotalDays < 1)
            {
                return string.Format("{0}时{1}分{2}秒", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                int days = (int)timeSpan.TotalDays;
                int hours = timeSpan.Hours;
                int minutes = timeSpan.Minutes;
                int seconds = timeSpan.Seconds;

                return string.Format("{0}天{1}时{2}分{3}秒", days, hours, minutes, seconds);
            }
        }
    }
}
