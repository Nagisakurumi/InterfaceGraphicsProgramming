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

        /// <summary>
        /// 脚本对象
        /// </summary>
        public static readonly DependencyProperty ScriptLayoutProperty = DependencyProperty.Register
            ("ScriptLayout", typeof(IScriptLayout), typeof(IParaItemControl));

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
        /// <summary>
        /// 脚本源
        /// </summary>
        public IScriptLayout ScriptLayout
        {
            get { return (IScriptLayout)GetValue(ScriptLayoutProperty); }
            set { SetValue(ScriptLayoutProperty, value); }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
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

            this.propertyCombox.ItemsSource = ScriptLayout.IPropertys;
            this.propertyCombox.DisplayMemberPath = "Name";
            if ((source as ParatItem) != null &&
                (source as ParatItem).LinkIProperty != null)
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
            (source as ParatItem).LinkIProperty = null;
        }
    }
}
