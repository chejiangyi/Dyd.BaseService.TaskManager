using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Domain.Model;
using XXF.BaseService.TaskManager;
using XXF.BaseService.TaskManager.SystemRuntime;

namespace Dyd.BaseService.TaskManager.Node.SystemRuntime
{
    /// <summary>
    /// 任务运行时信息
    /// </summary>
    public class NodeTaskRuntimeInfo
    {
        /// <summary>
        /// 任务所在的应用程序域
        /// </summary>
        public AppDomain Domain;
        /// <summary>
        /// 任务信息
        /// </summary>
        public tb_task_model TaskModel;
        /// <summary>
        /// 任务当前版本信息
        /// </summary>
        public tb_version_model TaskVersionModel;
        /// <summary>
        /// 应用程序域中任务dll实例引用
        /// </summary>
        public BaseDllTask DllTask;
        /// <summary>
        /// 任务锁机制,用于执行状态的锁定，保证任务单次运行
        /// </summary>
        public TaskLock TaskLock;
    }
}
