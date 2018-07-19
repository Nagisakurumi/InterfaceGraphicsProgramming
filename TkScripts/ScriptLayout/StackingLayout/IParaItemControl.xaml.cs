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
using TkScripts.Reflexui;

namespace TkScripts.ScriptLayout.StackingLayout
{
    /// <summary>
    /// IParatemControl.xaml 的交互逻辑
    /// </summary>
    public partial class IParaItemControl : UserControl
    {

        private IParatItem source = null;
        /// <summary>
        /// 是否作为输入的参数框
        /// </summary>
        private bool isAsInput = true;
        /// <summary>
        /// 数据源
        /// </summary>
        public IParatItem Source
        {
            get
            {
                return source;
            }
        }
        /// <summary>
        /// 是否作为输入的参数框
        /// </summary>
        public bool IsAsInput
        {
            get
            {
                return isAsInput;
            }

            set
            {
                isAsInput = value;
            }
        }

        public IParaItemControl()
        {
            InitializeComponent();
            this.Loaded += IParatemControl_Loaded;
        }

        private void IParatemControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (source != null) return;
            source = this.DataContext as IParatItem;
            if (source == null) return;
            if (IsAsInput)
            {
                IParaItemForEnum forenum = IParaItemForEnum.GetInstance();
                IReflexui iref = forenum.GetControlType(source.PIEnum);
                if (iref != null)
                {
                    iref.Bind(source, "Value");
                    this.content.Child = iref as Control;
                }
            }

            this.propertyCombox.ItemsSource = source.ParentItemBox.ScriptLayout.IPropertys;
            this.propertyCombox.DisplayMemberPath = "Name";
            if((source as StackParatItem) != null &&
                (source as StackParatItem).LinkIProperty != null)
            {
                isopen.IsChecked = true;
            }
            else
            {
                isopen.IsChecked = false;
            }
        }

        /// <summary>
        /// 取消选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isopen_Unchecked(object sender, RoutedEventArgs e)
        {
            if (source == null) return;
            (source as StackParatItem).LinkIProperty = null;
        }
    }
}
