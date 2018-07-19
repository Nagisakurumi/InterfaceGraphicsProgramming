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
            LinksChange();
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
            if (ipt.UIMain != null)
            {
                Tools.Bind(ipt, this, "UIMain", MLUIMain.UIMainProperty);
            }
            else if (UIMain == null)
            {
                UIMain = this;
            }
            //this.ToolTip = new TextBlock() { Text = ipt.TipText };
        }
        /// <summary>
        /// 中心点
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point(this.ActualHeight / 2, this.ActualHeight / 2);
            }
        }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius
        {
            get
            {
                return this.ActualHeight / 4;
            }
        }
        /// <summary>
        /// 连接点坐标相对于UIMain的坐标
        /// </summary>
        public override Point LinkPosition
        {
            get
            {
                Point p = cricle.TranslatePoint(new Point(), UIMain);
                return new Point(p.X + Center.X, p.Y + Center.Y);
            }
        }
        
        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Changed("Center");
            Changed("Radius");
        }
        /// <summary>
        /// 取消连接的时候去掉填充色
        /// </summary>
        /// <returns></returns>
        protected override void LinksChange()
        {
            if (this.LinksBezierLine.Count == 0)
            {
                cricle.Background = new SolidColorBrush(Color.FromArgb(00,00,00,00));
            }
            else
            {
                cricle.Background = cricle.BorderBrush;
            }
        }
    }
}
