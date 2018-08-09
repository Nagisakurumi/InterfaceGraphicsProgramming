using AvalonDock.Layout;
using AvalonDock.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TkScripts.Interface;

namespace TKScripts.Manager
{
    /// <summary>
    /// MainManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainManagerControl : UserControl
    {
        public MainManagerControl()
        {
            InitializeComponent();
            this.Loaded += MainManagerControl_Loaded;
            layoutContents.Add(Layout.Buttom, ButtonPanelGroup);
            layoutContents.Add(Layout.ButtomSide, ButtomSideGroup);
            layoutContents.Add(Layout.Content, MainDocument);
            layoutContents.Add(Layout.LeftUp, LeftUpPaneGroup);
            layoutContents.Add(Layout.LeftDown, LeftDownPaneGroup);
            layoutContents.Add(Layout.LeftSide, LeftSideGroup);
            layoutContents.Add(Layout.Right, RightPaneGroup);
            layoutContents.Add(Layout.RightSide, RightSideGroup);
            layoutContents.Add(Layout.TopSide, TopSideGroup);
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainManagerControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        #region 添加布局函数集合
        /// <summary>
        /// 获取一个布局可抛锚窗格
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="FatherLayout">父布局</param>
        /// <returns></returns>
        internal LayoutAnchorable AddLayoutAnchorablePane(LayoutAnchorablePaneGroup FatherLayout, string title)
        {
            LayoutAnchorablePane pane = new LayoutAnchorablePane();
            LayoutAnchorable anchorable = new LayoutAnchorable();
            anchorable.Title = title;
            pane.Children.Add(anchorable);
            FatherLayout.Children.Add(pane);
            return anchorable;
        }
        /// <summary>
        /// 添加一个可移动窗口
        /// </summary>
        /// <param name="pane"></param>
        /// <param name="title"></param>
        internal LayoutAnchorable AddLayoutAnchorable(LayoutAnchorablePane pane, string title)
        {
            LayoutAnchorable anchorable = new LayoutAnchorable();
            anchorable.Title = title;
            pane.Children.Add(anchorable);
            return anchorable;
        }
        /// <summary>
        /// 添加内容面板
        /// </summary>
        /// <param name="documentPane"></param>
        /// <param name="title"></param>
        internal LayoutDocument AddDocument(LayoutDocumentPane documentPane, string title)
        {
            LayoutDocument layout = new LayoutDocument();
            layout.Title = title;
            documentPane.Children.Add(layout);
            return layout;
        }
        /// <summary>
        /// 添加内容面板
        /// </summary>
        /// <param name="documentPane"></param>
        /// <param name="title"></param>
        internal LayoutAnchorable AddLayoutAnchorable(LayoutAnchorGroup pane, string title)
        {
            LayoutAnchorable anchorable = new LayoutAnchorable();
            anchorable.Title = title;
            pane.Children.Add(anchorable);
            return anchorable;
        }
        /// <summary>
        /// 添加一个控件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="element"></param>
        /// <param name="layout"></param>
        public bool AddUserControl(string key, FrameworkElement element, Layout layout, string name)
        {
            if(userControlExtend.ContainsKey(key))
            {
                return false;
            }
            UserControlExtend usd = new UserControlExtend()
            {
                Layout = layout,
                UserExtend = element,
                Title = name,
            };
            userControlExtend.Add(key, usd);
            return AddUserControl(usd);
        }
        /// <summary>
        /// 替换key容器中的内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool ReplaceUserControlByKey(string key, FrameworkElement element)
        {
            if(userControlExtend.ContainsKey(key))
            {
                if (userControlExtend[key].UserExtend == element)
                    return true;
                userControlExtend[key].UserExtend = element;
                userControlExtend[key].Element.Content = element;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 替换key容器中的内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool ReplaceUserControlByKey(string key, System.Windows.Forms.Control control)
        {
            if (userControlExtend.ContainsKey(key))
            {
                if (userControlExtend[key].UserExtend is WindowsFormsHost)
                {
                    if ((userControlExtend[key].UserExtend as WindowsFormsHost).Child == control)
                        return true;
                    (userControlExtend[key].UserExtend as WindowsFormsHost).Child = control;
                }
                else
                {
                    WindowsFormsHost cont = new WindowsFormsHost() { Child = control };
                    userControlExtend[key].UserExtend = cont;
                    userControlExtend[key].Element.Content = cont;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 修改容器的标题
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newtitle"></param>
        public void ModifyTitleById(string key, string newtitle)
        {
            if(userControlExtend.ContainsKey(key))
            {
                userControlExtend[key].Element.Title = newtitle;
                userControlExtend[key].Title = newtitle;
            }
        }
        /// <summary>
        /// 添加一个控件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="control"></param>
        /// <param name="layout"></param>
        public bool AddUserControl(string key, System.Windows.Forms.Control control, Layout layout, string name)
        {
            WindowsFormsHost windhost = new WindowsFormsHost();
            windhost.Child = control;
            return AddUserControl(key, windhost, layout, name);
        }
        /// <summary>
        /// 当因关闭导致的控件销毁时候进行重新创建
        /// </summary>
        /// <param name="usd"></param>
        /// <returns></returns>
        internal bool AddUserControl(UserControlExtend usd)
        {
            if(usd.Element != null)
            {
                usd.Element = null;
            }
            LayoutElement le = layoutContents[usd.Layout];
            if (le as LayoutAnchorablePane != null)
            {
                LayoutAnchorable lable = AddLayoutAnchorable(le as LayoutAnchorablePane, usd.Title);
                lable.Content = usd.UserExtend as FrameworkElement;
                usd.Element = lable;
            }
            else if (le as LayoutDocumentPane != null)
            {
                LayoutDocument lable = AddDocument(le as LayoutDocumentPane, usd.Title);
                lable.Content = usd.UserExtend as FrameworkElement;
                usd.Element = lable;
            }
            else if (le as LayoutAnchorGroup != null)
            {
                LayoutAnchorable lable = AddLayoutAnchorable(le as LayoutAnchorGroup, usd.Title);
                lable.Content = usd.UserExtend as FrameworkElement;
                usd.Element = lable;
            }
            return true;
        }

        /// <summary>
        /// 获取添加的用户控件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetUserExtendByKey(string key)
        {
            if(userControlExtend.ContainsKey(key))
            {
                return userControlExtend[key].UserExtend;
            }
            return null;
        }
        /// <summary>
        /// 删除用户扩展控件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DelUserExtendByKey(string key)
        {
            if (userControlExtend.ContainsKey(key))
            {
                LayoutElement le = layoutContents[userControlExtend[key].Layout];
                if (le as LayoutAnchorablePane != null)
                {
                    (le as LayoutAnchorablePane).Children.Remove(userControlExtend[key].Element
                        as LayoutAnchorable);
                }
                else if (le as LayoutDocumentPane != null)
                {
                    (le as LayoutDocumentPane).Children.Remove(userControlExtend[key].Element
                        as LayoutDocument);
                }
                else if (le as LayoutAnchorGroup != null)
                {
                    (le as LayoutAnchorGroup).Children.Remove(userControlExtend[key].Element
                        as LayoutAnchorable);
                }
                UserControlDeleted?.Invoke(key, userControlExtend[key]);
                userControlExtend.Remove(key);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 显示layoutpane
        /// </summary>
        /// <param name="layout"></param>
        internal bool ShowPane(LayoutElement layout)
        {
            if(Root.Hidden.Count > 0)
            {
                foreach (var item in Root.Hidden)
                {
                    if(item == layout)
                    {
                        item.Show();
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 显示key的面板
        /// </summary>
        /// <param name="key"></param>
        public void ShowPane(string key)
        {
            if (userControlExtend.ContainsKey(key))
            {
                if(ShowPane(userControlExtend[key].Element) == false)
                {
                    if(IsContains(userControlExtend[key].Element, userControlExtend[key].Layout) == false)
                    {
                        AddUserControl(userControlExtend[key]);
                    }
                }
            }
        }
        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="theme"></param>
        public void SetTheme(Theme theme)
        {
            if(theme == Theme.AeroTheme)
            {
                DockManager.Theme = new AeroTheme();
            }
            else if(theme == Theme.VS2010Theme)
            {
                DockManager.Theme = new VS2010Theme();
            }
        }
        /// <summary>
        /// 面板是否包含在容器中
        /// </summary>
        /// <param name="element"></param>
        /// <param name="layout"></param>
        /// <returns></returns>
        internal bool IsContains(LayoutElement element, Layout layout)
        {
            LayoutElement le = layoutContents[layout];
            if (le as LayoutAnchorablePane != null)
            {
                return (le as LayoutAnchorablePane).Children.Contains(element
                    as LayoutAnchorable);
            }
            else if (le as LayoutDocumentPane != null)
            {
                return (le as LayoutDocumentPane).Children.Contains(element
                    as LayoutDocument);
            }
            else if (le as LayoutAnchorGroup != null)
            {
                return (le as LayoutAnchorGroup).Children.Contains(element
                    as LayoutAnchorable);
            }
            else if (le as LayoutAnchorablePaneGroup != null)
            {
                return (le as LayoutAnchorablePaneGroup).Children.Contains(element
                    as LayoutAnchorablePane);
            }
            return false;
        }
        /// <summary>
        /// 获取当前激活的document
        /// </summary>
        /// <returns></returns>
        public object GetActiveDocument()
        {
            if (MainDocument.SelectedContent == null)
                return null;
            return MainDocument.SelectedContent.Content;
        }
        /// <summary>
        /// 激活key下的面板
        /// </summary>
        /// <param name="key"></param>
        public void SetActiveDocument(string key)
        {
            if(userControlExtend.ContainsKey(key) && userControlExtend[key].Element is LayoutDocument)
            {
                MainDocument.SelectedContentIndex = MainDocument.Children.IndexOf(userControlExtend[key].Element as LayoutDocument);
            }
        }
        #endregion
        #region 属性
        /// <summary>
        /// 用户控件删除事件
        /// </summary>
        public UserControlDeletedEvent UserControlDeleted = null;
        /// <summary>
        /// 存放用户扩展窗口
        /// </summary>
        private Dictionary<string, UserControlExtend> userControlExtend = new Dictionary<string, UserControlExtend>();
        /// <summary>
        /// 布局容器
        /// </summary>
        private Dictionary<Layout, LayoutElement> layoutContents = new Dictionary<Layout, LayoutElement>();
        /// <summary>
        /// 脚本列表
        /// </summary>
        private ObservableCollection<IScriptLayout> scriptLayouts = new ObservableCollection<IScriptLayout>();
        /// <summary>
        /// 属性列表
        /// </summary>
        private PropertyItControl PropertyControl = new PropertyItControl();
        /// <summary>
        /// 参数列表控件
        /// </summary>
        private PropertyItControl ParamterControl = new PropertyItControl();
        /// <summary>
        /// 脚本列表
        /// </summary>
        private ScriptControl ScriptControl = new ScriptControl();
        #endregion
    }
    /// <summary>
    /// 用户控件被删除
    /// </summary>
    /// <param name="key"></param>
    /// <param name="usd"></param>
    public delegate void UserControlDeletedEvent(string key, UserControlExtend usd);
}
