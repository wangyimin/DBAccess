using System;
using System.Collections.Concurrent;
using System.Data.Common;

namespace ConsoleApp.Base
{
    public class ConnectionFactory
    {
        private const int INIT_CONNECTIONS = 1;
        
        private static ObjectPool<ConnectionStatus> Connections;

        public static ConnectionStatus GetConnection(string driver)
        {
            if (Connections == null)
            {
                DbProviderFactory factory = null;
                if (driver.Equals(Constants.ORALCE_DRIVER))
                {
                    factory = DbProviderFactories.GetFactory(Constants.ORALCE_DRIVER);
                }
                else
                {
                    throw new NotSupportedException("Unsupport driver.");
                }

                Func<ConnectionStatus> creator = () =>
                {
                    ConnectionStatus cs = new ConnectionStatus();
                    cs.Connection = factory.CreateConnection();

                    cs.Connection.ConnectionString = Constants.ORACLE_CONNECTION_STRING;
                    cs.Connection.Open();

                    return cs;
                };

                Connections = new ObjectPool<ConnectionStatus>(creator, INIT_CONNECTIONS);
            }

            return Connections.Get();
        }

        public static void PutConnection(ConnectionStatus status)
        {
            Connections.Put(status);
        }
    }

    public class ObjectPool<T>
    {
        private ConcurrentBag<T> _objects;
        private readonly Func<T> _creator;

        public ObjectPool(Func<T> creator, int num)
        {
            if (creator == null)
            {
                throw new ArgumentNullException("Need to specify one function to generate object.");
            }

            _objects = new ConcurrentBag<T>();
            _creator = creator;

            // create the specified number of connection objects
            initialize(num);
        }

        private void initialize(int num)
        {
            for (int i = 0; i < num; i++)
            {
                T el = _creator();
                _objects.Add(el);

            }
        }

        public T Get(bool increment = true)
        {
            T el;
            if (_objects.TryTake(out el))
            {
                return el;
            }

            if (increment)
            {
                // allow pool to increase new connection object while necessary
                return _creator();
            }

            throw new InvalidOperationException("No object exists in pool.");
        }

        public void Put(T el)
        {
            _objects.Add(el);
        }
    }

}
