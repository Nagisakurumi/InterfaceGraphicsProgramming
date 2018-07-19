using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TKScripts.Manager
{
    /// <summary>
    /// 调试项
    /// </summary>
    public class DebugItem : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 子项
        /// </summary>
        private ObservableCollection<DebugItem> children = new ObservableCollection<DebugItem>();
        /// <summary>
        /// 要观察的变量
        /// </summary>
        private object watchObject = null;
        /// <summary>
        /// 变量名称
        /// </summary>
        private string name = "";
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
        /// 子项
        /// </summary>
        public ObservableCollection<DebugItem> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
                Changed("Children");
            }
        }
        /// <summary>
        /// 要观察的变量
        /// </summary>
        public object WatchObject
        {
            get
            {
                return watchObject;
            }

            set
            {
                watchObject = value;
                Changed("WatchObject");
            }
        }
        /// <summary>
        /// 变量名称
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
                Changed("Named");
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
                if (value && this.Children.Count != 0)
                {
                    foreach (var item in Children)
                    {
                        GetPropertyWatchForObject(item);
                    }
                }
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
        /// 属性改变通知
        /// </summary>
        /// <param name="name"></param>
        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// 设置监视对象
        /// </summary>
        /// <param name="debugitem">item</param>
        public static void GetPropertyWatchForObject(DebugItem debugitem)
        {
            if (debugitem.WatchObject is IEnumerable)
            {
                foreach (var item in debugitem.WatchObject as IEnumerable)
                {
                    try
                    {
                        debugitem.Children.Add(new DebugItem() { Name = item.ToString(), WatchObject = item });
                    }
                    catch (Exception)
                    {
                        debugitem.Children.Add(new DebugItem() { Name = item.ToString(), WatchObject = "监视内存值出错" });
                    }
                }
            }
            else
            {
                PropertyInfo[] propertyInfos = debugitem.WatchObject.GetType().GetProperties();
                FieldInfo[] fieldInfos = debugitem.WatchObject.GetType().GetFields();
                foreach (FieldInfo item in fieldInfos)
                {
                    try
                    {
                        debugitem.Children.Add(new DebugItem() { Name = item.Name, WatchObject = item.GetValue(debugitem.WatchObject) });
                    }
                    catch (Exception)
                    {
                        debugitem.Children.Add(new DebugItem() { Name = item.Name, WatchObject = "监视内存值出错" });
                    }
                }
                foreach (PropertyInfo item in propertyInfos)
                {
                    try
                    {
                        debugitem.Children.Add(new DebugItem() { Name = item.Name, WatchObject = item.GetValue(debugitem.WatchObject) });
                    }
                    catch (Exception)
                    {
                        debugitem.Children.Add(new DebugItem() { Name = item.Name, WatchObject = "监视内存值出错" });
                    }
                }

            }
        } 
    }
}
