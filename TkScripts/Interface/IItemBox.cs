using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TkScripts.Script;
using TkScripts.ScriptLayout;

namespace TkScripts.Interface
{
    /// <summary>
    /// 代码块
    /// </summary>
    
    public interface IItemBox : INotifyPropertyChanged, IDisplayInterface
    {
        /// <summary>
        /// ID
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 粗细
        /// </summary>
        double Thickness { get; set; }
        /// <summary>
        /// 函数类型
        /// </summary>
        ItemBoxEnum BoxType { get; set; }
        /// <summary>
        /// 是否拥有断点
        /// </summary>
        bool IsHasBreakPoint { get; set; }
        /// <summary>
        /// 所属脚本
        /// </summary>
        [JsonIgnore]
        IScriptLayout ScriptLayout { get; set; }
        /// <summary>
        /// 输入参数列表
        /// </summary>
        ObservableCollection<IParatItem> InputDatas { get; }

        /// <summary>
        /// 输入参数列表
        /// </summary>
        ObservableCollection<IParatItem> OutDatas { get; }
        /// <summary>
        /// 添加输入值
        /// </summary>
        /// <param name="pitem"></param>
        void AddInput(IParatItem pitem);
        /// <summary>
        /// 添加输出值
        /// </summary>
        /// <param name="pitem"></param>
        void AddOutput(IParatItem pitem);
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        IItemBox Clone();
        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        void Copy(IItemBox box);
        /// <summary>
        /// 属性改变
        /// </summary>
        void PropertyValueChanged(object sender, PropertyChangedEventArgs e);
        /// <summary>
        /// 脚本执行事件
        /// </summary>
        ScriptFunction ScriptFunction { get; set; }
        /// <summary>
        /// 克隆脚本函数地址
        /// </summary>
        /// <param name="opbox"></param>
        void CloneScriptFunction(IItemBox opbox);
        /// <summary>
        /// 设置脚本函数地址
        /// </summary>
        /// <param name="sf"></param>
        void SetScriptFunction(ScriptFunction sf);
        /// <summary>
        /// 脚本执行函数
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        ScriptOutput DoScriptFunction(ScriptInput si);
    }
    /// <summary>
    /// 属性
    /// </summary>
    public abstract class IPropertyIt : INotifyPropertyChanged, IDisplayInterface
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private FrameworkElement uiMain = null;
        private ParaItemEnum pIEnum;
        private string name = "";
        private object value = null;
        private string id = Tools.CreateId();
        private string child_id = Tools.CreateId();
        private string tipText = "";
        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource LogoPath { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        [ToJson(true)]
        public ParaItemEnum PIEnum
        {
            get
            {
                return pIEnum;
            }

            set
            {
                pIEnum = value;
                Changed("PIEnum");
            }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        [ToJson(true)]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                Changed("Name");
            }
        }
        /// <summary>
        /// 当前值
        /// </summary>
        [JsonIgnore]
        public object Value
        {
            get
            {
                if (value == null)
                {
                    return "未知";
                }
                try
                {
                    return value;
                }
                catch (Exception ex)
                {
                    return "转换出错";
                }
            }

            set
            {
                this.value = value;
                Changed("Value");
            }
        }
        /// <summary>
        /// 主显示
        /// </summary>
        public FrameworkElement UIMain
        {
            get
            {
                return uiMain;
            }

            set
            {
                uiMain = value;
                Changed("UIMain");
            }
        }
        /// <summary>
        /// id
        /// </summary>
        [ToJsonAttribute(true)]
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
        /// <summary>
        /// 子内容id
        /// </summary>
        [ToJsonAttribute(true)]
        public string Child_id
        {
            get
            {
                return child_id;
            }

            set
            {
                child_id = value;
                Changed("Child_id");
            }
        }
        /// <summary>
        /// 提示文字
        /// </summary>
        [ToJsonAttribute(true)]
        public string TipText
        {
            get
            {
                return tipText;
            }

            set
            {
                tipText = value;
            }
        }

