using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TkScripts.Script;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.BezierLinkLayout;
using TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout;
using static TkScripts.Interface.IScriptInterpreter;

namespace TkScripts.Interface
{
    /// <summary>
    /// 脚本编辑器
    /// </summary>
    public class IScriptLayout : UserControl, INotifyPropertyChanged, ICompile
    {
        /// <summary>
        /// 代码块存储名称
        /// </summary>
        public readonly static string ItemBoxsListName = "ItemBoxs";
        /// <summary>
        /// 属性存储名称
        /// </summary>
        public readonly static string IPropertyItsListName = "IPropertyIts";
        /// <summary>
        /// 脚本名称
        /// </summary>
        public readonly static string IScriptName = "ScriptName";
        /// <summary>
        /// 属性改变时候的通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 加载变量时候的赋值其他赋值操作（一些不可以保存的内存变量）
        /// </summary>
        public HalfCloneHandle IItemboxLoadEvent = null;
        /// <summary>
        /// 代码块被选中事件
        /// </summary>
        public ItemSelectedEvent IItemBoxSelected = null;
        /// <summary>
        /// 编译运行时候的信息回调
        /// </summary>
        public WriteStreamCallBack ComipleMessageCall = null;
        /// <summary>
        /// 脚本断点执行回调
        /// </summary>
        public ScriptBreakPointCallBackHandle ScriptBreakPoint
        {
            get
            {
                return iScriptInterpreter.ScriptBreakPoint;
            }
            set
            {
                iScriptInterpreter.ScriptBreakPoint = value;
            }
        }
        /// <summary>
        /// 属性改变
        /// </summary>
        /// <param name="name"></param>
        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 设置全局变量的值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public virtual void SetPropertyValue(string id, object value)
        {
            IPropertyIt pro = FindIPropertyById(id);
            if(pro != null)
            {
                pro.Value = value;
            }
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="iPropertyIt"></param>
        public virtual void Add(IPropertyIt iPropertyIt)
        {

        }
        /// <summary>
        /// 添加代码块
        /// </summary>
        /// <param name="iFunctionBox"></param>
        protected virtual void Add(IFunctionBox iFunctionBox)
        {

        }
        /// <summary>
        /// 添加代码块
        /// </summary>
        /// <param name="itembox"></param>
        public virtual void Add(IItemBox itembox)
        {

        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="propertyIt"></param>
        public virtual void Del(IPropertyIt propertyIt)
        {
            if (this.iPropertys.Contains(propertyIt))
            {
                this.iPropertys.Remove(propertyIt);
            }
        }
        /// <summary>
        /// 删除一个代码块
        /// </summary>
        /// <param name="iFunctionBox"></param>
        public virtual void Del(IFunctionBox iFunctionBox)
        {

        }
        /// <summary>
        /// 删除一个代码块
        /// </summary>
        /// <param name="box"></param>
        public virtual void Del(IItemBox box)
        {
            
        }
        /// <summary>
        /// 选中代码块
        /// </summary>
        /// <param name="box"></param>
        public virtual void SelectBox(IFunctionBox box)
        {
            IItemBoxSelected?.Invoke(box.DataContext as IItemBox);
        }
        /// <summary>
        /// 选中代码块
        /// </summary>
        /// <param name="box"></param>
        public virtual void SelectBox(IItemBox box)
        {
            IItemBoxSelected?.Invoke(box);
        }
        /// <summary>
        /// 删除所选择的
        /// </summary>
        public virtual void DeleteSelected()
        {
            
        }
        /// <summary>
        /// 设置函数处于运行
        /// </summary>
        /// <param name="color"></param>
        public virtual void SetFunctionBoxRun(Color color, IItemBox ibox)
        {
            this.Dispatcher.Invoke(new Action<Color, IItemBox>((c, ib) => {
                ib.BoxBrush = null;
                ib.BoxBrush = new SolidColorBrush(c);
            }), color, ibox);
        }
        /// <summary>
        /// 设置函数没有处于运行
        /// </summary>
        /// <param name="color"></param>
        public virtual void SetFunctionBoxStop(Color color, IItemBox ibox)
        {
            this.Dispatcher.Invoke(new Action<Color, IItemBox>((c, ib) => {
                ib.BoxBrush = null;
                ib.BoxBrush = new SolidColorBrush(c);
            }), color, ibox);
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="datas"></param>
        public virtual void SetFunctionData(TreeData datas)
        {
            this.treeDatas.Add(datas);
            this.DataTree.MyData = this.treeDatas;
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="datas"></param>
        public virtual void SetFunctionData(ObservableCollection<TreeData> datas)
        {
            foreach (var item in datas)
            {
                if(this.treeDatas.Contains(item) == false)
                    this.treeDatas.Add(item);
            }
            this.DataTree.MyData = this.treeDatas;
        }
        /// <summary>
        /// 从json加载
        /// </summary>
        /// <typeparam name="Ibox">iitembox的实际类型</typeparam>
        /// <typeparam name="IProperty">连接线的实际类型</typeparam>
        /// <typeparam name="IParaitem">参数的实际类型</typeparam>
        /// <param name="file">json文件路径</param>
        public virtual void LoadFromJson<Ibox, IProperty, IParaitem>(string file)
        {

        }
        /// <summary>
        /// 创建代码块事件
        /// </summary>
        /// <param name="data"></param>
        public virtual void DataTree_CreateCallback(IItemBox data)
        {

        }
        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public virtual bool SaveToJson(string filename)
        {
            return true;
        }
        /// <summary>
        /// 查找函数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MLParatItemLayout FindFunctionBoxById(string id, string childid)
        {
            foreach (var item in iFunctionBoxs)
            {
                IItemBox box = item.Ibox;
                if (box.Id == id)
                {
                    foreach (MLParatItemLayout pitem in item.Inputs)
                    {
                        IParatItem ipt = pitem.DataContext as IParatItem;
                        if (ipt.Id == childid)
                        {
                            return pitem;
                        }
                    }
                    foreach (MLParatItemLayout pitem in item.Outputs)
                    {
                        IParatItem ipt = pitem.DataContext as IParatItem;
                        if (ipt.Id == childid)
                        {
                            return pitem;
                        }
                    }
                    //return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 查找属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IPropertyIt FindIPropertyById(string id)
        {
            foreach (var item in iPropertys)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 清除所有
        /// </summary>
        /// <param name="clearMain">是否清除入口</param>
        public virtual void ClearAll(bool clearMain = false)
        {
            
        }
        /// <summary>
        /// 运行和编译
        /// </summary>
        /// <param name="wrs"></param>
        public virtual void RunCompile()
        {

        }
        /// <summary>
        /// 添加系统的代码块
        /// </summary>
        public static TreeData AddSystemBox()
        {
            TreeData system = new TreeData("系统");
            ItemBox ifitembox = new ItemBox()
            {
                BoxType = ItemBoxEnum.IF,
                Name = "分支",
                TipText = "分支",
            };
            ifitembox.AddInput(new ParatItem() { Name = "判断的值", PIEnum = ParaItemEnum.BOOL, TipText = "决定分支的走向" });
            //ifitembox.Add(new StackItemBox() { Name = "True", BoxType = ItemBoxEnum.ELSE });
            //ifitembox.Add(new StackItemBox() { Name = "False", BoxType = ItemBoxEnum.ELSE });

            ItemBox whileidx = new ItemBox()
            {
                BoxType = ItemBoxEnum.WHILE,
                Name = "循环(由起始和结束值决定次数)",
                TipText = "循环",
            };
            whileidx.AddInput(new ParatItem()
            {
                Name = "起始值",
                PIEnum = ParaItemEnum.INT,
                TipText = "起始值",
                Value = 0,
            });
            whileidx.AddInput(new ParatItem()
            {
                Name = "终止值",
                PIEnum = ParaItemEnum.INT,
                TipText = "终止值",
                Value = 10,
            });

            whileidx.AddOutput(new ParatItem()
            {
                Name = "迭代值",
                PIEnum = ParaItemEnum.INT,
                TipText = "迭代值"
            });

            ItemBox whilebool = new ItemBox()
            {
                BoxType = ItemBoxEnum.WHILE,
                Name = "循环(由bool参数决定次数)",
                TipText = "循环",
            };
            whilebool.AddInput(new ParatItem() { Name = "判断的值", PIEnum = ParaItemEnum.BOOL, TipText = "如果是true则继续循环，否则调出循环", Value = true, });

            ItemBox whiletime = new ItemBox()
            {
                BoxType = ItemBoxEnum.WHILE,
                Name = "循环(由时间参数决定次数)",
                TipText = "循环",
            };
            whiletime.AddInput(new ParatItem() { Name = "结束时间", PIEnum = ParaItemEnum.DATETIME, TipText = "如果当前事件超过设定的时间则跳出循环", Value = DateTime.Now, });

            ifitembox.ForgeBrush = new SolidColorBrush(Colors.Blue);
            whileidx.ForgeBrush = new SolidColorBrush(Colors.Blue);
            whilebool.ForgeBrush = new SolidColorBrush(Colors.Blue);
            whiletime.ForgeBrush = new SolidColorBrush(Colors.Blue);

            ifitembox.LogoPath = ScriptHelp.BrachImage;
            whileidx.LogoPath = ScriptHelp.WhileImage;
            whilebool.LogoPath = ScriptHelp.WhileImage;
            whiletime.LogoPath = ScriptHelp.WhileImage;
            system.Children.Add(new TreeData(ifitembox));
            system.Children.Add(new TreeData(whileidx));
            system.Children.Add(new TreeData(whilebool));
            system.Children.Add(new TreeData(whiletime));
            return system;
        }
        /// <summary>
        /// 添加功能函数
        /// </summary>
        /// <returns></returns>
        public static TreeData AddToolsFunction()
        {
            TreeData tdheadfordevice = new TreeData("功能函数");
            IItemBox box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.DelyTime);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.AddInt);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.AddFloat);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.TimeDesc);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.PrintObject);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.EqualInt);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.EqualDouble);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.GetNowTime);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.SetIntValue);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.SetFloatValue);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.SetBoolValue);
            box.ForgeBrush = new SolidColorBrush(Colors.Blue);
            box = null;
            return tdheadfordevice;
        }
        public virtual void IScriptLayout_KeyDown(object sender, KeyEventArgs e)
        { }
        /// <summary>
        /// 停止脚本的运行
        /// </summary>
        public virtual void StopRun()
        {

        }
        #region 属性
        /// <summary>
        /// 是否启用显示树
        /// </summary>
        private bool isUseTreeShow = false;
        /// <summary>
        /// 名称
        /// </summary>
        private string scriptName = "";
        /// <summary>
        /// 字体颜色风格
        /// </summary>
        protected Color forgeStyleColor = Colors.White;
        ///// <summary>
        ///// 字体颜色风格
        ///// </summary>
        //protected Color borderStyleColor = Colors.White;
        /// <summary>
        /// 代码块内容
        /// </summary>
        private DataTreeView dataTree = new DataTreeView();
        /// <summary>
        /// 显示树状数据弹出框
        /// </summary>
        protected Popup dataTreePop = new Popup();
        /// <summary>
        /// 属性集合
        /// </summary>
        protected ObservableCollection<IPropertyIt> iPropertys = new ObservableCollection<IPropertyIt>();
        /// <summary>
        /// 代码块控件集合
        /// </summary>
        protected ObservableCollection<IFunctionBox> iFunctionBoxs = new ObservableCollection<IFunctionBox>();
        /// <summary>
        /// 代码块集合
        /// </summary>
        protected ObservableCollection<IItemBox> itemboxs = new ObservableCollection<IItemBox>();
        /// <summary>
        /// 数据源
        /// </summary>
        protected ObservableCollection<TreeData> treeDatas = new ObservableCollection<TreeData>();
        /// <summary>
        /// 主任务入口
        /// </summary>
        protected LineItemBox mainIb = null;
        /// <summary>
        /// id
        /// </summary>
        private string id = Tools.CreateId();
        /// <summary>
        /// 脚本解释器
        /// </summary>
        protected IScriptInterpreter iScriptInterpreter = null;
        #endregion
        #region 访问器
        public ObservableCollection<IPropertyIt> IPropertys
        {
            get
            {
                return iPropertys;
            }
        }

        public ObservableCollection<IFunctionBox> IFunctionBoxs
        {
            get
            {
                return iFunctionBoxs;
            }
        }

        public ObservableCollection<IItemBox> Itemboxs
        {
            get
            {
                return itemboxs;
            }
        }
        /// <summary>
        /// 脚本入口
        /// </summary>
        public IItemBox MainIb
        {
            get
            {
                return mainIb;
            }
        }
        /// <summary>
        /// 字体颜色风格
        /// </summary>
        protected Color ForgeStyleColor
        {
            get
            {
                return forgeStyleColor;
            }

            set
            {
                forgeStyleColor = value;
                Changed("ForgeStyleColor");
                foreach (var item in itemboxs)
                {
                    item.ForgeBrush = new SolidColorBrush(forgeStyleColor);
                    foreach (var paratitem in item.InputDatas)
                    {
                        paratitem.Color = forgeStyleColor;
                        paratitem.ForgeColor = forgeStyleColor;
                    }
                    foreach (var paratitem in item.OutDatas)
                    {
                        paratitem.Color = forgeStyleColor;
                        paratitem.ForgeColor = forgeStyleColor;
                    }
                }
            }
        }
        /// <summary>
        /// 脚本名称
        /// </summary>
        public string ScriptName
        {
            get
            {
                return scriptName;
            }

            set
            {
                scriptName = value;
                Changed("ScriptName");
            }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public DataTreeView DataTree
        {
            get
            {
                return dataTree;
            }
        }
        /// <summary>
        /// 是否启用显示树
        /// </summary>
        public bool IsUseTreeShow
        {
            get
            {
                return isUseTreeShow;
            }

            set
            {
                isUseTreeShow = value;
            }
        }
        /// <summary>
        /// id
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                Changed("Id");
            }
        }
        #endregion
    }
}
