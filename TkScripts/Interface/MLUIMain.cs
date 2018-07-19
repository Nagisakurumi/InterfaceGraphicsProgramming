using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TkScripts.Interface
{
    public class MLUIMain : UserControl, INotifyPropertyChanged
    {
        

        /// <summary>
        /// 计算LinkPosition相对点的面板
        /// </summary>
        public static DependencyProperty UIMainProperty = DependencyProperty.Register("UIMain", typeof(UIElement), typeof(MLUIMain));
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 计算LinkPosition相对点的面板
        /// </summary>
        public UIElement UIMain
        {
            get
            {
                return (UIElement)GetValue(UIMainProperty);
            }

            set
            {
                SetValue(UIMainProperty, value);
            }
        }
    }
}
