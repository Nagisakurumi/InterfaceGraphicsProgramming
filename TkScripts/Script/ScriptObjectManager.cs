using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKScriptsServer.Agreement;

namespace TkScripts.Script
{
    /// <summary>
    /// 脚本运行期对象管理
    /// </summary>
    public class ScriptObjectManager : IDisposable
    {
        /// <summary>
        /// 属性
        /// </summary>
        private Dictionary<string, ScriptOutput> propertys = new Dictionary<string, ScriptOutput>();
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ScriptOutput GetValue(string propertyName)
        {
            if (propertys.ContainsKey(propertyName) == true)
            {
                return propertys[propertyName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private bool AddProperty(string propertyName, ScriptOutput value)
        {
            if (propertys.ContainsKey(propertyName) == false)
            {
                propertys.Add(propertyName, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string propertyName, ScriptOutput value)
        {
            if (propertys.ContainsKey(propertyName) == true)
            {
                propertys[propertyName] = null;
                propertys[propertyName] = value;
                return true;
            }
            else
            {
                return AddProperty(propertyName, value);
            }
        }

        public void Clear()
        {
            foreach (var item in propertys)
            {
                item.Value.Dispose();
            }
            propertys.Clear();
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            foreach (var item in propertys)
            {
                item.Value.Dispose();
            }
            propertys.Clear();
            propertys = null;
        }
    }
}
