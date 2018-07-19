using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TkScripts
{

    public enum ItemBoxEnum : int
    {
        [Description("获取属性的值")]
        GET,
        [Description("设置属性的值")]
        SET,
        [Description("函数")]
        FUNCTION,
        [Description("没有流程的函数,只获取值")]
        GETFUNCTION,
        [Description("脚本起始块")]
        MAIN,
        [Description("分支")]
        IF,
        [Description("分支")]
        ELSE,
        [Description("循环")]
        WHILE,
    }


    public enum ParaItemEnum : int
    {
        [Description("整形参数")]
        INT,
        [Description("浮点型参数")]
        FLOAT,
        [Description("布尔类型")]
        BOOL,
        [Description("字符串")]
        STRING,
        [Description("时间")]
        DATETIME,
        [Description("对象参数")]
        OBJECT,
        [Description("点")]
        POINT,
        [Description("枚举参数")]
        ENUM,
        [Description("空连接类型")]
        NULL,
        [Description("输入")]
        INPUT,
        [Description("输出")]
        OUTPUT,
    }
}
