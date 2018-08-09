using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TkScripts;
using TkScripts.Interface;
using TkScripts.ScriptLayout.BezierLinkLayout;

namespace TkScripts.ScriptLayout.BezierLinkLayout.ScriptIParameterLayout
{
    public class MLParatItemLayout : MLUIMain
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
                Changed("Index");
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        private int index = 0;

        /// <summary>
        /// 项内容
        /// </summary>
        protected IParatItem ipt = null;
        
        /// <summary>
        /// 连接点
        /// </summary>
        public virtual Point LinkPosition { get; set; }


        /// <summary>
        /// 渲染尺寸改变
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Changed("LinkPosition");
        }
        /// <summary>
        /// 引发坐标改变事件
        /// </summary>
        /// <param name="uie"></param>
        public void OpPositionChanged(UIElement uie = null)
        {
            if(uie != null)
            {
                UIMain = uie;
            }
            Changed("LinkPosition");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void InitParatItemLayout()
        {

        }
    }
}
