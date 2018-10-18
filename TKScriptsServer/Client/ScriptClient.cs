using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TKScriptsServer.Client.ClientLog;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TKScriptsServer.Client
{
    public class ScriptClient : IDisposable
    {
        /// <summary>
        /// 请求客户端
        /// </summary>
        private HttpClient httpClient = new HttpClient();
        /// <summary>
        /// 请求头的参数
        /// </summary>
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0";
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptClient()
        {
            //clientHandler.CookieContainer = CookieContainer;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", UserAgent);
            httpClient.Timeout = TimeSpan.FromHours(10);
            //httpClient.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            //httpClient.DefaultRequestHeaders.Add("Referer", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("KeepAlive", "true");

            Log.Write("初始化请求客户端!");
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Dispose()
        {
            httpClient.Dispose();
        }
        /// <summary>
        /// Post请求获取返回字符串
        /// </summary>
        /// <param name="smartQQAPI"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string PostStringAsync(string url, JObject content)
        {
            HttpResponseMessage httpResponseMessage = postResponseAsync(url, content).Result;
            return httpResponseMessage.Content.ReadAsStringAsync().Result;
        }
        /// <summary>
        /// 获取get请求的字符串流
        /// </summary>
        /// <param name="smartQQAPI"></param>
        /// <returns></returns>
        public string GetStringAsync(string url, params object[] ags)
        {
            string result = httpClient.GetStringAsync(url).Result;
            //reciveRef(refuri);
            return result;
        }
        #region private
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="smartQQAPI"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> postResponseAsync(string url, JObject content)
        {
            HttpContent hc = new StringContent(content.ToString(Formatting.None), Encoding.UTF8);
            hc.Headers.ContentType = MediaTypeHeaderValue.Parse("application/ x-www-form-urlencoded; charset=UTF-8");
            
            var response = httpClient.PostAsync(url, hc);
            response.Wait();
            return await response;
        }
        #endregion
    }
}
