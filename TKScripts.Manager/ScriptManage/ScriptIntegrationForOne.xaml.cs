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
using TkScripts.Interface;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.StackingLayout;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using TkScripts;
using TkScripts.Script;
using TKScriptsServer.Agreement;
using System.ComponentModel;
using PluginInterface;

namespace TKScripts.Manager.ScriptManage
{
    /// <summary>
    /// ScriptIntegrationForOne.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptIntegrationForOne : UserControl
    {
        #region 访问器
        /// <summary>
        /// 布局管理器
        /// </summary>
        public MainManagerControl MainLayoutManager
        {
            get
            {
                return mainContent;
            }
        }
        /// <summary>
        /// 脚本管理列表
        /// </summary>
        public ScriptControl ScriptControl { get; } = new ScriptControl();

        /// <summary>
        /// 函数的全局属性列表
        /// </summary>
        public PropertyItControl PropertyItControl { get; } = new PropertyItControl();

        /// <summary>
        /// 函数的参数列表
        /// </summary>
        public FunctionParaItemList FunctionParaItemList { get; } = new FunctionParaItemList();

        /// <summary>
        /// 日志窗口
        /// </summary>
        public SystemLogBox LogBox { get; } = new SystemLogBox();

        /// <summary>
        /// 代码块工具箱的数据源
        /// </summary>
        public ObservableCollection<TreeData> AllFunctionData { get; } = new ObservableCollection<TreeData>();

        /// <summary>
        /// 代码块工具箱
        /// </summary>
        public DataTreeView TreeView { get; set; } = null;
        /// <summary>
        /// /插件管理
        /// </summary>
        public Plugins.PluginManager PluginManager { get; private set; } = null;
        /// <summary>
        /// 控件列表
        /// </summary>
        public ObservableCollection<string> Controls { get; } = new ObservableCollection<string>();
        /// <summary>
        /// 脚本列表
        /// </summary>
        public ObservableCollection<StackingMainLayout> Scripts
        {
            get
            {
                return ScriptControl.Scripts;
            }
            set
            {
                ScriptControl.Scripts = value;
            }
        }
        /// <summary>
        /// 脚本监视窗口
        /// </summary>
        public ScriptDebugWindow ScriptDebugWindow { get; set; } = new ScriptDebugWindow();
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptIntegrationForOne()
        {
            InitializeComponent();
            this.Loaded += ScriptIntegrationForOne_Loaded;
            TreeView = FunctionDataList.FunctionView;

            PropertyItControl.ControlKey = "全局变量";
            FunctionParaItemList.ControlKey = "函数的参数列表";
            ScriptControl.ControlKey = "脚本列表";
            LogBox.ControlKey = "输出";
            TreeView.ControlKey = "代码工具箱";
            ScriptDebugWindow.ControlKey = "监视窗口";

            Controls.Add("全局变量");
            Controls.Add("函数的参数列表");
            Controls.Add("脚本列表");
            Controls.Add("输出");
            Controls.Add("代码工具箱");
            Controls.Add("监视窗口");

            mainContent.AddUserControl("全局变量", PropertyItControl, Layout.LeftUp, "全局变量");
            mainContent.AddUserControl("函数的参数列表", FunctionParaItemList, Layout.LeftDown, "函数的参数列表");
            mainContent.AddUserControl("脚本列表", ScriptControl, Layout.Right, "脚本列表");
            mainContent.AddUserControl("代码工具箱", TreeView, Layout.Buttom, "代码工具箱");
            mainContent.AddUserControl("输出", LogBox, Layout.Buttom, "输出");
            mainContent.AddUserControl("监视窗口", ScriptDebugWindow, Layout.Right, "监视窗口");


            AllFunctionData.Add(IScriptLayout.AddSystemBox());
            TreeView.MyData = AllFunctionData;
            TreeView.CreateCallback += TreeView_CreateCallback;
            //AllFunctionData.Add(IScriptLayout.AddSystemBox());
            //AllFunctionData.Add(IScriptLayout.AddToolsFunction());
            ScriptControl.ItemMouseDoubleClickEvent += ScriptControl_ItemMouseDoubleClickEvent;
            ScriptControl.ItemAddEvent += ScriptControl_ItemAddEvent;
            ScriptControl.ItemRemoveEvent += ScriptControl_ItemRemoveEvent;
            ScriptControl.ItemScriptNameChanged += ScriptControl_ItemScriptNameChanged;
            //ScriptControl.AddScript(new StackingMainLayout() { ScriptName = "测试脚本" });


            
        }
        #region 事件
        /// <summary>
        /// 脚本名称修改事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemScriptNameChanged(object selectedItem)
        {
            StackingMainLayout script = selectedItem as StackingMainLayout;
            mainContent.ModifyTitleById(script.Id, script.ScriptName);
        }
        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            StackingMainLayout script = mainContent.GetActiveDocument() as StackingMainLayout;
            if(script != null)
            {
                script.IScriptLayout_KeyDown(script, e);
            }
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptIntegrationForOne_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// 脚本删除事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemRemoveEvent(object selectedItem)
        {
            StackingMainLayout script = selectedItem as StackingMainLayout;
            script.IItemBoxSelected -= IItemBoxSelected;
            script.ScriptLayout.ComipleMessageCall -= LogBox.WritLog;
            mainContent.DelUserExtendByKey(script.Id);
            script.ScriptLayout.ScriptBreakPoint -= ScriptBreakPoint;

            script.ScriptLayout.IScriptInterpreter.ScriptRequest -= ScriptRequest;
            script.ScriptLayout.IScriptInterpreter.ScriptReponse -= ScriptReponse;
        }

        

        /// <summary>
        /// 脚本添加事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemAddEvent(object selectedItem)
        {
            StackingMainLayout script = selectedItem as StackingMainLayout;
            if (script.IItemBoxSelected == null)
            {
                script.IItemBoxSelected += IItemBoxSelected;
            }
            if (script.ScriptLayout.ComipleMessageCall == null)
            {
                script.ScriptLayout.ComipleMessageCall = LogBox.WritLog;
            }
            script.ScriptLayout.ScriptBreakPoint += ScriptBreakPoint;
            script.ScriptLayout.IScriptInterpreter.ScriptRequest += ScriptRequest;
            script.ScriptLayout.IScriptInterpreter.ScriptReponse += ScriptReponse;

        }

        /// <summary>
        /// 脚本断点回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="si"></param>
        /// <param name="so"></param>
        private void ScriptBreakPoint(IItemBox item, ScriptInput si, ScriptOutput so)
        {
            ScriptDebugWindow.UpdateWatchObjectSource(si, "ScriptInput");
        }

        /// <summary>
        /// 创建代码事件
        /// </summary>
        /// <param name="data"></param>
        private void TreeView_CreateCallback(IItemBox data)
        {
            if (mainContent.GetActiveDocument() != null)
            {
                StackingMainLayout script = mainContent.GetActiveDocument() as StackingMainLayout;
                script.DataTree_CreateCallback(data);
            }
        }

        /// <summary>
        /// 脚本请求回调
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void ScriptRequest(ScriptInput arg1, string arg2)
        {
            if (PluginManager == null) return;
            foreach (var item in PluginManager.PluginItems)
            {
                item.ScriptPlugin.SendToRequestFunction(arg1, arg2);
            }
        }
        /// <summary>
        /// 脚本返回回调
        /// </summary>
        /// <param name="obj"></param>
        private void ScriptReponse(ScriptOutput obj, string name)
        {
            if (PluginManager == null) return;
            foreach (var item in PluginManager.PluginItems)
            {
                item.ScriptPlugin.ReciveResponseFunction(obj, name);
            }
        }
        /// <summary>
        /// 脚本双击事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemMouseDoubleClickEvent(object selectedItem)
        {
            StackingMainLayout script = selectedItem as StackingMainLayout;
            if (mainContent.GetUserExtendByKey(script.Id) != null)
            {
                mainContent.ShowPane(script.Id);
            }
            else
            {
                mainContent.AddUserControl(script.Id, script, Layout.Content, script.ScriptName);
            }
            PropertyItControl.SetIScriptLayout(script);
            mainContent.SetActiveDocument(script.Id);
        }
        /// <summary>
        /// 代码块被选中
        /// </summary>
        /// <param name="scriptLayout"></param>
        /// <param name="itembox"></param>
        public void IItemBoxSelected(IScriptLayout scriptLayout, IItemBox itembox)
        {
            ItemboxParamterCom itemboxParamterCom = new ItemboxParamterCom() { ScriptLayout = scriptLayout, ItemBox = itembox };
            FunctionParaItemList.DataContext = itemboxParamterCom;
            //FunctionParaItemList.DataContext = itembox;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 设置初始数据源
        /// </summary>
        /// <param name="treedata"></param>
        public void SetFunctionData(ObservableCollection<TreeData> treedata)
        {
            foreach (var item in treedata)
            {
                AllFunctionData.Add(item);
            }
        }
        /// <summary>
        /// 重置用户函数
        /// </summary>
        public void RestFunctionData()
        {
            AllFunctionData.Clear();
            AllFunctionData.Add(IScriptLayout.AddSystemBox());
        }
        /// <summary>
        /// 显示key的控件
        /// </summary>
        /// <param name="key"></param>
        public void ShowPanel(string key)
        {
            if(Controls.Contains(key))
            {
                mainContent.ShowPane(key);
            }
        }
        /// <summary>
        /// 运行当前激活的脚本
        /// </summary>
        public void RunActiveScript()
        {
            IScriptLayout script = (mainContent.GetActiveDocument() as StackingMainLayout).ScriptLayout;
            script.RunCompile();
        }
        /// <summary>
        /// 停止运行当前激活的脚本
        /// </summary>
        public void StopActiveScript()
        {
            IScriptLayout script = (mainContent.GetActiveDocument() as StackingMainLayout).ScriptLayout;
            script.StopRun();
        }
        /// <summary>
        /// 设置控件浮动
        /// </summary>
        /// <param name="keyname"></param>
        /// <param name="rect"></param>
        public void SetControlFloat(string keyname, Rect rect)
        {
            mainContent.SetControlFloat(keyname, rect);
        }
        /// <summary>
        /// 获取当前激活中的脚本
        /// </summary>
        /// <returns></returns>
        public IScriptLayout GetActiveScript()
        {
            IScriptLayout script = mainContent.GetActiveDocument() as IScriptLayout;
            return script;
        }
        /// <summary>
        /// 从路径中加载所有脚本
        /// </summary>
        /// <param name="path"></param>
        public void LoadScripts(string path)
        {
            //IList<StackingMainLayout> scripts = ScriptHelp.Load(path);
            //ScriptControl.RemoveAll();
            //foreach (var item in scripts)
            //{
            //    ScriptControl.AddScript(item);
            //}
        }
        /// <summary>
        /// 加载插件
        /// </summary>
        public void LoadPlugins()
        {
            if(PluginManager == null)
            {
                PluginManager = new Plugins.PluginManager();
            }
            PluginManager.LoadPathDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            PluginManager.PluginNameEx = "Script_Plugin";
            PluginManager.PluginMainInputClassName = "MainClass";
            PluginManager.LoadPlugins();

            foreach (var item in PluginManager.PluginItems)
            {
                if (Controls.Contains(item.ScriptPlugin.Name) == true)
                {
                    MessageBox.Show("插件 : " + item.ScriptPlugin.Name + ", 已经存在!加载失败!");
                    continue;
                }
                item.ScriptPlugin.Start();
                Controls.Add(item.ScriptPlugin.Name);
                if (item.ScriptPlugin.PluginType == PluginInterface.PluginType.WPF)
                {
                    mainContent.AddUserControl(item.ScriptPlugin.Name, item.ScriptPlugin.GetPluginInterface() as FrameworkElement
                        , ChangeLayout(item.ScriptPlugin.LayoutType), item.ScriptPlugin.Name);
                }
                else
                {
                    mainContent.AddUserControl(item.ScriptPlugin.Name, item.ScriptPlugin.GetPluginInterface() as System.Windows.Forms.Control
                       , ChangeLayout(item.ScriptPlugin.LayoutType), item.ScriptPlugin.Name);
                }
            }
        }
        /// <summary>
        /// 转换布局
        /// </summary>
        /// <param name="layoutType"></param>
        /// <returns></returns>
        public Layout ChangeLayout(LayoutType layoutType)
        {
            switch (layoutType)
            {
                case LayoutType.Left:
                    return Layout.LeftUp;
                case LayoutType.Right:
                    return Layout.Right;
                case LayoutType.Top:
                    return Layout.TopSide;
                case LayoutType.Bottom:
                    return Layout.Buttom;
                default:
                    return Layout.Buttom;
            }
        }
        #endregion


    }

    internal class ItemboxParamterCom : INotifyPropertyChanged
    {

        private IScriptLayout scriptLayout = null;

        private IItemBox itemBox = null;

        public IScriptLayout ScriptLayout
        { get { return scriptLayout; } set { scriptLayout = value; Changed("ScriptLayout"); } }

        public IItemBox ItemBox { get { return itemBox; } set { itemBox = value; Changed("Itembox"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
