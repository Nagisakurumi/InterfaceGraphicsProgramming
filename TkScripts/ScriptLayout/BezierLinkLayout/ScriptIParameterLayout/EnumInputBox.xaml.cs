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
    /// EnumInputBox.xaml 的交互逻辑
    /// </summary>
    public partial class EnumInputBox : MLInputBox
    {
        public EnumInputBox()
        {
            InitializeComponent();
        }

        public void SetSource(IList<string> sources)
        {
            this.value.ItemsSource = sources;
        }

        public string Value
        {
            get
            {
                return value.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="ipt"></param>
        public override void SetValueBind(IParatItem ipt)
        {
            Tools.Bind(ipt, value, "Value", ComboBox.SelectedItemProperty);
        }
    }
}
