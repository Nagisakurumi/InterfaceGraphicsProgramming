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
        /// 连接的贝塞尔曲线
        /// </summary>
        public List<BezierLine> LinksBezierLine
        {
            get
            {
                return linksBezierLine;
            }
        }

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
        /// 是否允许连接
        /// </summary>
        /// <param name="mpl"></param>
        /// <returns></returns>
        public bool IsCanLink(BezierLine bz)
        {
            MLParatItemLayout mpl = bz.LeftElement as MLParatItemLayout == null ? bz.RightElement as MLParatItemLayout : bz.LeftElement as MLParatItemLayout;
            ParaItemEnum mplpe = (mpl.DataContext as IParatItem).PIEnum;
            ParaItemEnum thispe = (this.DataContext as IParatItem).PIEnum;
            if(mplpe != ParaItemEnum.INPUT && mplpe != ParaItemEnum.OUTPUT &&
                thispe != ParaItemEnum.INPUT && thispe != ParaItemEnum.OUTPUT)
            {
                return mplpe == thispe || (mplpe == ParaItemEnum.OBJECT || thispe == ParaItemEnum.OBJECT);
            }
            else if((mplpe == ParaItemEnum.OUTPUT || mplpe == ParaItemEnum.INPUT) &&
                 (thispe == ParaItemEnum.INPUT || thispe == ParaItemEnum.OUTPUT))
            {
                return mplpe != thispe;
            }
            return false;
        }
        /// <summary>
        /// 连接的贝塞尔曲线
        /// </summary>
        private List<BezierLine> linksBezierLine = new List<BezierLine>();
        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="bz"></param>
        /// <returns></returns>
        public virtual BezierLine SetLink(BezierLine bz)
        {
            BezierLine bl = null;
            if ((ipt.PIEnum == ParaItemEnum.OUTPUT || (this as InputParaItemLayout != null && ipt.PIEnum != ParaItemEnum.INPUT))
                && this.LinksBezierLine.Count > 0)
            {
                bl = this.linksBezierLine[0];
                this.LinksBezierLine.Remove(bl);
            }
            this.LinksBezierLine.Add(bz);
            LinksChange();
            return bl;
        }
        /// <summary>
        /// 取消这个点下的所有连接线
        /// </summary>
        /// <returns></returns>
        public virtual List<BezierLine> CancleLink()
        {
            List<BezierLine> ls = new List<BezierLine>();
            ls.AddRange(this.LinksBezierLine);
            this.LinksBezierLine.Clear();
            LinksChange();
            return ls;
        }
        /// <summary>
        /// 删除一条曲线
        /// </summary>
        /// <param name="bz"></param>
        public virtual void Del(BezierLine bz)
        {
            if(this.LinksBezierLine.Contains(bz))
            {
                this.LinksBezierLine.Remove(bz);
            }
            LinksChange();
        }
        /// <summary>
        /// 连接状态发生变化
        /// </summary>
        protected virtual void LinksChange()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void InitParatItemLayout()
        {

        }
    }
}
