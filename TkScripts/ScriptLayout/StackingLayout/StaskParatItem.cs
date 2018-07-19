using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TkScripts.Interface;

namespace TkScripts.ScriptLayout.StackingLayout
{
    public class StackParatItem : ParatItem
    {
        private IPropertyIt linkiProperty = null;
        /// <summary>
        /// 连接的属性id
        /// </summary>
        private string linkipropertyId = "";
        /// <summary>
        /// 属性
        /// </summary>
        [JsonIgnore]
        public IPropertyIt LinkIProperty
        {
            get
            {
                return linkiProperty;
            }

            set
            {
                linkiProperty = value;
                Changed("LinkIProperty");
                Changed("LinkPropertyName");
            }
        }
        /// <summary>
        /// 连接的属性id
        /// </summary>
        [ToJson(true)]
        public string LinkipropertyId
        {
            get
            {
                return linkiProperty != null ? linkiProperty.Id :linkipropertyId;
            }

            set
            {
                linkipropertyId = value;
            }
        }
    }
}
