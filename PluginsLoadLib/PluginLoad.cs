using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginsLoadLib
{
    /// <summary>
    /// 插件加载类
    /// </summary>
    public class PluginLoad
    {
        /// <summary>
        /// 加载插件的路径
        /// </summary>
        public string LoadPathDirectory { get; set; } = "";
        /// <summary>
        /// 作为插件的dll的专有包含名称
        /// </summary>
        public string PluginNameEx { get; set; } = "";
        /// <summary>
        /// 作为插件的入口类的名称
        /// </summary>
        public string PluginMainInputClassName { get; set; } = "";
        /// <summary>
        /// 插件的集合
        /// </summary>
        public List<PlugInfo> Plugins { get; } = new List<PlugInfo>();
        /// <summary>
        /// 构造函数
        /// </summary>
        public PluginLoad()
        {

        }
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <returns></returns>
        public void LoadPlugins()
        {
            if(LoadPathDirectory.Equals("") 
                || !Directory.Exists(LoadPathDirectory))
            {
                throw new Exception("加载插件异常!给定的插件路径异常!");
            }
            ///加载插件
            load(LoadPathDirectory);
        }

        /// <summary>
        /// 从文件夹中加载插件(递归的方式)
        /// </summary>
        /// <param name="path"></param>
        private void load(string path)
        {
            string [] files = Directory.GetFiles(path);
            string[] directorys = Directory.GetDirectories(path);

            foreach (var item in files)
            {
                ///满足是dll并且带有插件名称字样的dll加载
                if (Path.GetFileName(item).Contains(PluginNameEx) &&
                    Path.GetExtension(item).Equals(".dll"))
                {
                    Assembly assembly = Assembly.LoadFrom(item);
                    PlugInfo plugInfo = new PlugInfo();
                    plugInfo.FileInfo = new FileInfo(path);
                    plugInfo.Assembly = assembly;
                    plugInfo.Path = path;
                    IEnumerable<Type> types = assembly.GetExportedTypes().Where(p => p.Name.Equals(this.PluginMainInputClassName));
                    if (types.Count() == 0) continue;
                    plugInfo.MainClass = Activator.CreateInstance(
                        types.First()
                        );
                    this.Plugins.Add(plugInfo);
                }
            }

            foreach (var item in directorys)
            {
                load(item);
            }
        }
    }
}
