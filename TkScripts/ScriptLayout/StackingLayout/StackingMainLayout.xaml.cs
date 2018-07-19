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

namespace TkScripts.ScriptLayout.StackingLayout
{
    /// <summary>
    /// StackingMainLayout.xaml 的交互逻辑
    /// </summary>
    public partial class StackingMainLayout : IScriptLayout
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StackingMainLayout()
        {
            InitializeComponent();
            this.Loaded += StackingMainLayout_Loaded;

            iScriptInterpreter = new StackScriptOption();
            //iScriptInterpreter.ScriptBreakPoint += ScriptBreakPoint;

            dataTreePop.AllowsTransparency = true;
            dataTreePop.PopupAnimation = PopupAnimation.Fade;
            dataTreePop.StaysOpen = false;
            dataTreePop.PlacementTarget = XTreeView;
            dataTreePop.Placement = PlacementMode.Right;
            ///设置弹出框内容
            dataTreePop.Child = DataTree;

            DataTree.CreateCallback += DataTree_CreateCallback;

            this.MouseDown += StackingMainLayout_MouseDown;
            this.MouseUp += StackingMainLayout_MouseUp;
            ForgeStyleColor = Colors.Black;
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
                //StackItemBox box = selectedBox as StackItemBox;
                DragDrop.DoDragDrop(XTreeView, XTreeView.SelectedItem, DragDropEffects.Move);
            }
        }
        /// <summary>
        /// 获取box
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected StackItemBox getStackBoxOnDrap(UIElement element)
        {
            if(element is FunctionRow)
            {
                return (element as FunctionRow).DataContext as StackItemBox;
            }
            UIElement parent = VisualTreeHelper.GetParent(element) as UIElement;
            while (parent != null && parent as FunctionRow == null)
            {
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            if(parent is FunctionRow)
            {
                return (parent as FunctionRow).DataContext as StackItemBox;
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
            StackItemBox tobox = getStackBoxOnDrap(e.OriginalSource as UIElement);
            StackItemBox frombox = XTreeView.SelectedItem as StackItemBox;
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
        /// 被选中的框
        /// </summary>
        private StackItemBox selectedBox = null;
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
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released &&
               e.ChangedButton == MouseButton.Right)
            {
                ShowTree();
            }
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
        public override void IScriptLayout_KeyDown(object sender, KeyEventArgs e)
        {
            //开启提示框
            if(e.Key == Key.Enter)
            {
                ShowTree();
            }
            else if(e.Key == Key.F3)
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
                iScriptInterpreter.RunNextFunction();
            }
            else if (e.SystemKey == Key.F11)
            {
                iScriptInterpreter.RunOver();
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
        /// 双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fr_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowTree();
        }
        /// <summary>
        /// 创建代码块事件
        /// </summary>
        /// <param name="data"></param>
        public override void DataTree_CreateCallback(IItemBox data)
        {
            StackItemBox box = new StackItemBox();
            box.Copy(data);
            if(box.BoxType == ItemBoxEnum.IF)
            {
                box.Add(new StackItemBox() { Name = "True", BoxType = ItemBoxEnum.ELSE, LogoPath=ScriptHelp.ListImage });
                box.Add(new StackItemBox() { Name = "False", BoxType = ItemBoxEnum.ELSE, LogoPath = ScriptHelp.ListImage });
            }
            this.Add(box);
            HiddenTree();
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
        protected void AddRange(ObservableCollection<StackItemBox> boxs, int index)
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
        public override void Del(IItemBox box)
        {
            StackItemBox stackBox = box as StackItemBox;
            int index = -1;
            if (stackBox.ParentNode != null)
            {
                index = stackBox.ParentNode.Children.IndexOf(stackBox);
                if(stackBox.BoxType == ItemBoxEnum.IF)
                {
                    StackItemBox iftrue = stackBox.Children[0];
                    StackItemBox iffalse = stackBox.Children[1];
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
                    StackItemBox iftrue = stackBox.Children[0];
                    StackItemBox iffalse = stackBox.Children[1];
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
        /// 删除所选择的
        /// </summary>
        public override void DeleteSelected()
        {
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
        public override void SelectBox(IFunctionBox row)
        {
            SelectBox(row.Ibox);
        }
        /// <summary>
        /// 添加被选中的框
        /// </summary>
        /// <param name="row"></param>
        public override void SelectBox(IItemBox row)
        {
            selectedBox = row as StackItemBox;
            base.SelectBox(row);
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="row"></param>
        protected void CancleSelectedBox(FunctionRow row = null)
        {
            selectedBox.BoxBrush = new SolidColorBrush(Colors.Black);
            selectedBox = null;
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="iPropertyIt"></param>
        public override void Add(IPropertyIt iPropertyIt)
        {
            if (!iPropertys.Contains(iPropertyIt))
            {
                iPropertys.Add(iPropertyIt);
            }
        }
        /// <summary>
        /// 添加代码块
        /// </summary>
        /// <param name="itembox"></param>
        public override void Add(IItemBox itembox)
        {
            StackItemBox stackbox = itembox as StackItemBox;
            if(selectedBox != null && selectedBox.ParentNode != null)
            {

                selectedBox.ParentNode.Add(stackbox, selectedBox.ParentNode.Children.IndexOf(selectedBox) + 1);
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
            itembox.ScriptLayout = this;
        }
        /// <summary>
        /// 从文件添加的过程设置stackparatitem的连接属性
        /// </summary>
        /// <param name="parat"></param>
        internal void SetLinkIProperty(StackParatItem parat)
        {
            if(parat.LinkipropertyId != "")
            {
                parat.LinkIProperty = FindIPropertyById(parat.LinkipropertyId);
            }

        }
        /// <summary>
        /// 设置从文件加载时候设置执行函数的赋值
        /// </summary>
        /// <param name="itembox"></param>
        internal void SetItemboxDoFunction(StackItemBox itembox)
        {
            TreeData td = FunctionDataList.FunctionView.GetTreeDataByName(itembox.Name);
            if (td != null)
            {
                td.HalfClone(itembox);
                IItemboxLoadEvent?.Invoke(itembox, td.Data);
            }
        }
        /// <summary>
        /// 显示树
        /// </summary>
        protected virtual void ShowTree()
        {
            if(IsUseTreeShow)
                dataTreePop.IsOpen = true;
        }
        /// <summary>
        /// 隐藏树
        /// </summary>
        protected virtual void HiddenTree()
        {
            dataTreePop.IsOpen = false;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override bool SaveToJson(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Create))
            {
                
                string json = Tools.ConverScriptToJson(this);
                byte[] datas = System.Text.Encoding.UTF8.GetBytes(json);
                fs.Write(datas, 0, datas.Length);
                datas = null;
                json = "";
            }
            return true;
        }
        /// <summary>
        /// 从json加载
        /// </summary>
        /// <typeparam name="Ibox">iitembox的实际类型</typeparam>
        /// <typeparam name="IProperty">连接线的实际类型</typeparam>
        /// <typeparam name="IParaitem">参数的实际类型</typeparam>
        /// <param name="file">json文件路径</param>
        public override void LoadFromJson<Ibox, IProperty, IParaitem>(string file)
        {
            using (FileStream fs = File.Open(file, FileMode.Open))
            {
                byte[] datas = new byte[fs.Length];
                fs.Read(datas, 0, datas.Length);
                string json = Encoding.UTF8.GetString(datas);
                ClearAll(true);
                Tools.ConvertJsonToLayout<Ibox, IParaitem, IProperty>(json, this);
                datas = null;
                json = null;
            }
            
            //foreach (var item in itemboxs)
            //{
            //    foreach (StackParatItem paratitem in item.InputDatas)
            //    {
            //        if(paratitem.LinkipropertyId != null && paratitem.LinkipropertyId != "")
            //        {
            //            paratitem.LinkIProperty = FindIPropertyById(paratitem.LinkipropertyId);
            //        }
            //    }
            //    foreach (StackParatItem paratitem in item.OutDatas)
            //    {
            //        if (paratitem.LinkipropertyId != null && paratitem.LinkipropertyId != "")
            //        {
            //            paratitem.LinkIProperty = FindIPropertyById(paratitem.LinkipropertyId);
            //        }
            //    }
                
                
            //}

        }
        /// <summary>
        /// 清除所有
        /// </summary>
        /// <param name="clearMain">是否清除入口</param>
        public override void ClearAll(bool clearMain = false)
        {
            IFunctionBoxs.Clear();
            Itemboxs.Clear();
            IPropertys.Clear();
            iPropertys.Clear();
            if (clearMain == false)
            {
                Add(mainIb);
            }
        }
        /// <summary>
        /// 运行脚本
        /// </summary>
        public override void RunCompile()
        {
            iScriptInterpreter.RunScript(this);
        }
        /// <summary>
        /// 停止脚本的运行
        /// </summary>
        public override void StopRun()
        {

        }
        #endregion
    }
}
