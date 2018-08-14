using ConsoleApp.Base;
using log4net;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Data.Common;

namespace ConsoleApp.Utils
{
    public class OracleExecProc : IExecProc
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(OracleExecProc));
        public void ExecProc(DbCommand command, string proc, bool isFunc, params ParameterInfo[] para)
        {
            logger.Debug(proc);

            OracleCommand cmd = (OracleCommand)command;

            cmd.CommandText = proc;
            cmd.CommandType = CommandType.StoredProcedure;

            if (isFunc)
            {
                cmd.Parameters.Add(new OracleParameter(Constants.PROC_RET_CD, OracleDbType.Int32)).Direction = ParameterDirection.ReturnValue;
            }

            foreach (ParameterInfo obj in para)
            {
                if (obj.Direction == Direction.Input)
                {
                    cmd.Parameters.Add(obj.Name, obj.Value);
                }
                else
                {
                    cmd.Parameters.Add(obj.Name, ConvertUtils.Convert(obj.DataType), ConvertUtils.Convert(obj.Direction));

                }
            }

            if (!isFunc)
            {
                cmd.Parameters.Add(new OracleParameter(Constants.PROC_RET_CD, OracleDbType.Int32)).Direction = ParameterDirection.Output;
            }

            cmd.Parameters.Add(new OracleParameter(Constants.PROC_RET_MSG, ConvertUtils.Convert(typeof(string)), 256)).Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            string r = cmd.Parameters[Constants.PROC_RET_CD].Value.ToString();

            if (!"0".Equals(r))
            {
                throw new InvalidProgramException("Error [" + cmd.Parameters[Constants.PROC_RET_MSG].Value.ToString() + "] occurred in calling stored procedure.");
            }

        }
    }
}
