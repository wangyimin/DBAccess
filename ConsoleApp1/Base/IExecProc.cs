using System.Data.Common;


namespace ConsoleApp.Base
{
    public interface IExecProc
    {
        void ExecProc(DbCommand command, string proc, bool isFunc, params ParameterInfo[] par);
    }
}
