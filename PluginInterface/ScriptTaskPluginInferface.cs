using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKScriptsServer.Agreement;

namespace PluginInterface
{
    /// <summary>
    /// 脚本执行器插件接口
    /// </summary>
    public interface ScriptTaskPluginInferface
    {
        /// <summary>
        /// 启动
        /// </summary>
        void Start();

        /// <summary>
        /// 关闭
        /// </summary>
        void Dispose();

        /// <summary>
        /// 获取插件界面控件
        /// </summary>
        /// <returns></returns>
        object GetPluginInterface();
        /// <summary>
        /// 插件界面类型
        /// </summary>
        PluginType PluginType { get; }
        /// <summary>
        /// 布局类型
        /// </summary>
        LayoutType LayoutType { get; }
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 发送请求函数回调
        /// </summary>
        /// <param name="scriptInput">函数输入参数</param>
        /// <param name="url">函数地址</param>
        void SendToRequestFunction(ScriptInput scriptInput, string functioNname);
        /// <summary>
        /// 接受函数的返回
        /// </summary>
        /// <param name="scriptOutput">函数的返回参数</param>
        void ReciveResponseFunction(ScriptOutput scriptOutput, string functioNname);
    }

    /// <summary>
    /// 插件类型
    /// </summary>
    public enum PluginType
    {
        /// <summary>
        /// winform 控件  必须继承Control
        /// </summary>
        Winform,
        /// <summary>
        /// wpf 控件 必须继承Element
        /// </summary>
        WPF,
    }
    /// <summary>
    /// 布局类型
    /// </summary>
    public enum LayoutType
    {
        Left,
        Right,
        Top,
        Bottom,
    }
}
