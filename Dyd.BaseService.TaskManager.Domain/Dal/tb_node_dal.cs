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
	public partial class tb_node_dal
    {
        public int AddOrUpdate(DbConn PubConn, tb_node_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@nodename", model.nodename);
                ps.Add("@nodecreatetime", model.nodecreatetime);
                ps.Add("@nodeip", model.nodeip);
                ps.Add("@nodelastupdatetime", model.nodelastupdatetime);
                ps.Add("@ifcheckstate", model.ifcheckstate);
                ps.Add("@id", model.id);
                string updatecmd = "update tb_node set nodeip=@nodeip,nodelastupdatetime=@nodelastupdatetime where id=@id";
                string insertcmd = @"insert into tb_node(nodename,nodecreatetime,nodeip,ifcheckstate)
										   values(@nodename,@nodecreatetime,@nodeip,@ifcheckstate)";
                if (PubConn.ExecuteSql(updatecmd, ps.ToParameters()) <= 0)
                {
                    PubConn.ExecuteSql(insertcmd, ps.ToParameters());
                }
                return 1;
            });
        }

        public int Update(DbConn PubConn, tb_node_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@nodename", model.nodename);
              
                ps.Add("@nodeip", model.nodeip);
              
                ps.Add("@id", model.id);
                ps.Add("@ifcheckstate", model.ifcheckstate);
                string updatecmd = "update tb_node set nodeip=@nodeip,nodename=@nodename,ifcheckstate=@ifcheckstate where id=@id";
                
                return PubConn.ExecuteSql(updatecmd, ps.ToParameters()) ;
            });
        }

        public List<tb_node_model> GetList(DbConn PubConn, string keyword, string cstime, string cetime, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_node_model> Model = new List<tb_node_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = "";
                string sql = "select ROW_NUMBER() over(order by id desc) as rownum,id,nodename,nodecreatetime,nodeip,nodelastupdatetime,ifcheckstate from tb_node where 1=1 ";
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere = " and (nodename like '%'+@keyword+'%' or nodeip like '%'+@keyword+'%') ";
                }
                DateTime d = DateTime.Now;
                if (DateTime.TryParse(cstime, out d))
                {
                    ps.Add("CStime", Convert.ToDateTime(cstime));
                    sqlwhere += " and nodecreatetime>=@CStime";
                }
                if (DateTime.TryParse(cetime, out d))
                {
                    ps.Add("CEtime", Convert.ToDateTime(cetime));
                    sqlwhere += " and nodecreatetime<=@CEtime";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_node where 1=1 " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_node_model n = CreateModel(dr);
                Model.Add(n);
            }
            count = _count;
            return Model;
        }

        public List<tb_node_model> GetListAll(DbConn PubConn)
        {
            return SqlHelper.Visit<List<tb_node_model>>(ps =>
            {
                List<tb_node_model> Model = new List<tb_node_model>();
                string sql = "select id,nodename,nodecreatetime,nodeip,nodelastupdatetime,ifcheckstate from tb_node";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_node_model n = CreateModel(dr);
                    Model.Add(n);
                }
                return Model;
            });
        }

        public tb_node_model GetOneNode(DbConn PubConn, int id)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", id);
                string sql = "select id,nodename,nodecreatetime,nodeip,nodelastupdatetime,ifcheckstate from tb_node where id=@id";
                DataSet ds=new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                DataRow dr = ds.Tables[0].Rows[0];
                return CreateModel(dr);
            });
        }

        public bool DeleteOneNode(DbConn PubConn, int id)
        {
            return SqlHelper.Visit<bool>(ps =>
            {
                ps.Add("id", id);
                string sql = "delete from tb_node where (select count(1) from tb_task where nodeid=@id)=0 and id=@id";
                int i = PubConn.ExecuteSql(sql, ps.ToParameters());
                return i > 0;
            });
        }

        public int GetAvailableNode(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "select top 1 id from tb_node where (nodelastupdatetime='' or DATEDIFF(minute ,nodecreatetime,GETDATE())>20)";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, null);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                    return id;
                }
                else
                {
                    string sqlinsert = "insert into tb_node (nodename,nodecreatetime,nodeip)values('新增节点',getdate(),'') select @@IDENTITY";
                    int id = Convert.ToInt32(PubConn.ExecuteScalar(sqlinsert, null));
                    return id;
                }
            });
        }

        public List<tb_node_model> GetAllStopNodesWithNeedCheckState(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@nodelastupdatetime", PubConn.GetServerDate().AddMinutes(-5));
                string sql = "select id,nodename,nodecreatetime,nodeip,nodelastupdatetime,ifcheckstate from tb_node where nodelastupdatetime<@nodelastupdatetime and ifcheckstate='true'";
                DataSet ds = new DataSet();
                List<tb_node_model> rs = new List<tb_node_model>();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_node_model n = CreateModel(dr);
                    rs.Add(n);
                }
                return rs;
            });
        }
    }
}