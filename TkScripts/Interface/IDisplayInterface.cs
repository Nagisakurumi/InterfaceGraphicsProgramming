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
        /// <summary>
        /// 字体颜色
        /// </summary>
        [JsonIgnore]
        Brush ForgeBrush { get; set; }
        /// <summary>
        /// 框的颜色
        /// </summary>
        [JsonIgnore]
        Brush BoxBrush { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [JsonIgnore]
        ImageSource LogoPath { get; set; }
    }
}
