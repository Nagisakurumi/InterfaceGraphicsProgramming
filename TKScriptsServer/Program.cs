using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts;
using TkScripts.Script;
using TKScriptsServer.API;

namespace TKScriptsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Scripts scripts = new Scripts("http://127.0.0.1:7676/");

            scripts.AddScriptFunction("add", Add);
            scripts.Start();

            Console.WriteLine("等待!");
            Console.ReadKey();
        }


        [ScriptMeth("{'parameter':[" +
            "{'name':'add1','type':'int','defult':'0','enumdatas':'','tiptext':'加数1'}," +
            "{'name':'add2','type':'int','defult':'0','enumdatas':'','tiptext':'加数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'int','defult':'','enumdatas':'','tiptext':'结果'}" +
            "]}",
            "计算", ishasInput: true, ishasOutput: true, itemBoxEnum: ItemBoxEnum.FUNCTION)]
        public static ScriptOutput Add(ScriptInput scriptInput)
        {
            int add1 = Convert.ToInt32(scriptInput.GetValue("add1"));
            int add2 = Convert.ToInt32(scriptInput.GetValue("add2"));
            ScriptOutput scriptOutput = new ScriptOutput();
            scriptOutput.SetValue("result", add1 + add2);
            return scriptOutput;
        }
    }
}
