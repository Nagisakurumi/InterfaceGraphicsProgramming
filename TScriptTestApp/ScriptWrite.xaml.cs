
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TkScripts.Script;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.StackingLayout;
using TKScripts.Manager;
using TKScriptsServer.Agreement;
using TKScriptsServer.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static TKScriptsServer.Client.ClientLog;
using TkScripts.Interface;
using System.IO;


namespace TScriptTestApp
{
    /// <summary>
    /// MainLayout2.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptWrite : Window
    {
        public ScriptWrite()
        {
            InitializeComponent();
            this.Loaded += ScriptWrite_Loaded;
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptWrite_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region 属性
        /// <summary>
        /// 脚本运行线程
        /// </summary>
        private Thread scriptRun = null;
        /// <summary>
        /// 是否在运行
        /// </summary>
        protected bool isRunning = false;

        /// <summary>
        /// 服务地址
        /// </summary>
        private string serverUrl = "";
        /// <summary>
        /// 客户端
        /// </summary>
        private ScriptClient scriptClient = new ScriptClient();
        /// <summary>
        /// 脚本配置
        /// </summary>
        private ScriptConfig ScriptConfig = new ScriptConfig();
        #endregion
        #region 访问器
        /// <summary>
        /// 获取APIUrl
        /// </summary>
        public string GetApisUrl => serverUrl + "getallapis";


        #endregion


        /// <summary>
        /// 脚本内容点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scriptContent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            scriptContent.Focus();
        }
        /// <summary>
        /// 写出日志
        /// </summary>
        /// <param name="log"></param>
        protected void WriteLog(string log)
        {
            this.Dispatcher.Invoke(new Action<string>((s) => {
                //LogText.Text += "[" + DateTime.Now.ToString() + "] : " + s + "\n";
            }), log);
        }

        /// <summary>
        /// 按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            scriptContent.RunActiveScript();
        }
        /// <summary>
        /// 停止正在运行的脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (scriptRun != null)
            {
                scriptRun.Abort();
            }
        }

        #region 方法
        /// <summary>
        /// 设置设备对象
        /// </summary>
        /// <param name="device"></param>
        public void SetDevice()
        {
            //scriptContent.SetFunctionData(GetInterfaces(device));
        }


        #endregion
        #region Event
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            scriptContent.RunActiveScript();
        }
        /// <summary>
        /// 设置服务URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetServerUrl_Click(object sender, RoutedEventArgs e)
        {
            SetUrl setUrl = new SetUrl();
            serverUrl = setUrl.ShowWindow();
            setUrl.Close();
            setUrl = null;
            queryApis();
        }
        /// <summary>
        /// 加载脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "脚本项目文件 (*.sProject)|*.sProject";
            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ScriptConfig = JsonConvert.DeserializeObject<ScriptConfig>(openFileDialog.FileName.GetStringFromFile());

                string path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                foreach (var item in ScriptConfig.ScriptFiles)
                {
                    IScriptLayout scriptLayout = new IScriptLayout();
                    scriptLayout.LoadFromJson(System.IO.Path.Combine(path, item + ".script"));
                    scriptContent.ScriptControl.AddScript(StackingMainLayout.InstanceStackingMainLayout(scriptLayout));
                }
            }
        }
        /// <summary>
        /// 保存脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(scriptContent.ScriptControl.Scripts.Count == 0)
            {
                MessageBox.Show("并没有创建任何的脚本，所以不能保存!");
                return;
            }
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "脚本项目文件 (*.sProject)|*.sProject";
            saveFileDialog.DefaultExt = "sProject";
            saveFileDialog.AddExtension = true;
            string path = "";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ScriptConfig.ScriptFiles.Clear();
                foreach (var item in scriptContent.ScriptControl.Scripts)
                {
                    ScriptConfig.ScriptFiles.Add(item.ScriptName);
                }
                path = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                string json = JsonConvert.SerializeObject(ScriptConfig);
                ScriptConfig.SaveObjectToFile(saveFileDialog.FileName);

                foreach (var item in scriptContent.ScriptControl.Scripts)
                {
                    item.SaveToJson(System.IO.Path.Combine(path, item.ScriptName + ".script"));
                }
            }
        }
        #endregion
        #region private
        /// <summary>
        /// 获取所有的api
        /// </summary>
        private void queryApis()
        {
            try
            {
                List<ScriptMethAttribute> scriptMethAttributes = JsonConvert.DeserializeObject<List<ScriptMethAttribute>>(scriptClient.GetStringAsync(GetApisUrl));
                TreeData treeData = new TreeData("脚本函数");
                foreach (var item in scriptMethAttributes)
                {
                    ScriptTools.GetIItemboxByScriptMeth<ItemBox, ParatItem>(treeData, item);
                }
                ObservableCollection<TreeData> treeDatas = new ObservableCollection<TreeData>();
                treeDatas.Add(treeData);
                scriptContent.RestFunctionData();
                scriptContent.SetFunctionData(treeDatas);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show("请求失败, 请确认填写的url是否正确!");
            }
        }
        #endregion
    }
}
