using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;

namespace XXF.BaseService.TaskManager.dal
{
    public class tb_tempdata_dal
    {
        public virtual int SaveTempData(DbConn PubConn,int taskid,string json)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "update tb_tempdata set tempdatajson=@tempdatajson where taskid=@taskid";
                ps.Add("taskid", taskid);
                ps.Add("tempdatajson",json);
                return PubConn.ExecuteSql(cmd, ps.ToParameters());
            });
        }

        public virtual string GetTempData(DbConn PubConn, int taskid)
        {
            return SqlHelper.Visit(ps =>
            {
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select s.* from tb_tempdata s where s.taskid=@taskid");
                ps.Add("taskid", taskid);
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToString( ds.Tables[0].Rows[0]["tempdatajson"]);
                }
                return null;
            });
        }
    }
}
