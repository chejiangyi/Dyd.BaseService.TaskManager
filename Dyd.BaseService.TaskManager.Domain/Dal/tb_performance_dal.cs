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
	public partial class tb_performance_dal
    {
        public int AddOrUpdate(DbConn PubConn, tb_performance_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@cpu", model.cpu);
                ps.Add("@memory", model.memory);
                ps.Add("@installdirsize", model.installdirsize);
                ps.Add("@taskid", model.taskid);
                ps.Add("@nodeid", model.nodeid); 
                ps.Add("@lastupdatetime", model.lastupdatetime);
                ps.Add("@id", model.id);
                string updatecmd = "update tb_performance set cpu=@cpu,memory=@memory,installdirsize=@installdirsize,nodeid=@nodeid,lastupdatetime=@lastupdatetime where taskid=@taskid";
                string insertcmd = @"insert into tb_performance(cpu,memory,installdirsize,taskid,nodeid,lastupdatetime)
										   values(@cpu,@memory,@installdirsize,@taskid,@nodeid,@lastupdatetime)";
                if (PubConn.ExecuteSql(updatecmd, ps.ToParameters()) <= 0)
                {
                    PubConn.ExecuteSql(insertcmd, ps.ToParameters());
                }
                return 1;
            });
        }

        public List<tb_performanceinfo_model> GetAllWithTask(DbConn PubConn, string nodeid, string taskid, string orderby, DateTime? lastupdatetime)
        {
            return SqlHelper.Visit(ps =>
            {
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select p.*,n.nodename,t.taskname from tb_performance p,tb_node n,tb_task t where p.nodeid=n.id and p.taskid=t.id ");
                if (!string.IsNullOrEmpty(nodeid))
                { 
                    stringSql.Append(@" and p.nodeid=@nodeid ");
                    ps.Add("@nodeid",nodeid);
                }
                if (!string.IsNullOrEmpty(taskid))
                {
                    stringSql.Append(@" and taskid=@taskid ");
                    ps.Add("@taskid", taskid);
                }
                if (lastupdatetime!=null)
                {
                    stringSql.Append(@" and lastupdatetime>@lastupdatetime ");
                    ps.Add("@lastupdatetime", lastupdatetime);
                }
                if (!string.IsNullOrEmpty(orderby))
                {
                    stringSql.Append(@" order by  " + orderby);
                }
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_performanceinfo_model> rs = new List<tb_performanceinfo_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tb_performanceinfo_model m = new tb_performanceinfo_model();
                        m.model = CreateModel(dr);
                        m.nodename = Convert.ToString(dr["nodename"]);
                        m.taskname = Convert.ToString(dr["taskname"]);
                        rs.Add(m);
                    }
                }
                return rs;
            });
           
        }

        public List<tb_performanceinfo_model> GetAllWithNode(DbConn PubConn, string orderby, DateTime? lastupdatetime)
        {
            return SqlHelper.Visit(ps =>
            {
                StringBuilder stringSql = new StringBuilder();
                StringBuilder whereSql = new StringBuilder();
                if (lastupdatetime != null)
                {
                    whereSql.Append(@" and lastupdatetime>@lastupdatetime ");
                    ps.Add("@lastupdatetime", lastupdatetime);
                }
                stringSql.AppendFormat(@"select p.*, n.nodename,'' as taskname from ( select nodeid,0 as taskid,sum(cpu) as cpu,sum(memory) as memory,sum(installdirsize) as installdirsize,max(lastupdatetime) as lastupdatetime from tb_performance where 1=1 {0} group by nodeid ) p,tb_node n where p.nodeid=n.id",
                    whereSql.ToString());
                if (!string.IsNullOrEmpty(orderby))
                {
                    stringSql.Append(@" order by  "+orderby);
                }

                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_performanceinfo_model> rs = new List<tb_performanceinfo_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tb_performanceinfo_model m = new tb_performanceinfo_model();
                        m.model = CreateModel(dr);
                        m.nodename = Convert.ToString(dr["nodename"]);
                        m.taskname = Convert.ToString(dr["taskname"]);
                        rs.Add(m);
                    }
                }
                return rs;
            });

        }
    }
}