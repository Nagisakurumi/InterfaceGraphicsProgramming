using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginsLoadLib;
using PluginInterface;

namespace TKScripts.Manager.Plugins
{
    /// <summary>
    /// 插件管理
    /// </summary>
    public class PluginManager
    {
        /// <summary>
        /// 插件加载器
        /// </summary>
        private PluginLoad PluginLoad { get; } = new PluginLoad();
        /// <summary>
        /// 加载插件的路径
        /// </summary>
        public string LoadPathDirectory { get { return PluginLoad.LoadPathDirectory; }
            set { PluginLoad.LoadPathDirectory = value; }
        }
        /// <summary>
        /// 作为插件的dll的专有包含名称
        /// </summary>
        public string PluginNameEx
        {
            get { return PluginLoad.PluginNameEx; }
            set { PluginLoad.PluginNameEx = value; }
        }
        /// <summary>
        /// 作为插件的入口类的名称
        /// </summary>
        public string PluginMainInputClassName
        {
            get { return PluginLoad.PluginMainInputClassName; }
            set { PluginLoad.PluginMainInputClassName = value; }
        }
        /// <summary>
        /// 插件的集合
        /// </summary>
        public List<PluginItem> PluginItems { get; } = new List<PluginItem>();
        /// <summary>
        /// 加载插件
        /// </summary>
        public void LoadPlugins()
        {
            PluginLoad.LoadPlugins();

            foreach (var item in PluginLoad.Plugins)
            {
                PluginItem pluginItem = new PluginItem();
                pluginItem.ScriptPlugin = item.MainClass as ScriptTaskPluginInferface;
                pluginItem.PlugInfo = item;
                this.PluginItems.Add(pluginItem);
            }
        } 
    }
    /// <summary>
    /// 插件
    /// </summary>
    public class PluginItem
    {
        /// <summary>
        /// 插件
        /// </summary>
        public ScriptTaskPluginInferface ScriptPlugin { get; set; }
        /// <summary>
        /// 插件详情
        /// </summary>
        public PlugInfo PlugInfo { get; set; }
    }
}
