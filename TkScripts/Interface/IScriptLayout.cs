using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TkScripts.Script;
using TkScripts.ScriptLayout;
using TkScripts.ScriptLayout.StackingLayout;
using TKScriptsServer.Agreement;
using static TkScripts.Interface.IScriptInterpreter;


namespace TkScripts.Interface
{
    /// <summary>
    /// 脚本编辑器
    /// </summary>
    public class IScriptLayout : INotifyPropertyChanged, ICompile
    {
        /// <summary>
        /// 代码块存储名称
        /// </summary>
        public readonly static string ItemBoxsListName = "ItemBoxs";
        /// <summary>
        /// 属性存储名称
        /// </summary>
        public readonly static string IPropertyItsListName = "IPropertyIts";
        /// <summary>
        /// 脚本名称
        /// </summary>
        public readonly static string IScriptName = "ScriptName";
        /// <summary>
        /// 属性改变时候的通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 编译运行时候的信息回调
        /// </summary>
        public WriteStreamCallBack ComipleMessageCall = null;
        /// <summary>
        /// 脚本断点执行回调
        /// </summary>
        public ScriptBreakPointCallBackHandle ScriptBreakPoint
        {
            get
            {
                return IScriptInterpreter.ScriptBreakPoint;
            }
            set
            {
                IScriptInterpreter.ScriptBreakPoint = value;
            }
        }
        /// <summary>
        /// 解释器
        /// </summary>
        public IScriptInterpreter IScriptInterpreter { get; set; } = new StackScriptOption();
        /// <summary>
        /// 构造函数
        /// </summary>
        public IScriptLayout()
        {
           
        }
        /// <summary>
        /// 属性改变
        /// </summary>
        /// <param name="name"></param>
        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 设置全局变量的值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public virtual void SetPropertyValue(string id, object value)
        {
            IPropertyIt pro = FindIPropertyById(id);
            if(pro != null)
            {
                pro.Value = value;
            }
        }
        /// <summary>
        /// 添加代码块
        /// </summary>
        /// <param name="itembox"></param>
        public virtual void Add(ItemBox itembox)
        {
            this.Itemboxs.Add(itembox);
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="propertyIt"></param>
        public virtual void Add(PropertyIt propertyIt)
        {
            this.IPropertys.Add(propertyIt);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="propertyIt"></param>
        public virtual void Del(PropertyIt propertyIt)
        {
            if (this.iPropertys.Contains(propertyIt))
            {
                this.iPropertys.Remove(propertyIt);
            }
        }
        /// <summary>
        /// 删除一个代码块
        /// </summary>
        /// <param name="box"></param>
        public virtual void Del(IItemBox box)
        {
            
        }
        /// <summary>
        /// 删除所选择的
        /// </summary>
        public virtual void DeleteSelected()
        {
            
        }
        
        /// <summary>
        /// 从json加载
        /// </summary>
        /// <typeparam name="Ibox">iitembox的实际类型</typeparam>
        /// <typeparam name="IProperty">连接线的实际类型</typeparam>
        /// <typeparam name="IParaitem">参数的实际类型</typeparam>
        /// <param name="file">json文件路径</param>
        public virtual bool LoadFromJson(string file)
        {
            string json = "";
            using (FileStream filestream = File.Open(file, FileMode.Open))
            {
                byte[] datas = new byte[(int)filestream.Length];
                filestream.Read(datas, 0, datas.Length);
                json = System.Text.Encoding.UTF8.GetString(datas);
                datas = null;
            }
            if(json.Equals("") == false)
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(json);
                this.itemboxs = JsonConvert.DeserializeObject<ObservableCollection<ItemBox>>(jObject[ItemBoxsListName].ToString());
                ObservableCollection<PropertyIt> propertyIts = JsonConvert.DeserializeObject<ObservableCollection<PropertyIt>>(jObject[IPropertyItsListName].ToString());
                foreach (var item in propertyIts)
                {
                    this.IPropertys.Add(item);
                }
                propertyIts.Clear();
                propertyIts = null;
                this.scriptName = jObject[IScriptName].ToString();

                foreach (var item in itemboxs)
                {
                    setWhereLoadItemboxs(item);
                }
            }
            return true;
        }

        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public virtual bool SaveToJson(string filename)
        {
            string json = "{\"" + IScriptName + "\":\"" + scriptName + "\",\"" + ItemBoxsListName +
                            "\":" + JsonConvert.SerializeObject(Itemboxs) + ",\"" + IPropertyItsListName + "\":"
                            + JsonConvert.SerializeObject(IPropertys) + "}";
            using (FileStream filestream = File.Open(filename, FileMode.Create))
            {
                byte[] datas = System.Text.Encoding.UTF8.GetBytes(json);
                filestream.Write(datas, 0, datas.Length);
                datas = null;
            }
            return true;
        }
        /// <summary>
        /// 查找属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IPropertyIt FindIPropertyById(string id)
        {
            foreach (var item in iPropertys)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 运行和编译
        /// </summary>
        /// <param name="wrs"></param>
        public virtual Task<bool> RunCompile()
        {
            return IScriptInterpreter.RunScript(this);
        }
        ///// <summary>
        ///// 添加系统的代码块
        ///// </summary>
        //public static TreeData AddSystemBox()
        //{
        //    TreeData system = new TreeData("系统");
        //    ItemBox ifitembox = new ItemBox()
        //    {
        //        BoxType = ItemBoxEnum.IF,
        //        Name = "分支",
        //        TipText = "分支",
        //    };
        //    ifitembox.AddInput(new ParatItem() { Name = "判断的值", PIEnum = ParaItemEnum.BOOL, TipText = "决定分支的走向" });
        //    //ifitembox.Add(new StackItemBox() { Name = "True", BoxType = ItemBoxEnum.ELSE });
        //    //ifitembox.Add(new StackItemBox() { Name = "False", BoxType = ItemBoxEnum.ELSE });

        //    ItemBox whileidx = new ItemBox()
        //    {
        //        BoxType = ItemBoxEnum.WHILE,
        //        Name = "循环(由起始和结束值决定次数)",
        //        TipText = "循环",
        //    };
        //    whileidx.AddInput(new ParatItem()
        //    {
        //        Name = "起始值",
        //        PIEnum = ParaItemEnum.INT,
        //        TipText = "起始值",
        //        Value = 0,
        //    });
        //    whileidx.AddInput(new ParatItem()
        //    {
        //        Name = "终止值",
        //        PIEnum = ParaItemEnum.INT,
        //        TipText = "终止值",
        //        Value = 10,
        //    });

        //    whileidx.AddOutput(new ParatItem()
        //    {
        //        Name = "迭代值",
        //        PIEnum = ParaItemEnum.INT,
        //        TipText = "迭代值"
        //    });

        //    ItemBox whilebool = new ItemBox()
        //    {
        //        BoxType = ItemBoxEnum.WHILE,
        //        Name = "循环(由bool参数决定次数)",
        //        TipText = "循环",
        //    };
        //    whilebool.AddInput(new ParatItem() { Name = "判断的值", PIEnum = ParaItemEnum.BOOL, TipText = "如果是true则继续循环，否则调出循环", Value = true, });

        //    ItemBox whiletime = new ItemBox()
        //    {
        //        BoxType = ItemBoxEnum.WHILE,
        //        Name = "循环(由时间参数决定次数)",
        //        TipText = "循环",
        //    };
        //    whiletime.AddInput(new ParatItem() { Name = "结束时间", PIEnum = ParaItemEnum.DATETIME, TipText = "如果当前事件超过设定的时间则跳出循环", Value = DateTime.Now, });

        //    //ifitembox.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    //whileidx.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    //whilebool.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    //whiletime.ForgeBrush = new SolidColorBrush(Colors.Blue);

        //    ifitembox.LogoPath = ScriptHelp.BrachImage;
        //    whileidx.LogoPath = ScriptHelp.WhileImage;
        //    whilebool.LogoPath = ScriptHelp.WhileImage;
        //    whiletime.LogoPath = ScriptHelp.WhileImage;
        //    system.Children.Add(new TreeData(ifitembox));
        //    system.Children.Add(new TreeData(whileidx));
        //    system.Children.Add(new TreeData(whilebool));
        //    system.Children.Add(new TreeData(whiletime));
        //    return system;
        //}
        ///// <summary>
        ///// 添加功能函数
        ///// </summary>
        ///// <returns></returns>
        //public static TreeData AddToolsFunction()
        //{
        //    TreeData tdheadfordevice = new TreeData("功能函数");
        //    IItemBox box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.DelyTime);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.AddInt);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.AddFloat);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.TimeDesc);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.PrintObject);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.EqualInt);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.EqualDouble);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.GetNowTime);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.SetIntValue);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.SetFloatValue);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    box = ScriptTools.AddMeodthToItemBox<ItemBox, ParatItem>(tdheadfordevice, ScriptToolsFunction.SetBoolValue);
        //    //box.ForgeBrush = new SolidColorBrush(Colors.Blue);
        //    box = null;
        //    return tdheadfordevice;
        //}
        /// <summary>
        /// 停止脚本的运行
        /// </summary>
        public virtual void StopRun()
        {
            IScriptInterpreter.StopScript();
        }
        /// <summary>
        /// 在加载后调整itemboxs的关系
        /// </summary>
        /// <param name="itemBox"></param>
        private void setWhereLoadItemboxs(ItemBox itemBox)
        {

            setWhereLoadProperty(itemBox);

            if (itemBox.Children.Count > 0)
            {
                foreach (var item in itemBox.Children)
                {
                    if(item.ParentId != itemBox.Id)
                    {
                        throw new Exception("节点错误");
                    }
                    item.ParentNode = itemBox;
                    setWhereLoadItemboxs(item);
                }
            }

        }
        /// <summary>
        /// 在加载后设置属性的值
        /// </summary>
        /// <param name="itemBox"></param>
        private void setWhereLoadProperty(ItemBox itemBox)
        {
            foreach (ParatItem item in itemBox.GetInputParatItems())
            {
                if(item.LinkipropertyId.Equals("") == false)
                {
                    item.LinkIProperty = FindIPropertyById(item.LinkipropertyId);
                }
            }
            foreach (ParatItem item in itemBox.GetOutputParatItems())
            {
                if (item.LinkipropertyId.Equals("") == false)
                {
                    item.LinkIProperty = FindIPropertyById(item.LinkipropertyId);
                }
            }
        }
        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        private string scriptName = "";
        /// <summary>
        /// 属性集合
        /// </summary>
        protected ObservableCollection<IPropertyIt> iPropertys = new ObservableCollection<IPropertyIt>();
        /// <summary>
        /// 代码块集合
        /// </summary>
        protected ObservableCollection<ItemBox> itemboxs = new ObservableCollection<ItemBox>();
        /// <summary>
        /// 主任务入口
        /// </summary>
        protected ItemBox mainIb = null;
        /// <summary>
        /// id
        /// </summary>
        private string id = Tools.CreateId();
        #endregion
        #region 访问器
        /// <summary>
        /// 属性
        /// </summary>
        public ObservableCollection<IPropertyIt> IPropertys
        {
            get
            {
                return iPropertys;
            }
        }
        /// <summary>
        /// 代码块集合
        /// </summary>
        public ObservableCollection<ItemBox> Itemboxs
        {
            get
            {
                return itemboxs;
            }
        }
        /// <summary>
        /// 脚本入口
        /// </summary>
        public ItemBox MainIb
        {
            get
            {
                return mainIb;
            }
            set
            {
                mainIb = value;
            }
        }

        /// <summary>
        /// 脚本名称
        /// </summary>
        public string ScriptName
        {
            get
            {
                return scriptName;
            }

            set
            {
                scriptName = value;
                Changed("ScriptName");
            }
        }
        /// <summary>
        /// id
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                Changed("Id");
            }
        }
        #endregion
        #region Static
        /// <summary>
        /// 添加系统的代码块
        /// </summary>
        public static TreeData AddSystemBox()
        {
            TreeData system = new TreeData("系统");
            ItemBox ifitembox = new ItemBox()
            {
                BoxType = ItemBoxEnum.IF,
                Name = "分支",
                TipText = "分支",
            };
            ifitembox.AddInput(new ParatItem() { Name = "判断的值", PIEnum = ParaItemEnum.BOOL, TipText = "决定分支的走向" });
            //ifitembox.Add(new StackItemBox() { Name = "True", BoxType = ItemBoxEnum.ELSE });
            //ifitembox.Add(new StackItemBox() { Name = "False", BoxType = ItemBoxEnum.ELSE });

            ItemBox whileidx = new ItemBox()
            {
                BoxType = ItemBoxEnum.WHILE,
                Name = "循环(由起始和结束值决定次数)",
                TipText = "循环",
            };
            whileidx.AddInput(new ParatItem()
            {
                Name = "起始值",
                PIEnum = ParaItemEnum.INT,
                TipText = "起始值",
                Value = 0,
            });
            whileidx.AddInput(new ParatItem()
            {
                Name = "终止值",
                PIEnum = ParaItemEnum.INT,
                TipText = "终止值",
                Value = 10,
            });

            whileidx.AddOutput(new ParatItem()
            {
                Name = "迭代值",
                PIEnum = ParaItemEnum.INT,
                TipText = "迭代值"
            });

            ItemBox whilebool = new ItemBox()
            {
                BoxType = ItemBoxEnum.WHILE,
                Name = "循环(由bool参数决定次数)",
                TipText = "循环",
            };
            whilebool.AddInput(new ParatItem() { Name = "判断的值", PIEnum = ParaItemEnum.BOOL, TipText = "如果是true则继续循环，否则调出循环", Value = true, });

            ItemBox whiletime = new ItemBox()
            {
                BoxType = ItemBoxEnum.WHILE,
                Name = "循环(由时间参数决定次数)",
                TipText = "循环",
            };
            whiletime.AddInput(new ParatItem() { Name = "结束时间", PIEnum = ParaItemEnum.DATETIME, TipText = "如果当前事件超过设定的时间则跳出循环", Value = DateTime.Now, });


            ifitembox.LogoPath = ScriptHelp.BrachImage;
            whileidx.LogoPath = ScriptHelp.WhileImage;
            whilebool.LogoPath = ScriptHelp.WhileImage;
            whiletime.LogoPath = ScriptHelp.WhileImage;
            system.Children.Add(new TreeData(ifitembox));
            system.Children.Add(new TreeData(whileidx));
            system.Children.Add(new TreeData(whilebool));
            system.Children.Add(new TreeData(whiletime));
            return system;
        }
        #endregion
    }
}
