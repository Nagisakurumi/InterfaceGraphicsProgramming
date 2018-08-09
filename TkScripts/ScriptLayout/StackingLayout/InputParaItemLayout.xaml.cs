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
using TKScriptsServer.Agreement;

namespace TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout
{
    /// <summary>
    /// InputParaItemLayout.xaml 的交互逻辑
    /// </summary>
    public partial class InputParaItemLayout : MLParatItemLayout
    {
        public InputParaItemLayout()
        {
            InitializeComponent();
            this.Loaded += InputParaItemLayout_Loaded;
        }
        
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputParaItemLayout_Loaded(object sender, RoutedEventArgs e)
        {
            InitParatItemLayout();
            //LinksChange();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void InitParatItemLayout()
        {
            if (ipt != null) return;
            ipt = this.DataContext as IParatItem;
            if (ipt == null) return;
            if (ipt.PIEnum == ParaItemEnum.INT || ipt.PIEnum == ParaItemEnum.FLOAT ||
                ipt.PIEnum == ParaItemEnum.STRING)
            {
                ValueInputBox vib = new ValueInputBox();
                vib.DataContext = ipt;
                //vib.Value = Convert.ToDouble(ipt.Value);
                this.inputContent.Children.Add(vib);
            }
            else if (ipt.PIEnum == ParaItemEnum.ENUM || ipt.PIEnum == ParaItemEnum.BOOL)
            {
                EnumInputBox eib = new EnumInputBox();
                eib.DataContext = ipt;
                //eib.SetSource(ipt.EnumDatas);
                this.inputContent.Children.Add(eib);
            }
            //this.ToolTip = new TextBlock() { Text = ipt.TipText };
        }
        
    }
}
