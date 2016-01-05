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
	public partial class tb_error_dal
    {
        public int Add2(DbConn PubConn, tb_error_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@msg", model.msg);
                ps.Add("@errortype", model.errortype);
                ps.Add("@errorcreatetime", model.errorcreatetime);
                ps.Add("@taskid", model.taskid);
                ps.Add("@nodeid", model.nodeid);
                return PubConn.ExecuteSql(@"insert into tb_error(msg,errortype,errorcreatetime,taskid,nodeid)
										   values(@msg,@errortype,@errorcreatetime,@taskid,@nodeid)", ps.ToParameters());
            });
        }

        public List<tb_senderror_model> GetErrors(DbConn PubConn, int lastlogid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@lastlogid", lastlogid);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select top 100 s.*,t.taskcreateuserid,t.taskname from tb_error s,tb_task t where s.id>@lastlogid and s.taskid=t.id order by s.id desc");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_senderror_model> rs = new List<tb_senderror_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tb_senderror_model model = new tb_senderror_model();
                        model.error_model = CreateModel(dr);
                        model.taskcreateuserid = Convert.ToInt32(dr["taskcreateuserid"]);
                        model.taskname = Convert.ToString(dr["taskname"]);
                        rs.Add(model);
                    }
                }
                return rs;
            });
        }

        public List<tb_errorinfo_model> GetList(DbConn PubConn, string keyword, int id, string cstime, string cetime, int errortype, int taskid, int nodeid, int pagesize, int pageindex, out int count)
        {
            List<tb_errorinfo_model> model = new List<tb_errorinfo_model>();
            int _count = 0;
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = "";
                string sql = "select ROW_NUMBER() over(order by E.id desc) as rownum,E.*,ISNULL(T.taskcreateuserid,0) taskcreateuserid,T.taskname,u.username as taskcreateusername,n.nodename as tasknodename from tb_error E left join tb_task T on E.taskid=T.id left join tb_user u on u.id=T.taskcreateuserid left join tb_node n on n.id=E.nodeid where 1=1 ";
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and msg like '%'+@keyword+'%' ";
                }
                if (taskid != -1)
                {
                    ps.Add("taskid", taskid);
                    sqlwhere += " and E.taskid=@taskid";
                }
                if (nodeid != -1)
                {
                    ps.Add("nodeid", nodeid);
                    sqlwhere += " and E.nodeid=@nodeid";
                }
                if (id != -1)
                {
                    ps.Add("id", id);
                    sqlwhere = " and E.id=@id";
                }
                DateTime d = DateTime.Now;
                if (DateTime.TryParse(cstime, out d))
                {
                    ps.Add("CStime", Convert.ToDateTime(cstime));
                    sqlwhere += " and E.errorcreatetime>=@CStime";
                }
                if (DateTime.TryParse(cetime, out d))
                {
                    ps.Add("CEtime", Convert.ToDateTime(cetime));
                    sqlwhere += " and E.errorcreatetime<=@CEtime";
                }
                if (errortype != -1)
                {
                    ps.Add("errortype", errortype);
                    sqlwhere += " and E.errortype=@errortype";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_error E where 1=1 " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_errorinfo_model m = new tb_errorinfo_model();
                m.error_model = CreateModel(dr);
                m.taskcreateusername = Convert.ToString(dr["taskcreateusername"]);
                m.taskname = Convert.ToString(dr["taskname"]);
                m.tasknodename = Convert.ToString(dr["tasknodename"]);
                model.Add(m);
            }
            count = _count;
            return model;
        }
    }
}