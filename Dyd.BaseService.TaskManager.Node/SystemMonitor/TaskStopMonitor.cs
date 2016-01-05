using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Node.SystemRuntime;
using Dyd.BaseService.TaskManager.Node.Tools;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Node.SystemMonitor
{
    /// <summary>
    /// 任务的停止监控者
    /// 用于任务异常停止的检测
    /// </summary>
    public class TaskStopMonitor : BaseMonitor
    {
        public override int Interval
        {
            get
            {
                return 1000 * 60;//1分钟扫描
            }
        }

        private List<int> lastscantaskids = new List<int>();

        protected override void Run()
        {
            List<int> taskids = new List<int>();
            SqlHelper.ExcuteSql(GlobalConfig.TaskDataBaseConnectString, (c) =>
            {
                tb_task_dal taskdal = new tb_task_dal();
                taskids = taskdal.GetTaskIDsByState(c, (int)EnumTaskState.Running,GlobalConfig.NodeID);
            });
            List<int> currentscantaskids = new List<int>();
            foreach (var taskid in taskids)
            {
                try
                {
                    var taskruntimeinfo = TaskPoolManager.CreateInstance().Get(taskid.ToString());
                    if (taskruntimeinfo == null)
                    {
                        currentscantaskids.Add(taskid);
                    }

                    var recovertaskids = (from o in lastscantaskids
                                          from c in currentscantaskids
                                          where o == c
                                          select o).ToList();
                    if (recovertaskids != null && recovertaskids.Count > 0)
                        recovertaskids.ForEach((c) =>
                        {
                            LogHelper.AddTaskError("任务资源运行可能异常停止了", taskid, new Exception("任务处于运行状态，但是相应集群节点中，未发现任务在运行"));
                        });
                    lastscantaskids = currentscantaskids;
                }
                catch (Exception exp)
                {
                    LogHelper.AddNodeError("任务" + taskid + "检测是否异常终止出错", exp);
                }
            }
        }
    }
}
