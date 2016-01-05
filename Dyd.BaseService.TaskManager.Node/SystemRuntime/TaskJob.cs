using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Node.Tools;
using Quartz;
using XXF.Log;

namespace Dyd.BaseService.TaskManager.Node.SystemRuntime
{
    /// <summary>
    /// 通用任务的回调job
    /// </summary>
    public class TaskJob:IJob
    {
   
        public void Execute(JobExecutionContext context)
        {
            try
            {
                int taskid = Convert.ToInt32(context.JobDetail.Name);
                var taskruntimeinfo = TaskPoolManager.CreateInstance().Get(taskid.ToString());
                if (taskruntimeinfo == null || taskruntimeinfo.DllTask == null)
                {
                    LogHelper.AddTaskError("当前任务信息为空引用", taskid, new Exception());
                    return;
                }
                taskruntimeinfo.TaskLock.Invoke(() => {
                    try
                    {
                        taskruntimeinfo.DllTask.TryRun();
                    }
                    catch (Exception exp)
                    {
                        LogHelper.AddTaskError("任务" + taskid + "TaskJob回调时执行失败", taskid, exp);
                    }
                });
               
            }
            catch (Exception exp)
            {
                LogHelper.AddNodeError("任务回调时严重系统级错误", exp);
            }
        }
    }
}
