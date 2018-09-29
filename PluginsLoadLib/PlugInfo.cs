using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginsLoadLib
{
    public class PlugInfo
    {
        /// <summary>
        /// 插件
        /// </summary>
        public Assembly Assembly { get; set; }
        /// <summary>
        /// 插件路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// mainclass的type
        /// </summary>
        public Type MainType { get { return MainClass.GetType(); } }
        /// <summary>
        /// 入口类
        /// </summary>
        public object MainClass { get; set; }
        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get { return FileInfo.Name; } }
        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version { get { return Assembly.ImageRuntimeVersion; } }
        /// <summary>
        /// 执行mainclass中的方法
        /// </summary>
        /// <param name="functionname"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void DoFunctionFromMainClass(string functionname, params object [] param)
        {
            if(MainClass == null)
            {
                throw new Exception("mainclass 为null");
            }
            MainType.GetMethod(functionname).Invoke(this.MainClass, param.ToArray());
        }
    }
}
