using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using TkScripts.Interface;
using TkScripts.ScriptLayout.StackingLayout;
using TKScriptsServer.Agreement;
using TKScriptsServer.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TkScripts.Script
{
    /// <summary>
    /// STACK脚本解析类
    /// </summary>
    public class StackScriptOption : IScriptInterpreter
    {
        /// <summary>
        /// 和服务通讯
        /// </summary>
        private static readonly ScriptClient ScriptClient = new ScriptClient();
        /// <summary>
        /// 用于线程阻塞公共资源
        /// </summary>
        private AutoResetEvent threadAre = new AutoResetEvent(false);
        /// <summary>
        /// 是否是调试模式
        /// </summary>
        private bool isDebugMode = false;
        /// <summary>
        /// 是否终止运行
        /// </summary>
        private bool isStop = false;
        /// <summary>
        /// 脚本运行线程
        /// </summary>
        private Task<bool> scriptRunThread = null;
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override object GetValue(IParatItem item)
        {
            ParatItem ipt = item as ParatItem;
            if(ipt.LinkIProperty != null)
            {
                return manager.GetValue(ipt.LinkIProperty.Id).GetValue(ipt.LinkIProperty.Name);
            }
            else
            {
                return ipt.Value;
            }
        }
        /// <summary>
        /// 在内存管理中设置属性的值
        /// </summary>
        /// <param name="item"></param>
        protected override void SetValue(IParatItem item)
        {
            ParatItem ipt = item as ParatItem;
            if (ipt.LinkIProperty != null)
            {
                manager.GetValue(ipt.LinkIProperty.Id).SetValue(ipt.LinkIProperty.Name, ipt.Value);
            }
        }
        /// <summary>
        /// 获取whilebox的迭代值
        /// </summary>
        /// <param name="whilebox"></param>
        /// <param name="startidx"></param>
        /// <returns></returns>
        protected int GetWhileBoxIdx(ItemBox whilebox, int startidx)
        {
            int returnindex = 0;
            ScriptOutput soutidx = manager.GetValue(whilebox.Id);
            if (soutidx == null)
            {
                ScriptOutput whileboxidx = new ScriptOutput();
                whileboxidx.SetValue("Idx", startidx);
                manager.SetValue(whilebox.Id, whileboxidx);
                returnindex = startidx;
            }
            else
            {
                int index = (int)soutidx.GetValue("Idx");
                manager.GetValue(whilebox.Id).SetValue("Idx", index++);
                returnindex = index++;
                soutidx.SetValue("Idx", returnindex);
            }
            whilebox.OutDatas[0].Value = returnindex;
            SetValue(whilebox.OutDatas[0] as ParatItem);
            return returnindex;
        }
        /// <summary>
        /// 获取函数的输入参数数据
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        protected ScriptInput GetFunctionInput(ItemBox box)
        {
            ScriptInput si = new ScriptInput();
            foreach (var item in box.InputDatas)
            {
                if (item.PIEnum != ParaItemEnum.INPUT)
                {
                    si.SetValue(item.Name, GetValue(item as ParatItem));
                }
            }
            return si;
        }
        /// <summary>
        /// 执行准确的函数
        /// </summary>
        /// <param name="box"></param>
        protected void Dofunction(ItemBox box, WriteStreamCallBack wrs, IScriptLayout ml)
        {
            if(isStop)
            {
                throw new Exception("主动停止了脚本!");
            }
            ml.SetFunctionBoxRun(box);
            ScriptInput si = new ScriptInput();
            foreach (var item in box.InputDatas)
            {
                if(item.PIEnum != ParaItemEnum.INPUT)
                {
                    si.SetValue(item.Name, GetValue(item as ParatItem));
                }
            }
            
            ScriptOutput so = RunScript(box, si);
            //ScriptOutput so = null;             ////////////////////////////////////待修改
            if (so != null)
            {
                if(so.LogMessage != null && so.LogMessage.Equals("") == false)
                    wrs?.Invoke(so.LogMessage);
                foreach (var item in box.OutDatas)
                {
                    if(item.PIEnum != ParaItemEnum.OUTPUT)
                    {
                        item.Value = so.GetValue(item.Name);
                        SetValue(item as ParatItem);
                    }
                }
                
            }
            //检测是否需要断点停止
            checkScriptRunForStopPoint(box, si, null);
            if (so != null)
            {
                so.Dispose();
                so = null;
            }
            si.Dispose();
            si = null;
            ml.SetFunctionBoxStop(box);
        }
        /// <summary>
        /// 执行if框
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        protected void DoIfFunction(ItemBox box, WriteStreamCallBack wrs, IScriptLayout ml)
        {
            ScriptInput si = new ScriptInput();
            foreach (var item in box.InputDatas)
            {
                if (item.PIEnum != ParaItemEnum.INPUT)
                {
                    si.SetValue(item.Name, GetValue(item as ParatItem));
                }
            }

            //检测是否需要断点停止
            checkScriptRunForStopPoint(box, si, null);

            if (Convert.ToBoolean(si.GetFirst()))
            {
                RunningFunction((box as ItemBox).Children[0], wrs, ml);
            }
            else
            {
                RunningFunction((box as ItemBox).Children[1], wrs, ml);
            }
            si.Dispose();
            si = null;
        }
        /// <summary>
        /// 做循环
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        protected void DoWhileFunction(ItemBox box, WriteStreamCallBack wrs, IScriptLayout ml)
        {
            ItemBox sbox = box as ItemBox;
            if (box.InputDatas.Count == 1)
            {
                if (box.InputDatas[0].PIEnum == ParaItemEnum.BOOL)
                {
                    ScriptInput si = GetFunctionInput(box);
                    //检测是否需要断点停止
                    checkScriptRunForStopPoint(box, si, null);
                    while (true)
                    {
                        if (Convert.ToBoolean(si.GetFirst()))
                        {
                            RunningFunction(box, wrs, ml);
                        }
                        else
                        {
                            return;
                        }
                        si.Dispose();
                        si = null;
                        si = GetFunctionInput(box);
                    }
                }
                else if(box.InputDatas[0].PIEnum == ParaItemEnum.DATETIME)
                {
                    ScriptInput si = GetFunctionInput(box);
                    //检测是否需要断点停止
                    checkScriptRunForStopPoint(box, si, null);
                    while (true)
                    {
                        if (Convert.ToDateTime(si.GetFirst()) > DateTime.Now)
                        {
                            RunningFunction(box, wrs, ml);
                        }
                        else
                        {
                            return;
                        }
                        si.Dispose();
                        si = null;
                        si = GetFunctionInput(box);
                    }
                }
            }
            else
            {
                ScriptInput si = GetFunctionInput(box);
                while (true)
                {
                    int idxs = Convert.ToInt32(si.GetFirst());
                    int idxe = Convert.ToInt32(si.GetSecond());
                    int boxidx = GetWhileBoxIdx(sbox, idxs);
                    //if (box.IsHasBreakPoint)
                    //{
                    //    ScriptOutput so = new ScriptOutput();
                    //    so.SetValue("Idx", boxidx);
                    //    ScriptBreakPoint?.Invoke(box, si, so);
                    //}
                    //检测是否需要断点停止
                    checkScriptRunForStopPoint(box, si, null);

                    if (boxidx > idxe)
                    {
                        return;
                    }
                    else
                    {
                        RunningFunction(box, wrs, ml);
                    }
                    si.Dispose();
                    si = null;
                    si = GetFunctionInput(box);
                }
            }
            
        }
        /// <summary>
        /// 选中执行的函数
        /// </summary>
        /// <param name="box"></param>
        /// <param name="wrs"></param>
        /// <param name="ml"></param>
        protected void RunningFunction(IItemBox box, WriteStreamCallBack wrs, IScriptLayout ml)
        {
            foreach (var currentbox in (box as ItemBox).Children)
            {
                ml.SetFunctionBoxRun(currentbox);
                if (currentbox.BoxType == ItemBoxEnum.FUNCTION)
                {
                    Dofunction(currentbox, wrs, ml);
                }
                else if (currentbox.BoxType == ItemBoxEnum.IF)
                {
                    DoIfFunction(currentbox, wrs, ml);
                }
                else if (currentbox.BoxType == ItemBoxEnum.WHILE)
                {
                    DoWhileFunction(currentbox, wrs, ml);
                }
                ml.SetFunctionBoxStop(currentbox);
            }
        }
        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="m"></param>
        public override Task<bool> RunScript(IScriptLayout m)
        {
            if (scriptRunThread != null && scriptRunThread.IsCompleted == false) return null;

            scriptRunThread = new Task<bool>((obj) =>
            {
                IsRunning = true;
                IScriptLayout ml = obj as IScriptLayout;
                WriteStreamCallBack wrs = ml.ComipleMessageCall;

                try
                {
                    isStop = false;
                    //初始化flag
                    isDebugMode = false;
                    Init(ml);
                    wrs?.Invoke("赋值属性完成,开始执行脚本");
                    foreach (var currentbox in ml.Itemboxs)
                    {
                        //RunningFunction(currentbox, wrs, ml);
                        //ml.SetFunctionBoxRun(Colors.Red, currentbox);
                        if (currentbox.BoxType == ItemBoxEnum.FUNCTION)
                        {
                            Dofunction(currentbox, wrs, ml);
                        }
                        else if (currentbox.BoxType == ItemBoxEnum.IF)
                        {
                            DoIfFunction(currentbox, wrs, ml);
                        }
                        else if (currentbox.BoxType == ItemBoxEnum.WHILE)
                        {
                            DoWhileFunction(currentbox, wrs, ml);
                        }
                        //ml.SetFunctionBoxStop(Colors.White, currentbox);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    wrs?.Invoke("脚本运行失败");
                    Log.ScriptLog.Log.Write(new LogMessage("脚本运行失败", ex));
                    return false;
                }
                finally
                {
                    manager.Clear();
                    wrs?.Invoke("程序运行结束");
                    IsRunning = false;
                }
            }, m);
            scriptRunThread.Start();
            return scriptRunThread;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init(IScriptLayout ml)
        {
            if (ml.IPropertys.Count > 0)
            {
                foreach (var item in ml.IPropertys)
                {
                    ScriptOutput so = new ScriptOutput();
                    so.SetValue(item.Name, item.Value);
                    manager.SetValue(item.Id, so);
                }
            }
        }

        
        /// <summary>
        /// 脚本检测是否断点
        /// </summary>
        /// <param name="item"></param>
        /// <param name="si"></param>
        /// <param name="so"></param>
        protected void checkScriptRunForStopPoint(IItemBox item, ScriptInput si, ScriptOutput so)
        {
            if (item.IsHasBreakPoint || isDebugMode)
            {
                ScriptBreakPoint?.Invoke(item, si, null);
                runStop();
            }
        }
        /// <summary>
        /// 脚本阻塞等待
        /// </summary>
        protected void runStop()
        {
            threadAre.WaitOne();
        }
        /// <summary>
        /// 运行到下一个函数
        /// </summary>
        public override void RunNextFunction()
        {
            if (scriptRunThread == null || scriptRunThread.IsCompleted) return;
            threadAre.Set();
            isDebugMode = true;
        }
        /// <summary>
        /// 继续运行到下一个断点
        /// </summary>
        public override void RunOver()
        {
            if (scriptRunThread == null || scriptRunThread.IsCompleted) return;
            threadAre.Set();
            isDebugMode = false;
        }

        /// <summary>
        /// 运行带url的函数
        /// </summary>
        /// <param name="itemBox"></param>
        /// <param name="scriptInput"></param>
        /// <returns></returns>

        private ScriptOutput RunScript(IItemBox itemBox, ScriptInput scriptInput)
        {
            ScriptRequest?.Invoke(scriptInput, itemBox.Name);
            string json = ScriptClient.PostStringAsync(itemBox.ScriptUrl, (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(scriptInput)));
            ScriptOutput scriptOutput = JsonConvert.DeserializeObject<ScriptOutput>(json);
            
            ScriptReponse?.Invoke(scriptOutput, itemBox.Name);
            if (scriptOutput != null && scriptOutput.IsExecption)
            {
                throw new Exception("服务器给出停止信号!");
            }
            return scriptOutput;
        }
        /// <summary>
        /// 终止脚本的运行
        /// </summary>
        public override void StopScript()
        {
            this.isStop = true;
        }
    }
}
