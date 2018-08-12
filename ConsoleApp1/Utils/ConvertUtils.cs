using System;

namespace ConsoleApp.Utils
{
    public class ConvertUtils
    {
        public static object Convert(object val, Type type)
        {
            if (type == typeof(string))
            {
                return val.ToString().Replace("'", "''");
            }
            else if (type == typeof(int))
            {
                return Int32.Parse(val.ToString());
            }
            else if (type == typeof(double))
            {
                return Double.Parse(val.ToString());
            }
            else
            {
                throw new NotSupportedException("Unsupport type.");
            }
        }
    }
}
