using System.Collections;
using System.Data;

namespace ConsoleApp.Base
{
    public interface IFunctionAPI
    {
        void SetParameters(Hashtable hashtable);
        DataTable Execute();
    }
}
