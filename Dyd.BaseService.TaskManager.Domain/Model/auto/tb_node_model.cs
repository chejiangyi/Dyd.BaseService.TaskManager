using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// tb_node Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_node_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string nodename { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime nodecreatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string nodeip { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime nodelastupdatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool ifcheckstate { get; set; }
        
    }
}