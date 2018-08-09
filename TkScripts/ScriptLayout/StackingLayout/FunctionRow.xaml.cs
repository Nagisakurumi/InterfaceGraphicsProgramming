using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
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
    /// FunctionRow.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionRow : IFunctionBox
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FunctionRow()
        {
            InitializeComponent();
            this.Loaded += FunctionBox_Loaded;
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionBox_Loaded(object sender, RoutedEventArgs e)
        {
            InitFunction();
        }
        #region 属性

        //private BitmapImage bimage = new BitmapImage();
        #endregion
        #region 访问器

        #endregion
        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        public override void InitFunction()
        {
            if (ibox != null) return;
            ibox = this.DataContext as ItemBox;
            if (ibox == null) return;
            foreach (var item in Ibox.InputDatas)
            {
                AddInputData(item);
            }
            foreach (var item in Ibox.OutDatas)
            {
                AddOutputData(item);
            }
            Ibox.InputDatas.CollectionChanged += InputDatas_CollectionChanged;
            Ibox.OutDatas.CollectionChanged += OutDatas_CollectionChanged;
            //img.Source = ibox.LogoPath;
            //Tools.Bind(ibox, this, "Level", UserControl.MarginProperty, convert:new ThicknessCovert());
        }
        /// <summary>
        /// 添加输入参数
        /// </summary>
        /// <param name="ipt"></param>
        public override void AddInputData(IParatItem ipt)
        {
            //StackInput si = new StackInput();
            //si.SetOrigValues(iPropertys, ipt);
            //si.Margin = new Thickness(2);
            //si.Height = 25;
            //left.Children.Add(si);
        }
        /// <summary>
        /// 添加输出参数
        /// </summary>
        /// <param name="ipt"></param>
        public override void AddOutputData(IParatItem ipt)
        {
            //StackOutput si = new StackOutput();
            //si.SetOrigValues(iPropertys, ipt);
            //si.Margin = new Thickness(2);
            //si.Height = 25;
            //right.Children.Add(si);
        }
        ///// <summary>
        ///// 输入参数
        ///// </summary>
        //public override UIElementCollection Inputs
        //{
        //    get
        //    {
        //        return left.Children;
        //    }
        //}
        ///// <summary>
        ///// 输出参数
        ///// </summary>
        //public override UIElementCollection Outputs
        //{
        //    get
        //    {
        //        return right.Children;
        //    }
        //}
        #region 事件
        /// <summary>
        /// 集合改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutDatas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        /// <summary>
        /// 集合改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputDatas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 转换
    /// </summary>
    public class VisiableAndIsHasStopPoint : IValueConverter
    {
        /// <summary>
        /// 转换到绑定
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool ishas = (bool)value;
                if(ishas)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 转换到数据源
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Visibility vb = (Visibility)value;
                if(vb == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
