using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Domain.Dal;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.MonitorTasks
{
    /// <summary>
    /// 任务调度平台之长时间运行的任务终止-监控任务
    /// </summary>
    public class TaskManageLongRunningKillTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        /// <summary>
        /// 长时间运行任务的检测时间
        /// </summary>
        private int maxlongrunningtime = 1000 * 60 * 20;

        public override void Run()
        {
            //扫描
            SqlHelper.ExcuteSql(this.SystemRuntimeInfo.TaskConnectString, (c) =>
            {
                tb_task_dal taskdal = new tb_task_dal();
                var taskmodels = taskdal.GetLongRunningTaskIDs(c, maxlongrunningtime);
                foreach (var model in taskmodels)
                {
                    this.OpenOperator.Error("检测到任务运行超时,任务id:" + model.id + " 任务标题:" + model.taskname, new Exception());
                }
            });
        }

        public override void TestRun()
        {
            //this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            //this.AppConfig.Add("TaskDataBaseConnectString", "");

            base.TestRun();
        }

    }
}
