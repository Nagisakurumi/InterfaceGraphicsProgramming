using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// ScriptDebugWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptDebugWindow : UserControl
    {
        /// <summary>
        /// debug 监视内容
        /// </summary>
        private ObservableCollection<DebugItem> debugItems = new ObservableCollection<DebugItem>();
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptDebugWindow()
        {
            InitializeComponent();
            this.Loaded += ScriptDebugWindow_Loaded;
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptDebugWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.debugTree.ItemsSource = debugItems;

        }
        /// <summary>
        /// 修改监视的数据源
        /// </summary>
        /// <param name="watchobject"></param>
        /// <param name="watchname"></param>
        public void UpdateWatchObjectSource(object watchobject, string watchname)
        {
            this.Dispatcher.Invoke(new Action<object, string>((obj, name) => {
                debugItems.Clear();
                DebugItem baseitem = new DebugItem() { WatchObject = obj, Name = name };
                debugItems.Add(baseitem);
                DebugItem.GetPropertyWatchForObject(baseitem);
            }), watchobject, watchname);
        }
        
    }
}
