using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;

namespace TKScripts.Manager.ScriptManage.ManagerPlugin
{
    /// <summary>
    /// 任务项
    /// </summary>
    public class TaskItem : INotifyPropertyChanged
    {

        /// <summary>
        /// 次数
        /// </summary>
        private int times = 0;
        /// <summary>
        /// 脚本名称
        /// </summary>
        private string scriptName = "";
        /// <summary>
        /// 脚本对象
        /// </summary>
        [JsonIgnore]
        public IScriptLayout ScriptLayout { get; set; }

        /// <summary>
        /// 脚本名称
        /// </summary>
        public string ScriptName
        {
            get
            {
                if(ScriptLayout == null)
                    return scriptName;
                else
                {
                    return ScriptLayout.ScriptName;
                }
            }
            set
            {
                scriptName = value;
            }
        }
        /// <summary>
        /// 脚本状态
        /// </summary>
        public string Status
        {
            get
            {
                if (ScriptLayout == null)
                    return "脚本异常!";
                else{
                    return ScriptLayout.IScriptInterpreter.IsRunning ? "正在执行" : "没有运行";
                }
            }
        }
        /// <summary>
        /// 次数
        /// </summary>
        public int Times { get => times; set { times = value; Changed("Times"); } }

        /// <summary>
        /// 属性改变通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 改变通知
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    /// <summary>
    /// 脚本任务
    /// </summary>
    public class ScriptTask : INotifyPropertyChanged
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string name = "";
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                Changed("Name");
            }
        }
        

        /// <summary>
        /// 脚本任务列表
        /// </summary>
        public ObservableCollection<TaskItem> TaskItems { get; set; } = new ObservableCollection<TaskItem>();

        /// <summary>
        /// 属性改变通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 改变通知
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
