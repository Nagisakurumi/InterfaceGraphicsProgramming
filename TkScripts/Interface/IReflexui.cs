using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TkScripts.Interface
{
    public interface IReflexui
    {
        IParatItem Source { get; }

        string PropertyName { get; }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="PropertyName"></param>
        void Bind(IParatItem Source, string PropertyName);
    }
}
