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

namespace TKScripts.Manager.ScriptManage.ManagerPlugin
{
    /// <summary>
    /// TaskItem.xaml 的交互逻辑
    /// </summary>
    public partial class TaskItemControl : UserControl
    {
        public TaskItemControl()
        {
            InitializeComponent();
            this.Loaded += TaskItem_Loaded;
        }

        private void TaskItem_Loaded(object sender, RoutedEventArgs e)
        {
            nameBox.IsReadOnly = true;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            nameBox.IsReadOnly = false;
        }

        private void nameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            nameBox.IsReadOnly = true;
        }

        private void nameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                nameBox.IsReadOnly = true;
            }
        }
    }
}
