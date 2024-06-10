using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class dtime
    {

        /// <summary>
        /// 将日期字符串解析为 Unix 时间戳。
        /// </summary>
        /// <param name="dateStr">日期字符串。</param>
        /// <returns>Unix 时间戳（自1970年1月1日以来的秒数）。如果解析失败，则返回 -1。</returns>
        public static long strtotime(string dateStr)
        {
            DateTime dateTime;
            string[] formats = {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd",
            "MM/dd/yyyy HH:mm:ss",
            "MM/dd/yyyy",
            "MMM d, yyyy h:mm:ss tt",
            "MMM d, yyyy",
            "dddd, MMMM d, yyyy h:mm:ss tt",
            "dddd, MMMM d, yyyy",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyyMMddTHHmmssZ",
            "o"  // ISO 8601 format
        };

            if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
                return dateTimeOffset.ToUnixTimeSeconds();
            }
            else
            {
                return -1; // 如果解析失败，返回 -1
            }
        }
    }
}
