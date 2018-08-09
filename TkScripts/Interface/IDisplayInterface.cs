using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TkScripts.Interface
{
    public interface IDisplayInterface
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// 提示文字
        /// </summary>
        string TipText { get; set; }
    }
}
