global using static mdsj.libBiz.otherlib;
using Newtonsoft.Json;
using prjx;
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
        public static string GetFuwuci(string? text, HashSet<string> 商品与服务词库)
        {
            if (text == null)
                return null;
            string[] spltWds = SpltByFenci(ref text);
            foreach (string wd in spltWds)
            {
                if (商品与服务词库.Contains(wd))
                    return wd;
            }
            return null;
        }

        public static List<string> cvt2list(SortedList merchant1, string fld)
        {
            List<string> li = new List<string>();
            try
            {
                li.Add(TrimRemoveUnnecessaryCharacters4tgWhtapExt(LoadFieldFrmStlst(merchant1, fld, "").ToString()));

            }
            catch (Exception e)
            {

            }

            return li;
        }
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
               Print(" SendPhotoAsync " + de.Key);

                //  Program.botClient.send
                try
                {
                    act();

                }
                catch (Exception ex)
                {
                   Print(ex.ToString());
                    logErr2024(ex, "foreachChtSesses", "errlog", null);

                }

            }

        }

    }
}
