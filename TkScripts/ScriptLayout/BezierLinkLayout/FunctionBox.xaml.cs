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
using TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout;

namespace TkScripts.ScriptLayout.BezierLinkLayout
{
    /// <summary>
    /// FunctionBox.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionBox : IFunctionBox
    {
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionBox_Loaded(object sender, RoutedEventArgs e)
        {
            InitFunction();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void InitFunction()
        {
            if (ibox != null) return;
            ibox = this.DataContext as IItemBox;
            if (ibox == null) return;
            foreach (var item in ibox.InputDatas)
            {
                item.UIMain = this.UIMain;
                AddInputData(item);
            }
            foreach (var item in ibox.OutDatas)
            {
                item.UIMain = this.UIMain;
                AddOutputData(item);
            }
            ibox.InputDatas.CollectionChanged += InputDatas_CollectionChanged;
            ibox.OutDatas.CollectionChanged += OutDatas_CollectionChanged;
        }
        /// <summary>
        /// 集合改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutDatas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }
        /// <summary>
        /// 集合改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputDatas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// 添加一个输入
        /// </summary>
        /// <param name="ipt"></param>
        public override void AddInputData(IParatItem ipt)
        {
            InputParaItemLayout ipl = new InputParaItemLayout();
            ipl.Margin = new Thickness(4, 4, 4, 4);
            ipl.MouseDown += ParaItemLayout_MouseDown;
            ipl.MouseUp += ParaItemLayout_MouseUp;
            ipl.MouseMove += ParaItemLayout_MouseMove;
            ipl.MouseEnter += ParaItemLayout_MouseEnter;
            ipl.MouseLeave += ParaItemLayout_MouseLeave;
            ipl.Height = 25;
            ipl.DataContext = ipt;
            ipl.UIMain = this.UIMain;
            ipl.InitParatItemLayout();
            left.Children.Add(ipl);
        }

        /// <summary>
        /// 添加一个输出
        /// </summary>
        /// <param name="ipt"></param>
        public override void AddOutputData(IParatItem ipt)
        {
            OutputParaItemLayout opl = new OutputParaItemLayout();
            opl.Margin = new Thickness(4, 4, 4, 4);
            opl.MouseDown += ParaItemLayout_MouseDown;
            opl.MouseUp += ParaItemLayout_MouseUp;
            opl.MouseMove += ParaItemLayout_MouseMove;
            opl.MouseEnter += ParaItemLayout_MouseEnter;
            opl.MouseLeave += ParaItemLayout_MouseLeave;
            opl.Height = 25;
            opl.DataContext = ipt;
            opl.HorizontalAlignment = HorizontalAlignment.Right;
            opl.UIMain = this.UIMain;
            opl.InitParatItemLayout();
            right.Children.Add(opl);
        }

        private MLParatItemLayout mouseMoveUI = null;
        /// <summary>
        /// 鼠标悬停的元素
        /// </summary>
        public MLParatItemLayout MouseMoveUI
        {
            get
            {
                return mouseMoveUI;
            }
        }
        /// <summary>
        /// 控件被移动
        /// </summary>
        public void MoveChange()
        {
            foreach (var item in left.Children)
            {
                (item as MLParatItemLayout).OpPositionChanged();
            }
            foreach (var item in right.Children)
            {
                (item as MLParatItemLayout).OpPositionChanged();
            }
        }
        /// <summary>
        /// 框的区域
        /// </summary>
        public Rect Rect
        {
            get
            {
                return new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), this.ActualWidth, this.ActualHeight);
            }
        }

        public FunctionBox()
        {
            InitializeComponent();
            this.Loaded += FunctionBox_Loaded;
        }

        private void ParaItemLayout_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParaItemLayout_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParaItemLayout_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void ParaItemLayout_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseMoveUI = sender as MLParatItemLayout;
            //Console.WriteLine("进入" + index);
        }

        private void ParaItemLayout_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseMoveUI = null;
            //Console.WriteLine("离开" + index++);
        }
        /// <summary>
        /// 输入的元素集合
        /// </summary>
        public override UIElementCollection Inputs
        {
            get
            {
                return left.Children;
            }
        }
        /// <summary>
        /// 输出元素集合
        /// </summary>
        public override UIElementCollection Outputs
        {
            get
            {
                return right.Children;
            }
        }
    }
}
