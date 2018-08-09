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
    /// ValueInputBox.xaml 的交互逻辑
    /// </summary>
    public partial class ValueInputBox : MLInputBox
    {
        public ValueInputBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public double Value
        {
            get
            {
                try
                {
                    return Convert.ToDouble(value.Text);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set
            {
                this.value.Text = value.ToString();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="ipt"></param>
        public override void SetValueBind(IParatItem ipt)
        {
            Tools.Bind(ipt, value, "Value", TextBox.TextProperty);
        }


    }
}
