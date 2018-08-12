using System;
using System.Data;
using Oracle.DataAccess.Client;
using ConsoleApp.Base;

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

        public static OracleDbType Convert(Type type)
        {
            if (type == typeof(string))
            {
                return OracleDbType.Varchar2;
            }
            else if (type == typeof(int))
            {
                return OracleDbType.Int32;
            }
            else
            {
                throw new NotSupportedException("Unsupport type.");
            }
        }

        public static ParameterDirection Convert(Direction direction)
        {
            if (direction == Direction.Input)
            {
                return ParameterDirection.Input;
            }
            else if (direction == Direction.Output)
            {
                return ParameterDirection.Output;
            }
            else
            {
                throw new NotSupportedException("Unsupport type.");
            }
        }
    }
}
