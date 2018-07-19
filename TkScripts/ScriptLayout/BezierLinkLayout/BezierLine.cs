using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TkScripts.ScriptLayout.BezierLinkLayout
{
    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    public class BezierLine : Shape, IDisposable, INotifyPropertyChanged
    {
        #region 自定义属性
        /// <summary>
        ///  左边第二个定位点
        /// </summary>
        private Point leftCenterPoint = new Point();
        /// <summary>
        ///  右边第一个定位点
        /// </summary>
        private Point rightCenterPoint = new Point();
        /// <summary>
        /// 绘制的内容
        /// </summary>
        private Geometry geometryData = null;
        /// <summary>
        /// 表示有各种图形组成的复杂形状
        /// </summary>
        private PathGeometry connectorGeometry;
        /// <summary>
        /// 表示由各种元素组成的复杂路劲
        /// </summary>
        private PathFigure connectorPoints;
        /// <summary>
        /// 构建一个三次方的贝塞尔曲线
        /// </summary>
        private BezierSegment connectorCurve;
        /// <summary>
        /// 最大弯曲距离
        /// </summary>
        private double maxdis = 300;
        /// <summary>
        /// 最小距离
        /// </summary>
        private double mindis = 10;
        /// <summary>
        /// 弯曲率
        /// </summary>
        public float BendLow = 1.1f;
        /// <summary>
        /// 属性变更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 左边的值转换器
        /// </summary>
        private VPCovert vpLeftCoverter = new VPCovert();
        /// <summary>
        /// 右边值转换器
        /// </summary>
        private VPCovert vpRightCoverter = new VPCovert();
        /// <summary>
        /// 左边绑定的元素
        /// </summary>
        private FrameworkElement leftElement = null;
        /// <summary>
        /// 右边绑定的元素
        /// </summary>
        private FrameworkElement rightElement = null;
        /// <summary>
        /// 背景颜色
        /// </summary>
        //private SolidColorBrush solidColor = new SolidColorBrush();
        #endregion
        #region 读取器
        /// <summary>
        /// 笔触大小读取器
        /// </summary>
        public new double StrokeThickness
        {
            get
            {
                return base.StrokeThickness;
            }

            set
            {
                base.StrokeThickness = value;
                Changed("StrokeThickness");
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color
        {
            get
            {
                return (Stroke as SolidColorBrush).Color ;
            }
            set
            {
                (Stroke as SolidColorBrush).Color = value;
                Changed("Color");
            }
        }
        /// <summary>
        /// 贝塞尔曲线对象
        /// </summary>
        public Geometry Bezier
        {
            get
            {
                return geometryData;
            }
        }
        /// <summary>
        ///  左边第二个定位点
        /// </summary>
        public Point LeftCenterPoint
        {
            get
            {
                return leftCenterPoint;
            }

            set
            {
                leftCenterPoint = value;
                Changed("leftCenterPoint");
            }
        }
        /// <summary>
        ///  右边第一个定位点
        /// </summary>
        public Point RightCenterPoint
        {
            get
            {
                return rightCenterPoint;
            }

            set
            {
                rightCenterPoint = value;
                Changed("RightCenterPoint");
            }
        }
        /// <summary>
        /// 左边点
        /// </summary>
        public Point LeftPoint
        {
            get
            {
                return connectorPoints.StartPoint;
            }
        }
        /// <summary>
        /// 右边点
        /// </summary>
        public Point RightPoint
        {
            get
            {
                return connectorCurve.Point3;
            }
        }
        /// <summary>
        /// 返回要绘制的形状
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                return this.geometryData ?? Geometry.Empty;
            }
        }
        /// <summary>
        /// 左边绑定的元素
        /// </summary>
        public FrameworkElement LeftElement
        {
            get
            {
                return leftElement;
            }
        }
        /// <summary>
        /// 右边绑定的元素
        /// </summary>
        public FrameworkElement RightElement
        {
            get
            {
                return rightElement;
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">id</param>
        public BezierLine()
        {
            InitLineInformation();
        }

        #endregion
        #region 自定义函数
        /// <summary>
        /// 在已经设置信息的情况下初始化
        /// </summary>
        protected void InitLineInformation()
        {
            ///实例化
            connectorGeometry = new PathGeometry();
            connectorPoints = new PathFigure();
            connectorCurve = new BezierSegment();

            ///设置贝塞尔曲线的开始点
            //connectorPoints.StartPoint = StartPoint.position;

            connectorPoints.Segments.Add(connectorCurve);
            connectorGeometry.Figures.Add(connectorPoints);
            geometryData = connectorGeometry;

            vpLeftCoverter.ValueChangedCallBack += VpLeftCoverter_ValueChangedCallBack;
            vpRightCoverter.ValueChangedCallBack += VpRightCoverter_ValueChangedCallBack;
            Stroke = new SolidColorBrush(Colors.White);
        }
        /// <summary>
        /// 右边点变动的时候进行修改右边第一个定位点
        /// </summary>
        private void VpRightCoverter_ValueChangedCallBack(Point p)
        {
            ///起始点
            Point startp = LeftPoint;
            ///终止点
            Point endp = p;
            ///弯曲度
            double cury = Math.Abs(startp.Y - endp.Y) / BendLow;
            double curx = 0;
            ///需要的情况下才启用X方向的取值
            if(startp.X - endp.X > 0)
            {
                curx = Math.Abs(startp.X - endp.X) / BendLow;
            }
            double cur = curx > cury ? curx : cury;
            ///弯曲度
            double dis = cur > maxdis ? maxdis : cur;
            dis = dis < mindis ? mindis : dis;

            LeftCenterPoint = new Point(startp.X + dis, startp.Y);
            RightCenterPoint = new Point(endp.X - dis, endp.Y);
        }
        /// <summary>
        /// 左边点变动的时候进行修改左边第一个定位点
        /// </summary>
        private void VpLeftCoverter_ValueChangedCallBack(Point p)
        {
            ///起始点
            Point startp = p;
            ///终止点
            Point endp = RightPoint;
            ///弯曲度
            double cury = Math.Abs(startp.Y - endp.Y) / BendLow;
            double curx = 0;
            ///需要的情况下才启用X方向的取值
            if (startp.X - endp.X > 0)
            {
                curx = Math.Abs(startp.X - endp.X) / BendLow;
            }
            double cur = curx > cury ? curx : cury;
            ///弯曲度
            double dis = cur > maxdis ? maxdis : cur;
            dis = dis < mindis ? mindis : dis;

            LeftCenterPoint = new Point(startp.X + dis, startp.Y);
            RightCenterPoint = new Point(endp.X - dis, endp.Y);
        }

        #endregion
        #region 属性变更函数
        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="name"></param>
        protected void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 绑定起始点
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startP1"></param>
        /// <param name="startP2"></param>
        /// <param name="target"></param>
        /// <param name="targetproperty"></param>
        public void BindPointStart(object source, string startP)
        {
            Tools.Bind(source, connectorPoints, startP, PathFigure.StartPointProperty, vpLeftCoverter);
            Tools.Bind(this, connectorCurve, "LeftCenterPoint", BezierSegment.Point1Property);
            leftElement = source as FrameworkElement;
            Changed("LeftElement");

        }
        /// <summary>
        /// 绑定终点
        /// </summary>
        /// <param name="source"></param>
        /// <param name="endP1"></param>
        /// <param name="endP2"></param>
        /// <param name="target"></param>
        /// <param name="targetproperty"></param>
        public void BindPointEnd(object source, string endP)
        {
            Tools.Bind(this, connectorCurve, "RightCenterPoint", BezierSegment.Point2Property);
            Tools.Bind(source, connectorCurve, endP, BezierSegment.Point3Property, vpRightCoverter);
            rightElement = source as FrameworkElement;
            Changed("RightElement");
        }
        /// <summary>
        /// 清除起始点的绑定
        /// </summary>
        public void ClearStartBind()
        {
            Tools.ClearBind(connectorPoints, PathFigure.StartPointProperty);
            //connectorPoints.SetValue(PathFigure.StartPointProperty, new Point());
            //Binding bd = BindingOperations.GetBinding(connectorPoints, PathFigure.StartPointProperty);
            Tools.ClearBind(connectorCurve, BezierSegment.Point1Property);
            //connectorCurve.SetValue(BezierSegment.Point1Property, new Point());
        }
        /// <summary>
        /// 清除终点的绑定
        /// </summary>
        public void ClearEndBind()
        {
            Tools.ClearBind(connectorCurve, BezierSegment.Point2Property);
            //connectorCurve.SetValue(BezierSegment.Point2Property, new Point());
            Tools.ClearBind(connectorCurve, BezierSegment.Point3Property);
            //connectorCurve.SetValue(BezierSegment.Point3Property, new Point());
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
        #endregion
    }

    public delegate void ValueChangedCallBackHandle(Point newP);
    /// <summary>
    /// 左边点的转换
    /// </summary>
    public class VPCovert : IValueConverter
    {

        public event ValueChangedCallBackHandle ValueChangedCallBack = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ValueChangedCallBack?.Invoke((Point)value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
