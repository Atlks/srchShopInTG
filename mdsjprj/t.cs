using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx
{
    internal class t
    {

        static async Task Main2(string[] args)
        {
           Print(11);


            try
            {
                int x = 1;
                var y = 2;
            }
            catch (Exception ex)
            {
               Print("除法错误：" + ex.Message);
            }
        }
    }
}
