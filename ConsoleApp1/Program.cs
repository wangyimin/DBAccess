//using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using log4net;
using ConsoleApp.Base;
using ConsoleApp.Demo;
using System.Collections;
using ConsoleApp.Utils;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {

            Hashtable hs = new Hashtable();
            hs.Add("Name", new ParameterInfo("W01", typeof(string)));

            string func = "ConsoleApp.Demo.DemoFunctionAPI";
            IFunctionAPI api = (IFunctionAPI)Activator.CreateInstance(Type.GetType(func));

            ProcessWrapper.Process(api.SetParameters, hs);
            DataTable table = ProcessWrapper.Process<DataTable>(api.Execute);

            Trace.WriteLine("END");

        }

    }
}
