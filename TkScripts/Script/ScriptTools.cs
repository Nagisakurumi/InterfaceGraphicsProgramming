using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;
using TkScripts.ScriptLayout;
using TKScriptsServer.Agreement;
using TKScriptsServer.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TkScripts.Script
{
    /// <summary>
    /// 脚本数据操作工具
    /// </summary>
    public static class ScriptTools
    {
        /// <summary>
        /// 在tdfather子内容中添加一个对象的方法作为itembox
        /// </summary>
        /// <param name="tdfather"></param>
        /// <param name="obj"></param>
        /// <param name="medthname"></param>
        public static IItemBox AddMeodthToItemBox<IBox, IParat>(TreeData tdfather, ScriptFunction sf)
        {
            string medthname = sf.GetMethodInfo().Name;
            IBox box = (IBox)Activator.CreateInstance(typeof(IBox));
            IItemBox opitem = box as IItemBox;
            if(opitem == null)
            {
                throw new Exception("IBox， 不是一个有效的实现于IItemBox的类");
            }
            opitem.BoxType = ItemBoxEnum.FUNCTION;
            MethodInfo minfo = sf.GetMethodInfo();
            if (minfo != null)
            {
                ScriptMethAttribute attribute = minfo.GetCustomAttribute(typeof(ScriptMethAttribute), false) as ScriptMethAttribute;
                if (attribute != null)
                {
                    if (attribute.Name != "")
                    {
                        opitem.Name = attribute.Name;
                    }
                    else
                    {
                        opitem.Name = medthname;
                    }
                    opitem.BoxType = attribute.ItemBoxEnum;
                    opitem.TipText = attribute.Describe;
                    if (attribute.InputData.Count > 0)
                    {
                        foreach (var item in attribute.InputData)
                        {
                            IParat ipa = (IParat)Activator.CreateInstance(typeof(IParat));
                            IParatItem op = ipa as IParatItem;
                            //ParatItem op = new ParatItem(item.Name, LogLib.TClassOption.GetEnumTypeByString<ParaItemEnum>(item.Type), item.EnumDatas)
                            op.Name = item.Name;
                            op.PIEnum = LogLib.TClassOption.GetEnumTypeByString<ParaItemEnum>(item.Type);
                            op.EnumDatas = item.EnumDatas;
                            op.Value = item.DefultValue;
                            opitem.AddInput(op);
                            op.TipText = item.TipText;
                        }
                    }
                    if (attribute.OutputData.Count > 0)
                    {
                        foreach (var item in attribute.OutputData)
                        {
                            IParat ipa = (IParat)Activator.CreateInstance(typeof(IParat));

                            IParatItem op = ipa as IParatItem;
                            op.Name = item.Name;
                            op.PIEnum = LogLib.TClassOption.GetEnumTypeByString<ParaItemEnum>(item.Type);
                            op.EnumDatas = item.EnumDatas;
                            op.Value = item.DefultValue;
                            opitem.AddOutput(op);
                            op.TipText = item.TipText;
                        }
                    }
                    tdfather.Children.Add(new TreeData(opitem));
                }
            }
            //opitem = null;
            return opitem;
        }
        /// <summary>
        /// 获取服务中的所有函数列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<IItemBox> FromServerGetIItemboxInfos(string url)
        {
            ScriptClient scriptClient = new ScriptClient();

            string json = scriptClient.GetStringAsync(url);
            

            scriptClient.Dispose();
            scriptClient = null;
            return JsonConvert.DeserializeObject<List<IItemBox>>(json);
        }

        /// <summary>
        /// 获取iitembox
        /// </summary>
        /// <typeparam name="IBox"></typeparam>
        /// <typeparam name="IParat"></typeparam>
        /// <param name="tdfather"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static IItemBox GetIItemboxByScriptMeth<IBox, IParat>(TreeData tdfather, ScriptMethAttribute attribute)
        {
            IBox box = (IBox)Activator.CreateInstance(typeof(IBox));
            IItemBox opitem = box as IItemBox;
            if (attribute != null)
            {
               
                opitem.Name = attribute.Name;
                opitem.ScriptUrl = attribute.Url;
                opitem.BoxType = attribute.ItemBoxEnum;
                opitem.TipText = attribute.Describe;
                if (attribute.InputData.Count > 0)
                {
                    foreach (var item in attribute.InputData)
                    {
                        IParat ipa = (IParat)Activator.CreateInstance(typeof(IParat));
                        IParatItem op = ipa as IParatItem;
                        //ParatItem op = new ParatItem(item.Name, LogLib.TClassOption.GetEnumTypeByString<ParaItemEnum>(item.Type), item.EnumDatas)
                        op.Name = item.Name;
                        op.PIEnum = LogLib.TClassOption.GetEnumTypeByString<ParaItemEnum>(item.Type);
                        op.EnumDatas = item.EnumDatas;
                        op.Value = item.DefultValue;
                        opitem.AddInput(op);
                        op.TipText = item.TipText;
                    }
                }
                if (attribute.OutputData.Count > 0)
                {
                    foreach (var item in attribute.OutputData)
                    {
                        IParat ipa = (IParat)Activator.CreateInstance(typeof(IParat));

                        IParatItem op = ipa as IParatItem;
                        op.Name = item.Name;
                        op.PIEnum = LogLib.TClassOption.GetEnumTypeByString<ParaItemEnum>(item.Type);
                        op.EnumDatas = item.EnumDatas;
                        op.Value = item.DefultValue;
                        opitem.AddOutput(op);
                        op.TipText = item.TipText;
                    }
                }
                tdfather.Children.Add(new TreeData(opitem));
            }
            return opitem;
        }
    }
}
