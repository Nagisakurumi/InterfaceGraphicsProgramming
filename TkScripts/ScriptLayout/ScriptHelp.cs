using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using TkScripts.Interface;
//using TkScripts.ScriptLayout.BezierLinkLayout;
using TkScripts.ScriptLayout.StackingLayout;
using Newtonsoft.Json;

namespace TkScripts.ScriptLayout
{
    /// <summary>
    /// 脚本帮助类
    /// </summary>
    public static class ScriptHelp
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        private static string Scripts = "Scripts";
        /// <summary>
        /// 扩展名称集合
        /// </summary>
        private static Dictionary<string, string> exName = new Dictionary<string, string>()
        {
            //{ typeof(MainLayout).Name, "LineScript"},
            { typeof(StackingMainLayout).Name, "TreeScript"},
        };
        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="script"></param>
        /// <param name="directory"></param>
        public static void SaveOne(IScriptLayout script, string directory)
        {
            string path = directory + "//" + script.ScriptName + "." + exName[script.GetType().Name];
            script.SaveToJson(path);
        }
        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IScriptLayout LoadOne(string filename)
        {
            string exname = filename.Split('.').Last();
            IScriptLayout script = null;
            foreach (var item in exName)
            {
                if(item.Value == exname)
                {
                    //if(item.Key == typeof(MainLayout).Name)
                    //{
                    //    script = new MainLayout();
                    //}
                    if(item.Key == typeof(StackingMainLayout).Name)
                    {
                        //script = new StackingMainLayout();
                    }
                }
            }
            if(script == null)
            {
                return null;
            }
            script.LoadFromJson(filename);
            return script;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="slnpath">项目文件路径</param>
        public static void Save(string slnpath, IList<IScriptLayout> scripts)
        {
            StringBuilder sb = new StringBuilder();
            string directory = Path.GetDirectoryName(slnpath);

            sb.Append("{\"" + Scripts + "\":[");
            foreach (var item in scripts)
            {
                ScriptHelp.SaveOne(item, directory);
                sb.Append("\"" + item.ScriptName + "\",");
            }
            if(sb[sb.Length -1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]}");
            using (FileStream fs = File.Open(slnpath, FileMode.Create))
            {
                byte[] datas = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                fs.Write(datas, 0, datas.Length);
                datas = null;
            }
            sb.Clear();
            sb = null;
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="slnpath">项目文件路径</param>
        public static IList<IScriptLayout> Load(string slnpath)
        {
            string json = "";
            using (FileStream fs = File.Open(slnpath, FileMode.Open))
            {
                byte[] datas = new byte[fs.Length];
                fs.Read(datas, 0, datas.Length);
                json = System.Text.Encoding.UTF8.GetString(datas);
                datas = null;
            }
            JObject jobj = (JObject)JsonConvert.DeserializeObject(json);
            JToken scriptpaths = jobj[Scripts];
            string directory = Path.GetDirectoryName(slnpath);
            IList<IScriptLayout> scripts = new List<IScriptLayout>();
            foreach (var item in scriptpaths)
            {
                IScriptLayout script = ScriptHelp.LoadOne(Path.Combine(directory, item.ToString()));
                if (script != null)
                {
                    scripts.Add(script);
                }
            }
            return scripts;
        }

        /// <summary>
        /// 转换Bitmap到BitmapSource
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapSource GetBitmapSource(System.Drawing.Bitmap bmp)
        {
            System.Windows.Media.Imaging.BitmapFrame bf = null;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                bf = System.Windows.Media.Imaging.BitmapFrame.Create(ms, System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);

            }
            return bf;
            //return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }


        private static BitmapSource functionImage = null;
        /// <summary>
        /// 函数图标
        /// </summary>
        public static BitmapSource FunctionImage
        {
            get
            {
                if(functionImage == null)
                {
                    functionImage = GetBitmapSource(TkScripts.Properties.Resources.function);
                }
                return functionImage;
            }
        }
        private static BitmapSource brachImage = null;
        /// <summary>
        /// 函数图标
        /// </summary>
        public static BitmapSource BrachImage
        {
            get
            {
                if (brachImage == null)
                {
                    brachImage = GetBitmapSource(TkScripts.Properties.Resources.brach);
                }
                return brachImage;
            }
        }
        private static BitmapSource whileImage = null;
        /// <summary>
        /// 函数图标
        /// </summary>
        public static BitmapSource WhileImage
        {
            get
            {
                if (whileImage == null)
                {
                    whileImage = GetBitmapSource(TkScripts.Properties.Resources.loop);
                }
                return whileImage;
            }
        }
        /// <summary>
        /// 列表图标
        /// </summary>
        public static BitmapSource ListImage = GetBitmapSource(TkScripts.Properties.Resources.list);
    }
}
