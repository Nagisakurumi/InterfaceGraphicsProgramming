using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TkScripts.Interface;

namespace TkScripts.ScriptLayout.StackingLayout
{
    public class StackItemBox : ItemBox
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StackItemBox()
        {
            
        }
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
        private StackItemBox parentNode = null;
        /// <summary>
        /// 子元素
        /// </summary>
        public ObservableCollection<StackItemBox> Children
        {
            get
            {
                return children;
            }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        [JsonIgnore]
        public StackItemBox ParentNode
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
        /// 子元素
        /// </summary>
        private ObservableCollection<StackItemBox> children = new ObservableCollection<StackItemBox>();
        public override IItemBox Clone()
        {
            StackItemBox box = base.Clone() as StackItemBox;
            foreach (var item in this.Children)
            {
                box.Add(item.Clone() as StackItemBox);
            }
            return box;
        }
        /// <summary>
        /// 赋值数据
        /// </summary>
        /// <param name="box"></param>
        public override void Copy(IItemBox box)
        {
            this.Name = box.Name;
            this.BoxBrush = box.BoxBrush;
            this.TipText = box.TipText;
            this.Thickness = box.Thickness;
            this.BoxType = box.BoxType;
            this.ForgeBrush = box.ForgeBrush;
            this.ScriptFunction = box.ScriptFunction;
            this.LogoPath = box.LogoPath;
            this.InputDatas.Clear();
            this.OutDatas.Clear();
            foreach (var item in box.InputDatas)
            {
                StackParatItem pitem = new StackParatItem();
                pitem.Copy(item);
                this.AddInput(pitem);
            }
            foreach (var item in box.OutDatas)
            {
                StackParatItem pitem = new StackParatItem();
                pitem.Copy(item);
                this.AddOutput(pitem);
            }
        }
        /// <summary>
        /// 添加一个子元素
        /// </summary>
        /// <param name="stackItemBox"></param>
        public void Add(StackItemBox stackItemBox)
        {
            Children.Add(stackItemBox);
            stackItemBox.ParentNode = this;
        }
        /// <summary>
        /// 添加一个子元素
        /// </summary>
        /// <param name="stackItemBox"></param>
        public void Add(StackItemBox stackItemBox, int index)
        {
            Children.Insert(index, stackItemBox);
            stackItemBox.ParentNode = this;
        }
        /// <summary>
        /// 添加一个数组
        /// </summary>
        /// <param name="boxs"></param>
        /// <param name="index"></param>
        public void AddRange(ObservableCollection<StackItemBox> boxs, int index)
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
        public void Del(StackItemBox stackItemBox)
        {
            if(Children.Contains(stackItemBox))
            {
                Children.Remove(stackItemBox);
            }
        }
    }
}
