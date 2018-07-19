using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TkScripts.Script;

namespace TkScripts.ScriptLayout
{
    public class ScriptToolsFunction
    {
        [ScriptMeth("",
            "{'parameter':[" +
            "{'name':'NowTime','type':'DATETIME','defult':'','enumdatas':'','tiptext':'返回当前执行时候的时间'}" +
            "]}",
            "获取当前时间", ishasInput: false, ishasOutput: false, itemBoxEnum: ItemBoxEnum.FUNCTION)]
        public static ScriptOutput GetNowTime(ScriptInput si)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("NowTime", DateTime.Now);
            return so;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'int1','type':'INT','defult':'0','enumdatas':'','tiptext':'数1'}," +
            "{'name':'option','type':'ENUM','defult':'>','enumdatas':{'type':'value','value':'>,<,=,>=,<='},'tiptext':'进行比较的类型'}," +
            "{'name':'int2','type':'INT','defult':'0','enumdatas':'','tiptext':'数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'BOOL','defult':'','enumdatas':'','tiptext':'比较的结果'}" +
            "]}",
            "比较函数")]
        public static ScriptOutput EqualInt(ScriptInput si)
        {
            int int1 = Convert.ToInt32(si.GetValue("int1"));
            int int2 = Convert.ToInt32(si.GetValue("int2"));
            string option = Convert.ToString(si.GetValue("option"));
            bool result = false;
            switch (option)
            {
                case ">":
                    result = int1 > int2;
                    break;
                case "<":
                    result = int1 < int2;
                    break;
                case "=":
                    result = int1 == int2;
                    break;
                case ">=":
                    result = int1 >= int2;
                    break;
                case "<=":
                    result = int1 <= int2;
                    break;
                default:
                    break;
            }
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", result);
            return so;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'float1','type':'FLOAT','defult':'0','enumdatas':'','tiptext':'数1'}," +
            "{'name':'option','type':'ENUM','defult':'>','enumdatas':{'type':'value','value':'>,<,=,>=,<='},'tiptext':'进行比较的类型'}," +
            "{'name':'float2','type':'FLOAT','defult':'0','enumdatas':'','tiptext':'数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'BOOL','defult':'','enumdatas':'','tiptext':'比较的结果'}" +
            "]}",
            "比较函数")]
        public static ScriptOutput EqualDouble(ScriptInput si)
        {
            double int1 = Convert.ToDouble(si.GetValue("float1"));
            double int2 = Convert.ToDouble(si.GetValue("float2"));
            string option = Convert.ToString(si.GetValue("option"));
            bool result = false;
            switch (option)
            {
                case ">":
                    result = int1 > int2;
                    break;
                case "<":
                    result = int1 < int2;
                    break;
                case "=":
                    result = int1 == int2;
                    break;
                case ">=":
                    result = int1 >= int2;
                    break;
                case "<=":
                    result = int1 <= int2;
                    break;
                default:
                    break;
            }
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", result);
            return so;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'obj','type':'OBJECT','defult':'','enumdatas':'','tiptext':'数组'}" +
            "]}",
            "",
            "输出对象")]
        public static ScriptOutput PrintObject(ScriptInput si)
        {
            object obj = si.GetValue("obj") as object;
            si.Write(obj.ToString());
            return null;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'time1','type':'OBJECT','defult':1,'enumdatas':'','tiptext':'加数1'}," +
            "{'name':'time2','type':'OBJECT','defult':1,'enumdatas':'','tiptext':'加数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'TotalDays','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总天数'}," +
            "{'name':'TotalHours','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总小时'}," +
            "{'name':'TotalMinutes','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总分钟'}," +
            "{'name':'TotalSeconds','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总秒数'}," +
            "{'name':'TotalMilliseconds','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总毫秒数'}" +
            "]}",
            "time1 - time2")]
        public static ScriptOutput TimeDesc(ScriptInput si)
        {
            DateTime time1 = Convert.ToDateTime(si.GetValue("time1"));
            DateTime time2 = Convert.ToDateTime(si.GetValue("time2"));

            ScriptOutput so = new ScriptOutput();
            TimeSpan ts = time1 - time2;
            so.SetValue("TotalHours", ts.TotalHours);
            so.SetValue("TotalMilliseconds", ts.TotalMilliseconds);
            so.SetValue("TotalMinutes", ts.TotalMinutes);
            so.SetValue("TotalSeconds", ts.TotalSeconds);
            so.SetValue("TotalDays", ts.TotalDays);
            return so;
        }

        /// <summary>
        /// 延迟函数
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'time','type':'INT','defult':1000,'enumdatas':'','tiptext':'超时时间'}" +
            "]}",
            "",
            "延迟函数")]
        public static ScriptOutput DelyTime(ScriptInput idx)
        {
            Thread.Sleep(Convert.ToInt32(idx.GetFirst()));
            return null;
        }
        /// <summary>
        /// 整形相加
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'d1','type':'INT','defult':1,'enumdatas':'','tiptext':'加数1'}," +
            "{'name':'d2','type':'INT','defult':1,'enumdatas':'','tiptext':'加数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'INT','defult':'','enumdatas':'','tiptext':'求和的结果'}" +
            "]}",
            "整形相加")]
        public static ScriptOutput AddInt(ScriptInput idx)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", (Convert.ToInt32(idx.GetValue("d1")) + Convert.ToInt32(idx.GetValue("d2"))));
            idx.Write("AddInt结果:" + so.GetValue("result").ToString());
            return so;
        }
        /// <summary>
        /// 浮点型相加
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'f1','type':'FLOAT','defult':1,'enumdatas':'','tiptext':'加数1'}," +
            "{'name':'f2','type':'FLOAT','defult':1,'enumdatas':'','tiptext':'加数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'FLOAT','defult':'','enumdatas':'','tiptext':'求和的结果'}" +
            "]}",
            "浮点型相加")]
        public static ScriptOutput AddFloat(ScriptInput idx)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", Convert.ToDouble(idx.GetValue("f1")) + Convert.ToDouble(idx.GetValue("f2")));
            idx.Write("AddFloat结果:" + so.GetValue("result").ToString());
            return so;
        }
        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'value','type':'INT','defult':1,'enumdatas':'','tiptext':'设置值'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'INT','defult':'','enumdatas':'','tiptext':'设置的值'}" +
            "]}",
            "设置一个整形值", functionName:"设置一个整形值")]
        public static ScriptOutput SetIntValue(ScriptInput idx)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", Convert.ToInt32(idx.GetValue("value")));
            return so;
        }

        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'value','type':'FLOAT','defult':1,'enumdatas':'','tiptext':'设置值'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'FLOAT','defult':'','enumdatas':'','tiptext':'设置的值'}" +
            "]}",
            "设置一个浮点型值", functionName: "设置一个浮点型值")]
        public static ScriptOutput SetFloatValue(ScriptInput idx)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", Convert.ToDouble(idx.GetValue("value")));
            return so;
        }

        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'value','type':'BOOL','defult':true,'enumdatas':'','tiptext':'设置值'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'BOOL','defult':'','enumdatas':'','tiptext':'设置的值'}" +
            "]}",
            "设置一个bool值", functionName: "设置一个bool值")]
        public static ScriptOutput SetBoolValue(ScriptInput idx)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", Convert.ToBoolean(idx.GetValue("value")));
            return so;
        }


    }
}
