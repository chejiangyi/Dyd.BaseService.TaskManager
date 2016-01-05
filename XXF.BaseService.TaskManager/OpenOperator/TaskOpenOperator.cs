using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXF.BaseService.TaskManager.SystemRuntime;

namespace XXF.BaseService.TaskManager.OpenOperator
{
    /// <summary>
    /// 任务公开给第三方使用操作类
    /// </summary>
    public class TaskOpenOperator
    {
         /// <summary>
        /// 任务dll实例引用
        /// </summary>
        protected BaseDllTask DllTask = null;

        public TaskOpenOperator(BaseDllTask dlltask)
        {
            DllTask = dlltask;
        }

        /// <summary>
        /// 获取当前任务安装目录
        /// </summary>
        /// <returns></returns>
        public string GetTaskInstallDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\";
        }

        /// <summary>
        /// 保存任务临时数据至本地文件 ".json.txt"
        /// </summary>
        /// <param name="obj"></param>
        public void SaveLocalTempData(object obj)
        {
            DllTask.SystemRuntimeOperator.SaveLocalTempData(obj);
            obj = null;
        }
        /// <summary>
        /// 从本地临时文件获取任务临时数据 ".json.txt"
        /// </summary>
        /// <param name="obj"></param>
        public T GetLocalTempData<T>() where T : class
        {
            return DllTask.SystemRuntimeOperator.GetLocalTempData<T>();
        }
        /// <summary>
        /// 保存任务临时数据至数据库(数据不能太大,也不能很频繁)
        /// </summary>
        /// <param name="obj"></param>
        public void SaveDataBaseTempData(object obj)
        {
            if (DllTask.IsTesting == false)
            { 
                DllTask.SystemRuntimeOperator.SaveDataBaseTempData(obj);
                obj = null;
            }
        }
        /// <summary>
        /// 获取数据库任务临时数据
        /// </summary>
        /// <param name="obj"></param>
        public T GetDataBaseTempData<T>() where T : class
        {
            if (DllTask.IsTesting == false)
                return DllTask.SystemRuntimeOperator.GetDataBaseTempData<T>();
            else
                return null;
        }
        /// <summary>
        /// 写日志至线上数据库(不要频繁写日志，仅写一些便于分析的核心数据，或者非紧急的业务错误)
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            if (DllTask.IsTesting == false)
                DllTask.SystemRuntimeOperator.AddLog(new model.tb_log_model
                {
                    logcreatetime = DateTime.Now,
                    logtype = (byte)EnumTaskLogType.CommonLog,
                    msg = msg,
                    taskid = DllTask.SystemRuntimeInfo.TaskModel.id,
                    nodeid = DllTask.SystemRuntimeInfo.TaskModel.nodeid
                });
            else
                Debug.WriteLine("测试日志记录:" + msg);
        }
        /// <summary>
        /// 写错误日志至线上数据库,这些错误会通知到开发人员，所以不要写一些正常的业务错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exp"></param>
        public void Error(string msg, Exception exp)
        {
            msg = (msg + " 错误信息:" + exp.Message + " 堆栈打印:" + exp.StackTrace);
            if (DllTask.IsTesting == false)
                DllTask.SystemRuntimeOperator.AddError(new model.tb_error_model
                {
                    errorcreatetime = DateTime.Now,
                    errortype = (byte)EnumTaskLogType.CommonError,
                    msg = msg,
                    taskid = DllTask.SystemRuntimeInfo.TaskModel.id,
                    nodeid = DllTask.SystemRuntimeInfo.TaskModel.nodeid
                });
            else
                Debug.WriteLine("测试错误日志记录:" + msg);
        }
    }
}
