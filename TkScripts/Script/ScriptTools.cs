using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.BezierLinkLayout;

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
            opitem.LogoPath = ScriptHelp.FunctionImage;
            opitem.BoxType = ItemBoxEnum.FUNCTION;
            opitem.ScriptFunction += sf;
            MethodInfo minfo = sf.GetMethodInfo();
            if (minfo != null)
            {
                ScriptMethAttribute attribute = minfo.GetCustomAttribute(typeof(ScriptMethAttribute), false) as ScriptMethAttribute;
                if (attribute != null)
                {
                    if(attribute.Name != "")
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
    }
}
