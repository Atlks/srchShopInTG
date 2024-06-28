global using static mdsj.libBiz.otherlib;
using Newtonsoft.Json;
using prj202405;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mdsj.libBiz
{
    internal class otherlib
    {
        public static async Task foreachChtSesses(Action act)
        {
            var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
            //    chtsSess.Add(Program.groupId, "");

            //遍历方法三：遍历哈希表中的键值
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                var key = de.Key;
                Console.WriteLine(" SendPhotoAsync " + de.Key);

                //  Program.botClient.send
                try
                {
                    act();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    logErr2024(ex, "foreachChtSesses", "errlog", null);

                }

            }

        }

    }
}
