using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace TkScripts.Log
{
    /// <summary>
    /// 脚本日志
    /// </summary>
    public class ScriptLog
    {
        /// <summary>
        /// 日志
        /// </summary>
        public static LogLib.LogInfo Log = new LogInfo() { FileName = "Script"};
    }
}
