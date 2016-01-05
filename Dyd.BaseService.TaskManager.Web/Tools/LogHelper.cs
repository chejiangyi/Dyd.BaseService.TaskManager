using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dyd.BaseService.TaskManager.Web.Models;
using XXF.ProjectTool;
using XXF.Extensions;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;

namespace Dyd.BaseService.TaskManager.Web.Tools
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
        public static void AddLog(tb_log_model model)
        {
            try
            {
                SqlHelper.ExcuteSql(Config.TaskConnectString, (c) =>
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
        public static void AddError(tb_error_model model)
        {
            try
            {
                AddLog(new tb_log_model { logcreatetime = model.errorcreatetime, logtype = model.errortype, msg = model.msg, taskid = model.taskid });
                SqlHelper.ExcuteSql(Config.TaskConnectString, (c) =>
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
    }
}