using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Domain.Dal;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Node.SystemMonitor
{
    /// <summary>
    /// 节点心跳监控者
    /// 用于心跳通知数据库当前节点状态
    /// </summary>
    public class NodeHeartBeatMonitor:BaseMonitor
    {
        public override int Interval
        {
            get
            {
                return 5000;
            }
        }
        protected override void Run()
        {
            SqlHelper.ExcuteSql(GlobalConfig.TaskDataBaseConnectString, (c) =>
            {
                var sqldatetime = c.GetServerDate();
                tb_node_dal nodedal = new tb_node_dal();
                nodedal.AddOrUpdate(c, new Domain.Model.tb_node_model()
                {
                    nodecreatetime = sqldatetime, 
                    nodeip=System.Net.Dns.GetHostName(),
                    nodelastupdatetime = sqldatetime, 
                    nodename="新增节点", 
                    id=GlobalConfig.NodeID});
            });
            
        }
    }
}
