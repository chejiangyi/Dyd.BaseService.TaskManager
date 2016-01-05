using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace XXF.BaseService.TaskManager.model
{
    /// <summary>
    /// tb_task Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_task_model
    {
        /*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string taskname { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public int categoryid { get; set; }

        /// <summary>
        /// 节点id
        /// </summary>
        public int nodeid { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime taskcreatetime { get; set; }

        /// <summary>
        /// 任务更新时间
        /// </summary>
        public DateTime taskupdatetime { get; set; }

        /// <summary>
        /// 任务上一次执行时间
        /// </summary>
        public DateTime tasklaststarttime { get; set; }

        /// <summary>
        /// 任务上一次结束时间
        /// </summary>
        public DateTime tasklastendtime { get; set; }

        /// <summary>
        /// 任务出错时间
        /// </summary>
        public DateTime tasklasterrortime { get; set; }

        /// <summary>
        /// 任务连续出错次数
        /// </summary>
        public int taskerrorcount { get; set; }

        /// <summary>
        /// 任务总成功运行次数
        /// </summary>
        public int taskruncount { get; set; }

        /// <summary>
        /// 任务创建人id
        /// </summary>
        public int taskcreateuserid { get; set; }

        /// <summary>
        /// 任务执行状态，查看代码枚举
        /// </summary>
        public Byte taskstate { get; set; }

        /// <summary>
        /// 任务版本号
        /// </summary>
        public int taskversion { get; set; }

        /// <summary>
        /// 任务app配置字典
        /// </summary>
        public string taskappconfigjson { get; set; }

        /// <summary>
        /// 任务执行频率cron表达式
        /// </summary>
        public string taskcron { get; set; }

        /// <summary>
        /// 任务入口函数dll文件名
        /// </summary>
        public string taskmainclassdllfilename { get; set; }

        /// <summary>
        /// 任务入口执行函数的路径
        /// </summary>
        public string taskmainclassnamespace { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string taskremark { get; set; }

    }
}