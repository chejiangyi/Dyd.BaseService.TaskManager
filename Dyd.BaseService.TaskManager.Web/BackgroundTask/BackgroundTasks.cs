using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XXF.Log;

namespace Dyd.BaseService.TaskManager.Web.BackgroundTask
{
    /// <summary>
    /// 后台任务集合
    /// 单例模式
    /// </summary>
    public class BackgroundTasks
    {
        private static List<BackgroundTask> Tasks = new List<BackgroundTask>();
        private static System.Threading.Thread _thread = null;
        private static object _lock = new object();
        private static BackgroundTasks _backgroundtask = new BackgroundTasks();

        private BackgroundTasks()
        {
            try
            {
                //注册任务
                Tasks.Add(new NodeStateCheckTask());
                _thread = new System.Threading.Thread(Run);
                _thread.Start();
            }
            catch (Exception exp)
            {
                ErrorLog.Write("后台任务-初始化出错:" + exp.Message, exp);
            }
        }
        /// <summary>
        /// 单例启动后台任务
        /// </summary>
        /// <returns></returns>
        public static BackgroundTasks Create()
        {
            lock (_lock)
            {
                if (_backgroundtask == null)
                    _backgroundtask = new BackgroundTasks();
                return _backgroundtask;
            }
        }

        private static void Run()
        {
            while (true)
            {
                foreach (var t in Tasks)
                {
                    try
                    {
                        t.Run();
                    }
                    catch (Exception exp)
                    {
                        ErrorLog.Write("后台任务-" + t.GetType() + ":" + exp.Message, exp);
                    }
                }
                //5分钟一次
                System.Threading.Thread.Sleep(1000 * 60 * 5);
            }
        }
    }
    /// <summary>
    /// 单个后台任务基类
    /// </summary>
    public class BackgroundTask
    {
        public virtual void Run()
        {

        }
    }
}