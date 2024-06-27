global using static mdsj.lib.cast;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mdsj.lib
{
    internal class cast
    {

        public static HashSet<string> ConvertToUpperCase(HashSet<string> originalSet)
        {
            return new HashSet<string>(originalSet.Select(s => s.ToUpper()));
        }
        public static HashSet<string> castArr2set(string[] stringArray)
        {
            HashSet<string> hashSet = new HashSet<string>(
                     stringArray.Where(s => !string.IsNullOrWhiteSpace(s))
                 );
            return hashSet;
        }
    }
}
