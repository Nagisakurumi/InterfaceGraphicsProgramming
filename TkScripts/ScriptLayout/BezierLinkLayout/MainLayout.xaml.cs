using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TkScripts.Interface;
using TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout;

namespace TkScripts.ScriptLayout.BezierLinkLayout
{
    /// <summary>
    /// MainLayout.xaml 的交互逻辑
    /// </summary>
    public partial class MainLayout : IScriptLayout
    {
        public MainLayout()
        {
            InitializeComponent();
            this.Loaded += MainLayout_Loaded;
        }
        

        /// <summary>
        /// 创建代码块事件
        /// </summary>
        /// <param name="data"></param>
        public override void DataTree_CreateCallback(IItemBox data)
        {
            ItemBox box = new ItemBox();
            box.Copy(data);
            this.Add(box);
            HiddenTree();
        }
        #region 属性
        /// <summary>
        /// 是否进行框选操作
        /// </summary>
        protected bool isToRect = false;
        /// <summary>
        /// 框选
        /// </summary>
        private Border rectangle = new Border() { BorderBrush = new SolidColorBrush(Colors.Yellow),
            BorderThickness = new Thickness(2), CornerRadius = new CornerRadius(3), Background = new SolidColorBrush(Color.FromArgb(100, 0, 100, 100))};
        /// <summary>
        /// 中心点
        /// </summary>
        private readonly Point center = new Point(-502000, -502000);
        /// <summary>
        /// 当前鼠标位置
        /// </summary>
        private Point currentPosition = new Point();
        /// <summary>
        /// 贝塞尔曲线集合
        /// </summary>
        private List<BezierLine> bzLines = new List<BezierLine>();
        /// <summary>
        /// 当前鼠标悬停的
        /// </summary>
        private FunctionBox currentMouseOverFunctionBox = null;
        /// <summary>
        /// 脚本入口
        /// </summary>
        private FunctionBox mainFb = new FunctionBox();
        /// <summary>
        /// 数据树
        /// </summary>
        private DataTreeView dataTree = new DataTreeView();
        /// <summary>
        /// 显示树状数据弹出框
        /// </summary>
        protected Popup dataTreePop = new Popup();
        /// <summary>
        /// 被选择的框
        /// </summary>
        private List<FunctionBox> selectedBoxs = new List<FunctionBox>();
        /// <summary>
        /// 用于移动，前一个点
        /// </summary>
        private Point forntPoint = new Point();
        /// <summary>
        /// 是否进行移动
        /// </summary>
        private bool isMove = false;
        /// <summary>
        /// 是否按下alt键
        /// </summary>
        private bool isPressAlt = false;
        /// <summary>
        /// 允许移动的差值
        /// </summary>
        private double canMoveDis = 1;
        /// <summary>
        /// 缩放率
        /// </summary>
        private double scalevalue = 1;
        
        
        #endregion
        #region 依赖
        /// <summary>
        /// 内容面板X坐标
        /// </summary>
        public static DependencyProperty ContentXProperty = DependencyProperty.Register("ContentX", typeof(double), typeof(MainLayout));
        /// <summary>
        /// 内容面板Y坐标
        /// </summary>
        public static DependencyProperty ContentYProperty = DependencyProperty.Register("ContentY", typeof(double), typeof(MainLayout));
        #endregion
        #region 访问器
        /// <summary>
        /// 缩放率
        /// </summary>
        public double Scalevalue
        {
            get
            {
                return scalevalue;
            }
        }
        /// <summary>
        /// 内容面板X坐标
        /// </summary>
        public double ContentX
        {
            get
            {
                return (double)GetValue(ContentXProperty);
            }
            set
            {
                SetValue(ContentXProperty, value);
            }
        }
        /// <summary>
        /// 内容面板Y坐标
        /// </summary>
        public double ContentY
        {
            get
            {
                return (double)GetValue(ContentYProperty);
            }
            set
            {
                SetValue(ContentYProperty, value);
            }
        }
        /// <summary>
        /// 缩放变换
        /// </summary>
        public ScaleTransform STransform
        {
            get
            {
                if(contentsCanvas.RenderTransform as TransformGroup == null)
                {
                    contentsCanvas.RenderTransform = new TransformGroup();
                    ScaleTransform scaleTransform = new ScaleTransform();
                    TranslateTransform translateTransform = new TranslateTransform();
                    (contentsCanvas.RenderTransform as TransformGroup).Children.Add(scaleTransform);
                    (contentsCanvas.RenderTransform as TransformGroup).Children.Add(translateTransform);
                    return scaleTransform;
                }
                foreach (var item in (contentsCanvas.RenderTransform as TransformGroup).Children)
                {
                    if(item as ScaleTransform != null)
                    {
                        return item as ScaleTransform;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 位移变换
        /// </summary>
        public TranslateTransform LTransform
        {
            get
            {
                if (contentsCanvas.RenderTransform as TransformGroup == null)
                {
                    contentsCanvas.RenderTransform = new TransformGroup();
                    ScaleTransform scaleTransform = new ScaleTransform();
                    TranslateTransform translateTransform = new TranslateTransform();
                    (contentsCanvas.RenderTransform as TransformGroup).Children.Add(scaleTransform);
                    (contentsCanvas.RenderTransform as TransformGroup).Children.Add(translateTransform);
                    return translateTransform;
                }
                foreach (var item in (contentsCanvas.RenderTransform as TransformGroup).Children)
                {
                    if (item as TranslateTransform != null)
                    {
                        return item as TranslateTransform;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 贝塞尔曲线集合
        /// </summary>
        internal List<BezierLine> BzLines
        {
            get
            {
                return bzLines;
            }
        }
        /// <summary>
        /// 内容面板
        /// </summary>
        public UIElement ContentPanel
        {
            get
            {
                return contentsCanvas;
            }
        }
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="trd"></param>
        public void Add(TreeData trd)
        {
            dataTree.MyData.Add(trd);
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="propertyIt"></param>
        public override void Add(IPropertyIt propertyIt)
        {
            this.iPropertys.Add(propertyIt);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="propertyIt"></param>
        public void Del(IPropertyIt propertyIt)
        {
            if(this.iPropertys.Contains(propertyIt))
            {
                this.iPropertys.Remove(propertyIt);
            }
        }
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="trd"></param>
        /// <returns></returns>
        public bool Del(TreeData trd)
        {
            return dataTree.MyData.Remove(trd);
        }
        #endregion
        #region 方法
        /// <summary>
        /// 删除所有选中的框
        /// </summary>
        public void DeleteSelected()
        {
            if (selectedBoxs.Count > 0)
            {
                foreach (var item in selectedBoxs)
                {
                    Del(item);
                }
            }
            selectedBoxs.Clear();
        }
        /// <summary>
        /// 显示框选
        /// </summary>
        /// <param name="rect"></param>
        protected void SetRectShow(Rect rect)
        {
            rectangle.Visibility = Visibility.Visible;
            Canvas.SetLeft(rectangle, rect.X);
            Canvas.SetTop(rectangle, rect.Y);
            rectangle.Width = rect.Width;
            rectangle.Height = rect.Height;
        }
        /// <summary>
        /// 隐藏框选
        /// </summary>
        protected void HidenRect()
        {
            rectangle.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 放大
        /// </summary>
        protected void AddScale()
        {
            if(Scalevalue > 0.45)
            {
                scalevalue -= 0.007;
            }
            else
            {
                return;
            }
            Point p = Mouse.GetPosition(this.contentsCanvas);
            STransform.CenterX = p.X;
            STransform.CenterY = p.Y;
            STransform.ScaleX = Scalevalue;
            STransform.ScaleY = Scalevalue;
        }
        /// <summary>
        /// 设置缩放率
        /// </summary>
        /// <param name="scalevalue">缩放率</param>
        /// <param name="center">缩放中心点</param>
        public void SetScale(double scalevalue, Point center)
        {
            this.scalevalue = scalevalue;
            STransform.CenterX = center.X;
            STransform.CenterY = center.Y;
            STransform.ScaleX = scalevalue;
            STransform.ScaleY = scalevalue;
        }
        /// <summary>
        /// 缩小
        /// </summary>
        protected void DescScale()
        {
            if(Scalevalue < 2)
            {
                scalevalue += 0.007;
            }
            else
            {
                return;
            }
            Point p = Mouse.GetPosition(this.contentsCanvas);
            STransform.CenterX = p.X;
            STransform.CenterY = p.Y;
            STransform.ScaleX = Scalevalue;
            STransform.ScaleY = Scalevalue;
        }
        /// <summary>
        /// 清除所有
        /// </summary>
        /// <param name="clearMain">是否清除入口</param>
        public override void ClearAll(bool clearMain = false)
        {
            iFunctionBoxs.Clear();
            itemboxs.Clear();
            bzLines.Clear();
            selectedBoxs.Clear();
            iPropertys.Clear();
            this.contentsCanvas.Children.Clear();
            if(clearMain == false)
            {
                Add(mainIb);
            }
            this.contentsCanvas.Children.Add(rectangle);
        }
        

        /// <summary>
        /// 显示树
        /// </summary>
        protected virtual void ShowTree()
        {
            dataTreePop.IsOpen = true;
        }
        /// <summary>
        /// 隐藏树
        /// </summary>
        protected virtual void HiddenTree()
        {
            dataTreePop.IsOpen = false;
        }
        /// <summary>
        /// 添加一个函数框
        /// </summary>
        /// <param name="itbox"></param>
        public void Add(IItemBox item, bool isaddposition)
        {
            LineItemBox itbox = item as LineItemBox;
            FunctionBox fb = new FunctionBox();
            if (isaddposition == true)
            {
                itbox.ContentX = currentPosition.X;
                itbox.ContentY = currentPosition.Y;
            }
            fb.UIMain = contentsCanvas;
            fb.DataContext = itbox;
            itbox.Thickness = 6;
            Itemboxs.Add(itbox);
            fb.InitFunction();
            Add(fb);
        }
        /// <summary>
        /// 添加一个函数框
        /// </summary>
        /// <param name="itbox"></param>
        public override void Add(IItemBox item)
        {
            LineItemBox itbox = item as LineItemBox;
            FunctionBox fb = new FunctionBox();
            itbox.ContentX = currentPosition.X;
            itbox.ContentY = currentPosition.Y;
            fb.UIMain = contentsCanvas;
            fb.DataContext = itbox;
            itbox.Thickness = 6;
            Itemboxs.Add(itbox);
            fb.InitFunction();
            Add(fb);
        }
        /// <summary>
        /// 添加函数框
        /// </summary>
        /// <param name="fb"></param>
        protected override void Add(IFunctionBox ffb)
        {
            FunctionBox fb = ffb as FunctionBox;
            contentsCanvas.Children.Add(fb);
            iFunctionBoxs.Add(fb);
            fb.MouseEnter += Fb_MouseEnter;
            fb.MouseLeave += Fb_MouseLeave;
        }
        /// <summary>
        /// 删除一个
        /// </summary>
        /// <param name="fb"></param>
        public void Del(FunctionBox fb)
        {
            if(iFunctionBoxs.Contains(fb))
            {
                iFunctionBoxs.Remove(fb);
            }
            if(contentsCanvas.Children.Contains(fb))
            {
                contentsCanvas.Children.Remove(fb);
            }
            fb.MouseEnter -= Fb_MouseEnter;
            fb.MouseLeave -= Fb_MouseLeave;
            
            foreach (var item in fb.Inputs)
            {
                List<BezierLine> bzs = new List<BezierLine>((item as MLParatItemLayout).LinksBezierLine);
                if (bzs.Count != 0)
                    foreach (var bz in bzs)
                    {
                        Del(bz);
                    }
                bzs.Clear();
                bzs = null;
            }
            foreach (var item in fb.Outputs)
            {
                List<BezierLine> bzs = new List<BezierLine>((item as MLParatItemLayout).LinksBezierLine);
                if (bzs.Count != 0)
                    foreach (var bz in bzs)
                    {
                        Del(bz);
                    }
                bzs.Clear();
                bzs = null;
            }
            IItemBox ibox = fb.Ibox;
            if(ibox != null)
            {
                this.Itemboxs.Remove(ibox);
            }
            fb.DataContext = null;
            fb = null;
        }
        /// <summary>
        /// 曲线
        /// </summary>
        /// <param name="bz"></param>
        public void Add(BezierLine bz)
        {
            //bz.Stroke = new SolidColorBrush(Colors.White);
            bz.StrokeThickness = 5;
            contentsCanvas.Children.Add(bz);
            BzLines.Add(bz);
        }
        /// <summary>
        /// 从json加载
        /// </summary>
        /// <typeparam name="Ibox">iitembox的实际类型</typeparam>
        /// <typeparam name="Ibezier">连接线的实际类型</typeparam>
        /// <typeparam name="IParaitem">参数的实际类型</typeparam>
        /// <param name="file">json文件路径</param>
        public override void LoadFromJson<Ibox,IProperty, IParaitem>(string file)
        {
            using (FileStream fs = File.Open(file, FileMode.Open))
            {
                byte[] datas = new byte[fs.Length];
                fs.Read(datas, 0, datas.Length);
                string json = Encoding.UTF8.GetString(datas);
                ClearAll(true);
                Tools.ConvertJsonToLayout<Ibox, BezierLine, IParaitem, IProperty>(json, this);
                datas = null;
                json = null;
            }
            foreach (var item in itemboxs)
            {
                if(item.BoxType == ItemBoxEnum.MAIN)
                {
                    this.mainIb = item as LineItemBox;
                }
                else if(item.BoxType == ItemBoxEnum.GET || item.BoxType == ItemBoxEnum.SET)
                {
                    IPropertyIt ip = FindIPropertyById(item.Id);
                    if(ip != null)
                        ip.PropertyChanged += item.PropertyValueChanged;
                }
                else
                {
                    TreeData td = dataTree.GetTreeDataByName(item.Name);
                    if (td != null)
                    {
                        td.HalfClone(item);
                        IItemboxLoadEvent?.Invoke(item, td.Data);
                    }
                }
            }
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override bool SaveToJson(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Create))
            {
                byte[] datas = System.Text.Encoding.UTF8.GetBytes(Tools.ConverScriptToJson(this));
                fs.Write(datas, 0, datas.Length);
                datas = null;
            }
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="bz"></param>
        public void Del(BezierLine bz)
        {
            if (bz == null) return;
            if(contentsCanvas.Children.Contains(bz))
            {
                contentsCanvas.Children.Remove(bz);
            }
            if(BzLines.Contains(bz))
            {
                BzLines.Remove(bz);
            }
            if((bz.LeftElement as MLParatItemLayout) != null)
                (bz.LeftElement as MLParatItemLayout).Del(bz);
            if(bz.RightElement as MLParatItemLayout != null)
                (bz.RightElement as MLParatItemLayout).Del(bz);
            bz.ClearEndBind();
            bz.ClearStartBind();
            bz.Dispose();
            bz = null;
        }
        
        /// <summary>
        /// 设置单个为被选中
        /// </summary>
        /// <param name="box"></param>
        public override void SelectBox(IFunctionBox box)
        {
            foreach (var item in iFunctionBoxs)
            {
                if(box == item)
                {
                    (item.Ibox).Thickness = 6;
                }
                else
                {
                    (item.Ibox).Thickness = 2;
                }
            }
            Canvas.SetZIndex(box, 9999);
            selectedBoxs.Clear();
            selectedBoxs.Add(box as FunctionBox);
            base.SelectBox(box);
        }
        /// <summary>
        /// 设置呗框选的盒子
        /// </summary>
        /// <param name="rect"></param>
        protected void setSelectedBox(Rect rect)
        {
            foreach (var ibox in iFunctionBoxs)
            {
                FunctionBox item = ibox as FunctionBox;
                if(this.selectedBoxs.Contains(item) == false && Tools.IsContinsRect(rect, item.Rect))
                {
                    (item.Ibox).Thickness = 6;
                    Canvas.SetZIndex(item, 100);
                    this.selectedBoxs.Add(item);
                }
            }
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        protected void cancleSelectBox()
        {
            foreach (var item in iFunctionBoxs)
            {
                Canvas.SetZIndex(item, 0);
                (item.Ibox).Thickness = 2;
            }
            selectedBoxs.Clear();
        }
        /// <summary>
        /// 移动当前选中的框
        /// </summary>
        /// <param name="pos">移动的差值</param>
        protected void moveSelectedBox(Point pos)
        {
            foreach (var item in selectedBoxs)
            {
                LineItemBox ibox = item.Ibox as LineItemBox;
                ibox.ContentX += pos.X;
                ibox.ContentY += pos.Y;
                item.MoveChange();
            }
            
        }
        /// <summary>
        /// 取消MLParatItemLayout上所有的连线
        /// </summary>
        /// <param name="mpl"></param>
        protected void removeMLParamitemBezierLine(MLParatItemLayout mpl)
        {
            List<BezierLine> bs = mpl.CancleLink();
            foreach (var item in bs)
            {
                Del(item);
            }
            IParatItem ip = mpl.DataContext as IParatItem;
            ip.LinkIParatItem = null;
            bs.Clear();
        }
        /// <summary>
        /// 移动内容面板
        /// </summary>
        /// <param name="pos"></param>
        protected void moveContentCanvas(Point pos)
        {
            ContentX += pos.X * Scalevalue;
            ContentY += pos.Y * Scalevalue;
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="datas"></param>
        public void SetFunctionData(ObservableCollection<TreeData> datas)
        {
            this.dataTree.MyData = datas;
        }
        /// <summary>
        /// 连接贝塞尔曲线到2个点
        /// </summary>
        /// <param name="bz"></param>
        /// <param name="mlp1"></param>
        /// <param name="mlp2"></param>
        public void LinkBezier(BezierLine bz, MLParatItemLayout mlp1, MLParatItemLayout mlp2, bool isAddtoCanvas = true)
        {
            if (bz == null || mlp1 == null || mlp2 == null) return;
            BezierLine ol = mlp1.SetLink(bz);
            if(ol != null)
            {
                Del(ol);
            }
            ol = mlp2.SetLink(bz);
            if (ol != null)
            {
                Del(ol);
            }
            bz.ClearStartBind();
            bz.ClearEndBind();
            if(mlp1 as BezierLinkLayout.ScriptIParameterLayout.InputParaItemLayout != null)
            {
                bz.BindPointStart(mlp2, "LinkPosition");
                bz.BindPointEnd(mlp1, "LinkPosition");
            }
            else
            {
                bz.BindPointStart(mlp1, "LinkPosition");
                bz.BindPointEnd(mlp2, "LinkPosition");
            }

            IParatItem ipl = (bz.LeftElement.DataContext as IParatItem);
            IParatItem ipr = (bz.RightElement.DataContext as IParatItem);
            ipl.LinkIParatItem = ipr;
            ipr.LinkIParatItem = ipl;
            bz.Color = ipl.Color;
            if(isAddtoCanvas)
                Add(bz);
        }
        #endregion
        #region 事件
        
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLayout_Loaded(object sender, RoutedEventArgs e)
        {
            mainIb = new LineItemBox() { BoxType = ItemBoxEnum.MAIN, Name = "任务入口" };
            mainIb.AddOutput(new ParatItem() { Name = "输出", PIEnum = ParaItemEnum.OUTPUT, Color = Colors.AntiqueWhite, UIMain = contentsCanvas });
            ContentX = center.X;
            ContentY = center.Y;
            dataTree.CreateCallback += DataTree_CreateCallback;

            dataTreePop.AllowsTransparency = true;
            dataTreePop.PopupAnimation = PopupAnimation.Fade;
            dataTreePop.StaysOpen = false;
            dataTreePop.Placement = PlacementMode.Mouse;
            ///设置弹出框内容
            dataTreePop.Child = dataTree;

            this.Add(mainIb);
            mainIb.ContentX = -center.X;
            mainIb.ContentY = -center.Y;

            this.contentsCanvas.Children.Add(rectangle);
            HidenRect();
        }
        /// <summary>
        /// 按键弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainLayout_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                isPressAlt = false;
            }
            if (e.Key == Key.F1)
            {
                if (MainIb == null)
                {
                    ContentX = -center.X;
                    ContentY = -center.Y;
                }
                else
                {
                    ContentX = -mainIb.ContentX;
                    ContentY = -mainIb.ContentY;
                }
            }
        }
        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainLayout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                isPressAlt = true;
            }
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fb_MouseLeave(object sender, MouseEventArgs e)
        {
            currentMouseOverFunctionBox = null;
        }
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fb_MouseEnter(object sender, MouseEventArgs e)
        {
            currentMouseOverFunctionBox = sender as FunctionBox;
        }
        /// <summary>
        /// 创建时候使用
        /// </summary>
        private BezierLine creatBz = null;
        /// <summary>
        /// 贝塞尔曲线连接的时候前一个连接点
        /// </summary>
        private BezierLinkLayout.ScriptIParameterLayout.MLParatItemLayout previous = null;
        /// <summary>
        /// 创建时候使用的结束点
        /// </summary>
        private Point creatPoint = new Point();
        /// <summary>
        /// 是否创建曲线
        /// </summary>
        private bool iscreateBz = false;
        /// <summary>
        /// 是否有移动过canvas
        /// </summary>
        private bool isMoveCanvas = false;
        /// <summary>
        /// 是否有移动过选择的框
        /// </summary>
        private bool isMoveBoxs = false;
       

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ///取消连线
            if(e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released &&
                isPressAlt && currentMouseOverFunctionBox != null && currentMouseOverFunctionBox.MouseMoveUI != null)
            {
                removeMLParamitemBezierLine(currentMouseOverFunctionBox.MouseMoveUI);
                return;
            }
            if(e.RightButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released
                && currentMouseOverFunctionBox == null)
            {
                isMove = true;
                forntPoint = e.GetPosition(contentsCanvas);
            }
            else if (e.LeftButton == MouseButtonState.Pressed && currentMouseOverFunctionBox != null &&
                currentMouseOverFunctionBox.MouseMoveUI == null)
            {
                isMove = true;
                forntPoint = e.GetPosition(contentsCanvas);
            }
            else if(e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released
                && currentMouseOverFunctionBox == null)
            {
                cancleSelectBox();
                isToRect = true;
                forntPoint = e.GetPosition(contentsCanvas);
            }
            else if(e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released
                && currentMouseOverFunctionBox != null && currentMouseOverFunctionBox.MouseMoveUI != null)
            {
                HiddenTree();
                iscreateBz = true;
                creatBz = null;
                
                creatBz = new BezierLine();
                previous = currentMouseOverFunctionBox.MouseMoveUI;
                if (currentMouseOverFunctionBox.MouseMoveUI as BezierLinkLayout.ScriptIParameterLayout.InputParaItemLayout == null)
                {
                    creatPoint = e.GetPosition(contentsCanvas);
                    creatBz.BindPointEnd(this, "CreatPoint");
                    creatBz.BindPointStart(currentMouseOverFunctionBox.MouseMoveUI, "LinkPosition");
                    //startisleft = false;
                    
                }
                else
                {
                    creatBz.BindPointEnd(currentMouseOverFunctionBox.MouseMoveUI, "LinkPosition");
                    creatPoint = e.GetPosition(contentsCanvas);
                    creatBz.BindPointStart(this, "CreatPoint");
                    //startisleft = true;
                    
                }
                
                if (creatBz != null)
                {
                    Add(creatBz);
                    Canvas.SetZIndex(creatBz, -10);
                }
                //contentsCanvas.CaptureMouse(); //限制鼠标
            }
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition = e.GetPosition(contentsCanvas);
            if(isMove && e.RightButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released
                && Tools.TwoPointsDistance(currentPosition, forntPoint) > canMoveDis)
            {
                moveContentCanvas(Tools.TwoPointDec(currentPosition, forntPoint));
                this.Cursor = Cursors.ScrollAll;
                isMoveCanvas = true;
            }
            else if(isMove && e.LeftButton == MouseButtonState.Pressed && Tools.TwoPointsDistance(currentPosition, forntPoint) > canMoveDis)
            {
                moveSelectedBox(Tools.TwoPointDec(currentPosition, forntPoint));
                forntPoint = currentPosition;
                isMoveBoxs = true;
            }
            else if(isToRect && Tools.TwoPointsDistance(currentPosition, forntPoint) > canMoveDis)
            {
                Rect rect = Tools.GetRectFromPoint(forntPoint, currentPosition);
                SetRectShow(rect);
                setSelectedBox(rect);
            }
            if (iscreateBz && creatBz != null)
            {
                Changed("CreatPoint");
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            //contentsCanvas.ReleaseMouseCapture();
            ///如果是开启代码窗口的话
            if (!isMoveCanvas && e.RightButton == MouseButtonState.Released && e.LeftButton == MouseButtonState.Released && e.ChangedButton == MouseButton.Right)
            {
                ShowTree();
            }
            else if (iscreateBz && creatBz != null)
            {
                if(currentMouseOverFunctionBox != null && currentMouseOverFunctionBox.MouseMoveUI != null && 
                    currentMouseOverFunctionBox.MouseMoveUI != creatBz.LeftElement && currentMouseOverFunctionBox.MouseMoveUI 
                    != creatBz.RightElement && currentMouseOverFunctionBox.MouseMoveUI.IsCanLink(creatBz))
                {
                    LinkBezier(creatBz, previous, currentMouseOverFunctionBox.MouseMoveUI, false);
                    creatBz = null;
                }
                else if(creatBz != null)
                {
                    Del(creatBz);
                    creatBz = null;
                }
            }
            else if (!isMoveBoxs && e.LeftButton == MouseButtonState.Released && currentMouseOverFunctionBox != null &&
                currentMouseOverFunctionBox.MouseMoveUI == null && e.ChangedButton == MouseButton.Left)
            {
                SelectBox(currentMouseOverFunctionBox);
            }
            
            iscreateBz = false;
            isMoveCanvas = false;
            isMoveBoxs = false;
            ///取消移动设定
            isMove = false;
            ///取消框选
            isToRect = false;
            HidenRect();
        }
        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            ///取消移动设定
            isMove = false;
            ///取消框选
            isToRect = false;
            HidenRect();
            //contentsCanvas.ReleaseMouseCapture();
            if (creatBz != null)
            {
                Del(creatBz);
            }
            creatBz = null;
            iscreateBz = false;
            isMoveCanvas = false;
            isMoveBoxs = false;
        }
        /// <summary>
        /// 滚轮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta == 120)
            {
                DescScale();
            }
            else if(e.Delta == -120)
            {
                AddScale();
            }
        }
        #endregion


    }
    public delegate void HalfCloneHandle(IItemBox load, IItemBox lib); 
}
