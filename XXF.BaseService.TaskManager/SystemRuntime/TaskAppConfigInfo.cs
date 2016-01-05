using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace XXF.BaseService.TaskManager.SystemRuntime
{
    /// <summary>
    /// 任务配置信息，类似app.config中的配置，仅支持字典
    /// 正式环境在任务调度平台中配置获取
    /// 测试环境需要自己重新创建实例赋值
    /// </summary>
    [Serializable]
    public class TaskAppConfigInfo:Dictionary<string,string>
    {
        public TaskAppConfigInfo():base()
        { }

        //父类实现了ISerializable接口的，子类也必须有序列化构造函数，否则反序列化时会出错。
        protected TaskAppConfigInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
 
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
