using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace XXF.BaseService.TaskManager.model
{
    /// <summary>
    /// tb_log Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_log_model
    {
        /*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Byte logtype { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime logcreatetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taskid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int nodeid { get; set; }

    }
}