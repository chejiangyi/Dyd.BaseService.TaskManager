using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    public class tb_errorinfo_model
    {
        /// <summary>
        /// 基础错误模型
        /// </summary>
        public tb_error_model error_model{get;set;}
        /// <summary>
        /// 任务名
        /// </summary>
        public string taskname { get; set; }
        public string taskcreateusername { get; set; }
        public string tasknodename { get; set; }
    }
}
