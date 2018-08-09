using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TkScripts.Interface;
using TKScriptsServer.Agreement;

namespace TkScripts.Reflexui
{
    public class IParaItemForEnum
    {
        private static IParaItemForEnum iParaItemForEnum = null;
        /// <summary>
        /// 类型字典
        /// </summary>
        internal Dictionary<ParaItemEnum, IReflexui> typeDictionary = new Dictionary<ParaItemEnum, IReflexui>()
        {
            { ParaItemEnum.INT, new ValueControl()},
            { ParaItemEnum.BOOL, new ENUMControl()},
            { ParaItemEnum.DATETIME, new ValueControl()},
            { ParaItemEnum.ENUM, new ENUMControl()},
            { ParaItemEnum.FLOAT, new ValueControl()},
            { ParaItemEnum.POINT, new PointControl()},
            { ParaItemEnum.STRING, new ValueControl()},
        };
        /// <summary>
        /// 单例
        /// </summary>
        private IParaItemForEnum()
        {

        }
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static IParaItemForEnum GetInstance()
        {
            if(iParaItemForEnum == null)
            {
                iParaItemForEnum = new IParaItemForEnum();
            }
            return iParaItemForEnum;
        }
        /// <summary>
        /// 添加一个类型和控件的映射
        /// </summary>
        /// <param name="penum"></param>
        /// <param name="iref"></param>
        public void SetType(ParaItemEnum penum, IReflexui iref)
        {
            if(iref as Control == null)
            {
                throw new Exception("添加的控件类型异常,请继承Cntrol， 并且实现IReflexui方法");
            }
            if(this.typeDictionary.ContainsKey(penum))
            {
                this.typeDictionary[penum] = null;
                this.typeDictionary[penum] = iref;
            }
            else
            {
                this.typeDictionary.Add(penum, iref);
            }
        }
        /// <summary>
        /// 获取类型相对的控件
        /// </summary>
        /// <param name="penum"></param>
        /// <returns></returns>
        public IReflexui GetControlType(ParaItemEnum penum)
        {
            if (this.typeDictionary.ContainsKey(penum))
            {
                return Activator.CreateInstance(this.typeDictionary[penum].GetType()) as IReflexui;
            }
            else
            {
                return null;
            }
        }
    }
}
