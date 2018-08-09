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

namespace TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout
{
    /// <summary>
    /// OutputParaItemLayout.xaml 的交互逻辑
    /// </summary>
    public partial class OutputParaItemLayout : MLParatItemLayout
    {
        public OutputParaItemLayout()
        {
            InitializeComponent();
            this.Loaded += OutputParaItemLayout_Loaded;
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputParaItemLayout_Loaded(object sender, RoutedEventArgs e)
        {
            InitParatItemLayout();

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void InitParatItemLayout()
        {
            if (ipt != null) return;
            ipt = this.DataContext as IParatItem;
            if (ipt == null) return;
            //this.ToolTip = new TextBlock() { Text = ipt.TipText };
        }




    }
}
