using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.TaskManager.Domain.Model;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Domain.Dal
{
	public partial class tb_version_dal
    {
        public virtual tb_version_model GetCurrentVersion(DbConn PubConn, int taskid,int version)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@taskid", taskid);
                ps.Add("@version", version);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select s.* from tb_version s where s.taskid=@taskid and s.version=@version");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    return CreateModel(ds.Tables[0].Rows[0]);
                }
                return null;
            });
        }

        public int GetVersion(DbConn PubConn, int taskid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("taskid", taskid);
                string sql = "select max(version) version from tb_version where taskid=@taskid";
                int version = Convert.ToInt32(PubConn.ExecuteScalar(sql, ps.ToParameters()));
                return version;
            });
        }

        public List<tb_version_model> GetTaskVersion(DbConn PubConn, int taskid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@taskid", taskid);
                string sql = "select version,zipfilename from tb_version where taskid=@taskid";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                List<tb_version_model> model = new List<tb_version_model>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_version_model m = CreateModel(dr);
                    model.Add(m);
                }
                return model;
            });
        }
    }
}