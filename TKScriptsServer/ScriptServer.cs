using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using static TKScriptsServer.ServerLog;
using System.IO;
using TKScriptsServer.API;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace TKScriptsServer
{
    /// <summary>
    /// 脚本服务器
    /// </summary>
    public class ScriptServer
    {
        /// <summary>
        /// 消息队列锁
        /// </summary>
        private object watchLock = new object();
        /// <summary>
        /// http服务监听
        /// </summary>
        private HttpListener httpListener = null;
        /// <summary>
        /// 监听端口线程
        /// </summary>
        private Thread threadWatchPort = null;
        /// <summary>
        /// 请求地址
        /// </summary>
        private string prefixesPath = "";
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptServer()
        {
            httpListener = new HttpListener();
        }
        #region Propertys
        /// <summary>
        /// 请求回调
        /// </summary>
        public Action<HttpListenerRequest, HttpListenerResponse> RequestCallBack = null;
        /// <summary>
        /// 监听的地址
        /// </summary>
        public string PrefixesPath
        {
            get
            {
                return prefixesPath;
            }
            set
            {
                removePrefixesPath(prefixesPath);
                prefixesPath = value;
                addPrefixesPath(prefixesPath);
            }
        }
        #endregion
        #region public
        /// <summary>
        /// 启动监听服务
        /// </summary>
        public void Start()
        {
            threadWatchPort = new Thread(watchHttp);
            threadWatchPort.IsBackground = true;
            threadWatchPort.Start();
            //设置匿名访问
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Start();
            Log.Write("开启监听服务!, 监听地址 : ", httpListener.Prefixes.ToArray()[0]);
        }

        
        #endregion

        #region private
        /// <summary>
        /// 监听线程
        /// </summary>
        private void watchHttp()
        {
            while (true)
            {
                try
                {
                    //等待请求，阻塞
                    var context = httpListener.GetContext();
                    //new Thread(ctext => {
                        lock (watchLock)
                        {
                            //HttpListenerContext httpListenerContext = ctext as HttpListenerContext;
                            HttpListenerContext httpListenerContext = context;
                            //取得请求的对象
                            HttpListenerRequest request = httpListenerContext.Request;
                            //string message = getRequestMessage(request);
                            // 取得回应对象
                            HttpListenerResponse response = httpListenerContext.Response;
                            RequestCallBack(request, response);
                        }
                }
                //    })
                //    { IsBackground = true, }.Start(context);
                //}
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }
        /// <summary>
        /// 添加http监听路径
        /// </summary>
        /// <param name="path">需要监听的端口和地址组成的基础路径</param>
        /// <returns>是否添加监听成功</returns>
        private bool addPrefixesPath(string path)
        {
            if (httpListener.Prefixes.Contains(path) == false)
            {
                httpListener.Prefixes.Add(path);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除监听的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool removePrefixesPath(string path)
        {
            if (httpListener.Prefixes.Contains(path) == true)
            {
                httpListener.Prefixes.Remove(path);
                return true;
            }
            return false;
        }
        #endregion
        #region static
        /// <summary>
        /// /获取请求的数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RequestMsg GetRequestMessage(HttpListenerRequest request)
        {
            if(request.HttpMethod.Equals("POST"))
            {
                MemoryStream memoryStream = new MemoryStream();
                request.InputStream.CopyTo(memoryStream);
                //string message = System.Text.Encoding.UTF8.GetString(datas);
                //datas = null;
                string values = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                Dictionary<string, string> valuePairs = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(values);
                
                //JObject jObject = (JObject)Newtonsoft.Json.JsonConvert.SerializeObject(values);
                //foreach (var item in jObject)
                //{
                //    valuePairs.Add(item.Key, item.Value.ToString());
                //}
                return new RequestMsg() { Stream = memoryStream.ToArray(), HttpMode = HttpMode.POST,
                ApiName = request.RawUrl.Substring(1, request.RawUrl.Length - 1), ValuePairs = valuePairs,};
            }
            else if(request.HttpMethod.Equals("GET"))
            {
                Match para = new Regex("/(.*?)\\?(.*)").Match(request.RawUrl);
                Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                string[] values = para.Groups[2].Value.Split('&');
                foreach (var item in values)
                {
                    string[] items = item.Split('=');
                    if(items.Length >= 2)
                        valuePairs.Add(items[0], items[1]);
                    items = null;
                }
                string apiName = para.Groups[1].Value;
                if(apiName.Equals("") && request.RawUrl.Contains("?") == false)
                {
                    apiName = request.RawUrl.Substring(1);
                }
                return new RequestMsg() { ApiName = apiName, ValuePairs = valuePairs, HttpMode = HttpMode.GET };
            }
            return null;
        }
        /// <summary>
        /// 获取流
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static byte[] GetRequestStream(HttpListenerRequest request)
        {
            byte[] datas = new byte[request.InputStream.Length];
            request.InputStream.Read(datas, 0, datas.Length);
            return datas;
        }
        #endregion
    }
}
