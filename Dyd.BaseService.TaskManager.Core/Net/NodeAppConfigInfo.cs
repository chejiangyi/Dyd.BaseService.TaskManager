using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.Core.Net
{
    /// <summary>
    /// 集群节点配置信息
    /// </summary>
    public class NodeAppConfigInfo
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 任务数据库连接
        /// </summary>
        public string TaskDataBaseConnectString { get; set; }
    }
}
