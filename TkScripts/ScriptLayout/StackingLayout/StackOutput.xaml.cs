using System;
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
using TkScripts.Interface;

namespace TkScripts.ScriptLayout.StackingLayout
{
    /// <summary>
    /// StackOutput.xaml 的交互逻辑
    /// </summary>
    public partial class StackOutput : UserControl
    {

        #region 属性
        /// <summary>
        /// 参数
        /// </summary>
        private ParatItem paramter = null;
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        public StackOutput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="ips"></param>
        public void SetOrigValues(ObservableCollection<IPropertyIt> ips, IParatItem ip)
        {
            propertyCombox.ItemsSource = ips;
            propertyCombox.DisplayMemberPath = "Name";
            //output.DataContext = ip;
            //output.InitParatItemLayout();
            this.DataContext = ip;
            paramter = ip as ParatItem;
            if(paramter.LinkIProperty != null)
            {
                this.isopen.IsChecked = true;
            }
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isopen_Unchecked(object sender, RoutedEventArgs e)
        {
            paramter.LinkIProperty = null;
        }
    }
}
