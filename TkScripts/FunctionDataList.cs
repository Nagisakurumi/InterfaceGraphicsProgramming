using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TkScripts.ScriptLayout;

namespace TkScripts
{
    /// <summary>
    /// 函数数据列表
    /// </summary>
    public class FunctionDataList
    {

        /// <summary>
        /// 数据列表
        /// </summary>
        private static DataTreeView functionView = new DataTreeView();
        /// <summary>
        /// 函数列表
        /// </summary>
        public static DataTreeView FunctionView
        {
            get
            {
                return functionView;
            }
        }
    }
}
