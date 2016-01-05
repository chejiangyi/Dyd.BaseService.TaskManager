using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Node.Corn;
using Quartz;
using Quartz.Impl;

namespace Dyd.BaseService.TaskManager.Node.SystemRuntime
{
    /// <summary>
    /// 全局任务池管理
    /// 管理任务的移入，移除任务运行池
    /// 全局仅一个实例
    /// </summary>
    public class TaskPoolManager:IDisposable
    {
        /// <summary>
        /// 任务运行池
        /// </summary>
        private static Dictionary<string, NodeTaskRuntimeInfo> TaskRuntimePool = new Dictionary<string, NodeTaskRuntimeInfo>();
        /// <summary>
        /// 任务池管理者,全局仅一个实例
        /// </summary>
        private static TaskPoolManager _taskpoolmanager;
        /// <summary>
        /// 任务池管理操作锁标记
        /// </summary>
        private static object _locktag = new object();
        /// <summary>
        /// 任务池执行计划
        /// </summary>
        private static IScheduler _sched;

        /// <summary>
        /// 静态初始化
        /// </summary>
        static TaskPoolManager()
        {
            _taskpoolmanager = new TaskPoolManager();
            ISchedulerFactory sf = new StdSchedulerFactory();
            _sched = sf.GetScheduler();
            _sched.Start();
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public virtual void Dispose()
        {
            if (_sched != null && !_sched.IsShutdown)
                _sched.Shutdown();
        }
        /// <summary>
        /// 获取任务池的全局唯一实例
        /// </summary>
        /// <returns></returns>
        public static TaskPoolManager CreateInstance()
        {
            return _taskpoolmanager;
        }
        /// <summary>
        /// 将任务移入任务池
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="taskruntimeinfo"></param>
        /// <returns></returns>
        public bool Add(string taskid, NodeTaskRuntimeInfo taskruntimeinfo)
        {
            lock (_locktag)
            {
                if (!TaskRuntimePool.ContainsKey(taskid))
                {
                    JobDetail jobDetail = new JobDetail(taskruntimeinfo.TaskModel.id.ToString(), taskruntimeinfo.TaskModel.categoryid.ToString(), typeof(TaskJob));// 任务名，任务组，任务执行类  
                    var trigger = CornFactory.CreateTigger(taskruntimeinfo);
                    _sched.ScheduleJob(jobDetail, trigger);  

                    TaskRuntimePool.Add(taskid, taskruntimeinfo);
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 将任务移出任务池
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public bool Remove(string taskid)
        {
            lock (_locktag)
            {
                if (TaskRuntimePool.ContainsKey(taskid))
                {
                    var taskruntimeinfo = TaskRuntimePool[taskid];
                    _sched.PauseTrigger(taskruntimeinfo.TaskModel.id.ToString(), taskruntimeinfo.TaskModel.categoryid.ToString());// 停止触发器  
                    _sched.UnscheduleJob(taskruntimeinfo.TaskModel.id.ToString(), taskruntimeinfo.TaskModel.categoryid.ToString());// 移除触发器  
                    _sched.DeleteJob(taskruntimeinfo.TaskModel.id.ToString(), taskruntimeinfo.TaskModel.categoryid.ToString());// 删除任务

                    TaskRuntimePool.Remove(taskid);
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 获取任务池中任务运行时信息
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public NodeTaskRuntimeInfo Get(string taskid)
        {
            if (!TaskRuntimePool.ContainsKey(taskid))
            {
                return null;
            }
            lock (_locktag)
            {
                if (TaskRuntimePool.ContainsKey(taskid))
                {
                    return TaskRuntimePool[taskid];
                }
                return null;
            }
        }
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        public List<NodeTaskRuntimeInfo> GetList()
        {
            return TaskRuntimePool.Values.ToList();
        }
    }
}
