using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXF.BaseService.TaskManager.SystemRuntime;

namespace Dyd.BaseService.TaskManager.Demo
{
    /// <summary>
    /// 仅用于任务本地调试使用
    /// 需要将项目配置为->控制台应用程序
    /// </summary>
    public class Program
    {
        [STAThread]
        static void Main()
        {
            DemoTask task = new DemoTask();

            task.TestRun();
        }
    }
}
