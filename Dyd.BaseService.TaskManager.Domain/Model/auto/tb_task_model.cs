using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// tb_task Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_task_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string taskname { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int categoryid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int nodeid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime taskcreatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime taskupdatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime tasklaststarttime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime tasklastendtime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime tasklasterrortime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int taskerrorcount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int taskruncount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int taskcreateuserid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Byte taskstate { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int taskversion { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string taskappconfigjson { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string taskcron { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string taskmainclassnamespace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taskmainclassdllfilename { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taskremark { get; set; }
        
    }
}