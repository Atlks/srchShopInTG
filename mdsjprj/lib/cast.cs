global using static mdsj.lib.cast;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace mdsj.lib
{
    internal class cast
    {
        public static double toNumber(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
            {
                Console.WriteLine("Input string cannot be null or whitespace.");
                return 0;
                //    throw new ArgumentNullException(nameof(str), "Input string cannot be null or whitespace.");
            }

            if (double.TryParse(str, out double result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Input string is not in the correct format for a double.");
                //  throw new FormatException("Input string is not in the correct format for a double.");
                return 0;
            }

        }


        public static SortedList ConvertToSortedList(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            SortedList sortedList = new SortedList();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead)
                {
                    object value = property.GetValue(obj);
                    sortedList.Add(property.Name, value);
                }
            }

            return sortedList;
        }

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
