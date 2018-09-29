using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static TKScriptsServer.ServerLog;
using TKScriptsServer.Agreement;

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
        public Dictionary<string, ScriptAPI> scriptAPIs { get; } = new Dictionary<string, ScriptAPI>();
        /// <summary>
        /// 更新API请求
        /// </summary>
        public Action updateAPIsRequest = null;
        /// <summary>
        /// 发送消息到客户端信息回调
        /// </summary>
        public Action<ScriptOutput> sendMsgToClient = null;
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
        protected void RequestCallBack(HttpListenerRequest request, HttpListenerResponse response)
        {
            RequestMsg requestMsg = null;
            try
            {
                requestMsg = ScriptServer.GetRequestMessage(request);
                if (requestMsg.ApiName.Equals(GetAllAPIUrlAPIName))
                {
                    updateAPIsRequest?.Invoke();
                    writeResponse(getAllApi(), response, 200);
                }
                else if (scriptAPIs.ContainsKey(requestMsg.ApiName))
                {
                    ScriptInput scriptInput = requestMsg.ValuePairs;
                    ScriptOutput scriptOutput = scriptAPIs[requestMsg.ApiName].ScriptFunction(scriptInput);
                    writeResponse(scriptOutput, response, 200);
                    scriptOutput.Dispose();
                    scriptInput.Dispose();
                    scriptOutput = null;
                    scriptInput = null;
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
            finally
            {
                if(requestMsg != null)
                {
                    requestMsg.Dispose();
                    requestMsg = null;
                }
            }
        }

        /// <summary>
        /// 添加脚本函数
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="scriptFunction">脚本函数</param>
        public bool AddScriptFunction(string url, ScriptFunction scriptFunction)
        {
            if(scriptAPIs.ContainsKey(url) == false)
            {
                scriptAPIs.Add(url, new ScriptAPI(scriptServer.PrefixesPath + url, scriptFunction));
                return true;
            }
            else
            {
                scriptAPIs[url] = new ScriptAPI(scriptServer.PrefixesPath + url, scriptFunction);
                return true;
            }
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
            List<ScriptMethAttribute> scriptMeths = new List<ScriptMethAttribute>();
            foreach (var item in scriptAPIs)
            {
                scriptMeths.Add(item.Value.ScriptMethAttribute);
            }
            return JsonConvert.SerializeObject(scriptMeths).ToString();
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
        private void writeResponse(ScriptOutput obj, HttpListenerResponse response, int statuCode)
        {
            sendMsgToClient?.Invoke(obj);
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
