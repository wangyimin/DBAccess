using System;
using log4net;

namespace ConsoleApp.Base
{
    public static class ProcessWrapper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProcessWrapper));
        public static T Process<V, T>(Func<V, T> method, V para)
        {
            try
            {
                return method.Invoke(para);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public static T Process<T>(Func<T> method)
        {
            try
            {
                return method.Invoke();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public static void Process(Action method)
        {
            try
            {
                method.Invoke();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public static void Process<V>(Action<V> method, V arg)
        {
            try
            {
                method(arg);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}
