using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Node;
using XXF.Api;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.WinService
{
    public class NodeService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("NodeID"))
                {
                    GlobalConfig.NodeID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NodeID"]);
                }
                if (string.IsNullOrWhiteSpace(GlobalConfig.TaskDataBaseConnectString) || GlobalConfig.NodeID <= 0)
                {
                    string url = GlobalConfig.TaskManagerWebUrl.TrimEnd('/') + "/OpenApi/" + "GetNodeConfigInfo/";
                    ClientResult r = ApiHelper.Get(url, new
                    {

                    });
                    if (r.success == false)
                    {
                        throw new Exception("请求" + url + "失败,请检查配置中“任务调度平台站点url”配置项");
                    }

                    dynamic appconfiginfo = ApiHelper.Data(r);
                    string connectstring = appconfiginfo.TaskDataBaseConnectString;
                    appconfiginfo.TaskDataBaseConnectString = StringDESHelper.DecryptDES(connectstring, "dyd88888888");

                    if (string.IsNullOrWhiteSpace(GlobalConfig.TaskDataBaseConnectString))
                        GlobalConfig.TaskDataBaseConnectString = appconfiginfo.TaskDataBaseConnectString;
                    if (GlobalConfig.NodeID <= 0)
                        GlobalConfig.NodeID = appconfiginfo.NodeID;
                }

                XXF.Common.IOHelper.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + GlobalConfig.TaskSharedDllsDir + @"\");
                CommandQueueProcessor.Run();

                //注册后台监控
                GlobalConfig.Monitors.Add(new Dyd.BaseService.TaskManager.Node.SystemMonitor.TaskRecoverMonitor());
                GlobalConfig.Monitors.Add(new Dyd.BaseService.TaskManager.Node.SystemMonitor.TaskPerformanceMonitor());
                GlobalConfig.Monitors.Add(new Dyd.BaseService.TaskManager.Node.SystemMonitor.NodeHeartBeatMonitor());
                GlobalConfig.Monitors.Add(new Dyd.BaseService.TaskManager.Node.SystemMonitor.TaskStopMonitor());

                Node.Tools.LogHelper.AddNodeLog("节点windows服务启动成功");
            }
            catch (Exception exp)
            {
                Node.Tools.LogHelper.AddNodeError("节点windows服务启动失败", exp);
            }
        }

        protected override void OnStop()
        {
            Node.Tools.LogHelper.AddNodeLog("节点windows服务停止");
        }
    }
}
