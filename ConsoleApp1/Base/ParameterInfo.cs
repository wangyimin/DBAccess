using System;

namespace ConsoleApp.Base
{
    public class ParameterInfo
    {
        public object Value;
        public Type DataType;

        public ParameterInfo(object value, Type dataType)
        {
            Value = value;
            DataType = dataType;
        }
    }
}
