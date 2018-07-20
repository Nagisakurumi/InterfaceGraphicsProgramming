using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace TkScripts.Script
{
    /// <summary>
    /// 脚本函数规范
    /// </summary>
    /// <param name="scriptInput"></param>
    /// <returns></returns>
    public delegate ScriptOutput ScriptFunction(ScriptInput scriptInput);
    /// <summary>
    /// 日志写入回调
    /// </summary>
    /// <param name="log"></param>
    public delegate void WriteStreamCallBack(string log);
    /// <summary>
    /// 脚本参数类
    /// </summary>
    public class TaskScript : IDisposable
    {
        /// <summary>
        /// 用于保存返回的结果
        /// </summary>
        public Dictionary<string, object> datas = new Dictionary<string, object>();
        /// <summary>
        /// 日志
        /// </summary>
        [JsonIgnore]
        public WriteStreamCallBack WriteStream = null;
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            if (datas != null)
            {
                datas.Clear();
            }
            datas = null;
            WriteStream = null;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">数据对应的key</param>
        /// <returns>返回对应的数据</returns>
        public object GetValue(string key)
        {
            if (datas.ContainsKey(key))
            {
                return datas[key];
            }
            return null;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, object value)
        {
            if(datas.ContainsKey(key))
            {
                datas[key] = null;
                datas[key] = value;
                return true;
            }
            else
            {
                datas.Add(key, value);
                return true;
            }
        }

        /// <summary>
        /// 获取第一个值
        /// </summary>
        /// <returns></returns>
        public object GetFirst()
        {
            if(datas.Count > 0)
            {
                return datas.First().Value;
            }
            return null;
        }
        /// <summary>
        /// 获取第二个
        /// </summary>
        /// <returns></returns>
        public object GetSecond()
        {
            int idx = 0;
            foreach (var item in datas)
            {
                if (idx == 1)
                    return item.Value;
                idx++;
            }
            return null;
        }
        /// <summary>
        /// 写入输出日志
        /// </summary>
        /// <param name="log"></param>
        public void Write(string log)
        {
            WriteStream?.Invoke(log);
        }
    }

    /// <summary>
    /// 返回值
    /// </summary>
    public class ScriptOutput : TaskScript
    {
        /// <summary>
        /// 是否出现运行异常
        /// </summary>
        private bool isExecption = false;
        /// <summary>
        /// 是否出现运行异常
        /// </summary>
        public bool IsExecption
        {
            get
            {
                return isExecption;
            }

            set
            {
                isExecption = value;
            }
        }
    }
    /// <summary>
    /// 参数
    /// </summary>
    public class ScriptInput : TaskScript
    {

    }
    /// <summary>
    /// 属性映射类
    /// </summary>
    public struct DataContext
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type;
        /// <summary>
        /// 参数默认值
        /// </summary>
        public object DefultValue;
        /// <summary>
        /// 参数枚举列表(参数的可选值)
        /// </summary>
        public List<string> EnumDatas;
        /// <summary>
        /// 参数的提示文字
        /// </summary>
        public string TipText;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defultvalue"></param>
        /// <param name="enumdatas"></param>
        public DataContext(string name, string type, object defultvalue = null, List<string> enumdatas = null, string tipText = "")
        {
            this.Name = name;
            this.Type = type;
            this.DefultValue = defultvalue;
            this.EnumDatas = enumdatas;
            this.TipText = tipText;
        }
    }
    /// <summary>
    /// 脚本函数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptMethAttribute : Attribute
    {
        /// <summary>
        /// 所有类型
        /// </summary>
        private Dictionary<string, Type> types = new Dictionary<string, Type>();

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe = "";
        /// <summary>
        /// 名称
        /// </summary>
        public string Name = "";
        /// <summary>
        /// 代码块类型
        /// </summary>
        public ItemBoxEnum ItemBoxEnum = ItemBoxEnum.FUNCTION;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inputAttribute">输入数据</param>
        /// <param name="outputAttribute">输出数据</param>
        /// <param name="describe">描述</param>
        /// <param name="type1"><类型一/param>
        /// <param name="type2">类型二</param>
        /// <param name="type3">类型三</param>
        /// <param name="ishasInput">是否有输入连接</param>
        /// <param name="ishasOutput">是否有输出连接</param>
        public ScriptMethAttribute(string inputAttribute, string outputAttribute, string describe, Type [] type = null, bool ishasInput = false, bool ishasOutput = false, string functionName = "", ItemBoxEnum itemBoxEnum = ItemBoxEnum.FUNCTION)
        {
            if (type != null)
            {
                foreach (var item in type)
                {
                    types.Add(item.FullName, item);
                }
                type = null;
            }
            if (ishasInput)
            {
                InputData.Add(new DataContext("输入", "INPUT"));
            }
            if(ishasOutput)
            {
                OutputData.Add(new DataContext("输出", "OUTPUT"));
            }
            if(inputAttribute.Equals("") == false)
                add(InputData, (JObject)JsonConvert.DeserializeObject(inputAttribute));
            if (outputAttribute.Equals("") == false)
                add(OutputData, (JObject)JsonConvert.DeserializeObject(outputAttribute));
            inputAttribute = null;
            outputAttribute = null;
            this.Describe = describe;
            ItemBoxEnum = itemBoxEnum;
            Name = functionName;
        }

        /// <summary>
        /// 转换json到数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="json"></param>
        protected void add(List<DataContext> data, JObject json)
        {
            JToken jk = json["parameter"];
            foreach (var item in jk)
            {
                data.Add(new DataContext(item["name"].ToString(), item["type"].ToString(), 
                    item["defult"].ToString(), getEnumDatas(item["enumdatas"]), item["tiptext"].ToString()));
            }
            json = null;
        }
        /// <summary>
        /// 获取枚举数据源
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected List<string> getEnumDatas(JToken value)
        {
            if(value == null || value.ToString() == "")
            {
                return null;
            }
            else if(value["type"].ToString() == "value")
            {
                return value["value"].ToString().Split(',').ToList();
            }
            else if(value["type"].ToString() == "index")
            {
                return GetEnumListString(getIndexType(Convert.ToInt32(value["value"].ToString())));
            }
            else
            {
                if(value["type"].ToString() == "type" && types.ContainsKey(value["value"].ToString()))
                    return GetEnumListString(types[value["value"].ToString()]);
            }
            return null;
        }

        /// <summary>
        /// 获取序号中的tyoe
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns></returns>
        protected Type getIndexType(int index)
        {
            int idx = 0;
            foreach (var item in types)
            {
                if(idx == index)
                {
                    return item.Value;
                }
                idx++;
            }
            return null;
        }
        /// <summary>
        /// 输入参数
        /// </summary>
        public List<DataContext> InputData { get; } = new List<DataContext>();

        /// <summary>
        /// 输出参数
        /// </summary>
        public List<DataContext> OutputData { get; } = new List<DataContext>();

        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetEnumListString(Type tp)
        {
            List<string> ls = new List<string>();
            foreach (var item in Enum.GetValues(tp))
            {
                ls.Add(item.ToString());
            }
            return ls;
        }
    }
}
