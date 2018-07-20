using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKScriptsServer.API
{
    public class RequestMsg
    {
        /// <summary>
        /// 请求模式
        /// </summary>
        public HttpMode HttpMode;
        /// <summary>
        /// 请求的参数
        /// </summary>
        public Dictionary<string, string> ValuePairs;
        /// <summary>
        /// 请求的流
        /// </summary>
        public byte[] Stream;
        /// <summary>
        /// 请求的地址
        /// </summary>
        public string ApiName;
    }



    public enum HttpMode
    {
        /// <summary>
        /// get请求
        /// </summary>
        GET,
        /// <summary>
        /// post请求
        /// </summary>
        POST,
    }
}
