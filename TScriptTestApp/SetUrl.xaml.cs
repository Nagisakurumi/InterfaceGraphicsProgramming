using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TScriptTestApp
{
    /// <summary>
    /// SetUrl.xaml 的交互逻辑
    /// </summary>
    public partial class SetUrl : Window
    {
        public SetUrl()
        {
            InitializeComponent();
            this.Loaded += SetUrl_Loaded;
        }
        /// <summary>
        /// thisload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetUrl_Loaded(object sender, RoutedEventArgs e)
        {
            url.Focus();
            url.Text = "http://127.0.0.1:7676/";
        }

        private void url_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <returns></returns>
        public string ShowWindow()
        {
            if(this.ShowDialog() == true)
            {
                return url.Text;
            }
            else
            {
                return "";
            }
        }
    }
}
