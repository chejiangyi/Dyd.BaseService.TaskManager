using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.TaskManager.Node.Tools;

namespace Dyd.BaseService.TaskManager.Node.SystemMonitor
{
    /// <summary>
    /// 基础监控者
    /// </summary>
    public abstract class BaseMonitor
    {
        protected System.Threading.Thread _thread;
        /// <summary>
        /// 监控间隔时间 （毫秒）
        /// </summary>
        public virtual int Interval { get; set; }

        public BaseMonitor()
        {
            _thread = new System.Threading.Thread(TryRun);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void TryRun()
        {
            while (true)
            {
                try
                {
                    Run();
                    System.Threading.Thread.Sleep(Interval);
                }
                catch(Exception exp) 
                {
                    LogHelper.AddNodeError(this.GetType().Name+"监控严重错误",exp);
                }
            }
        }

        /// <summary>
        /// 监控执行方法约定
        /// </summary>
        protected virtual void Run()
        {

        }

    }
}
