using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XXF.BaseService.TaskManager.SystemRuntime
{
    /// <summary>
    /// 任务锁机制
    /// 双重锁保护
    /// </summary>
    public class TaskLock
    {
        //处理锁机制
        private bool _ifrunning = false;
        private object _firstlockTag = new object();//用于锁_ifrunning变量
        private object _twolock = new object();
        //内部加锁
        private bool TryToLockSingleInstance()
        {
            if (_ifrunning == true)
                return false;
            lock (_firstlockTag)
            {
                if (_ifrunning == true)
                    return false;
                else
                {
                    _ifrunning = true;
                    return true;
                }
            }
        }
        //内部释放锁
        private void EndToLockSingleInstance()
        {
            _ifrunning = false;
        }

        public void Invoke(Action action)
        {
            //上次未结束，不再触发,仅运行一次实例
            if (this.TryToLockSingleInstance())
            {
                try
                {
                    lock (_twolock)
                    {
                        action();
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    EndToLockSingleInstance();
                }
            }
        }
    }
}
