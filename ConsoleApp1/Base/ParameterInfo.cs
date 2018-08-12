using System;

namespace ConsoleApp.Base
{

    public class ParameterInfo
    {
        public string Name;
        public object Value;
        public Type DataType;
        public Direction Direction;

        public ParameterInfo(object value, Type dataType)
        {
            Value = value;
            DataType = dataType;
        }

        public ParameterInfo(string name, object value, Type dataType, Direction direction)
        {
            Name = name;
            Value = value;
            DataType = dataType;
            Direction = direction;
        }
    }

    public enum Direction
    {
        Input,
        Output,
    }
}
