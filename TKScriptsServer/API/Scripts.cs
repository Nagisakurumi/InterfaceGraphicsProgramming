using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Script;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static TKScriptsServer.ServerLog;

namespace TKScriptsServer.API
{
    /// <summary>
    /// 脚本控制
    /// </summary>
    public class Scripts
    {
        /// <summary>
        /// 获取所有api列表时候使用的 地址段
        /// </summary>
        public static string GetAllAPIUrlAPIName = "getallapis";
        /// <summary>
        /// 服务
        /// </summary>
        private ScriptServer scriptServer = new ScriptServer();
        /// <summary>
        /// 脚本api列表
        /// </summary>
        internal Dictionary<string, ScriptAPI> scriptAPIs { get; } = new Dictionary<string, ScriptAPI>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="listenerPath">监听的url</param>
        public Scripts(string listenerPath)
        {
            scriptServer.PrefixesPath = listenerPath;
            scriptServer.RequestCallBack += RequestCallBack;

        }
        /// <summary>
        /// 请求回调
        /// </summary>
        /// <param name="arg1">请求流</param>
        /// <param name="response">回写流</param>
        public void RequestCallBack(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                RequestMsg requestMsg = ScriptServer.GetRequestMessage(request);
                if (requestMsg.ApiName.Equals(GetAllAPIUrlAPIName))
                {
                    writeResponse(getAllApi(), response, 200);
                }
                else if (scriptAPIs.ContainsKey(requestMsg.ApiName))
                {
                    ScriptInput scriptInput = new ScriptInput();
                    foreach (var item in requestMsg.ValuePairs)
                    {
                        scriptInput.SetValue(item.Key, item.Value);
                    }
                    ScriptOutput scriptOutput = scriptAPIs[requestMsg.ApiName].ScriptFunction(scriptInput);
                    writeResponse(scriptInput, response, 200);
                }
                else
                {
                    ///回复客户端异常
                    writeResponse(response, 201);
                }
            }
            catch (Exception ex)
            {
                writeResponse(response, 201);
                Log.Write("客户端调用出错", ex);
            }
        }

        /// <summary>
        /// 添加脚本函数
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="scriptFunction">脚本函数</param>
        public bool AddScriptFunction(string url, ScriptFunction scriptFunction)
        {
            if(scriptAPIs.ContainsKey(scriptServer.PrefixesPath + url) == false)
            {
                scriptAPIs.Add(url, new ScriptAPI(scriptServer.PrefixesPath + url, scriptFunction));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除一个脚本函数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool RemoveScriptFunction(string url)
        {
            if (scriptAPIs.ContainsKey(scriptServer.PrefixesPath + url) == true)
            {
                scriptAPIs.Remove(url);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            scriptServer.Start();
        }


        #region private
        /// <summary>
        /// 获取所有的api信息
        /// </summary>
        /// <returns></returns>
        private string getAllApi()
        {
            return JsonConvert.SerializeObject(scriptAPIs).ToString();
        }
        /// <summary>
        /// 回复客户端信息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">返回请求</param>
        /// <param name="statuCode">返回的状态码</param>
        private void writeResponse(string msg, HttpListenerResponse response, int statuCode)
        {
            byte[] datas = System.Text.Encoding.UTF8.GetBytes(msg);
            response.ContentLength64 = datas.Length;
            response.OutputStream.Write(datas, 0, datas.Length);
            response.StatusCode = statuCode;
            response.OutputStream.Close();
        }
        /// <summary>
        /// 回复客户端
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="response"></param>
        /// <param name="statuCode"></param>
        private void writeResponse(object obj, HttpListenerResponse response, int statuCode)
        {
            writeResponse(JsonConvert.SerializeObject(obj), response, statuCode);
        }
        /// <summary>
        /// 回复客户端
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statuCode"></param>
        private void writeResponse(HttpListenerResponse response, int statuCode)
        {
            writeResponse("", response, statuCode);
        }
        #endregion
    }
}
