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
using TKScriptsServer.Agreement;
using TKScriptsServer.Client;

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
        /// 函数类型
        /// </summary>
        ItemBoxEnum BoxType { get; set; }
        /// <summary>
        /// 是否拥有断点
        /// </summary>
        bool IsHasBreakPoint { get; set; }
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
        /// 获取输入的列表
        /// </summary>
        /// <returns></returns>
        ObservableCollection<IParatItem> GetInputParatItems();
        /// <summary>
        /// 获取输出列表
        /// </summary>
        /// <returns></returns>
        ObservableCollection<IParatItem> GetOutputParatItems();
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
        /// 脚本请求地址
        /// </summary>
        string ScriptUrl { get; set; }
    }
    /// <summary>
    /// 属性
    /// </summary>
    public abstract class IPropertyIt : INotifyPropertyChanged, IDisplayInterface
    {
        /// <summary>
        /// 属性变更
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性变更通知
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private ParaItemEnum pIEnum;
        private string name = "";
        private object value = null;
        private string id = Tools.CreateId();
        private string tipText = "";
        /// <summary>
        /// 图标
        /// </summary>
        [JsonIgnore]
        public ImageSource LogoPath { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
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
        /// <summary>
        /// 提示文字
        /// </summary>
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
        private IItemBox parentItemBox = null;
        private List<string> enumDatas = null;

        /// <summary>
        /// 参数的类型
        /// </summary>
        public ParaItemEnum PIEnum
        {
            get
            {
                return pIEnum;
            }

            set
            {
                pIEnum = value;
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

        /// <summary>
        /// 属性改变通知
        /// </summary>
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
            ip.enumDatas = enumDatas;
            ip.name = name;
            ip.parentItemBox = parentItemBox;
            ip.value = value;
            ip.pIEnum = pIEnum;
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
            this.Name = parat.Name;
            this.PIEnum = parat.PIEnum;
            this.TipText = parat.TipText;
            this.Value = parat.Value;
        }
    }
    /// <summary>
    /// 参数类
    /// </summary>
    public class ParatItem : IParatItem
    {
        private IPropertyIt linkiProperty = null;
        /// <summary>
        /// 连接的属性id
        /// </summary>
        private string linkipropertyId = "";

        /// <summary>
        /// 属性
        /// </summary>
        [JsonIgnore]
        public IPropertyIt LinkIProperty
        {
            get
            {
                return linkiProperty;
            }

            set
            {
                linkiProperty = value;
                Changed("LinkIProperty");
                Changed("LinkPropertyName");
            }
        }
        /// <summary>
        /// 连接的属性id
        /// </summary>
        public string LinkipropertyId
        {
            get
            {
                return linkiProperty != null ? linkiProperty.Id : linkipropertyId;
            }

            set
            {
                linkipropertyId = value;
            }
        }
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
        #region 属性
        private string id = Tools.CreateId();
        protected bool isHasBreakPoint = false;
        private string name = "";
        private string tipText = "";
        private string parentNodeId = "";
        private ItemBoxEnum boxType = ItemBoxEnum.FUNCTION;
        private ObservableCollection<ParatItem> inputDatas = new ObservableCollection<ParatItem>();
        private ObservableCollection<ParatItem> outputDatas = new ObservableCollection<ParatItem>();
        private ImageSource logoPath = ScriptHelp.FunctionImage;
        /// <summary>
        /// 是否显示
        /// </summary>
        private Visibility isVisiblity = Visibility.Visible;
        /// <summary>
        /// 是否扩展
        /// </summary>
        private bool isExpanded = false;
        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool isSelected = false;
        /// <summary>
        /// 父节点
        /// </summary>
        private ItemBox parentNode = null; 
        #endregion
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
        #region 访问器
        /// <summary>
        /// 图标路径
        /// </summary>
        [JsonIgnore]
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
        public ObservableCollection<ParatItem> InputDatas
        {
            get
            {
                return inputDatas;
            }
        }
        /// <summary>
        /// 输出数据列表
        /// </summary>
        public ObservableCollection<ParatItem> OutDatas
        {
            get
            {
                return outputDatas;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
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

        /// <summary>
        /// 是否拥有断点
        /// </summary>
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
        /// 脚本url
        /// </summary>
        public string ScriptUrl { get; set; }

        /// <summary>
        /// 属性改变
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 子元素
        /// </summary>
        public ObservableCollection<ItemBox> Children { get; set; } = new ObservableCollection<ItemBox>();
        /// <summary>
        /// 父节点
        /// </summary>
        [JsonIgnore]
        public ItemBox ParentNode
        {
            get
            {
                return parentNode;
            }

            set
            {
                parentNode = value;
                Changed("ParentNode");
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public Visibility IsVisiblity
        {
            get
            {
                return isVisiblity;
            }

            set
            {
                isVisiblity = value;
                Changed("IsVisiblity");
            }
        }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }

            set
            {
                isExpanded = value;
                Changed("IsExpanded");
            }
        }
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                Changed("IsSelected");
            }
        }
        /// <summary>
        /// 父节点的id
        /// </summary>
        public string ParentId
        {
            get
            {
                if(parentNode == null)
                {
                    return parentNodeId;
                }
                else
                {
                    return parentNode.Id;
                }
            }
            set
            {
                this.parentNodeId = value;
            }
        }
        #endregion

        /// <summary>
        /// 属性修改的名称
        /// </summary>
        /// <param name="name"></param>
        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        /// <summary>
        /// 添加输入
        /// </summary>
        /// <param name="pitem"></param>
        public void AddInput(IParatItem pitem)
        {
            ParatItem paratItem = pitem as ParatItem;
            pitem.ParentItemBox = this;
            this.InputDatas.Add(paratItem);
        }
        /// <summary>
        /// 添加输出
        /// </summary>
        /// <param name="pitem"></param>
        public void AddOutput(IParatItem pitem)
        {
            ParatItem paratItem = pitem as ParatItem;
            pitem.ParentItemBox = this;
            this.OutDatas.Add(paratItem);
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
            ib.tipText = tipText;
            ib.BoxType = BoxType;
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
            this.tipText = box.TipText;
            this.BoxType = box.BoxType;
            this.InputDatas.Clear();
            this.ScriptUrl = box.ScriptUrl;
            this.OutDatas.Clear();
            ObservableCollection<IParatItem> paratItems = box.GetInputParatItems();
            foreach (var item in paratItems)
            {
                this.AddInput(item.Clone());
            }
            paratItems.Clear();
            paratItems = box.GetOutputParatItems();
            foreach (var item in paratItems)
            {
                this.AddOutput(item.Clone());
            }
            paratItems.Clear();
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
        /// 获取输入列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<IParatItem> GetInputParatItems()
        {
            ObservableCollection<IParatItem> IParatItems = new ObservableCollection<IParatItem>();
            foreach (var item in inputDatas)
            {
                IParatItems.Add(item as IParatItem);
            }
            return IParatItems;
        }
        /// <summary>
        /// 获取输出列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<IParatItem> GetOutputParatItems()
        {
            ObservableCollection<IParatItem> IParatItems = new ObservableCollection<IParatItem>();
            foreach (var item in outputDatas)
            {
                IParatItems.Add(item as IParatItem);
            }
            return IParatItems;
        }

        /// <summary>
        /// 添加一个子元素
        /// </summary>
        /// <param name="stackItemBox"></param>
        public void Add(ItemBox stackItemBox)
        {
            Children.Add(stackItemBox);
            stackItemBox.ParentNode = this;
        }
        /// <summary>
        /// 添加一个子元素
        /// </summary>
        /// <param name="stackItemBox"></param>
        public void Add(ItemBox stackItemBox, int index)
        {
            Children.Insert(index, stackItemBox);
            stackItemBox.ParentNode = this;
        }

        /// <summary>
        /// 添加一个数组
        /// </summary>
        /// <param name="boxs"></param>
        /// <param name="index"></param>
        public void AddRange(ObservableCollection<ItemBox> boxs, int index)
        {
            foreach (var item in boxs)
            {
                item.ParentNode = this;
                this.Children.Insert(index++, item);
            }

        }
        /// <summary>
        /// 删除一个代码块
        /// </summary>
        /// <param name="stackItemBox"></param>
        public void Del(ItemBox stackItemBox)
        {
            if (Children.Contains(stackItemBox))
            {
                Children.Remove(stackItemBox);
            }
        }
    }
    /// <summary>
    /// 属性类
    /// </summary>
    public class PropertyIt : IPropertyIt
    {
    }
}
