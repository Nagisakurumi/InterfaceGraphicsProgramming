using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKScripts.Manager
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// 从文件读取数据
        /// </summary>
        /// <param name="path">文件名</param>
        /// <returns></returns>
        public static string GetStringFromFile(this string path)
        {
            string json = "";
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] datas = new byte[stream.Length];
                stream.Read(datas, 0, datas.Length);
                json = System.Text.Encoding.UTF8.GetString(datas);
                datas = null;
            }
            return json;
        }
        /// <summary>
        /// 保存对象到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveObjectToFile(this object obj, string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj);
                using (FileStream stream = File.Open(path, FileMode.Create))
                {
                    byte[] datas = System.Text.Encoding.UTF8.GetBytes(json);
                    stream.Write(datas, 0, datas.Length);
                    datas = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 查找一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T FindOne<T>(this IEnumerable<T> ts, Func<T, bool> func)
        {
            IEnumerable<T> list = ts.Where(func);
            if(list.Count() > 0)
            {
                return list.First();
            }
            return default(T);
        }
    }
}
