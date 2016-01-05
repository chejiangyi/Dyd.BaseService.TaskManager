using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// tb_tempdata Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_tempdata_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int taskid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string tempdatajson { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime tempdatalastupdatetime { get; set; }
        
    }
}