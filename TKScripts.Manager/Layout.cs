using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKScripts.Manager
{
    /// <summary>
    /// 布局枚举
    /// </summary>
    public enum Layout
    {
        /// <summary>
        /// 左上
        /// </summary>
        LeftUp,
        /// <summary>
        /// 左下
        /// </summary>
        LeftDown,
        /// <summary>
        /// 右
        /// </summary>
        Right,
        /// <summary>
        /// 下
        /// </summary>
        Buttom,
        /// <summary>
        /// 中间
        /// </summary>
        Content,
        /// <summary>
        /// 上边条
        /// </summary>
        TopSide,
        /// <summary>
        /// 左边条
        /// </summary>
        LeftSide,
        /// <summary>
        /// 右边条
        /// </summary>
        RightSide,
        /// <summary>
        /// 下边条
        /// </summary>
        ButtomSide,
    }
    /// <summary>
    /// 用户拓展控件
    /// </summary>
    public class UserControlExtend
    {
        /// <summary>
        /// 所在布局
        /// </summary>
        public Layout Layout = Layout.Buttom;
        /// <summary>
        /// 布局父控件
        /// </summary>
        public LayoutContent Element = null;
        /// <summary>
        /// 用户控件
        /// </summary>
        public object UserExtend = null;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title = "";
    }

    /// <summary>
    /// 主题
    /// </summary>
    public enum Theme
    {
        /// <summary>
        /// vs2010主题
        /// </summary>
        VS2010Theme,
        /// <summary>
        /// aero主题
        /// </summary>
        AeroTheme,
    }
}
