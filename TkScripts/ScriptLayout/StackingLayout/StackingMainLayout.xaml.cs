using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
using TkScripts.Interface;
using TkScripts.ScriptLayout.StackingLayout;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TkScripts.Script;
using System.Threading;
using TKScriptsServer.Agreement;
using System.ComponentModel;
using static TkScripts.Interface.IScriptInterpreter;

namespace TkScripts.ScriptLayout.StackingLayout
{
    /// <summary>
    /// StackingMainLayout.xaml 的交互逻辑
    /// </summary>
    public partial class StackingMainLayout : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StackingMainLayout()
        {
            InitializeComponent();
            this.Loaded += StackingMainLayout_Loaded;

            //iScriptInterpreter.ScriptBreakPoint += ScriptBreakPoint;



            this.MouseDown += StackingMainLayout_MouseDown;
            this.MouseUp += StackingMainLayout_MouseUp;
            XTreeView.Drop += XTreeView_Drop;
            XTreeView.MouseMove += XTreeView_MouseMove;
            XTreeView.DragOver += XTreeView_DragOver;
            
        }
        /// <summary>
        /// 拖入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && XTreeView.SelectedItem != null &&
                e.RightButton == MouseButtonState.Released && XTreeView.AllowDrop)
            {
                //ItemBox box = selectedBox as ItemBox;
                DragDrop.DoDragDrop(XTreeView, XTreeView.SelectedItem, DragDropEffects.Move);
            }
        }
        /// <summary>
        /// 获取box
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected ItemBox getStackBoxOnDrap(UIElement element)
        {
            if(element is FunctionRow)
            {
                return (element as FunctionRow).DataContext as ItemBox;
            }
            UIElement parent = VisualTreeHelper.GetParent(element) as UIElement;
            while (parent != null && parent as FunctionRow == null)
            {
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            if(parent is FunctionRow)
            {
                return (parent as FunctionRow).DataContext as ItemBox;
            }
            return null;
        }
        /// <summary>
        /// 拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                ItemBox tobox = getStackBoxOnDrap(e.OriginalSource as UIElement);
                ItemBox frombox = XTreeView.SelectedItem as ItemBox;
                if (tobox != null && frombox != null && tobox != frombox
                    && tobox.BoxType != ItemBoxEnum.IF && (tobox.BoxType == ItemBoxEnum.ELSE
                    || tobox.BoxType == ItemBoxEnum.WHILE))
                {
                    if (frombox.ParentNode == null)
                    {
                        Itemboxs.Remove(frombox);
                        tobox.Add(frombox);
                    }
                    else
                    {
                        frombox.ParentNode.Del(frombox);
                        tobox.Add(frombox);
                    }
                }
                else if (tobox != null && frombox != null && tobox.BoxType == ItemBoxEnum.FUNCTION)
                {
                    if (frombox.ParentNode == null)
                    {
                        Itemboxs.Remove(frombox);
                    }
                    else
                    {
                        frombox.ParentNode.Del(frombox);
                    }
                    if (tobox.ParentNode == null)
                    {
                        Itemboxs.Insert(Itemboxs.IndexOf(tobox), frombox);
                    }
                    else
                    {
                        tobox.ParentNode.Children.Insert(tobox.ParentNode.Children.IndexOf(tobox), frombox);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ScriptLog.Log.Write(ex);
            }
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackingMainLayout_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        #region 属性
        /// <summary>
        /// 代码块被选中事件
        /// </summary>
        public ItemSelectedEvent IItemBoxSelected = null;
        /// <summary>
        /// 被选中的框
        /// </summary>
        private ItemBox selectedBox = null;
        
        #endregion
        #region 访问器
        /// <summary>
        /// 脚本
        /// </summary>
        public IScriptLayout ScriptLayout { get; private set; }
        /// <summary>
        /// 代码块的集合
        /// </summary>
        public ObservableCollection<ItemBox> Itemboxs => ScriptLayout.Itemboxs;
        /// <summary>
        /// 属性集合
        /// </summary>
        public ObservableCollection<IPropertyIt> IPropertys => ScriptLayout.IPropertys;
        /// <summary>
        /// 脚本id
        /// </summary>
        public string Id => ScriptLayout.Id;
        /// <summary>
        /// 脚本名称
        /// </summary>
        public string ScriptName { get => ScriptLayout.ScriptName; set => ScriptLayout.ScriptName = value; }
        /// <summary>
        /// 属性改变
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged { add { ScriptLayout.PropertyChanged += value; } remove
            { ScriptLayout.PropertyChanged -= value; } }

        
        #endregion
        #region 事件
        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackingMainLayout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackingMainLayout_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// 按键抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IScriptLayout_KeyUp(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// 按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IScriptLayout_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F3)
            {
                XTreeView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD4, 0xCD, 0xCD));
                XTreeView.AllowDrop = false;
            }
            else if(e.Key == Key.F4)
            {
                XTreeView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x94, 0x94));
                XTreeView.AllowDrop = true;
            }
            else if(e.Key == Key.Delete)
            {
                DeleteSelected();
            }
            else if(e.Key == Key.F9)
            {
                selectedBox.IsHasBreakPoint = !selectedBox.IsHasBreakPoint;
            }
            else if(e.SystemKey == Key.F10)
            {
                ScriptLayout.IScriptInterpreter.RunNextFunction();
            }
            else if (e.SystemKey == Key.F11)
            {
                ScriptLayout.IScriptInterpreter.RunOver();
            }
        }
        /// <summary>
        /// 框被点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectBox(sender as FunctionRow);
        }
        /// <summary>
        /// 创建代码块事件
        /// </summary>
        /// <param name="data"></param>
        public void DataTree_CreateCallback(IItemBox data)
        {
            ItemBox box = new ItemBox();
            box.Copy(data);
            if(box.BoxType == ItemBoxEnum.IF)
            {
                box.Add(new ItemBox() { Name = "True", BoxType = ItemBoxEnum.ELSE, LogoPath=ScriptHelp.ListImage });
                box.Add(new ItemBox() { Name = "False", BoxType = ItemBoxEnum.ELSE, LogoPath = ScriptHelp.ListImage });
            }
            this.Add(box);
        }
        /// <summary>
        /// 选项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IItemBox box = XTreeView.SelectedItem as IItemBox;
            SelectBox(box);
        }
        #endregion


        #region 方法
        /// <summary>
        /// 添加一个数组
        /// </summary>
        /// <param name="boxs"></param>
        /// <param name="index"></param>
        protected void AddRange(ObservableCollection<ItemBox> boxs, int index)
        {
            foreach (var item in boxs)
            {
                item.ParentNode = null;
                this.Itemboxs.Insert(index++, item);
            }

        }
        /// <summary>
        /// 删除一个代码块
        /// </summary>
        /// <param name="box"></param>
        public void Del(IItemBox box)
        {
            ItemBox stackBox = box as ItemBox;
            int index = -1;
            if (stackBox.ParentNode != null)
            {
                index = stackBox.ParentNode.Children.IndexOf(stackBox);
                if(stackBox.BoxType == ItemBoxEnum.IF)
                {
                    ItemBox iftrue = stackBox.Children[0];
                    ItemBox iffalse = stackBox.Children[1];
                    if (iftrue.Children.Count > 0)
                    {
                        stackBox.ParentNode.AddRange(iftrue.Children, index);
                    }
                    if (iffalse.Children.Count > 0)
                    {
                        stackBox.ParentNode.AddRange(iffalse.Children, index + iftrue.Children.Count);
                    }
                }
                else
                {
                    if (stackBox.Children.Count > 0)
                    {
                        stackBox.ParentNode.AddRange(stackBox.Children, index);
                    }
                }
                stackBox.ParentNode.Del(stackBox);
                stackBox = null;
            }
            else
            {
                index = Itemboxs.IndexOf(stackBox);
                if (stackBox.BoxType == ItemBoxEnum.IF)
                {
                    ItemBox iftrue = stackBox.Children[0];
                    ItemBox iffalse = stackBox.Children[1];
                    if (iftrue.Children.Count > 0)
                    {
                        AddRange(iftrue.Children, index);
                    }
                    if (iffalse.Children.Count > 0)
                    {
                        AddRange(iffalse.Children, index + iftrue.Children.Count);
                    }
                }
                else
                {
                    if (stackBox.Children.Count > 0)
                    {
                        AddRange(stackBox.Children, index);
                    }
                }
                Itemboxs.Remove(stackBox);
            }
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="propertyIt"></param>
        public void Del(IPropertyIt propertyIt)
        {
            if(IPropertys.Contains(propertyIt))
            {
                IPropertys.Remove(propertyIt);
            }
        }
        /// <summary>
        /// 删除所选择的
        /// </summary>
        public void DeleteSelected()
        {
            if(selectedBox == null)
            {
                selectedBox = XTreeView.SelectedItem as ItemBox;
            }
            if(selectedBox != null)
            {
                Del(selectedBox);
            }
            selectedBox = null;
        }
        /// <summary>
        /// 添加被选中的框
        /// </summary>
        /// <param name="row"></param>
        public void SelectBox(IFunctionBox row)
        {
            SelectBox(row.Ibox);

        }
        /// <summary>
        /// 添加被选中的框
        /// </summary>
        /// <param name="row"></param>
        public void SelectBox(IItemBox row)
        {
            selectedBox = row as ItemBox;
            this.IItemBoxSelected?.Invoke(this.ScriptLayout, row);
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="row"></param>
        protected void CancleSelectedBox(FunctionRow row = null)
        {
            //selectedBox.BoxBrush = new SolidColorBrush(Colors.Black);
            selectedBox = null;
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="iPropertyIt"></param>
        public void Add(IPropertyIt iPropertyIt)
        {
            if (!IPropertys.Contains(iPropertyIt))
            {
                IPropertys.Add(iPropertyIt);
            }
        }
        /// <summary>
        /// 添加代码块
        /// </summary>
        /// <param name="itembox"></param>
        public void Add(IItemBox itembox)
        {
            ItemBox stackbox = itembox as ItemBox;
            if(selectedBox != null && selectedBox.ParentNode != null)
            {
                if(selectedBox.BoxType != ItemBoxEnum.ELSE)
                    selectedBox.ParentNode.Add(stackbox, selectedBox.ParentNode.Children.IndexOf(selectedBox) + 1);
                else
                {
                    selectedBox.Add(stackbox);
                }
            }
            else
            {
                if(selectedBox != null)
                    this.Itemboxs.Insert(Itemboxs.IndexOf(selectedBox) + 1, stackbox);
                else
                {
                    this.Itemboxs.Add(stackbox);
                }
            }
            //itembox.ScriptLayout = this;
        }
        /// <summary>
        /// 从文件添加的过程设置stackparatitem的连接属性
        /// </summary>
        /// <param name="parat"></param>
        internal void SetLinkIProperty(ParatItem parat)
        {
            if(parat.LinkipropertyId != "")
            {
                //parat.LinkIProperty = FindIPropertyById(parat.LinkipropertyId);
            }

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool SaveToJson(string filename)
        {
            return ScriptLayout.SaveToJson(filename);
        }
        /// <summary>
        /// 从json加载
        /// </summary>
        /// <typeparam name="Ibox">iitembox的实际类型</typeparam>
        /// <typeparam name="IProperty">连接线的实际类型</typeparam>
        /// <typeparam name="IParaitem">参数的实际类型</typeparam>
        /// <param name="file">json文件路径</param>
        public void LoadFromJson(string file)
        {
            ScriptLayout.LoadFromJson(file);
        }

        /// <summary>
        /// 设置函数处于运行
        /// </summary>
        /// <param name="color"></param>
        public virtual void SetFunctionBoxRun(Color color, IItemBox ibox)
        {
            this.Dispatcher.Invoke(new Action<Color, IItemBox>((c, ib) => {
                //ib.BoxBrush = null;
                //ib.BoxBrush = new SolidColorBrush(c);
            }), color, ibox);
        }
        /// <summary>
        /// 设置函数没有处于运行
        /// </summary>
        /// <param name="color"></param>
        public virtual void SetFunctionBoxStop(Color color, IItemBox ibox)
        {
            this.Dispatcher.Invoke(new Action<Color, IItemBox>((c, ib) => {
                //ib.BoxBrush = null;
                //ib.BoxBrush = new SolidColorBrush(c);
            }), color, ibox);
        }
        /// <summary>
        /// 清除所有
        /// </summary>
        /// <param name="clearMain">是否清除入口</param>
        public void ClearAll(bool clearMain = false)
        {
            //IFunctionBoxs.Clear();
            Itemboxs.Clear();
            IPropertys.Clear();
            if (clearMain == false)
            {
                Add(ScriptLayout.MainIb);
            }
        }
        /// <summary>
        /// 运行脚本
        /// </summary>
        public void RunCompile()
        {
            ScriptLayout.RunCompile();
        }
        /// <summary>
        /// 停止脚本的运行
        /// </summary>
        public void StopRun()
        {

        }
        #endregion
        #region static
        /// <summary>
        /// 获取一个依附脚本的界面
        /// </summary>
        /// <param name="scriptLayout"></param>
        /// <returns></returns>
        public static StackingMainLayout InstanceStackingMainLayout(IScriptLayout scriptLayout)
        {
            return new StackingMainLayout() { ScriptLayout = scriptLayout, DataContext = scriptLayout };
        }
        #endregion
    }
}
