
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TkScripts;
using TkScripts.Interface;
using TkScripts.Script;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.StackingLayout;
using TKScriptsServer.Agreement;

namespace TScriptTestApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            propertysList.ItemsSource = IPropertys;
            this.KeyDown += ScriptWindow_KeyDown;
            this.KeyUp += ScriptWindow_KeyUp;
            //scriptContent.HalfCloneBack += ScriptContent_HalfCloneBack;
            //this.Closing += ScriptWindow_Closing;
            SetDevice();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            scriptContent.IScriptLayout_KeyDown(scriptContent, e);
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
        #endregion

        /// <summary>
        /// 脚本赋值
        /// </summary>
        /// <param name="load"></param>
        /// <param name="lib"></param>
        private void ScriptContent_HalfCloneBack(IItemBox load, IItemBox lib)
        {
            ItemBox opbox = load as ItemBox;
            ItemBox libbox = lib as ItemBox;

            if (opbox != null && libbox != null)
            {
                //opbox.CloneScriptFunction(libbox);
            }
        }

        /// <summary>
        /// 按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                scriptContent.DeleteSelected();
            scriptContent.IScriptLayout_KeyDown(scriptContent, e);
        }
        /// <summary>
        /// 按键抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptWindow_KeyUp(object sender, KeyEventArgs e)
        {
            scriptContent.IScriptLayout_KeyUp(scriptContent, e);
        }
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
                LogText.Text += "[" + DateTime.Now.ToString() + "] : " + s + "\n";
            }), log);
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPropertysList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ListBox box = sender as ListBox;
                if (box == null || box.SelectedItems == null) return;
                PropertyIt data = box.SelectedItem as PropertyIt;
                if (data == null) return;
                DragDrop.DoDragDrop(box, data, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_KeyDown(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_MouseMove(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            scriptRun = new Thread(() => {
                StackScriptOption option = new StackScriptOption();
                option.RunScript(this.scriptContent.ScriptLayout);
                option = null;
            })
            { IsBackground = true, };
            scriptRun.Start();
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
        #region 访问器
        /// <summary>
        /// 属性集合
        /// </summary>
        public ObservableCollection<IPropertyIt> IPropertys
        {
            get
            {
                return scriptContent.IPropertys;
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
        /// <summary>
        /// 设置设备对象
        /// </summary>
        /// <param name="device"></param>
        public void SetDevice()
        {
            //scriptContent.SetFunctionData(GetInterfaces(device));
        }
        #endregion
        #region 创建属性

        /// <summary>
        /// 创建一个属性，如果存在则只修改值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void CreateProperty(object value, ParaItemEnum type, string name, bool isaddtoIPropertys = false)
        {
            if (IPropertys.Where(p => p.Name == name).Count() == 0)
            {
                IPropertys.Add(new PropertyIt()
                {
                    Name = name,
                    PIEnum = type,
                    Value = value,
                });
                if (isaddtoIPropertys)
                {
                    scriptContent.Add(IPropertys.Last());
                }
                if (name == "TaskPhones")
                {
                    IPropertys.Last().Id = Guid.Empty.ToString();
                }
            }
            else
            {
                IPropertyIt ipi = IPropertys.Where(p => p.Name == name).First();
                ipi.Value = value;
            }
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="ipi"></param>
        /// <param name="isaddtoscriptcontent"></param>
        public void AddProperty(IPropertyIt ipi, bool isaddtoscriptcontent = false)
        {
            if (this.IPropertys.Contains(ipi) == false)
            {
                this.IPropertys.Add(ipi);
                if (isaddtoscriptcontent)
                {
                    scriptContent.Add(IPropertys.Last());
                }
            }
        }
        private void CreateInt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IPropertys.Add(new PropertyIt()
            {
                Name = getPropertyName(),
                PIEnum = ParaItemEnum.INT,
                Value = 0,
            });
            scriptContent.Add(IPropertys.Last());
        }
        private void CreateFloat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IPropertys.Add(new PropertyIt()
            {
                Name = getPropertyName(),
                PIEnum = ParaItemEnum.FLOAT,
                Value = 0,
            });
            scriptContent.Add(IPropertys.Last());
        }
        private void CreateBool_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IPropertys.Add(new PropertyIt()
            {
                Name = getPropertyName(),
                PIEnum = ParaItemEnum.BOOL,
                Value = false,
            });
            scriptContent.Add(IPropertys.Last());
        }
        private void CreateDate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IPropertys.Add(new PropertyIt()
            {
                Name = getPropertyName(),
                PIEnum = ParaItemEnum.DATETIME,
                Value = DateTime.Now,
            });
            scriptContent.Add(IPropertys.Last());
        }
        private void CreateObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IPropertys.Add(new PropertyIt()
            {
                Name = getPropertyName(),
                PIEnum = ParaItemEnum.OBJECT,
                //Value = new List<Phone>(),
            });
            scriptContent.Add(IPropertys.Last());
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (propertysList.SelectedItem != null)
            {
                IPropertys.Remove(propertysList.SelectedItem as PropertyIt);
                scriptContent.Del(propertysList.SelectedItem as PropertyIt);
            }
        }



        #endregion
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Script(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "json文件 (*.json)|*.json";
            if (sd.ShowDialog() == true)
            {
                using (FileStream fs = File.Open(sd.FileName, FileMode.Create))
                {
                    //byte[] datas = System.Text.Encoding.UTF8.GetBytes(Tools.ConverScriptToJson(scriptContent));
                    //fs.Write(datas, 0, datas.Length);
                    //datas = null;
                }
            }
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Script(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "json文件 (*.json)|*.json";
            if (open.ShowDialog() == true)
            {
                IPropertyIt propertyIt = this.IPropertys.Where(p => p.Id == Guid.Empty.ToString()).First();
                this.IPropertys.Clear();
                AddProperty(propertyIt);
                //scriptContent.LoadFromJson<ItemBox, BezierLine, ParatItem>(open.FileName);
            }
        }
    }
}
