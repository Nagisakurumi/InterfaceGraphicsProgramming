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
using TkScripts.ScriptLayout.BezierLinkLayout;
using TkScripts.ScriptLayout.StackingLayout;
using System.Collections.ObjectModel;
using TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout;

namespace TkScripts
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,Inherited = true, AllowMultiple = true)]
    public class ToJsonAttribute : Attribute
    {
        public bool IsToJson = false;
        /// <summary>
        /// 是否在转换为json的时候进行转换
        /// </summary>
        /// <param name="isCanToJson"></param>
        public ToJsonAttribute(bool isCanToJson)
        {
            IsToJson = isCanToJson;
        }
    }


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

        /// <summary>
        /// 转换脚本到json
        /// </summary>
        /// <param name="ml"></param>
        /// <returns></returns>
        public static string ConverScriptToJson(MainLayout ml)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append(IItemboxListToJson(ml.Itemboxs) + ",");
            sb.Append(BezierLinesListToJson(ml.BzLines) + ",");
            sb.Append(IPropertyListToJson(ml.IPropertys));
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 转换脚本到json
        /// </summary>
        /// <param name="ml"></param>
        /// <returns></returns>
        public static string ConverScriptToJson(StackingMainLayout ml)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"ScriptName\":" + "\"" + ml.ScriptName + "\",");
            sb.Append(IItemboxListToJson(ml.Itemboxs) + ",");
            sb.Append(IPropertyListToJson(ml.IPropertys));
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// IPropertyList转换为json
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        internal static string IPropertyListToJson(IList<IPropertyIt> ipts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + IScriptLayout.IPropertyItsListName + "\":");
            sb.Append("[");
            foreach (var item in ipts)
            {
                sb.Append(IPropertyToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            return sb.ToString();
        }
        /// <summary>
        /// 转换IParatItem到json
        /// </summary>
        /// <param name="ipt"></param>
        /// <returns></returns>
        internal static string IPropertyToJson(IPropertyIt ipt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            Type tp = ipt.GetType();
            PropertyInfo[] pis = tp.GetProperties();
            foreach (var item in pis)
            {
                ToJsonAttribute attribute = item.GetCustomAttribute(typeof(ToJsonAttribute), false) as ToJsonAttribute;
                if (attribute != null)
                {
                    if (item.GetValue(ipt) != null)
                        sb.Append("\"" + item.Name + "\":\"" + item.GetValue(ipt).ToString() + "\",");
                }
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// IItemBox转换为json
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        internal static string IItemboxListToJson(IList<IItemBox> boxs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + IScriptLayout.ItemBoxsListName + "\":");
            sb.Append("[");
            foreach (var item in boxs)
            {
                if(item is StackItemBox)
                    sb.Append(IItemBoxToJson(item as StackItemBox) + ",");
                else
                    sb.Append(IItemBoxToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// IItemBox转换为json
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static string IItemBoxToJson(IItemBox box)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            Type tp = box.GetType();
            PropertyInfo[] pis = tp.GetProperties();
            foreach (var item in pis)
            {
                ToJsonAttribute attribute = item.GetCustomAttribute(typeof(ToJsonAttribute), false) as ToJsonAttribute;
                if(attribute != null)
                {
                    if (item.Name == "Children")
                    {
                        foreach (var childrenitem in item.GetValue(box) as ObservableCollection<StackItemBox>)
                        {
                            
                        }
                    }
                    sb.Append("\"" + item.Name + "\":\"" + item.GetValue(box) + "\",");
                }
            }
            sb.Append("\"InputDatas\":[");
            foreach (var item in box.InputDatas)
            {
                sb.Append(IParatItemToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("],");
            sb.Append("\"OutDatas\":[");
            foreach (var item in box.OutDatas)
            {
                sb.Append(IParatItemToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// IItemBox转换为json
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static string IItemBoxToJson(StackItemBox box)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            Type tp = box.GetType();
            PropertyInfo[] pis = tp.GetProperties();
            foreach (var item in pis)
            {
                ToJsonAttribute attribute = item.GetCustomAttribute(typeof(ToJsonAttribute), false) as ToJsonAttribute;
                if (attribute != null)
                {
                    if (item.Name == "Children")
                    {
                        foreach (var childrenitem in item.GetValue(box) as ObservableCollection<StackItemBox>)
                        {

                        }
                    }
                    sb.Append("\"" + item.Name + "\":\"" + item.GetValue(box) + "\",");
                }
            }
            sb.Append("\"InputDatas\":[");
            foreach (var item in box.InputDatas)
            {
                sb.Append(IParatItemToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("],");
            sb.Append("\"OutDatas\":[");
            foreach (var item in box.OutDatas)
            {
                sb.Append(IParatItemToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("],");
            sb.Append("\"Children\":[");
            foreach (var item in box.Children)
            {
                sb.Append(IItemBoxToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 转换IParatItem到json
        /// </summary>
        /// <param name="ipt"></param>
        /// <returns></returns>
        internal static string IParatItemToJson(IParatItem ipt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            Type tp = ipt.GetType();
            PropertyInfo[] pis = tp.GetProperties();
            foreach (var item in pis)
            {
                ToJsonAttribute attribute = item.GetCustomAttribute(typeof(ToJsonAttribute), false) as ToJsonAttribute;
                if (attribute != null)
                {
                    if(item.GetValue(ipt) != null)
                        sb.Append("\"" + item.Name + "\":\"" + item.GetValue(ipt).ToString() + "\",");
                }
            }
            if(sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 转换贝塞尔曲线列表到json
        /// </summary>
        /// <param name="bzs"></param>
        /// <returns></returns>
        internal static string BezierLinesListToJson(IList<BezierLine> bzs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"beziers\":");
            sb.Append("[");
            foreach (var item in bzs)
            {
                sb.Append(BezierLinesToJson(item) + ",");
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            return sb.ToString();
        }
        /// <summary>
        /// 转换贝塞尔曲线到
        /// </summary>
        /// <param name="bz"></param>
        /// <returns></returns>
        internal static string BezierLinesToJson(BezierLine bz)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            IParatItem leftipt = bz.LeftElement.DataContext as IParatItem;
            IParatItem rightipt = bz.RightElement.DataContext as IParatItem;
            sb.Append("\"left\":{\"parentid\":\"" + leftipt.ParentItemBox.Id + "\",\"childid\":\"" + leftipt.Id + "\"},");
            sb.Append("\"right\":{\"parentid\":\"" + rightipt.ParentItemBox.Id + "\",\"childid\":\"" + rightipt.Id + "\"},");

            Type tp = bz.GetType();
            PropertyInfo[] pis = tp.GetProperties();
            foreach (var item in pis)
            {
                ToJsonAttribute attribute = item.GetCustomAttribute(typeof(ToJsonAttribute), false) as ToJsonAttribute;
                if (attribute != null)
                {
                    if (item.GetValue(bz) != null)
                        sb.Append("\"" + item.Name + "\":\"" + item.GetValue(bz).ToString() + "\",");
                }
            }
            if (sb[sb.Length - 1].Equals(','))
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 转换json到控件
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ml"></param>
        public static void ConvertJsonToLayout<Ibox,IBLine,IPItem, IPty>(string json, MainLayout ml)
        {
            JObject jo =(JObject)JsonConvert.DeserializeObject(json);
            JToken boxs = jo["boxs"];
            foreach (var item in boxs)
            {
                ml.Add(ConvertJsonToIItemBox<Ibox, IPItem>(item, ml), false);
            }
            JToken lines = jo["beziers"];
            foreach (var item in lines)
            {
                ConvertJsonToBezierline<IBLine>(item, ml);
            }
            JToken ptys = jo["propertys"];
            foreach (var item in ptys)
            {
                ml.Add(ConvertJsonToIProperty<IPty>(item));
            }
            //return null;
        }

        /// <summary>
        /// 转换json到控件
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ml"></param>
        public static void ConvertJsonToLayout<Ibox, IPItem, IPty>(string json, StackingMainLayout ml)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JToken boxs = jo[IScriptLayout.ItemBoxsListName];
            JToken name = jo[IScriptLayout.IScriptName];
            ml.ScriptName = name.ToString();
            JToken ptys = jo[IScriptLayout.IPropertyItsListName];
            foreach (var item in ptys)
            {
                ml.Add(JsonConvert.DeserializeObject<IPty>(item.ToString()) as IPropertyIt);
            }
            foreach (var item in boxs)
            {
                ml.Add(ConvertJsonToIItemBox<Ibox, IPItem>(item, ml));
            }
        }
        /// <summary>
        /// 转换json到IProperty
        /// </summary>
        /// <typeparam name="IPItem"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static IPropertyIt ConvertJsonToIProperty<IPty>(JToken json)
        {
            if (json.Type != JTokenType.Object)
                return null;
            IPty t = (IPty)Activator.CreateInstance(typeof(IPty));
            Type tp = t.GetType();
            if (t as IPropertyIt == null)
            {
                return null;
            }
            foreach (JProperty item in json)
            {
                PropertyInfo info = tp.GetProperty(item.Name);
                if(info != null)
                    info.SetValue(t, ConvertToObject(item.Value.ToString(), info.PropertyType));
            }
            return t as IPropertyIt;
        }
        /// <summary>
        /// 转换json到bezierline
        /// </summary>
        /// <typeparam name="IPItem"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static void ConvertJsonToBezierline<IBLine>(JToken json, MainLayout ml)
        {
            if (json.Type != JTokenType.Object)
                return;
            IBLine t = (IBLine)Activator.CreateInstance(typeof(IBLine));
            BezierLine bz = t as BezierLine;
            Type tp = t.GetType();
            if (t as BezierLine == null)
            {
                return;
            }
            MLParatItemLayout left = null, right = null;

            foreach (JProperty item in json)
            {
                if(item.Name == "left")
                {
                    left = ml.FindFunctionBoxById((item.Value.First as JProperty).Value.ToString(),
                        (item.Value.Last as JProperty).Value.ToString());
                }
                else if(item.Name == "right")
                {
                    right = ml.FindFunctionBoxById((item.Value.First as JProperty).Value.ToString(),
                        (item.Value.Last as JProperty).Value.ToString());
                }
                else
                {
                    PropertyInfo info = tp.GetProperty(item.Name);
                    info.SetValue(t, ConvertToObject(item.Value.ToString(), info.PropertyType));
                }
            }
            ml.LinkBezier(bz, left, right);
        }
        /// <summary>
        /// 转换json到box
        /// </summary>
        /// <typeparam name="Ibox"></typeparam>
        /// <typeparam name="IPItem"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static IItemBox ConvertJsonToIItemBox<Ibox, IPItem>(JToken json, IScriptLayout script)
        {
            if (json.Type != JTokenType.Object)
                return null;
            Ibox t = (Ibox)Activator.CreateInstance(typeof(Ibox));
            IItemBox box = t as IItemBox;
            Type tp = t.GetType();
            if (box == null)
            {
                return null;
            }
            foreach (JProperty item in json)
            {
                if (item.Name == "InputDatas")
                {
                    foreach (JToken inputitem in item.Value)
                    {
                        IParatItem ipt = JsonConvert.DeserializeObject<IPItem>(inputitem.ToString()) as IParatItem;
                        if (ipt != null)
                            box.AddInput(ipt);
                        if(ipt is StackParatItem)
                        {
                            (script as StackingMainLayout).SetLinkIProperty(ipt as StackParatItem);
                        }
                    }
                }
                else if (item.Name == "OutDatas")
                {
                    foreach (JToken inputitem in item.Value)
                    {
                        //IParatItem ipt = ConvertJsonToIParatItem<IPItem>(inputitem);
                        IParatItem ipt = JsonConvert.DeserializeObject<IPItem>(inputitem.ToString()) as IParatItem;
                        if (ipt != null)
                            box.AddOutput(ipt);
                        if (ipt is StackParatItem)
                        {
                            (script as StackingMainLayout).SetLinkIProperty(ipt as StackParatItem);
                        }
                    }
                }
                else
                {
                    PropertyInfo info = tp.GetProperty(item.Name);
                    if(item.Name == "Children")
                    {
                        StackItemBox stackbox = box as StackItemBox;
                        ObservableCollection<StackItemBox> children = new ObservableCollection<StackItemBox>();
                        foreach (var child in item.Value)
                        {
                            children.Add(ConvertJsonToIItemBox<StackItemBox, IPItem>(child, script) as StackItemBox);
                            children.Last().ScriptLayout = script;
                        }
                        foreach (var child in children)
                        {
                            (script as StackingMainLayout).SetItemboxDoFunction(child);
                            stackbox.Add(child);
                        }
                        (script as StackingMainLayout).SetItemboxDoFunction(stackbox);
                        children.Clear();
                        children = null;
                    }
                    else
                    {
                        if (info != null)
                            info.SetValue(t, ConvertToObject(item.Value.ToString(), info.PropertyType));
                    }
                    
                }
            }
            return box;
        }
        /// <summary>
        /// 转换json到IParatItem
        /// </summary>
        /// <typeparam name="IPItem"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static IParatItem ConvertJsonToIParatItem<IPItem>(JToken json)
        {
            if (json.Type != JTokenType.Object)
                return null;
            IPItem t = (IPItem)Activator.CreateInstance(typeof(IPItem));
            Type tp = t.GetType();
            if(t as IParatItem == null)
            {
                return null;
            }
            foreach (JProperty item in json)
            {
                PropertyInfo info = tp.GetProperty(item.Name);
                if (info != null)
                    info.SetValue(t, ConvertToObject(item.Value.ToString(), info.PropertyType));
            }
            return t as IParatItem;
        }

        /// <summary>
        /// 转换string到对象
        /// </summary>
        internal static object ConvertToObject(string value, Type tp)
        {
            if(tp.FullName == "System.Object")
            {
                return value;
            }
            if(tp.BaseType.Name == typeof(Enum).Name)
            {
                foreach (var item in Enum.GetValues(tp))
                {
                    if(value == item.ToString())
                    {
                        return item;
                    }
                }
                return null;
            }
            else
            {
                string type = tp.FullName;
                switch (type)
                {
                    case "System.Int32":
                        return Convert.ToInt32(value);
                    case "System.Double":
                        return Convert.ToDouble(value);
                    case "System.Boolean":
                        return Convert.ToBoolean(value);
                    case "System.String":
                        return value;
                    case "System.DateTime":
                        return Convert.ToDateTime(value);
                    default:
                        throw new Exception("气场");
                }
            }
        }
    }
}
