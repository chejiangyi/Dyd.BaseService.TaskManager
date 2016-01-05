using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// tb_version Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_version_model
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
        public int version { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime versioncreatetime { get; set; }
        
        /// <summary>
        /// 压缩文件二进制文件
        /// </summary>
        public byte[] zipfile { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string zipfilename { get; set; }
        
    }
}