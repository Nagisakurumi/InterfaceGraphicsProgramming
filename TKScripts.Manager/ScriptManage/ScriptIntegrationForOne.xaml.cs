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

namespace TKScripts.Manager.ScriptManage
{
    /// <summary>
    /// ScriptIntegrationForOne.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptIntegrationForOne : UserControl
    {

        #region 属性
        /// <summary>
        /// 数据
        /// </summary>
        private DataTreeView treeView = null;
        /// <summary>
        /// 脚本列表控件
        /// </summary>
        private ScriptControl scriptControl = new ScriptControl();
        /// <summary>
        /// 属性列表控件
        /// </summary>
        private PropertyItControl propertyItControl = new PropertyItControl();
        /// <summary>
        /// 函数参数列表控件
        /// </summary>
        private FunctionParaItemList functionParaItemList = new FunctionParaItemList();
        /// <summary>
        /// 脚本监视窗口
        /// </summary>
        private ScriptDebugWindow scriptDebugWindow = new ScriptDebugWindow();
        /// <summary>
        /// 日志窗口
        /// </summary>
        private SystemLogBox logBox = new SystemLogBox();
        /// <summary>
        /// 所有的函数数据源
        /// </summary>
        private ObservableCollection<TreeData> allFunctionData = new ObservableCollection<TreeData>();
        #endregion
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
        public ScriptControl ScriptControl
        {
            get
            {
                return scriptControl;
            }
        }
        /// <summary>
        /// 函数的全局属性列表
        /// </summary>
        public PropertyItControl PropertyItControl
        {
            get
            {
                return propertyItControl;
            }
        }
        /// <summary>
        /// 函数的参数列表
        /// </summary>
        public FunctionParaItemList FunctionParaItemList
        {
            get
            {
                return functionParaItemList;
            }
        }
        /// <summary>
        /// 日志窗口
        /// </summary>
        public SystemLogBox LogBox
        {
            get
            {
                return logBox;
            }
        }
        /// <summary>
        /// 代码块工具箱的数据源
        /// </summary>
        public ObservableCollection<TreeData> AllFunctionData
        {
            get
            {
                return allFunctionData;
            }
        }
        /// <summary>
        /// 代码块工具箱
        /// </summary>
        public DataTreeView TreeView
        {
            get
            {
                return treeView;
            }

            set
            {
                treeView = value;
            }
        }
        /// <summary>
        /// 脚本列表
        /// </summary>
        public ObservableCollection<IScriptLayout> Scripts
        {
            get
            {
                return scriptControl.Scripts;
            }
            set
            {
                scriptControl.Scripts = value;
            }
        }
        /// <summary>
        /// 脚本监视窗口
        /// </summary>
        public ScriptDebugWindow ScriptDebugWindow
        {
            get
            {
                return scriptDebugWindow;
            }

            set
            {
                scriptDebugWindow = value;
            }
        }
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptIntegrationForOne()
        {
            InitializeComponent();
            this.Loaded += ScriptIntegrationForOne_Loaded;
            TreeView = FunctionDataList.FunctionView;
            mainContent.AddUserControl("property", PropertyItControl, Layout.LeftUp, "全局变量");
            mainContent.AddUserControl("paraItem", FunctionParaItemList, Layout.LeftDown, "函数的参数列表");
            mainContent.AddUserControl("script", ScriptControl, Layout.Right, "脚本列表");
            mainContent.AddUserControl("logBox", LogBox, Layout.Buttom, "输出");
            mainContent.AddUserControl("treeView", TreeView, Layout.LeftSide, "代码工具箱");
            mainContent.AddUserControl("watchView", ScriptDebugWindow, Layout.Right, "监视窗口");
            TreeView.MyData = AllFunctionData;
            TreeView.CreateCallback += TreeView_CreateCallback;
            AllFunctionData.Add(IScriptLayout.AddSystemBox());
            AllFunctionData.Add(IScriptLayout.AddToolsFunction());
            ScriptControl.ItemMouseDoubleClickEvent += ScriptControl_ItemMouseDoubleClickEvent;
            ScriptControl.ItemAddEvent += ScriptControl_ItemAddEvent;
            ScriptControl.ItemRemoveEvent += ScriptControl_ItemRemoveEvent;
            scriptControl.ItemScriptNameChanged += ScriptControl_ItemScriptNameChanged;
            //ScriptControl.AddScript(new StackingMainLayout() { ScriptName = "测试脚本" });

        }
        #region 事件
        /// <summary>
        /// 脚本名称修改事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemScriptNameChanged(object selectedItem)
        {
            IScriptLayout script = selectedItem as IScriptLayout;
            mainContent.ModifyTitleById(script.Id, script.ScriptName);
        }
        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            IScriptLayout script = mainContent.GetActiveDocument() as IScriptLayout;
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
            IScriptLayout script = selectedItem as IScriptLayout;
            script.IItemBoxSelected -= IItemBoxSelected;
            script.ComipleMessageCall -= LogBox.WritLog;
            mainContent.DelUserExtendByKey(script.Id);
            script.ScriptBreakPoint -= ScriptBreakPoint;
        }
        /// <summary>
        /// 脚本添加事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemAddEvent(object selectedItem)
        {
            IScriptLayout script = selectedItem as IScriptLayout;
            if (script.IItemBoxSelected == null)
            {
                script.IItemBoxSelected += IItemBoxSelected;
            }
            if (script.ComipleMessageCall == null)
            {
                script.ComipleMessageCall = LogBox.WritLog;
            }
            script.ScriptBreakPoint += ScriptBreakPoint;
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
                IScriptLayout script = mainContent.GetActiveDocument() as IScriptLayout;
                script.DataTree_CreateCallback(data);
            }
        }
        /// <summary>
        /// 脚本双击事件
        /// </summary>
        /// <param name="selectedItem"></param>
        private void ScriptControl_ItemMouseDoubleClickEvent(object selectedItem)
        {
            IScriptLayout script = selectedItem as IScriptLayout;
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
        /// <param name="itembox"></param>
        public void IItemBoxSelected(IItemBox itembox)
        {
            FunctionParaItemList.DataContext = itembox;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 设置初始数据源
        /// </summary>
        /// <param name=""></param>
        public void SetFunctionData(ObservableCollection<TreeData> treedata)
        {
            foreach (var item in treedata)
            {
                AllFunctionData.Add(item);
            }
        }
        /// <summary>
        /// 运行当前激活的脚本
        /// </summary>
        public void RunActiveScript()
        {
            IScriptLayout script = mainContent.GetActiveDocument() as IScriptLayout;
            script.RunCompile();
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
            IList<IScriptLayout> scripts = ScriptHelp.Load(path);
            ScriptControl.RemoveAll();
            foreach (var item in scripts)
            {
                ScriptControl.AddScript(item);
            }
        }
        #endregion

        
    }
}
