using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 缅甸商家
{
    internal class pinlun
    {
        public static void savePinlun( object frm, String Strfile)
        {
            if (!System.IO.File.Exists(Strfile))
                System.IO.File.WriteAllText(Strfile, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(System.IO.File.ReadAllText(Strfile));

            // 将List转换为ArrayList
            ArrayList arrayList = new ArrayList(list);
            //    ArrayList chtsSesss =(ArrayList) JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Strfile))!;

            //if (chtsSesss.Contains(Convert.ToString(chtid)))
            //{
            //    return;
            //}


            arrayList.Add( frm);

                System.IO.File.WriteAllText(Strfile, JsonConvert.SerializeObject(arrayList, Newtonsoft.Json.Formatting.Indented));

            
        }
    }
}
