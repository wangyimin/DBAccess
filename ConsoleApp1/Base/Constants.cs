namespace ConsoleApp.Base
{
    public class Constants
    {
        public const string ORALCE_DRIVER = "Oracle.DataAccess.Client";

        public const string ORACLE_CONNECTION_STRING =
            "User Id=test;Password=test;" +
            "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS =(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orclpdb)));";


        public static readonly string PROC_RET_CD = "r";
        public static readonly string PROC_RET_MSG = "msg";
    }
}
