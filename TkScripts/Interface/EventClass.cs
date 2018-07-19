using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TkScripts.Interface
{
    /// <summary>
    /// itembox选中事件
    /// </summary>
    /// <param name="selectedItembox"></param>
    public delegate void ItemSelectedEvent(IItemBox selectedItembox);
    /// <summary>
    /// 项鼠标事件
    /// </summary>
    /// <param name="selectedItem"></param>
    public delegate void ItemEvent(object selectedItem);
}
