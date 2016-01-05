using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// 发送时使用的错误信息model
    /// </summary>
    public class tb_senderror_model
    {
        /// <summary>
        /// 基础错误模型
        /// </summary>
        public tb_error_model error_model{get;set;}
        /// <summary>
        /// 任务创建用户id
        /// </summary>
        public int taskcreateuserid { get; set; }
        /// <summary>
        /// 任务名
        /// </summary>
        public string taskname { get; set; }
    }

    
}
