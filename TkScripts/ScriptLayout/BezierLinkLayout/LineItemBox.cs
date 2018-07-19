using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;

namespace TkScripts.ScriptLayout.BezierLinkLayout
{
    public class LineItemBox : ItemBox
    {
        private double contentX = 0;
        private double contentY = 0;
        /// <summary>
        /// 绑定使用的坐标X
        /// </summary>
        [ToJson(true)]
        public double ContentX
        {
            get
            {
                return contentX;
            }

            set
            {
                contentX = value;
                Changed("ContentX");
            }
        }
        /// <summary>
        /// 绑定使用的坐标Y
        /// </summary>
        [ToJson(true)]
        public double ContentY
        {
            get
            {
                return contentY;
            }

            set
            {
                contentY = value;
                Changed("ContentY");
            }
        }
        /// <summary>
        /// 前一个
        /// </summary>
        [JsonIgnore]
        public virtual IItemBox Previous
        {
            get
            {
                foreach (var item in InputDatas)
                {
                    if (item.PIEnum == ParaItemEnum.INPUT)
                    {
                        return item.LinkIParatItem.ParentItemBox;
                    }
                }
                return null;
            }

            set
            {
                Changed("Previous");
            }
        }
        /// <summary>
        /// 下一个
        /// </summary>
        public virtual IItemBox Next(int index)
        {
            int idx = 0;
            foreach (var item in OutDatas)
            {
                if (item.PIEnum == ParaItemEnum.OUTPUT)
                {
                    if (idx == index)
                    {
                        if (item.LinkIParatItem != null)
                        {
                            return item.LinkIParatItem.ParentItemBox;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    idx++;
                }
            }
            return null;
        }
        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="isSelected"></param>
        public void SetSelectedStuta(bool isSelected)
        {
            if (isSelected)
            {
                this.Thickness = 6;
            }
            else
            {
                this.Thickness = 2;
            }
        }
    }
}