        public Brush ForgeBrush
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Brush BoxBrush
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 获取一个 属性获取框
        /// </summary>
        /// <returns></returns>
        public virtual IItemBox GetGetPropertyBox()
        {
            IItemBox ibx = new ItemBox();
            ibx.Id = this.Id;
            ibx.Name = "获取" + this.name;
            ibx.BoxType = ItemBoxEnum.GET;
            ibx.TipText = "设置" + name + "属性的值";
            IParatItem ipt = new ParatItem();
            //ipt.Id = this.Child_id;
            ipt.UIMain = UIMain;
            ipt.Name = Name;
            ipt.PIEnum = this.PIEnum;
            ipt.ParentItemBox = ibx;
            ipt.Value = value;
            ibx.OutDatas.Add(ipt);
            this.PropertyChanged += ibx.PropertyValueChanged;
            return ibx;
        }
        /// <summary>
        /// 获取设置属性的设置框
        /// </summary>
        /// <returns></returns>
        public virtual IItemBox GetSetPropertyBox()
        {
            IItemBox ibx = new ItemBox();
            ibx.Id = this.Id;
            ibx.Name = "设置" + this.name;
            ibx.BoxType = ItemBoxEnum.SET;
            ibx.TipText = "设置" + name + "属性的值";
            IParatItem ipt = new ParatItem();
            ipt.Name = "输入";
            ipt.PIEnum = ParaItemEnum.INPUT;
            ipt.UIMain = UIMain;
            ipt.ParentItemBox = ibx;
            ibx.InputDatas.Add(ipt);

            ipt = new ParatItem();
            ipt.Name = "输出";
            ipt.PIEnum = ParaItemEnum.OUTPUT;
            ipt.UIMain = UIMain;
            ipt.ParentItemBox = ibx;
            ibx.OutDatas.Add(ipt);

            ipt = new ParatItem();
            ipt.UIMain = UIMain;
            ipt.Name = Name;
            ipt.Value = value;
            ipt.PIEnum = this.PIEnum;
            ipt.ParentItemBox = ibx;
            //ipt.Id = this.Child_id;
            ibx.InputDatas.Add(ipt);

            ipt = new ParatItem();
            ipt.UIMain = UIMain;
            ipt.Name = Name;
            ipt.Value = value;
            ipt.PIEnum = this.PIEnum;
            ipt.ParentItemBox = ibx;
            //ipt.Id = this.Child_id;
            ibx.OutDatas.Add(ipt);
            this.PropertyChanged += ibx.PropertyValueChanged;
            return ibx;
        }
    }
    /// <summary>
    /// 参数项
    /// </summary>
    public abstract class IParatItem : INotifyPropertyChanged, IDisplayInterface
    {
        private string id = "";
        private ParaItemEnum pIEnum;
        private string tipText = "";
        private object value;
        private string name;
        private IParatItem linkIParatItem = null;
        private IItemBox parentItemBox = null;
        private SolidColorBrush sold = new SolidColorBrush(Colors.White);
        private SolidColorBrush forgeBrush = new SolidColorBrush(Colors.Black);
        private List<string> enumDatas = null;

        /// <summary>
        /// 计算LinkPosition相对点的面板
        /// </summary>
        private UIElement uiMain = null;
        /// <summary>
        /// 参数的类型
        /// </summary>
        [ToJsonAttribute(true)]
        public ParaItemEnum PIEnum
        {
            get
            {
                return pIEnum;
            }

            set
            {
                pIEnum = value;
                switch (PIEnum)
                {
                    case ParaItemEnum.INT:
                        this.Color = Color.FromArgb(0xff, 0xcf, 0xcf, 0x24);
                        break;
                    case ParaItemEnum.FLOAT:
                        this.Color = Color.FromArgb(0xff, 0x64, 0xc7, 0x27);
                        break;
                    case ParaItemEnum.BOOL:
                        this.Color = Color.FromArgb(0xff, 0x27, 0xc7, 0xb9);
                        break;
                    case ParaItemEnum.STRING:
                        this.Color = Color.FromArgb(0xff, 0x2c, 0x6b, 0xc7);
                        break;
                    case ParaItemEnum.DATETIME:
                        this.Color = Color.FromArgb(0xff, 0xc7, 0x2c, 0x5d);
                        break;
                    case ParaItemEnum.OBJECT:
                        this.Color = Color.FromArgb(0xff, 0xb6, 0x22, 0x0b);
                        break;
                    case ParaItemEnum.ENUM:
                        this.Color = Color.FromArgb(0xff, 0x97, 0x0b, 0xb6);
                        break;
                    case ParaItemEnum.NULL:
                        this.Color = Colors.Black;
                        break;
                    case ParaItemEnum.INPUT:
                        break;
                    case ParaItemEnum.OUTPUT:
                        break;
                    default:
                        break;
                }
                if (PIEnum == ParaItemEnum.BOOL)
                {
                    if (EnumDatas != null)
                    {
                        EnumDatas.Clear();
                        EnumDatas = null;
                    }
                    EnumDatas = new List<string>() { "true", "false" };
                }
                Changed("Value");
            }
        }
        /// <summary>
        /// 参数的值
        /// </summary>
        [ToJsonAttribute(true)]
        public object Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                Changed("Value");
            }
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        [ToJsonAttribute(true)]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                Changed("Name");
            }
        }
        /// <summary>
        /// 枚举类型参数的数据源
        /// </summary>
        public List<string> EnumDatas
        {
            get
            {
                if(this.PIEnum == ParaItemEnum.BOOL
                    && (this.enumDatas == null || this.enumDatas.Count == 0))
                {
                    enumDatas = new List<string>() { "true", "false" };
                }
                //value = "true";
                return enumDatas;
            }

            set
            {
                enumDatas = value;
                Changed("EnumDatas");
            }
        }
        /// <summary>
        /// 计算LinkPosition相对点的面板
        /// </summary>
        [JsonIgnore]
        public UIElement UIMain
        {
            get
            {
                return uiMain;
            }

            set
            {
                uiMain = value;
                Changed("UIMain");
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        [JsonIgnore]
        public Color Color
        {
            get
            {
                return sold.Color;
            }

            set
            {
                sold.Color = value;
                Changed("Color");
                Changed("BorderBrush");
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        [JsonIgnore]
        public Color ForgeColor
        {
            get
            {
                return forgeBrush.Color;
            }

            set
            {
                forgeBrush.Color = value;
                Changed("ForgeColor");
                Changed("ForgeBrush");
            }
        }
        /// <summary>
        /// 连接的对象
        /// </summary>
        [JsonIgnore]
        public IParatItem LinkIParatItem
        {
            get
            {
                return linkIParatItem;
            }

            set
            {
                linkIParatItem = value;
                Changed("LinkIParatItem");
            }
        }
        /// <summary>
        /// 依存的函数体
        /// </summary>
        [JsonIgnore]
        public IItemBox ParentItemBox
        {
            get
            {
                return parentItemBox;
            }

            internal set
            {
                parentItemBox = value;
            }
        }
        /// <summary>
        /// id
        /// </summary>
        [ToJsonAttribute(true)]
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
        /// <summary>
        /// 提示文字
        /// </summary>
        [ToJson(true)]
        public string TipText
        {
            get
            {
                return tipText;
            }

            set
            {
                tipText = value;
                Changed("TipText");
            }
        }

        Brush IDisplayInterface.ForgeBrush
        {
            get
            {
                return forgeBrush;
            }

            set
            {
                //forgeBrush = value;
            }
        }

        Brush IDisplayInterface.BoxBrush
        {
            get
            {
                return sold;
            }

            set
            {
                
            }
        }

        public ImageSource LogoPath
        {
            get
            {
                return parentItemBox.LogoPath;
            }

            set
            {
                parentItemBox.LogoPath = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性改变
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public virtual IParatItem Clone()
        {
            IParatItem ip = (IParatItem)Activator.CreateInstance(GetType());

            ip.Color = Color;
            ip.enumDatas = enumDatas;
            ip.name = name;
            ip.parentItemBox = parentItemBox;
            ip.value = value;
            ip.pIEnum = pIEnum;
            ip.uiMain = uiMain;
            ip.TipText = TipText;
            return ip;
        }
        /// <summary>
        /// 赋值数据
        /// </summary>
        /// <param name=""></param>
        public virtual void Copy(IParatItem parat)
        {
            this.enumDatas = parat.enumDatas;
            this.linkIParatItem = parat.linkIParatItem;
            this.Name = parat.Name;
            this.PIEnum = parat.PIEnum;
            this.TipText = parat.TipText;
            this.UIMain = parat.UIMain;
            this.Value = parat.Value;
        }
    }
    /// <summary>
    /// 参数类
    /// </summary>
    public class ParatItem : IParatItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParatItem()
        {
            this.Id = Tools.CreateId();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itembox"></param>
        /// <param name="enumDatas"></param>
        public ParatItem(string name, ParaItemEnum pie, List<string> enumDatas = null)
        {
            this.Id = Tools.CreateId();
            this.Name = name;
            this.EnumDatas = enumDatas;
            this.PIEnum = pie;
        }
    }
    /// <summary>
    /// 函数类
    /// </summary>
    public class ItemBox : IItemBox
    {
        private string id = Tools.CreateId();
        protected bool isHasBreakPoint = false;
        private string name = "";
        private string tipText = "";
        private IScriptLayout scriptLayout = null;
        private double thickness = 2;
        private ItemBoxEnum boxType = ItemBoxEnum.FUNCTION;
        private SolidColorBrush forgeBrush = new SolidColorBrush(Colors.White);
        private SolidColorBrush boxBrush = new SolidColorBrush(Colors.White);
        private ObservableCollection<IParatItem> inputDatas = new ObservableCollection<IParatItem>();
        private ObservableCollection<IParatItem> outputDatas = new ObservableCollection<IParatItem>();
        private ImageSource logoPath = ScriptHelp.FunctionImage;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemBox(string name, string tip)
        {
            this.Name = name;
            this.TipText = tip;
        }
        public ItemBox()
        {
            this.Id = Tools.CreateId();
        }
        /// <summary>
        /// 图标路径
        /// </summary>
        public ImageSource LogoPath
        {
            get
            {
                return logoPath;
            }
            set
            {
                logoPath = value;
                Changed("LogoPath");
            }
        }
        
        /// <summary>
        /// 输入数据
        /// </summary>
        public ObservableCollection<IParatItem> InputDatas
        {
            get
            {
                return inputDatas;
            }
        }
        /// <summary>
        /// 输出数据列表
        /// </summary>
        public ObservableCollection<IParatItem> OutDatas
        {
            get
            {
                return outputDatas;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [ToJson(true)]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                Changed("Name");
            }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        [JsonIgnore]
        public Brush ForgeBrush
        {
            get
            {
                return forgeBrush;
            }
            set
            {
                forgeBrush = (SolidColorBrush)value;
                Changed("ForgeBrush");
            }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        [JsonIgnore]
        public Color ForgeColor
        {
            get
            {
                return forgeBrush.Color;
            }
            set
            {
                forgeBrush.Color = value;
                Changed("ForgeColor");
                Changed("ForgeBrush");
            }
        }
        /// <summary>
        /// 边框画刷
        /// </summary>
        [JsonIgnore]
        public Brush BoxBrush
        {
            get
            {
                return boxBrush;
            }

            set
            {
                boxBrush = (SolidColorBrush)value;
                Changed("BoxBrush");
            }
        }
        
        [ToJson(true)]
        public ItemBoxEnum BoxType
        {
            get
            {
                return boxType;
            }

            set
            {
                boxType = value;
            }
        }
        /// <summary>
        /// 提示文字
        /// </summary>
        [ToJson(true)]
        public string TipText
        {
            get
            {
                return tipText;
            }

            set
            {
                tipText = value;
                Changed("TipText");
            }
        }
        [JsonIgnore]
        public double Thickness
        {
            get
            {
                return thickness;
            }

            set
            {
                thickness = value;
                Changed("Thickness");
            }
        }
        /// <summary>
        /// id
        /// </summary>
        [ToJson(true)]
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
        /// <summary>
        /// 脚本函数
        /// </summary>
        [JsonIgnore]
        ScriptFunction IItemBox.ScriptFunction
        {
            get
            {
                return ScriptFunction;
            }

            set
            {
                ScriptFunction = value;
            }
        }
        /// <summary>
        /// 所属脚本
        /// </summary>
        [JsonIgnore]
        public IScriptLayout ScriptLayout
        {
            get
            {
                return scriptLayout;
            }

            set
            {
                scriptLayout = value;
            }
        }
        /// <summary>
        /// 是否拥有断点
        /// </summary>
        [ToJson(true)]
        public bool IsHasBreakPoint
        {
            get
            {
                return isHasBreakPoint;
            }

            set
            {
                isHasBreakPoint = value;
                Changed("IsHasBreakPoint");
            }
        }

        /// <summary>
        /// 属性改变
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 脚本函数
        /// </summary>
        public ScriptFunction ScriptFunction = null;

        /// <summary>
        /// 属性修改的名称
        /// </summary>
        /// <param name="name"></param>
        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        public ScriptOutput DoScriptFunction(ScriptInput si)
        {
            return ScriptFunction?.Invoke(si);
        }
        /// <summary>
        /// 添加输入
        /// </summary>
        /// <param name="pitem"></param>
        public void AddInput(IParatItem pitem)
        {
            pitem.ParentItemBox = this;
            pitem.UIMain = this.scriptLayout;
            this.InputDatas.Add(pitem);
        }
        /// <summary>
        /// 添加输出
        /// </summary>
        /// <param name="pitem"></param>
        public void AddOutput(IParatItem pitem)
        {
            pitem.ParentItemBox = this;
            pitem.UIMain = this.scriptLayout;
            this.OutDatas.Add(pitem);
        }
        /// <summary>
        /// 克隆一份
        /// </summary>
        /// <returns></returns>
        public virtual IItemBox Clone()
        {
            //ItemBox ib = new ItemBox();
            ItemBox ib = (ItemBox)Activator.CreateInstance(GetType());
            ib.name = name;
            ib.boxBrush = boxBrush;
            ib.tipText = tipText;
            ib.Thickness = thickness;
            ib.BoxType = BoxType;
            ib.forgeBrush = forgeBrush;
            ib.ScriptFunction = ScriptFunction;
            foreach (var item in inputDatas)
            {
                ib.AddInput(item.Clone());
            }
            foreach (var item in outputDatas)
            {
                ib.AddOutput(item.Clone());
            }
            return ib;
        }
        /// <summary>
        /// 从box中复制信息
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public virtual void Copy(IItemBox box)
        {
            //this.Id = box.Id;
            this.name = box.Name;
            this.BoxBrush = box.BoxBrush;
            this.tipText = box.TipText;
            this.Thickness = box.Thickness;
            this.BoxType = box.BoxType;
            this.BoxBrush = box.ForgeBrush;
            this.logoPath = box.LogoPath;
            this.ScriptFunction = box.ScriptFunction;
            this.InputDatas.Clear();
            this.OutDatas.Clear();
            foreach (var item in box.InputDatas)
            {
                this.AddInput(item.Clone());
            }
            foreach (var item in box.OutDatas)
            {
                this.AddOutput(item.Clone());
            }
        }
        /// <summary>
        /// 克隆脚本函数将opbox的函数地址复制给此对象
        /// </summary>
        /// <param name="opbox"></param>
        public void CloneScriptFunction(IItemBox opbox)
        {
            this.ScriptFunction = opbox.ScriptFunction;
        }
        /// <summary>
        /// 熟悉改变事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        public void PropertyValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if(sender as IPropertyIt != null && e.PropertyName == "Name")
            {
                IPropertyIt ip = sender as IPropertyIt;
                if (this.boxType == ItemBoxEnum.SET)
                {
                    this.Name = "设置" + ip.Name + "属性的值";
                    if (this.InputDatas.Count > 1)
                    {
                        InputDatas[1].Name = ip.Name;
                    }
                    if (this.outputDatas.Count > 1)
                    {
                        outputDatas[1].Name = ip.Name;
                    }
                }
                else if(this.boxType == ItemBoxEnum.GET)
                {
                    this.Name = "获取" + ip.Name + "属性的值";
                    if (this.outputDatas.Count > 1)
                    {
                        outputDatas[0].Name = ip.Name;
                    }
                }
                
            }
        }
        /// <summary>
        /// 设置脚本函数地址
        /// </summary>
        /// <param name="sf"></param>
        public void SetScriptFunction(ScriptFunction sf)
        {
            this.ScriptFunction = sf;
        }

        
    }
    /// <summary>
    /// 属性类
    /// </summary>
    public class PropertyIt : IPropertyIt
    {
    }
}
