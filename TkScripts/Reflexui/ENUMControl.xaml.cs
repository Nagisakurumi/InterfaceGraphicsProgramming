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
using TkScripts.Interface;

namespace TkScripts.Reflexui
{
    /// <summary>
    /// ENUMControl.xaml 的交互逻辑
    /// </summary>
    public partial class ENUMControl : UserControl, IReflexui
    {
        private string propertyName = "";
        private IParatItem paraItem = null;
        public ENUMControl()
        {
            InitializeComponent();
        }

        public string PropertyName
        {
            get
            {
                return PropertyName;
            }
        }

        public IParatItem Source
        {
            get
            {
                return paraItem;
            }
        }

        public void Bind(IParatItem Source, string PropertyName)
        {
            this.paraItem = Source;
            this.propertyName = PropertyName;
            this.content.ItemsSource = Source.EnumDatas;
            Tools.Bind(Source, this.content, PropertyName, ComboBox.SelectedItemProperty, mode: BindingMode.TwoWay);
        }
    }
}
