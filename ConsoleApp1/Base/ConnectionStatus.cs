using System;
using System.Data.Common;

namespace ConsoleApp.Base
{
    public class ConnectionStatus : IDisposable
    {
        public DbConnection Connection { get; set; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
