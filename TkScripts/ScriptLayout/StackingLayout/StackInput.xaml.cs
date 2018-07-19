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
using TkScripts.ScriptLayout.StackingLayout;

namespace TkScripts.ScriptLayout.StackingLayout
{
    /// <summary>
    /// StackInput.xaml 的交互逻辑
    /// </summary>
    public partial class StackInput : UserControl
    {
        public StackInput()
        {
            InitializeComponent();
            this.Loaded += StackInput_Loaded;
        }
        #region 属性
        /// <summary>
        /// 参数
        /// </summary>
        private StackParatItem paramter = null;
        #endregion
        #region 访问器

        #endregion
        #region 事件
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackInput_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="fbs"></param>
        public void SetOrigValues(ObservableCollection<IPropertyIt> ips, IParatItem ip)
        {
            this.propertyCombox.ItemsSource = ips;
            this.propertyCombox.DisplayMemberPath = "Name";
            if (ips.Count > 0)
            {
                propertyCombox.SelectedIndex = 0;
            }

            input.DataContext = ip;
            input.InitParatItemLayout();
            this.DataContext = ip;
            paramter = ip as StackParatItem;
        }
        /// <summary>
        /// 取消选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isopen_Unchecked(object sender, RoutedEventArgs e)
        {
            paramter.LinkIProperty = null;
        }
        #endregion


    }
}
