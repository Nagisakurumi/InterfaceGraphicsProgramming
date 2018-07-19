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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TkScripts.ScriptLayout
{
    /// <summary>
    /// PropertyBox.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyBox : UserControl
    {
        public PropertyBox()
        {
            InitializeComponent();
        }
    

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            content.IsReadOnly = true;
            content.BorderThickness = new Thickness(0);
            content.Background = null;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                content.IsReadOnly = true;
                content.BorderThickness = new Thickness(0);
                content.Background = null;
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F2)
            {
                content.IsReadOnly = false;
                content.BorderThickness = new Thickness(1);
                content.Background = new SolidColorBrush(Colors.White);
                content.Focus();
            }
        }
    }
}
