using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Oracle.DataAccess.Client;
using ConsoleApp.Utils;

namespace ConsoleApp.Base
{
    public class SessionFactory : IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SessionFactory));

        private static readonly string PROC_RET_CD = "r";
        private static readonly string PROC_RET_MSG = "msg";


        private ConnectionStatus _status;
        private DbConnection _connection;
        private DbCommand _command;
        private DbTransaction _tx;

        //public static Hashtable SchemaMeta = new Hashtable();
        private bool _borrowed;
 
        public SessionFactory()
        {
            _status = ConnectionFactory.GetConnection();
            _borrowed = true;

            _connection = _status.Connection;

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Close();
                _connection.Open();
            }

            _command = _connection.CreateCommand();
        }
 
        public virtual DataTable CreateQuery(string sql)
        {
            logger.Debug(sql);

            _command.CommandText = sql;

            DbDataReader reader = _command.ExecuteReader();
            
            /***
            //maybe it will be used in future
            DataTable tablemeta = reader.GetSchemaTable();
            if (SchemaMeta[tablemeta.Rows[0]["BaseTableName"]] == null)
            {
                SchemaMeta.Add(tablemeta.Rows[0]["BaseTableName"], tablemeta);
            }
            ***/

            DataTable dt = new DataTable();
            dt.Load(reader);

            reader.Close();

            return dt;
        }

        public virtual void CreateStoredProcedure(string proc, bool IsFunction, params ParameterInfo[] para)
        {
            logger.Debug(proc);

            OracleCommand cmd = (OracleCommand)_command;

            cmd.CommandText = proc;
            cmd.CommandType = CommandType.StoredProcedure;

            if (IsFunction)
            {
                cmd.Parameters.Add(new OracleParameter(PROC_RET_CD, OracleDbType.Int32)).Direction = ParameterDirection.ReturnValue;
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

            if (!IsFunction)
            {
                cmd.Parameters.Add(new OracleParameter(PROC_RET_CD, OracleDbType.Int32)).Direction = ParameterDirection.Output;
            }

            cmd.Parameters.Add(new OracleParameter(PROC_RET_MSG, ConvertUtils.Convert(typeof(string)), 256)).Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            string r = cmd.Parameters[PROC_RET_CD].Value.ToString();
            
            if (!"0".Equals(r))
            {
                throw new InvalidProgramException("Error [" + cmd.Parameters[PROC_RET_MSG].Value.ToString() + "] occurred in calling stored procedure.");
            }
            
        }

        public virtual List<T> CreateQuery<T>(string sql)
        {
            DataTable dt = CreateQuery(sql);
 
            List<T> lst = new List<T>();
 
            foreach (DataRow dr in dt.Rows)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    //DBNull maybe cause any problem...
                    //need to define property of class with allowable null
                    prop.SetValue(obj, dr[prop.Name], null);
                }
 
                lst.Add(obj);
            }

            return lst;
        }
 

        public virtual int CreateDDL(string sql)
        {
            _command.CommandText = sql;
 
            return _command.ExecuteNonQuery();
        }
 
        public virtual void BeginTransaction()
        {
            _tx = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }
 
        public virtual void Commit()
        {
            _tx.Commit();
        }
 
        public virtual void Rollback()
        {
            _tx.Rollback();
        }

        public virtual void Close()
        {
            _tx.Dispose();
            _command.Dispose();

            //attention: use using block or call this method explicitly to avoid memeory leak
            if (_borrowed)
            {
                ConnectionFactory.PutConnection(_status);
                _borrowed = false;
            }
        } 

        public virtual void Dispose()
        {
            Close();
        }
    }
}
