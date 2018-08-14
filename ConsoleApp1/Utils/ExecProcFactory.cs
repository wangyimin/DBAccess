using System;
using ConsoleApp.Base;

namespace ConsoleApp.Utils
{
    public class ExecProcFactory
    {
        public static IExecProc GetExecProc(string driver)
        {
            if (driver.Equals(Constants.ORALCE_DRIVER))
            {
                return new OracleExecProc();
            }

            throw new NotSupportedException("Unsupport driver.");
        }
    }
}
