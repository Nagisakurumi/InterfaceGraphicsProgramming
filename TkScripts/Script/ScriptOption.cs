
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TkScripts;
using TkScripts.Interface;
using TkScripts.ScriptLayout.BezierLinkLayout;
using TKScriptsServer.Agreement;

namespace TkScripts.Script
{
    /// <summary>
    /// 脚本解析类
    /// </summary>
    public class ScriptOption
    {
        /// <summary>
        /// 获取左值
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        protected static object getValue(ParatItem pi, ScriptObjectManager sm)
        {
            if(pi.LinkIParatItem == null)
            {
                return pi.Value;
            }
            else
            {
                if(pi.LinkIParatItem.ParentItemBox.BoxType == ItemBoxEnum.GETFUNCTION)
                {
                    object reo = null;
                    ScriptOutput so = (pi.LinkIParatItem.ParentItemBox as ItemBox).DoScriptFunction(null);
                    reo = so.GetValue(pi.LinkIParatItem.Name);
                    so.Dispose();
                    so = null;
                    return reo;
                }
                else
                    return sm.GetValue(pi.LinkIParatItem.ParentItemBox.Id).GetValue(pi.LinkIParatItem.Name);
            }
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="ib"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        protected static IItemBox DoFunction(ItemBox itembox, ScriptObjectManager sm, WriteStreamCallBack wrs)
        {
            LineItemBox ib = itembox as LineItemBox;
            if (ib.BoxType == ItemBoxEnum.IF)
            {
                ScriptInput si = new ScriptInput();
                //si.WriteStream += wrs;
                foreach (var item in ib.InputDatas)
                {
                    if (item.PIEnum != ParaItemEnum.OUTPUT && item.PIEnum != ParaItemEnum.INPUT && item.PIEnum != ParaItemEnum.NULL)
                        si.SetValue(item.Name, getValue(item as ParatItem, sm));
                }
                if (Convert.ToBoolean(si.GetFirst()))
                {
                    return ib.Next(0);
                }
                else
                {
                    return ib.Next(1);
                }
            }
            else if (ib.BoxType == ItemBoxEnum.SET)
            {
                ScriptOutput so = new ScriptOutput();
                so.SetValue(ib.InputDatas[1].Name, getValue(ib.InputDatas[1] as ParatItem, sm));
                //so.SetValue(ib.InputDatas[1].Name, ib.InputDatas[1].Value);
                sm.SetValue(ib.Id, so);
                return ib.Next(0);
            }
            else
            {
                ScriptInput si = new ScriptInput();
                si.WriteStream += wrs;
                foreach (var item in ib.InputDatas)
                {
                    if (item.PIEnum != ParaItemEnum.OUTPUT && item.PIEnum != ParaItemEnum.INPUT && item.PIEnum != ParaItemEnum.NULL)
                        si.SetValue(item.Name, getValue(item as ParatItem, sm));
                }
                ScriptOutput so = (ib as ItemBox).DoScriptFunction(si);
                if (so != null)
                    sm.SetValue(ib.Id, so);
                return ib.Next(0);
            }
        }
        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="ml"></param>
        public static void RunScript(MainLayout ml, WriteStreamCallBack wrs, IList<IPropertyIt> propertys, WriteStreamCallBack runover = null)
        {
            ScriptObjectManager sm = new ScriptObjectManager();
            if(propertys.Count > 0)
            {
                foreach (var item in propertys)
                {
                    ScriptOutput so = new ScriptOutput();
                    so.SetValue(item.Name, item.Value);
                    sm.SetValue(item.Id, so);
                }
            }
            try
            {
                IItemBox currentib = (ml.MainIb as LineItemBox).Next(0);
                IItemBox next = null;
                while (currentib != null)
                {
                    ml.SetFunctionBoxRun(System.Windows.Media.Colors.Red, currentib);
                    next = DoFunction(currentib as ItemBox, sm, wrs);
                    ml.SetFunctionBoxStop(System.Windows.Media.Colors.White, currentib);
                    currentib = next;
                }
            }
            catch (Exception ex)
            {
                wrs?.Invoke("脚本运行失败");
                Log.Write(new LogMessage("脚本运行失败", ex));
            }
            finally
            {
                sm.Dispose();
                sm = null;
                runover?.Invoke("程序运行结束");
            }
        }
    }
}
