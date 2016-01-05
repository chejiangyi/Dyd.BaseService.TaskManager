using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace XXF.BaseService.TaskManager.SystemRuntime
{
    /// <summary>
    /// 任务安全卸载类
    /// </summary>
    public class TaskSafeDisposeOperator
    {
        private int MaxWaitTime = 5;//秒
        private const int CheckStateTime = 1;//秒

        public TaskDisposedState DisposedState =  TaskDisposedState.None;

        public TaskSafeDisposeOperator(int maxwaittime=5)
        {
            MaxWaitTime = maxwaittime;
            DisposedState = TaskDisposedState.None;
        }
        /// <summary>
        /// 阻塞等待资源释放标识,若DisposedState=Finished,则终止等待;若超时,则报错
        /// </summary>
        public void WaitDisposeFinished()
        {
            int count = 0;
            while ((count * CheckStateTime) < MaxWaitTime)
            {
                if(DisposedState == TaskDisposedState.Finished)
                    return;
                System.Threading.Thread.Sleep(CheckStateTime*1000);
                count++;
            }
            throw new TaskSafeDisposeTimeOutException();
        }
    }
    /// <summary>
    /// 任务当前资源释放状态
    /// </summary>
    public enum TaskDisposedState
    {
        [Description("未开始")]
        None,
        [Description("正在释放")]
        Disposing,
        [Description("释放完毕")]
        Finished,
    }
    /// <summary>
    /// 任务资源安全释放超时错误
    /// </summary>
    [Serializable]
    public class TaskSafeDisposeTimeOutException:Exception
    {
        public TaskSafeDisposeTimeOutException()
            : base("任务终止时,资源未释放超时。请检查代码是否在检测到任务处于DisposedState=Disposing时,释放任务当前资源,并终止任务继续运行业务代码,并将DisposedState=Finished")
        {
 
        }

         //父类实现了ISerializable接口的，子类也必须有序列化构造函数，否则反序列化时会出错。
        protected TaskSafeDisposeTimeOutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
