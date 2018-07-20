using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKScriptsServer.API
{
    public abstract class API
    {
        /// <summary>
        /// api请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public JObject Data { get; set; }
        /// <summary>
        /// 请求源
        /// </summary>
        public string Refresh { get; set; }
        /// <summary>
        /// 填充get请求的后续参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string BuildUrl(params object[] args)
        {
            var i = 1;
            return args.Aggregate(Url, (current, arg) => current.Replace("{" + i++ + "}", arg.ToString()));
        }
    }
}
