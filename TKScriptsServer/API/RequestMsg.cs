using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKScriptsServer.Agreement;

namespace TKScriptsServer.API
{
    public class RequestMsg : IDisposable
    {
        /// <summary>
        /// 请求模式
        /// </summary>
        public HttpMode HttpMode;
        /// <summary>
        /// 请求的参数
        /// </summary>
        public ScriptInput ValuePairs;
        /// <summary>
        /// 请求的流
        /// </summary>
        public byte[] Stream;
        /// <summary>
        /// 请求的地址
        /// </summary>
        public string ApiName;
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            ValuePairs.Dispose();
            ValuePairs = null;
            Stream = null;
            ApiName = null;
        }
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
