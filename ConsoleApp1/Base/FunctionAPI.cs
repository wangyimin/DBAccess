using ConsoleApp.Utils;
using System;
using System.Collections;
using System.Data;

namespace ConsoleApp.Base
{
    public abstract class FunctionAPI : IFunctionAPI
    {
        protected Hashtable _hs;

        public virtual void SetParameters(Hashtable hashtable)
        {
            _hs = hashtable;
            
            // prevent any SQL injection occur.
            foreach(var s in _hs.Keys)
            {
                ParameterInfo para = (ParameterInfo)_hs[s];
                para.Value = ConvertUtils.Convert(para.Value, para.DataType);
            }
        }

        public virtual DataTable Execute()
        {
            throw new NotSupportedException("Unable to call directly.");
        }
    }
}
