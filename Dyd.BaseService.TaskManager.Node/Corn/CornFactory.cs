using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.TaskManager.Core.CustomCorn;
using Dyd.BaseService.TaskManager.Node.SystemRuntime;
using Quartz;

namespace Dyd.BaseService.TaskManager.Node.Corn
{
    public class CornFactory
    {
        public static Trigger CreateTigger(NodeTaskRuntimeInfo taskruntimeinfo)
        {
            if (taskruntimeinfo.TaskModel.taskcron.Contains("["))
            {
                var customcorn = CustomCornFactory.GetCustomCorn(taskruntimeinfo.TaskModel.taskcron);
                customcorn.Parse();
                if (customcorn is SimpleCorn || customcorn is RunOnceCorn)
                {
                    var simplecorn = customcorn as SimpleCorn;
                    // 定义调度触发规则，比如每1秒运行一次，共运行8次
                    SimpleTrigger simpleTrigger = new SimpleTrigger(taskruntimeinfo.TaskModel.id.ToString(), taskruntimeinfo.TaskModel.categoryid.ToString());
                    if (simplecorn.ConInfo.StartTime != null)
                        simpleTrigger.StartTimeUtc = simplecorn.ConInfo.StartTime.Value.ToUniversalTime();
                    //else
                    //    simpleTrigger.StartTimeUtc = DateTime.Now.ToUniversalTime();
                    if (simplecorn.ConInfo.EndTime != null)
                        simpleTrigger.EndTimeUtc = simplecorn.ConInfo.EndTime.Value.ToUniversalTime();
                    if (simplecorn.ConInfo.RepeatInterval != null)
                        simpleTrigger.RepeatInterval = TimeSpan.FromSeconds(simplecorn.ConInfo.RepeatInterval.Value);
                    else
                        simpleTrigger.RepeatInterval = TimeSpan.FromSeconds(1);
                    if (simplecorn.ConInfo.RepeatCount != null)
                        simpleTrigger.RepeatCount = simplecorn.ConInfo.RepeatCount.Value - 1;//因为任务默认执行一次，所以减一次
                    else
                        simpleTrigger.RepeatCount = int.MaxValue;//不填，则默认最大执行次数
                    return simpleTrigger;
                }
                return null;
            }
            else
            {
                CronTrigger trigger = new CronTrigger(taskruntimeinfo.TaskModel.id.ToString(), taskruntimeinfo.TaskModel.categoryid.ToString());// 触发器名,触发器组  
                trigger.CronExpressionString = taskruntimeinfo.TaskModel.taskcron;// 触发器时间设定  
                return trigger;
            }
        }
    }
}
