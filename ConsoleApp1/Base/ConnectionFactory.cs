using System;
using System.Collections.Concurrent;
using System.Data.Common;

namespace ConsoleApp.Base
{
    public class ConnectionFactory : IDisposable
    {
        private const int INIT_CONNECTIONS = 1;

        private DbProviderFactory _factory;
        private static ObjectPool<ConnectionStatus> _pool;

        public ConnectionFactory(string driver)
        {
            if (_pool == null)
            {
                if (driver.Equals(Constants.ORALCE_DRIVER))
                {
                    _factory = DbProviderFactories.GetFactory(Constants.ORALCE_DRIVER);
                }
                else
                {
                    throw new NotSupportedException("Unsupport driver.");
                }

                Func<ConnectionStatus> creator = () =>
                {
                    ConnectionStatus cs = new ConnectionStatus();
                    cs.Connection = _factory.CreateConnection();

                    cs.Connection.ConnectionString = Constants.ORACLE_CONNECTION_STRING;
                    cs.Connection.Open();

                    return cs;
                };

                _pool = new ObjectPool<ConnectionStatus>(creator, INIT_CONNECTIONS);
            }
        }

        public ConnectionStatus GetConnection()
        {
            return _pool.Borrow();
        }

        public void PutConnection(ConnectionStatus status)
        {
            _pool.Return(status);
        }

        public void Dispose()
        {
            _pool.Clear();
        }
    }

    public class ObjectPool<T> where T : IDisposable
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
            _initialize(num);
        }

        private void _initialize(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Add();
            }
        }

        public void Add()
        {
            T el = _creator();
            _objects.Add(el);
        }

        public T Borrow(bool increment = true)
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

        public void Return(T el)
        {
            _objects.Add(el);
        }

        public int GetPoolSize()
        {
            return _objects.Count;
        }

        public void Remove(T el)
        {
            throw new NotSupportedException("Remove is unspported.");
        }

        public void Clear()
        {
            while (!_objects.IsEmpty)
            {
                if (_objects.TryTake(out T el))
                {
                    el.Dispose();
                }
            }
        }
    }

}
