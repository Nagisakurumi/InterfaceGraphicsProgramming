using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using TkScripts.Script;

namespace ServerTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://127.0.0.1:7676/getapi?dd=1";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("KeepAlive", "true");
            //HttpResponseMessage httpRequestMessage = httpClient.GetAsync(url).Result;
            HttpResponseMessage response = null;
            response = httpClient.GetAsync("http://127.0.0.1:7676/getallapis").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            var content = new JObject {
                { "add1", "1"},
                { "add2", "1"},
            };
            //string contentstring = $"r={content.ToString(Formatting.None)}";
            HttpContent hc = new StringContent(content.ToString(Formatting.None), Encoding.UTF8);
            hc.Headers.ContentType = MediaTypeHeaderValue.Parse("application/ x-www-form-urlencoded; charset=UTF-8");
            response = httpClient.PostAsync("http://127.0.0.1:7676/add", hc).Result;

            //ScriptOutput value = JsonConvert.DeserializeObject<ScriptOutput>(response.Content.ReadAsStringAsync().Result);

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine("等待!");
            Console.ReadKey();
        }
    }
}
