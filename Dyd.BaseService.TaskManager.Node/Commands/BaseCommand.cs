using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Domain.Model;

namespace Dyd.BaseService.TaskManager.Node.Commands
{
    /// <summary>
    /// 基础任务命令
    /// </summary>
    public abstract class BaseCommand
    {
        /// <summary>
        /// 任务信息model
        /// </summary>
        public tb_command_model CommandInfo { get; set; }

        /// <summary>
        /// 命令执行方法约定
        /// </summary>
        public virtual void Execute()
        {

        }
    }


}
