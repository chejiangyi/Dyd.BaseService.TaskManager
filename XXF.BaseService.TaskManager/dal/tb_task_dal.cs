using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;

namespace XXF.BaseService.TaskManager.dal
{
    public class tb_task_dal
    {

        public int UpdateLastStartTime(DbConn PubConn, int id, DateTime time)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "update tb_task set tasklaststarttime=@tasklaststarttime where id=@id";
                ps.Add("id", id);
                ps.Add("tasklaststarttime", time);
                return PubConn.ExecuteSql(cmd, ps.ToParameters());
            });
        }

        public int UpdateLastEndTime(DbConn PubConn, int id, DateTime time)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "update tb_task set tasklastendtime=@tasklastendtime where id=@id";
                ps.Add("id", id);
                ps.Add("tasklastendtime", time);
                return PubConn.ExecuteSql(cmd, ps.ToParameters());
            });
        }

        public int UpdateTaskError(DbConn PubConn, int id, DateTime time)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "update tb_task set taskerrorcount=taskerrorcount+1,tasklasterrortime=@tasklasterrortime where id=@id";
                ps.Add("id", id);
                ps.Add("tasklasterrortime", time);
                return PubConn.ExecuteSql(cmd, ps.ToParameters());
            });
        }

        public int UpdateTaskSuccess(DbConn PubConn, int id)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "update tb_task set taskerrorcount=0,taskruncount=taskruncount+1 where id=@id";
                ps.Add("id", id);
                return PubConn.ExecuteSql(cmd, ps.ToParameters());
            });
        }
    }
}
