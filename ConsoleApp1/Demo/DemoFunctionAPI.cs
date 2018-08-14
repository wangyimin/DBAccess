using ConsoleApp.Base;
using System.Collections.Generic;
using System.Data;

namespace ConsoleApp.Demo
{
    public class DemoFunctionAPI : FunctionAPI, IFunctionAPI
    {
        public override DataTable Execute()
        {
            ParameterInfo para = (ParameterInfo)this._hs["Name"];

            using (SessionFactory sf = new SessionFactory())
            {
                sf.BeginTransaction();
                
                DataTable dt = sf.CreateQuery("SELECT * FROM TEST WHERE NAME = '" + para.Value + "'");

                List<Test> lst = sf.CreateQuery<Test>("SELECT * FROM TEST");

                ParameterInfo[] pi = { new ParameterInfo("Name", "W01", typeof(string), Direction.Input) };
                sf.CreateStoredProcedure("testfunc", true, pi);

                sf.Commit();

                return dt;
            }
        }
    }
}
