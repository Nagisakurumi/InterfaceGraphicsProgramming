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
using TkScripts.Interface;
using TkScripts.ScriptLayout.StackingLayout;

namespace TKScripts.Manager
{
    /// <summary>
    /// ScriptItem.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptItem : UserControl
    {
        public ScriptItem()
        {
            InitializeComponent();
            this.Loaded += ScriptItem_Loaded;
            this.LostFocus += ScriptItem_LostFocus;
        }
        /// <summary>
        /// 失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptItem_LostFocus(object sender, RoutedEventArgs e)
        {
            this.scriptName.BorderThickness = new Thickness(0);
            this.scriptName.Background = null;
            this.scriptName.IsEnabled = false;
        }



        /// <summary>
        /// 项双击事件
        /// </summary>
        public event ItemEvent ItemMouseDoubleClickEvent = null;
        /// <summary>
        /// 脚本名称修改
        /// </summary>
        public event ItemEvent ItemScriptNameChanged = null;
        /// <summary>
        /// 前一次点击
        /// </summary>
        private DateTime forntTime = DateTime.Now;
        /// <summary>
        /// 绑定的脚本
        /// </summary>
        private StackingMainLayout script = null;
        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_OnceMouseDown(object sender, MouseButtonEventArgs e)
        {
            StackingMainLayout spt = ((sender as FrameworkElement).DataContext as StackingMainLayout);
            if (spt.ScriptName == script.ScriptName)
            {
                double seconds = (DateTime.Now - forntTime).TotalSeconds;
                if (seconds > 0.4 && seconds < 1.8)
                {
                    this.scriptName.BorderThickness = new Thickness(2);
                    this.scriptName.Background = new SolidColorBrush(Colors.White);
                    this.scriptName.IsEnabled = true;
                }
                else if (seconds <= 0.4)
                {
                    ItemMouseDoubleClickEvent?.Invoke(this.DataContext);
                }
                Console.WriteLine("时间:" + seconds);
                Console.WriteLine("对象:" + spt.ScriptName + ",本对象:" + script.ScriptName);
                forntTime = DateTime.Now;
                //forntObject = scriptname;
            }
        }
        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scriptName_LostFocus(object sender, RoutedEventArgs e)
        {
            this.scriptName.BorderThickness = new Thickness(0);
            this.scriptName.Background = null;
            this.scriptName.IsEnabled = false;
        }
        /// <summary>
        /// 按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scriptName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.scriptName.BorderThickness = new Thickness(0);
                this.scriptName.Background = null;
                this.scriptName.IsEnabled = false;
            }
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptItem_Loaded(object sender, RoutedEventArgs e)
        {
            script = this.DataContext as StackingMainLayout;
            if(script != null)
            {
                script.PropertyChanged += Script_PropertyChanged;
            }
        }
        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Script_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "ScriptName")
            {
                ItemScriptNameChanged?.Invoke(this.DataContext);
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
