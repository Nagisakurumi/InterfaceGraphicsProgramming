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
using TkScripts;
using TkScripts.Interface;

namespace TKScripts.Manager
{
    /// <summary>
    /// 创建属性事件
    /// </summary>
    /// <param name="property"></param>
    /// <param name="pienum"></param>
    public delegate IPropertyIt CreateIPropertyEventCallBack(IPropertyIt propertyIt, ParaItemEnum pienum);
    /// <summary>
    /// PropertyItControl.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyItControl : UserControl
    {
        /// <summary>
        /// 脚本对象
        /// </summary>
        private IScriptLayout script = null;
        /// <summary>
        /// 创建属性事件
        /// </summary>
        public CreateIPropertyEventCallBack CreateIPropertyEvent = null;
        public PropertyItControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="script"></param>
        public void SetIScriptLayout(IScriptLayout script)
        {
            this.script = script;
            this.propertysList.ItemsSource = this.script.IPropertys;
        }

        #region 事件
        /// <summary>
        /// 创建整形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateInt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (script == null) return;
            IPropertyIt pit = new PropertyIt();
            pit.PIEnum = ParaItemEnum.INT;
            pit.Name = getPropertyName();
            pit.Value = 0;
            if(CreateIPropertyEvent != null)
                pit = CreateIPropertyEvent?.Invoke(pit, ParaItemEnum.INT);
            if (pit != null)
            {
                script.Add(pit);
            }
        }
        /// <summary>
        /// 创建浮点型变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFloat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (script == null) return;
            IPropertyIt pit = new PropertyIt();
            pit.PIEnum = ParaItemEnum.FLOAT;
            pit.Name = getPropertyName();
            pit.Value = 0;
            if (CreateIPropertyEvent != null)
                pit = CreateIPropertyEvent?.Invoke(pit, ParaItemEnum.FLOAT);
            if (pit != null)
            {
                script.Add(pit);
            }
        }
        /// <summary>
        /// 创建bool变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateBool_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (script == null) return;
            IPropertyIt pit = new PropertyIt();
            pit.PIEnum = ParaItemEnum.BOOL;
            pit.Name = getPropertyName();
            pit.Value = 0;
            if (CreateIPropertyEvent != null)
                pit = CreateIPropertyEvent?.Invoke(pit, ParaItemEnum.BOOL);
            if (pit != null)
            {
                script.Add(pit);
            }
        }
        /// <summary>
        /// 创建时间变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (script == null) return;
            IPropertyIt pit = new PropertyIt();
            pit.PIEnum = ParaItemEnum.DATETIME;
            pit.Name = getPropertyName();
            pit.Value = DateTime.Now;
            if (CreateIPropertyEvent != null)
                pit = CreateIPropertyEvent?.Invoke(pit, ParaItemEnum.DATETIME);
            if (pit != null)
            {
                script.Add(pit);
            }
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (script == null) return;
            IPropertyIt pit = new PropertyIt();
            pit.PIEnum = ParaItemEnum.OBJECT;
            pit.Name = getPropertyName();
            pit.Value = 0;
            if (CreateIPropertyEvent != null)
                pit = CreateIPropertyEvent?.Invoke(pit, ParaItemEnum.OBJECT);
            if (pit != null)
            {
                script.Add(pit);
            }
        }
        /// <summary>
        /// 创建点对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePoint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (script == null) return;
            IPropertyIt pit = new PropertyIt();
            pit.PIEnum = ParaItemEnum.POINT;
            pit.Name = getPropertyName();
            pit.Value = DateTime.Now;
            if (CreateIPropertyEvent != null)
                pit = CreateIPropertyEvent?.Invoke(pit, ParaItemEnum.POINT);
            if (pit != null)
            {
                script.Add(pit);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (propertysList.SelectedItem != null)
            {
                //IPropertys.Remove(propertysList.SelectedItem as PropertyIt);
                script.Del(propertysList.SelectedItem as PropertyIt);
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <returns></returns>
        protected string getPropertyName()
        {
            string name = "未命名";
            int index = 0;
            while (IPropertys.Count > 0 && IPropertys.Where(p => p.Name == (name + index++)).Count() != 0)
            { }
            return name + index;
        }
        #endregion
        #region 访问器
        /// <summary>
        /// 属性集合
        /// </summary>
        public ObservableCollection<IPropertyIt> IPropertys
        {
            get
            {
                return script.IPropertys;
            }
        }

        #endregion

        
    }
}
