using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;

namespace TKScripts.Manager
{
    /// <summary>
    /// 脚本配置
    /// </summary>
    public class ScriptConfig
    {
        /// <summary>
        /// 脚本列表
        /// </summary>
        public List<string> ScriptFiles { get; set; } = new List<string>();
        /// <summary>
        /// 全局属性
        /// </summary>
        public ObservableCollection<IPropertyIt> PropertyIts { get; set; } = new ObservableCollection<IPropertyIt>();
    }
}
