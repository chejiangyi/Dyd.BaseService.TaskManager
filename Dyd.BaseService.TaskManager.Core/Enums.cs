using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.Core
{
    /// <summary>
    /// 任务命令名称
    /// </summary>
    public enum EnumTaskCommandName
    {
        [Description("关闭任务")]
        StopTask=0,
        [Description("开启任务")]
        StartTask=1,
        [Description("重启任务")]
        ReStartTask=2,
        [Description("卸载任务")]
        UninstallTask=3,
    }
    /// <summary>
    /// 任务命令状态
    /// </summary>
    public enum EnumTaskCommandState
    {
        [Description("未执行")]
        None=0,
        [Description("执行错误")]
        Error=1,
        [Description("成功执行")]
        Success=2
    }
    /// <summary>
    /// 任务执行状态
    /// </summary>
    public enum EnumTaskState
    {
        [Description("停止")]
        Stop=0,
        [Description("运行中")]
        Running=1,
    }
    /// <summary>
    /// 系统用户角色
    /// </summary>
    public enum EnumUserRole
    {
        [Description("管理员")]
        Admin=0,
        [Description("开发人员")]
        Developer=1,
        [Description("无控制")]
        None = -1
    }
}
