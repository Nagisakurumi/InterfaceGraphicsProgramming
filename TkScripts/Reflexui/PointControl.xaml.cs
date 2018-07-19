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
    /// PointControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointControl : UserControl, IReflexui
    {
        public PointControl()
        {
            InitializeComponent();
        }
        private string propertyName = "";
        private IParatItem paraItem = null;


        public void Bind(IParatItem Source, string PropertyName)
        {
            this.paraItem = Source;
            this.propertyName = PropertyName;
            Tools.Bind(Source, this.content, PropertyName, TextBox.TextProperty, convert:new TextBoxToPoint(), mode: BindingMode.TwoWay);
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


    public class TextBoxToPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return value.ToString();
            }
            catch (Exception)
            {
                return new Point().ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Point p = new Point();
                string[] values = value.ToString().Split(',');
                p.X = System.Convert.ToDouble(values[0]);
                p.Y = System.Convert.ToDouble(values[1]);
                return p;
            }
            catch (Exception)
            {
                return new Point();
            }
        }
    }
}
