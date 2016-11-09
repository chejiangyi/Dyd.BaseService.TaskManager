国外网速慢,最新版本迁移至http://git.oschina.net/chejiangyi/Dyd.BaseService.TaskManager

## .net 简单任务调度平台 ##

用于.net dll,exe的任务的挂载，任务的隔离，调度执行，访问权限控制，监控，管理，日志，错误预警，性能分析等。

1) 平台基于quartz.net进行任务调度功能开发，采用C#代码编写, 支持corn表达式和第三方自定义的corn表达式扩展。

2) 架构以插件形式开发，具有良好的功能扩展性，稳定性，简单性，便于第三方开发人员进一步进行功能扩展。

3) 支持多节点集群，便于集群服务器的资源有效分配，任务的相互隔离。

4) 支持邮件形式的错误预警，便于运维及时处理任务异常等。


-- 车江毅 2015-06-17


## Dyd.BaseService.TaskManager源码部署须知： ##
- 1、删除各个项目中Newtonsoft.Json.dll的引用
- 2、检查各项目中XXF.dll的引用，确保引用了“引用”文件夹中的XXF.dll
- 3、执行,建模脚本.txt中的sql server脚本语句
- 4、【Dyd.BaseService.TaskManager.Web】 任务管理后台，部署此站点，修改web.config数据库的字符串连接
- 5、修改登录代码和用户权限（看源码），不用源码内部通过crm登录
- 6、【Dyd.BaseService.TaskManager.MonitorTasks】 监控任务，此任务需要重新生成，打包此此文件（原有的打包文件可以直接删除）
- 7、【Dyd.BaseService.TaskManager.WinService】 进行服务安装，修改安装包的实际路径
- 8、管理后台，可以正常访问后，请参照 “任务调度平台安装->任务调度平台部署流程(示例).xls”,来新建任务，调通任务可以正常进行，则为成功
<div>*(感谢俞忠亮同学整理)*<div>
## 开源相关群: .net 开源基础服务 238543768 ##
(大家都有本职工作，也许不能及时响应和跟踪解决问题，请谅解。)

