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

namespace TkScripts.ScriptLayout
{
    /// <summary>
    /// DataTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class DataTreeView : UserControl
    {
        internal DataTreeView()
        {
            InitializeComponent();
        }


        #region 内部属性
        /// <summary>
        /// 点击时间
        /// </summary>
        private DateTime _clickTime = new DateTime();
        /// <summary>
        /// 真实与树绑定的数据源
        /// </summary>
        private ObservableCollection<TreeData> _myData = new ObservableCollection<TreeData>();
        /// <summary>
        /// 存放所有数据用于查询的优化
        /// </summary>
        private List<TreeData> _myDataList = new List<TreeData>();
        /// <summary>
        /// 创建
        /// </summary>
        public event CreateCallbackHandle CreateCallback = null;
        #endregion
        #region 属性读取器
        /// <summary>
        /// 控件key
        /// </summary>
        public string ControlKey { get; set; }
        /// <summary>
        /// 树状数据
        /// </summary>
        public ObservableCollection<TreeData> MyData
        {
            get { return _myData; }
            set
            {
                _myData = value;
                this.XTreeView.ItemsSource = this._myData;
                _myData.CollectionChanged += _myData_CollectionChanged;
                UpdateCheckData();
            }
        }
        
        #endregion
        #region 自定义函数
        /// <summary>
        /// 创建代码块
        /// </summary>
        protected void CreateCodeBox()
        {
            TreeData td = (TreeData)XTreeView.SelectedItem;
            ///如果不是最终节点则退出
            if (td.IsDataNode == false)
            {
                return;
            }
            CreateCallback?.Invoke(td.Data);
        }
        /// <summary>
        /// 根据名字获取内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TreeData GetTreeDataByName(string name)
        {
            foreach (var item in _myDataList)
            {
                if(item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }
        #region 新方法
        /// <summary>
        /// 更新用于查询的数据
        /// </summary>
        protected void UpdateCheckData()
        {
            _myDataList.Clear();
            foreach (TreeData item in MyData)
            {
                ToAddListData(item);
            }
        }
        /// <summary>
        /// 递归添加项
        /// </summary>
        /// <param name="item">检查的项</param>
        private void ToAddListData(TreeData item)
        {
            _myDataList.Add(item);
            ///递归退出条件
            if (item.Children.Count == 0)
            {
                return;
            }
            foreach (TreeData child in item.Children)
            {
                ToAddListData(child);
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="key">要查询的关键词</param>
        protected void SearchDataNode(string key)
        {
            foreach (TreeData item in _myDataList)
            {
                item.IsExpanded = true;
                if (item.IsDataNode && item.Name.IndexOf(key, StringComparison.CurrentCultureIgnoreCase) == -1)
                {
                    item.IsVisiblity = Visibility.Collapsed;
                }
                else
                {
                    item.IsVisiblity = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// 当没有查询的时候变回所有的数据
        /// </summary>
        protected void ReturnAllData()
        {
            foreach (TreeData item in _myDataList)
            {
                item.IsExpanded = false;
                item.IsVisiblity = Visibility.Visible;
            }
        }
        #endregion
        #endregion
        #region 事件绑定
        /// <summary>
        /// 容器内容改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _myData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCheckData();
        }
        /// <summary>
        /// 当搜索框的内容更新的时候修改树的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == null || tb.Text == "")
            {
                ReturnAllData();
            }
            else
            {
                SearchDataNode(tb.Text);
            }
        }
        /// <summary>
        /// 选项改变的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
        /// <summary>
        /// 键盘抬起的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CreateCodeBox();
            }
        }
        /// <summary>
        /// 左击按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ///获取当前时间
            DateTime nowTime = DateTime.Now;
            TimeSpan times = nowTime - _clickTime;
            if (times.TotalMilliseconds <= 200)
            {
                CreateCodeBox();
            }
            ///更新点击时间
            _clickTime = DateTime.Now;
        }
        #endregion
    }
}
