using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Node.SystemRuntime;
using Dyd.BaseService.TaskManager.Node.Tools;
using XXF.Log;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Node.SystemMonitor
{
    /// <summary>
    /// 任务的回收监控者
    /// 用于任务异常卸载的资源回收
    /// </summary>
    public class TaskRecoverMonitor : BaseMonitor
    {
        public override int Interval
        {
            get
            {
                return 1000*60;//1分钟扫描
            }
        }

        private List<int> lastscantaskids = new List<int>();

        protected override void Run()
        {
            List<int> taskids = new List<int>();
            SqlHelper.ExcuteSql(GlobalConfig.TaskDataBaseConnectString, (c) =>
                {
                    tb_task_dal taskdal = new tb_task_dal();
                    taskids = taskdal.GetTaskIDsByState(c, (int)EnumTaskState.Stop,GlobalConfig.NodeID);
                });
            List<int> currentscantaskids = new List<int>();
            foreach (var taskid in taskids)
            {
                try
                {
                    var taskruntimeinfo = TaskPoolManager.CreateInstance().Get(taskid.ToString());
                    if (taskruntimeinfo != null)
                    {
                        currentscantaskids.Add(taskid);
                    }

                    var recovertaskids = (from o in lastscantaskids
                                          from c in currentscantaskids
                                          where o == c
                                          select o).ToList();
                    if (recovertaskids != null && recovertaskids.Count > 0)
                        recovertaskids.ForEach((c) => {
                            LogHelper.AddTaskError("任务资源运行异常,可能需要手动卸载",taskid,new Exception("任务处于停止状态，但是相应集群节点中，发现任务存在在运行池中未释放"));
                        });
                    lastscantaskids = currentscantaskids;
                }
                catch (Exception exp)
                {
                    LogHelper.AddNodeError("任务"+taskid+"资源回收出错", exp);
                }
            }
        }
    }
}
