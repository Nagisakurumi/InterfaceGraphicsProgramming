using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TkScripts.Interface
{
    public interface ISave
    {
        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="path"></param>
        void SaveToJson(string content);
        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <param name="path"></param>
        void LoadFromJson<IType>(string content);
    }

    
}
