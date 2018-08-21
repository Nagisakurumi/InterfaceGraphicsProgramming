using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TKScripts.Manager
{
    /// <summary>
    /// SystemLogBox.xaml 的交互逻辑
    /// </summary>
    public partial class SystemLogBox : UserControl
    {
        public SystemLogBox()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控件key
        /// </summary>
        public string ControlKey { get; internal set; }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logmessage"></param>
        public void WritLog(string logmessage)
        {
            this.Dispatcher.Invoke(new Action<string>((s) => {
                LogText.Text += "[" + DateTime.Now.ToString() + "] : " + s + "\n";
                LogRich.ScrollToEnd();
            }), logmessage);
        }
    }
}
