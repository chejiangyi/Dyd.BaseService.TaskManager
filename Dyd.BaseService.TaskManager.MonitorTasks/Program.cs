using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.MonitorTasks
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            TaskManageErrorSendTask task = new TaskManageErrorSendTask();
            task.SystemRuntimeInfo = new XXF.BaseService.TaskManager.SystemRuntime.TaskSystemRuntimeInfo() { TaskConnectString = "server=192.168.17.201;Initial Catalog=dyd_bs_task;User ID=sa;Password=Xx~!@#;" };

            task.TestRun();
        }
    }
}