## 任务demo示例 ##
<pre class="brush:c#;toolbar: true; auto-links: false;">using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Dyd.BaseService.TaskManager.Demo
{
    /// &lt;summary&gt;
    /// 任务调度平台 任务插件的写法demo及说明
    /// &lt;/summary&gt;
    public class DemoTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        /// &lt;summary&gt;
        /// 任务调度平台根据发布的任务时间配置，定时回调运行方法
        /// 开发人员的任务插件必须要重载并该方法
        /// &lt;/summary&gt;
        public override void Run()
        {
            /* 
             * this.OpenOperator 用于任务调度平台提供给第三方使用的所有api接口封装
             */
 
            /*获取当前任务dll安装目录*/
            this.OpenOperator.GetTaskInstallDirectory();
 
            /*打印一条日志到任务调度平台,因为日志会存到平台数据库，所以日志要精简，对任务出错时有分析价值【注意：不要频繁打印无用的，非必要的，对分析无价值的日志信息】*/
            this.OpenOperator.Log(&quot;这里打印一条日志到任务调度平台&quot;);
 
            /*打印一条错误到任务调度平台,因为日志会存到平台数据库，所以日志要精简，对任务出错时有分析价值【注意：不要频繁打印无用的，非必要的，对分析无价值的日志信息】
             *后续任务会有增加优先级区分，根据任务的优先级,错误的出现频率等,错误日志会定期推送到开发者邮箱和短信*/
            this.OpenOperator.Error(&quot;这里打印一条错误日志到任务调度平台&quot;, new Exception(&quot;错误msg信息&quot;));
 
            /*从数据库获取任务的临时数据，临时数据以jason的形式保存在数据库里面，便于任务上下文的恢复和信息传递【注意：不应用于&quot;频繁的&quot;存储&quot;大量的&quot;临时数据，会操作网络耗时和数据库性能差】*/
            var databasetempinfo = this.OpenOperator.GetDataBaseTempData&lt;DemoTaskDatabaseTempInfo&gt;();
            if (databasetempinfo == null)//若任务第一次运行，可能没有临时数据。当然也可以在发布任务的时候配置临时数据也可。
            {
                databasetempinfo = new DemoTaskDatabaseTempInfo();
                databasetempinfo.LastLogID = 0;
            }
 
            /*将任务的临时数据持久化到数据库中，临时数据以json的形式保存在数据库里面，便于任务上下文的恢复和信息传递【注意：不应用于&quot;频繁的&quot;存储&quot;大量的&quot;临时数据，会操作网络耗时和数据库性能差】
              若临时数据用于下一次使用，必须要执行此方法，否则下次无法获取【注意:执行此方法,当前临时数据有可能被重置为null，便于内存资源释放】*/
            this.OpenOperator.SaveDataBaseTempData(databasetempinfo);
 
            /*从本地安装目录中获取任务的临时数据，临时数据以jason的形式保存在本地，便于任务上下文的恢复和信息传递【注意：本地临时数据一般用于保存&quot;大量的&quot;临时数据】*/
            var localtempinfo = this.OpenOperator.GetLocalTempData&lt;DemoTaskLocalTempInfo&gt;();
            if (localtempinfo == null)//若任务第一次运行，可能没有临时数据。当然也可以在发布任务的时候上传临时数据json至安装压缩包中也可。
            {
                localtempinfo = new DemoTaskLocalTempInfo();
                localtempinfo.file = new byte[0];
            }
 
            /*将任务的临时数据持久化到本地安装目录中，临时数据以json的形式保存在本地安装目录里面，便于任务上下文的恢复和信息传递【注意：本地临时数据一般用于保存&quot;大量的&quot;临时数据】
              若临时数据用于下一次使用，必须要执行此方法，否则下次可能无法获取【注意:执行此方法,当前临时数据有可能被重置为null，便于内存资源释放】*/
            this.OpenOperator.SaveLocalTempData(localtempinfo);
 
            {
                string msg = &quot;执行业务中....&quot;+ this.AppConfig[&quot;sendmailhost&quot;];
                Debug.WriteLine(msg);
                System.IO.File.AppendAllText(this.OpenOperator.GetTaskInstallDirectory()+&quot;业务.txt&quot;, msg);
            }
        }
        /// &lt;summary&gt;
        /// 开发人员自测运行入口
        /// 需要将项目配置为-&gt;控制台应用程序，写好Program类和Main入口函数
        /// &lt;/summary&gt;
        public override void TestRun()
        {
            /*测试环境下任务的配置信息需要手工填写,正式环境下需要配置在任务配置中心里面*/
            this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            this.AppConfig.Add(&quot;sendmailhost&quot;, &quot;smtp.163.com&quot;);
            this.AppConfig.Add(&quot;sendmailname&quot;, &quot;fengyeguigui@163.com&quot;);
            this.AppConfig.Add(&quot;password&quot;, &quot;******&quot;);
 
            base.TestRun();
        }
    }
 
    /// &lt;summary&gt;
    /// 任务调度平台之临时数据信息,用于任务上下文的信息传递。
    /// 将会以json形式保存在任务调度平台数据库中,便于下一次回调运行时恢复并使用。【注意：不应用于&quot;频繁的&quot;存储&quot;大量的&quot;临时数据，会操作网络耗时和数据库性能差】
    /// &lt;/summary&gt;
    public class DemoTaskDatabaseTempInfo
    {
        public int LastLogID { get; set; }
    }
 
    /// &lt;summary&gt;
    /// 任务调度平台之临时数据信息,用于任务上下文的信息传递。
    /// 将会以json形式保存在任务调度平台本地安装文件夹中,便于下一次回调运行时恢复并使用。【注意：本地临时数据一般用于保存&quot;大量的&quot;临时数据】
    /// &lt;/summary&gt;
    public class DemoTaskLocalTempInfo
    {
        public byte[] file { get; set; }
    }
}</pre>

## web后端部分截图及安装 ##

<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093517_4KvA_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093517_diZM_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093517_MdYV_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093517_r7IR_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093518_4Hvd_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093518_WV03_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093518_9m4C_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093518_wmiF_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093518_zAvV_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093519_zIrW_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093519_vSpM_2379842.png"/>
</p>
<p>
    <img src="http://static.oschina.net/uploads/space/2015/0925/093519_EcBE_2379842.png"/>
</p>

## 未来构想： ##

- 1) 任务故障转移: 检测到任务持续故障n次或者故障频率，判定进行异地节点/节点集群内的任务启动，可支持n次故障恢复。
- 2）任务负载均衡: 多个任务并行执行，用于高资源负载任务的多节点运行。
- 3）任务拆分: 一个父级任务可以创建多个子任务，并对任务进行管理，调度，故障恢复，预警等。
