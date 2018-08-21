using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.StackingLayout;

namespace TKScripts.Manager
{
    
    /// <summary>
    /// ScriptControls.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptControl : UserControl
    {
        public ScriptControl()
        {
            InitializeComponent();
            this.Loaded += ScriptControl_Loaded;
        }
        #region 事件

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptControl_Loaded(object sender, RoutedEventArgs e)
        {
            scriptList.ItemsSource = scripts;
        }
        
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void create_Click(object sender, RoutedEventArgs e)
        {
            AddScript();
        }
        /// <summary>
        /// 脚本双击事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptItem_ItemMouseDoubleClickEvent(object selectedItem)
        {
            ItemMouseDoubleClickEvent?.Invoke(selectedItem);
        }
        /// <summary>
        /// 脚本名称修改事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptItem_ItemScriptNameChanged(object selectedItem)
        {
            ItemScriptNameChanged?.Invoke(selectedItem);
        }
        #endregion
        #region 属性
        /// <summary>
        /// 控件key
        /// </summary>
        public string ControlKey { get; internal set; }
        /// <summary>
        /// 项双击事件
        /// </summary>
        public event ItemEvent ItemMouseDoubleClickEvent = null;
        /// <summary>
        /// 脚本创建回调
        /// </summary>
        public event ItemEvent ItemAddEvent = null;
        /// <summary>
        /// 脚本移除回调
        /// </summary>
        public event ItemEvent ItemRemoveEvent = null;
        /// <summary>
        /// 脚本名称修改
        /// </summary>
        public event ItemEvent ItemScriptNameChanged = null;
        /// <summary>
        /// 脚本列表
        /// </summary>
        private ObservableCollection<StackingMainLayout> scripts = new ObservableCollection<StackingMainLayout>();

        #endregion
        #region 访问器
        /// <summary>
        /// 脚本列表
        /// </summary>
        public ObservableCollection<StackingMainLayout> Scripts
        {
            get
            {
                return scripts;
            }

            set
            {
                scripts = value;
                scriptList.ItemsSource = scripts;
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 添加一个脚本
        /// </summary>
        /// <param name="script"></param>
        public void AddScript(StackingMainLayout script)
        {
            if(Scripts.Contains(script) == false)
            {
                Scripts.Add(script);
                ItemAddEvent?.Invoke(script);
            }
        }
        /// <summary>
        /// 添加一个脚本
        /// </summary>
        /// <param name="script"></param>
        public StackingMainLayout AddScript()
        {
            IScriptLayout script = new IScriptLayout();
            script.ScriptName = GetScriptName();
            StackingMainLayout stackingMainLayout = StackingMainLayout.InstanceStackingMainLayout(script);
            Scripts.Add(stackingMainLayout);

            ItemAddEvent?.Invoke(stackingMainLayout);
            return stackingMainLayout;
        }
        /// <summary>
        /// 删除一个脚本
        /// </summary>
        /// <param name="script"></param>
        public void DelScript(StackingMainLayout script)
        {
            if (Scripts.Contains(script) == true)
            {
                Scripts.Remove(script);
                ItemRemoveEvent?.Invoke(script);
            }
        }
        /// <summary>
        /// 删除所有脚本
        /// </summary>
        public void RemoveAll()
        {
            if (Scripts.Count == 0) return;
            StackingMainLayout script = Scripts.First();
            while (script != null)
            {
                DelScript(script);
                if (Scripts.Count == 0)
                    break;
                script = Scripts.First();
            }
        }
        /// <summary>
        /// 获取脚本名称
        /// </summary>
        /// <returns></returns>
        public string GetScriptName()
        {
            string name = "未命名";
            int index = 0;
            while (scripts.Count > 0 && scripts.Where(p => p.Name == (name + index++)).Count() != 0)
            { }
            return name + index;
        }
        /// <summary>
        /// 移除脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_Click(object sender, RoutedEventArgs e)
        {
            if(scriptList.SelectedItem != null)
            {
                DelScript(scriptList.SelectedItem as StackingMainLayout);
            }
        }

        #endregion

        
    }
}
