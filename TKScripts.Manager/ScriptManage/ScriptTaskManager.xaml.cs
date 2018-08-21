using Newtonsoft.Json;
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
using TKScripts.Manager.ScriptManage.ManagerPlugin;

namespace TKScripts.Manager.ScriptManage
{
    /// <summary>
    /// ScriptTaskManager.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptTaskManager : UserControl
    {
        /// <summary>
        /// 按下去的点
        /// </summary>
        private Point pressPoint = new Point();
        /// <summary>
        /// 是否正在运行任务
        /// </summary>
        private bool isRunningTask = false;
        /// <summary>
        /// 项目路径
        /// </summary>
        private string projectPath = "";
        /// <summary>
        /// 运行任务
        /// </summary>
        private Task runTask = null;
        /// <summary>
        /// 脚本配置
        /// </summary>
        private ScriptConfig ScriptConfig = new ScriptConfig();
        /// <summary>
        /// 脚本列表
        /// </summary>
        public ObservableCollection<IScriptLayout> ScriptLayouts { get; set; } = new ObservableCollection<IScriptLayout>();

        /// <summary>
        /// 任务列表
        /// </summary>
        public ObservableCollection<ScriptTask> TaskItems { get; set; } = new ObservableCollection<ScriptTask>();
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptTaskManager()
        {
            InitializeComponent();
            this.Loaded += ScriptTaskManager_Loaded;
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptTaskManager_Loaded(object sender, RoutedEventArgs e)
        {
            scripts.ItemsSource = ScriptLayouts;
            tasks.ItemsSource = TaskItems;
            taskItem.Drop += TaskItem_Drop;
        }
        /// <summary>
        /// 拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskItem_Drop(object sender, DragEventArgs e)
        {
            if (tasks.SelectedItem == null)
                return;
            IScriptLayout scriptLayout = e.Data.GetData(typeof(IScriptLayout)) as IScriptLayout;
            (tasks.SelectedItem as ScriptTask).TaskItems.Add(new TaskItem() { ScriptLayout = scriptLayout
            , Times = 0, });

        }

        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Task(object sender, RoutedEventArgs e)
        {
            TaskItems.SaveObjectToFile(System.IO.Path.Combine(projectPath, "projectTask.task"));
        }
        /// <summary>
        /// 加载脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Script(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "脚本项目文件 (*.sProject)|*.sProject";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ScriptConfig = JsonConvert.DeserializeObject<ScriptConfig>(openFileDialog.FileName.GetStringFromFile());

                string path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                projectPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                foreach (var item in ScriptConfig.ScriptFiles)
                {
                    IScriptLayout scriptLayout = new IScriptLayout();
                    scriptLayout.LoadFromJson(System.IO.Path.Combine(path, item + ".script"));
                    scriptLayout.ComipleMessageCall += logBox.WritLog;
                    ScriptLayouts.Add(scriptLayout);
                }
                //加载任务
                if(System.IO.File.Exists(System.IO.Path.Combine(projectPath, "projectTask.task")))
                {
                    ObservableCollection<ScriptTask> scriptTasks = Newtonsoft.Json.JsonConvert.
                        DeserializeObject<ObservableCollection<ScriptTask>>(System.IO.Path.Combine(projectPath, "projectTask.task")
                        .GetStringFromFile());
                    foreach (var listitem in scriptTasks)
                    {
                        foreach (var item in listitem.TaskItems)
                        {
                            IScriptLayout script = ScriptLayouts.FindOne(p => p.ScriptName == item.ScriptName);
                            if (script != null)
                                item.ScriptLayout = script;
                        }
                        TaskItems.Add(listitem);
                    }
                    scriptTasks.Clear();
                    scriptTasks = null;
                }
            }
        }
        /// <summary>
        /// 脚本列表移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Script_Move(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(scripts);
            if(e.LeftButton == MouseButtonState.Pressed &&
                Math.Sqrt(Math.Pow(point.X - pressPoint.X, 2) + Math.Pow(point.Y - pressPoint.Y, 2)) > 20)
            {
                DragDrop.DoDragDrop(scripts, scripts.SelectedItem, DragDropEffects.Copy);
            }
        }
        /// <summary>
        /// 点击时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Script_Down(object sender, MouseButtonEventArgs e)
        {
            pressPoint = e.GetPosition(scripts);
        }
        /// <summary>
        /// 运行选中的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_Task(object sender, RoutedEventArgs e)
        {
            if (runTask != null && runTask.IsCompleted == false)
            {
                MessageBox.Show("上次的任务还没有执行完成!");
                return;
            }
            if(tasks.SelectedItem != null)
            {
                runTask = new Task((task) => 
                {
                    ScriptTask scriptTask = task as ScriptTask;
                    foreach (var item in scriptTask.TaskItems)
                    {
                        Task<bool> taskitem = item.ScriptLayout.RunCompile();
                        logBox.WritLog("脚本" + item.ScriptLayout.ScriptName + "运行结果 : " +
                            (taskitem.Result ? "成功" : "失败"));
                    }
                }, tasks.SelectedItem as ScriptTask);
                runTask.Start();
            }
        }
        /// <summary>
        /// 运行所有任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_AllTask(object sender, RoutedEventArgs e)
        {
            if (runTask != null && runTask.IsCompleted == false)
            {
                MessageBox.Show("上次的任务还没有执行完成!");
                return;
            }
            if (tasks.SelectedItem != null)
            {
                runTask = new Task(() =>
                {
                    foreach (var scriptTask in TaskItems)
                    {
                        logBox.WritLog("开始运行任务 ：" + scriptTask.Name);
                        foreach (var item in scriptTask.TaskItems)
                        {
                            Task<bool> taskitem = item.ScriptLayout.RunCompile();
                            logBox.WritLog("脚本" + item.ScriptLayout.ScriptName + "运行结果 : " +
                                (taskitem.Result ? "成功" : "失败"));
                        }
                    }
                    
                });
                runTask.Start();
            }
        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Task(object sender, RoutedEventArgs e)
        {
            TaskItems.Add(new ScriptTask() { Name = getName() });
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Task(object sender, RoutedEventArgs e)
        {
            if(tasks.SelectedItem != null)
            {
                TaskItems.Remove(tasks.SelectedItem as ScriptTask);
            }
        }
        /// <summary>
        /// 获取不重复的名称
        /// </summary>
        /// <returns></returns>
        private string getName()
        {
            int i = 0;
            string name = "未命名";
            while (TaskItems.Where(p=>p.Name == name).ToList().Count != 0)
            {
                name = name + i.ToString();
            }
            return name;
        }
        /// <summary>
        /// 选项改变的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tasks.SelectedItem == null) return;
            taskItem.ItemsSource = (tasks.SelectedItem as ScriptTask).TaskItems;
        }
    }
}
