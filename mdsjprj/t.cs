using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prj202405
{
    internal class t
    {

        static async Task Main2(string[] args)
        {
            Console.WriteLine(11);


            try
            {
                int x = 1;
                var y = 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine("除法错误：" + ex.Message);
            }
        }
    }
}
