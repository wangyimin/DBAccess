using ConsoleApp.Base;
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

                sf.Commit();

                return dt;
            }
        }
    }
}
