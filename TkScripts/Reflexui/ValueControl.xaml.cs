using System;
using System.Collections.Generic;
using System.Globalization;
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
using TkScripts.Interface;

namespace TkScripts.Reflexui
{
    /// <summary>
    /// INTControl.xaml 的交互逻辑
    /// </summary>
    public partial class ValueControl : UserControl, IReflexui
    {

        private string propertyName = "";
        private IParatItem paraItem = null;

        public ValueControl()
        {
            InitializeComponent();
            this.Loaded += INTControl_Loaded;
        }

        private void INTControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void Bind(IParatItem Source, string PropertyName)
        {
            this.paraItem = Source;
            this.propertyName = PropertyName;
            Tools.Bind(Source, this.content, PropertyName, TextBox.TextProperty, mode: BindingMode.TwoWay);
        }

        public string PropertyName
        {
            get
            {
                return propertyName;
            }
        }
        public IParatItem Source
        {
            get
            {
                return paraItem;
            }
        }
    }
}
