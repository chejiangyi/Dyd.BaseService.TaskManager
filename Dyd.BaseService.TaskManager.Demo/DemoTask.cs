using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.Demo
{
    /// <summary>
    /// 任务调度平台 任务插件的写法demo及说明
    /// </summary>
    public class DemoTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        /// <summary>
        /// 任务调度平台根据发布的任务时间配置，定时回调运行方法
        /// 开发人员的任务插件必须要重载并该方法
        /// </summary>
        public override void Run()
        {
            /* 
             * this.OpenOperator 用于任务调度平台提供给第三方使用的所有api接口封装
             */

            /*获取当前任务dll安装目录*/
            this.OpenOperator.GetTaskInstallDirectory();

            /*打印一条日志到任务调度平台,因为日志会存到平台数据库，所以日志要精简，对任务出错时有分析价值【注意：不要频繁打印无用的，非必要的，对分析无价值的日志信息】*/
            this.OpenOperator.Log("这里打印一条日志到任务调度平台");

            /*打印一条错误到任务调度平台,因为日志会存到平台数据库，所以日志要精简，对任务出错时有分析价值【注意：不要频繁打印无用的，非必要的，对分析无价值的日志信息】
             *后续任务会有增加优先级区分，根据任务的优先级,错误的出现频率等,错误日志会定期推送到开发者邮箱和短信*/
            this.OpenOperator.Error("这里打印一条错误日志到任务调度平台", new Exception("错误msg信息"));

            /*从数据库获取任务的临时数据，临时数据以jason的形式保存在数据库里面，便于任务上下文的恢复和信息传递【注意：不应用于"频繁的"存储"大量的"临时数据，会操作网络耗时和数据库性能差】*/
            var databasetempinfo = this.OpenOperator.GetDataBaseTempData<DemoTaskDatabaseTempInfo>();
            if (databasetempinfo == null)//若任务第一次运行，可能没有临时数据。当然也可以在发布任务的时候配置临时数据也可。
            {
                databasetempinfo = new DemoTaskDatabaseTempInfo();
                databasetempinfo.LastLogID = 0;
            }

            /*将任务的临时数据持久化到数据库中，临时数据以json的形式保存在数据库里面，便于任务上下文的恢复和信息传递【注意：不应用于"频繁的"存储"大量的"临时数据，会操作网络耗时和数据库性能差】
              若临时数据用于下一次使用，必须要执行此方法，否则下次无法获取【注意:执行此方法,当前临时数据有可能被重置为null，便于内存资源释放】*/
            this.OpenOperator.SaveDataBaseTempData(databasetempinfo);

            /*从本地安装目录中获取任务的临时数据，临时数据以jason的形式保存在本地，便于任务上下文的恢复和信息传递【注意：本地临时数据一般用于保存"大量的"临时数据】*/
            var localtempinfo = this.OpenOperator.GetLocalTempData<DemoTaskLocalTempInfo>();
            if (localtempinfo == null)//若任务第一次运行，可能没有临时数据。当然也可以在发布任务的时候上传临时数据json至安装压缩包中也可。
            {
                localtempinfo = new DemoTaskLocalTempInfo();
                localtempinfo.file = new byte[0];
            }

            /*将任务的临时数据持久化到本地安装目录中，临时数据以json的形式保存在本地安装目录里面，便于任务上下文的恢复和信息传递【注意：本地临时数据一般用于保存"大量的"临时数据】
              若临时数据用于下一次使用，必须要执行此方法，否则下次可能无法获取【注意:执行此方法,当前临时数据有可能被重置为null，便于内存资源释放】*/
            this.OpenOperator.SaveLocalTempData(localtempinfo);

            {
                string msg = "执行业务中...."+ this.AppConfig["sendmailhost"];
                Debug.WriteLine(msg);
                System.IO.File.AppendAllText(this.OpenOperator.GetTaskInstallDirectory()+"业务.txt", msg);
            }
        }
        /// <summary>
        /// 开发人员自测运行入口
        /// 需要将项目配置为->控制台应用程序，写好Program类和Main入口函数
        /// </summary>
        public override void TestRun()
        {
            /*测试环境下任务的配置信息需要手工填写,正式环境下需要配置在任务配置中心里面*/
            this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            this.AppConfig.Add("sendmailhost", "smtp.163.com");
            this.AppConfig.Add("sendmailname", "fengyeguigui@163.com");
            this.AppConfig.Add("password", "472790378@");

            base.TestRun();
        }
    }

    /// <summary>
    /// 任务调度平台之临时数据信息,用于任务上下文的信息传递。
    /// 将会以json形式保存在任务调度平台数据库中,便于下一次回调运行时恢复并使用。【注意：不应用于"频繁的"存储"大量的"临时数据，会操作网络耗时和数据库性能差】
    /// </summary>
    public class DemoTaskDatabaseTempInfo
    {
        public int LastLogID { get; set; }
    }

    /// <summary>
    /// 任务调度平台之临时数据信息,用于任务上下文的信息传递。
    /// 将会以json形式保存在任务调度平台本地安装文件夹中,便于下一次回调运行时恢复并使用。【注意：本地临时数据一般用于保存"大量的"临时数据】
    /// </summary>
    public class DemoTaskLocalTempInfo
    {
        public byte[] file { get; set; }
    }
}
