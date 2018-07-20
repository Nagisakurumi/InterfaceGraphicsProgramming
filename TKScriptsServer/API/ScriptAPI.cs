using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Script;

namespace TKScriptsServer.API
{
    /// <summary>
    /// 脚本函数
    /// </summary>
    public class ScriptAPI
    {
        /// <summary>
        /// 函数地址
        /// </summary>
        [JsonIgnore]
        public ScriptFunction ScriptFunction { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// apiId
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 脚本信息
        /// </summary>
        public ScriptMethAttribute ScriptMethAttribute { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scriptFunction"></param>
        public ScriptAPI(string baseUrl,ScriptFunction scriptFunction)
        {
            this.Url = baseUrl;
            this.ScriptFunction = scriptFunction;
            MethodInfo minfo = scriptFunction.GetMethodInfo();
            this.Id = minfo.DeclaringType.FullName + minfo.Name;
            ScriptMethAttribute = minfo.GetCustomAttribute(typeof(ScriptMethAttribute), false) as ScriptMethAttribute;
            if(ScriptMethAttribute.Name.Equals(""))
            {
                ScriptMethAttribute.Name = minfo.Name;
            }
        }
    }
}
