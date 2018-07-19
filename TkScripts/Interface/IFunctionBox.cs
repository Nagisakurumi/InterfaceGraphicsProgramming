using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TkScripts.Interface
{
    public class IFunctionBox : MLUIMain
    {

        protected IItemBox ibox = null;
        /// <summary>
        /// 代码内容
        /// </summary>
        public IItemBox Ibox
        {
            get
            {
                return ibox;
            }
        }
        /// <summary>
        /// 输入的元素集合
        /// </summary>
        public virtual UIElementCollection Inputs
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 输出元素集合
        /// </summary>
        public virtual UIElementCollection Outputs
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 所有的输入参数
        /// </summary>
        public virtual ObservableCollection<IParatItem> InputParamters
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 所有的输出参数
        /// </summary>
        public virtual ObservableCollection<IParatItem> OutputParamters
        {
            get
            {
                return null;
            }
        }
        public virtual void InitFunction()
        {

        }
        /// <summary>
        /// 添加一个输入
        /// </summary>
        /// <param name="ipt"></param>
        public virtual void AddInputData(IParatItem ipt)
        {
            
        }

        /// <summary>
        /// 添加一个输出
        /// </summary>
        /// <param name="ipt"></param>
        public virtual void AddOutputData(IParatItem ipt)
        {
            
        }
        /// <summary>
        /// 设置函数处于运行
        /// </summary>
        /// <param name="color"></param>
        public void SetFunctionBoxRun(Color color)
        {
            this.Dispatcher.Invoke(new Action<Color>((c) => {
                ibox.BoxBrush = null;
                ibox.BoxBrush = new SolidColorBrush(c);
            }), color);
        }
        /// <summary>
        /// 设置函数没有处于运行
        /// </summary>
        /// <param name="color"></param>
        public void SetFunctionBoxStop(Color color)
        {
            this.Dispatcher.Invoke(new Action<Color>((c) => {
                ibox.BoxBrush = null;
                ibox.BoxBrush = new SolidColorBrush(c);
            }), color);
        }
    }
}
