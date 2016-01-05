using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXF.BaseService.TaskManager.model;

namespace XXF.BaseService.TaskManager.SystemRuntime
{
    /// <summary>
    /// 任务系统运行时信息
    /// </summary>
    [Serializable]
    public class TaskSystemRuntimeInfo
    {
        /// <summary>
        /// 任务数据库连接字符串
        /// </summary>
        public string TaskConnectString { get; set; }
        /// <summary>
        /// 任务信息
        /// </summary>
        public tb_task_model TaskModel { get; set; }
    }
}
