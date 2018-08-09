using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Script;

namespace TkScripts.Interface
{
    public interface ICompile
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        Task<bool> RunCompile();
        /// <summary>
        /// 暂停
        /// </summary>
        void StopRun();
    }
}
