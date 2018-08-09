using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TkScripts.Interface;
using TkScripts.Script;

namespace TkScripts.ScriptLayout
{
    public class TreeData : INotifyPropertyChanged
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="box"></param>
        public TreeData(IItemBox box)
        {
            this.Name = box.Name;
            this.data = box;
            this.TipText = box.TipText;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public TreeData(string name)
        {
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 是否展开
        /// </summary>
        private bool isExpanded = false;
        /// <summary>
        /// 是否选择的
        /// </summary>
        private bool isSelected = false;
        /// <summary>
        /// 显示方式
        /// </summary>
        private Visibility isVisiblity = Visibility.Visible;
        /// <summary>
        /// 子树
        /// </summary>
        private ObservableCollection<TreeData> children = new ObservableCollection<TreeData>();
        /// <summary>
        /// 数据
        /// </summary>
        private IItemBox data = null;
        /// <summary>
        /// 显示用的名称
        /// </summary>
        private string name = "";
        /// <summary>
        /// 提示文字
        /// </summary>
        private string tipText = "";
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
        /// 是否选择的
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
        /// 子树
        /// </summary>
        public ObservableCollection<TreeData> Children
        {
            get
            {
                return children;
            }
        }
        /// <summary>
        /// 显示方式
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
        /// 是否是数据节点
        /// </summary>
        public bool IsDataNode
        {
            get
            {
                return this.children.Count == 0;
            }
        }
        /// <summary>
        /// 显示用名称
        /// </summary>
        public string Name
        {
            get
            {
                if (this.Data == null)
                {
                    return name;
                }
                else
                {
                    return Data.Name;
                }
            }

            set
            {
                if(this.Data == null)
                {
                    name = value;
                }
                Changed("Name");
            }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public IItemBox Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                Changed("Name");
            }
        }
        /// <summary>
        /// 提示文字
        /// </summary>
        public string TipText
        {
            get
            {
                if (this.Data == null)
                {
                    return tipText;
                }
                else
                {
                    return Data.TipText;
                }
            }

            set
            {
                if (this.Data == null)
                {
                    tipText = value;
                }
                Changed("Name");
            }
        }
    }
    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="data"></param>
    public delegate void CreateCallbackHandle(IItemBox data);
}
