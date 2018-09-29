using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;
using TkScripts.Script;
using TKScriptsServer.Agreement;

namespace TkScripts.Interface
{
    /// <summary>
    /// 脚本解析
    /// </summary>
    public abstract class IScriptInterpreter
    {
        /// <summary>
        /// 脚本运行断点回调
        /// </summary>
        public ScriptBreakPointCallBackHandle ScriptBreakPoint = null;
        /// <summary>
        /// 脚本请求回调
        /// </summary>
        public Action<ScriptInput, string> ScriptRequest;
        /// <summary>
        /// 脚本返回回调
        /// </summary>
        public Action<ScriptOutput, string> ScriptReponse;
        /// <summary>
        /// 对象管理
        /// </summary>
        protected ScriptObjectManager manager = new ScriptObjectManager();
        /// <summary>
        /// 是否处于运行状态
        /// </summary>
        public bool IsRunning { get; protected set; }
        /// <summary>
        /// 初始化解释器
        /// </summary>
        protected abstract void Init(IScriptLayout ml);
        /// <summary>
        /// 设置到内存管理中的值
        /// </summary>
        /// <param name="item"></param>
        protected abstract void SetValue(IParatItem item);
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract object GetValue(IParatItem item);
        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="ml"></param>
        /// <param name="wrs"></param>
        public abstract Task<bool> RunScript(IScriptLayout ml);
        /// <summary>
        /// 终止脚本的运行
        /// </summary>
        public abstract void StopScript();
        /// <summary>
        /// 脚本运行断点回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="si"></param>
        /// <param name="so"></param>
        public delegate void ScriptBreakPointCallBackHandle(IItemBox item, ScriptInput si, ScriptOutput so);
        /// <summary>
        /// 运行到下一个函数
        /// </summary>
        public abstract void RunNextFunction();
        /// <summary>
        /// 继续运行到下一个断点
        /// </summary>
        public abstract void RunOver();
    }
}
