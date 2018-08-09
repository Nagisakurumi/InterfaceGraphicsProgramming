using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using LogLib;
using TkScripts.Interface;
using TkScripts.ScriptLayout.StackingLayout;
using System.Collections.ObjectModel;
using System.IO;

namespace TkScripts
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="target">目标</param>
        /// <param name="sourcepathname">源路径</param>
        /// <param name="depend">目标依赖属性</param>
        public static void Bind(object source, DependencyObject target, string sourcepathname, DependencyProperty depend, IValueConverter convert = null, BindingMode mode = BindingMode.Default)
        {
            if (convert == null)
            {
                Binding bind = new Binding() { Source = source, Path = new PropertyPath(sourcepathname), Mode = mode };
                BindingOperations.SetBinding(target, depend, bind);
            }
            else
            {
                Binding bind = new Binding() { Source = source, Path = new PropertyPath(sourcepathname), Mode = mode, Converter = convert };
                BindingOperations.SetBinding(target, depend, bind);
            }
        }
        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        public static void ClearBind(DependencyObject target, DependencyProperty property)
        {
            BindingOperations.ClearBinding(target, property);
        }

        /// <summary>
        /// 检测rect2是否包含在rect1中
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static bool IsContinsRect(Rect rect1, Rect rect2)
        {
            return rect1.Contains(rect2);
        }
        /// <summary>
        /// 从点获取rect
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="pe"></param>
        /// <returns></returns>
        public static Rect GetRectFromPoint(Point ps, Point pe)
        {
            return new Rect(ps, pe);
        }
        /// <summary>
        /// 计算2个点的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double TwoPointsDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)
                );
        }
        /// <summary>
        /// P1-P2
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Point TwoPointDec(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// 创建一个可用的随机id
        /// </summary>
        /// <returns></returns>
        public static string CreateId()
        {
            return DateTime.Now.ToString() + Guid.NewGuid().ToString();
        }
        
    }
}
