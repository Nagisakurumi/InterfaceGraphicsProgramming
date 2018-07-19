using System;
using System.Collections.Generic;
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
        /// 脚本对象
        /// </summary>
        public IScriptLayout ScriptLayout { get; set; }

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
}
