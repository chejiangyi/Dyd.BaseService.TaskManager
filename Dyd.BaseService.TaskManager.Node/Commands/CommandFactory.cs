using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Domain.Model;

namespace Dyd.BaseService.TaskManager.Node.Commands
{
    /// <summary>
    /// 命令执行工厂
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandInfo"></param>
        public static void Execute(tb_command_model commandInfo)
        {
            string namespacestr = typeof(BaseCommand).Namespace;
            var obj = Assembly.GetAssembly(typeof(BaseCommand)).CreateInstance(namespacestr + "." + commandInfo.commandname.ToString() + "Command", true);
            if (obj != null && obj is BaseCommand)
            {
                var command = (obj as BaseCommand);
                command.CommandInfo = commandInfo;
                command.Execute();
            }
        }
    }
}
