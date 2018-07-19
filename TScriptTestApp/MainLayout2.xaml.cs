
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TkScripts.Script;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.StackingLayout;
using TKScripts.Manager;

namespace TScriptTestApp
{
    /// <summary>
    /// MainLayout2.xaml 的交互逻辑
    /// </summary>
    public partial class MainLayout2 : Window
    {
        public MainLayout2()
        {
            InitializeComponent();
        }

        #region 属性
        /// <summary>
        /// 脚本运行线程
        /// </summary>
        private Thread scriptRun = null;
        /// <summary>
        /// 是否在运行
        /// </summary>
        protected bool isRunning = false;
        #endregion



        /// <summary>
        /// 脚本内容点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scriptContent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            scriptContent.Focus();
        }
        /// <summary>
        /// 写出日志
        /// </summary>
        /// <param name="log"></param>
        protected void WriteLog(string log)
        {
            this.Dispatcher.Invoke(new Action<string>((s) => {
                //LogText.Text += "[" + DateTime.Now.ToString() + "] : " + s + "\n";
            }), log);
        }

        /// <summary>
        /// 按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_KeyDown(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_MouseMove(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            scriptRun = new Thread(() => {
                StackScriptOption option = new StackScriptOption();
                //option.RunScript(this.scriptContent, WriteLog, WriteLog);
                option = null;
            })
            { IsBackground = true, };
            scriptRun.Start();
        }
        /// <summary>
        /// 停止正在运行的脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (scriptRun != null)
            {
                scriptRun.Abort();
            }
        }

        #region 方法
        /// <summary>
        /// 设置设备对象
        /// </summary>
        /// <param name="device"></param>
        public void SetDevice()
        {
            //scriptContent.SetFunctionData(GetInterfaces(device));
        }

        #endregion
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            scriptContent.RunActiveScript();
        }
    }
}
