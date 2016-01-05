using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Web.Models;
using Dyd.BaseService.TaskManager.Web.Tools;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Web.BackgroundTask
{
    public class NodeStateCheckTask:BackgroundTask
    {
        public override void Run()
        {
            List<tb_node_model> models = new List<tb_node_model>();
            SqlHelper.ExcuteSql(Config.TaskConnectString, (c) => {
                Domain.Dal.tb_node_dal dal = new tb_node_dal();
                models = dal.GetAllStopNodesWithNeedCheckState(c);
            });
            foreach (var m in models)
            {
                LogHelper.AddError(new tb_error_model() { errorcreatetime=DateTime.Now, errortype=(int)XXF.BaseService.TaskManager.SystemRuntime.EnumTaskLogType.SystemError,
                    msg=string.Format("当前节点:{0}【{1}】运行已经停止,请及时检查！",m.nodename,m.id),
                    nodeid=m.id, taskid=0 });
            }
        }
    }
}