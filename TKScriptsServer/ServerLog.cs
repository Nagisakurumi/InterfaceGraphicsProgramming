using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKScriptsServer
{
    /// <summary>
    /// 服务器的日志
    /// </summary>
    internal static class ServerLog
    {
        /// <summary>
        /// 日志
        /// </summary>
        public static LogLib.LogInfo Log = new LogLib.LogInfo() { FileName = "Server" };
    }
}
