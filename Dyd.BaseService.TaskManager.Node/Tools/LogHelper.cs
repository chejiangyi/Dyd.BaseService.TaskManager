using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XXF.ProjectTool;
using XXF.Extensions;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using XXF.Log;

namespace Dyd.BaseService.TaskManager.Node.Tools
{
    /// <summary>
    /// 节点内部日志操作类
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="model"></param>
        private static void AddLog(tb_log_model model)
        {
            try
            {
                SqlHelper.ExcuteSql(GlobalConfig.TaskDataBaseConnectString, (c) =>
                {
                    tb_log_dal logdal = new tb_log_dal();
                    model.msg = model.msg.SubString2(1000);
                    logdal.Add2(c, model);
                });
            }
            catch (Exception exp)
            {
                XXF.Log.ErrorLog.Write("添加日志至数据库出错", exp);
            }
        }
        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="model"></param>
        private static void AddError(tb_error_model model)
        {
            try
            {
                AddLog(new tb_log_model { logcreatetime = model.errorcreatetime, logtype = model.errortype, msg = model.msg, taskid = model.taskid,nodeid=GlobalConfig.NodeID });
                SqlHelper.ExcuteSql(GlobalConfig.TaskDataBaseConnectString, (c) =>
                {
                    tb_error_dal errordal = new tb_error_dal();
                    model.msg = model.msg.SubString2(1000);
                    errordal.Add2(c, model);
                });
            }
            catch (Exception exp)
            {
                XXF.Log.ErrorLog.Write("添加错误日志至数据库出错", exp);
            }
        }
        /// <summary>
        /// 添加节点日志
        /// </summary>
        /// <param name="msg"></param>
        public static void AddNodeLog(string msg)
        {
            CommLog.Write(msg);
            tb_log_model model = new tb_log_model()
            {
                logcreatetime = DateTime.Now,
                logtype = (byte)XXF.BaseService.TaskManager.SystemRuntime.EnumTaskLogType.SystemLog,
                msg = msg,
                taskid = 0,
                nodeid = GlobalConfig.NodeID
            };
            AddLog(model);
        }
        /// <summary>
        /// 添加节点错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void AddNodeError(string msg, Exception exp)
        {
            if (exp == null)
                exp = new Exception();
            ErrorLog.Write(msg, exp);
            tb_error_model model = new tb_error_model()
            {
                errorcreatetime = DateTime.Now,
                errortype = (byte)XXF.BaseService.TaskManager.SystemRuntime.EnumTaskLogType.SystemError,
                msg = msg+" 错误信息:"+exp.Message + " 堆栈:"+exp.StackTrace,
                taskid = 0,
                nodeid = GlobalConfig.NodeID
            };
            AddError(model);
        }
        /// <summary>
        /// 添加任务日志
        /// </summary>
        /// <param name="msg"></param>
        public static void AddTaskLog(string msg, int taskid)
        {
            tb_log_model model = new tb_log_model()
            {
                logcreatetime = DateTime.Now,
                logtype = (byte)XXF.BaseService.TaskManager.SystemRuntime.EnumTaskLogType.CommonLog,
                msg = msg,
                taskid = taskid,
                nodeid = GlobalConfig.NodeID
            };
            AddLog(model);
        }
        /// <summary>
        /// 添加任务错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void AddTaskError(string msg, int taskid, Exception exp)
        {
            ErrorLog.Write(msg+"[taskid:"+taskid+"]", exp);
            if (exp == null)
                exp = new Exception();
            tb_error_model model = new tb_error_model()
            {
                errorcreatetime = DateTime.Now,
                errortype = (byte)XXF.BaseService.TaskManager.SystemRuntime.EnumTaskLogType.CommonError,
                msg = msg + " 错误信息:" + exp.Message + " 堆栈:" + exp.StackTrace,
                taskid = taskid,
                nodeid = GlobalConfig.NodeID
            };
            AddError(model);
        }
    }
}
