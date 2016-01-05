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
    public partial class tb_command_dal
    {
        public List<tb_command_model> GetNodeCommands(DbConn PubConn, int nodeid, int lastmaxid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@nodeid", nodeid);
                ps.Add("@id", lastmaxid);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select s.* from tb_command s where (s.nodeid=@nodeid or s.nodeid=0) and s.id>@id order by id asc");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_command_model> rs = new List<tb_command_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rs.Add(CreateModel(dr));
                    }
                }
                return rs;
            });
        }

        public int GetMaxCommandID(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select max(id) from tb_command s ");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                  return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                return 0;
            });
        }

        public void UpdateCommandState(DbConn PubConn,int id,int commandstate)
        {
            SqlHelper.Visit(ps =>
            {
                ps.Add("@commandstate", commandstate);
                ps.Add("@id", id);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"update tb_command set commandstate=@commandstate where id=@id");
                return PubConn.ExecuteSql(stringSql.ToString(),ps.ToParameters());
            });
        }

        public List<tb_command_model_Ex> GetList(DbConn PubConn, int commandstate, int taskid, int nodeid, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_command_model_Ex> Model = new List<tb_command_model_Ex>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = "";
                string sql = "select ROW_NUMBER() over(order by C.id desc) as rownum,C.*,T.taskname,n.nodename from tb_command C LEFT JOIN tb_task T on  C.taskid=T.id left join tb_node n on C.nodeid=n.id where 1=1 ";
                if (taskid!=-1)
                {
                    ps.Add("taskid", taskid);
                    sqlwhere = " and C.taskid=@taskid ";
                }
                if (nodeid != -1)
                {
                    ps.Add("nodeid", nodeid);
                    sqlwhere = " and C.nodeid=@nodeid ";
                }
                if (commandstate != -1)
                {
                    ps.Add("commandstate", commandstate);
                    sqlwhere = " and C.commandstate=@commandstate ";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_command C where 1=1 " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_command_model_Ex n = CreateModelEX(dr);
                Model.Add(n);
            }
            count = _count;
            return Model;
        }

        public virtual tb_command_model_Ex CreateModelEX(DataRow dr)
        {
            var o = new tb_command_model_Ex();

            //
            if (dr.Table.Columns.Contains("id"))
            {
                o.id = dr["id"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("command"))
            {
                o.command = dr["command"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("commandname"))
            {
                o.commandname = dr["commandname"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("commandstate"))
            {
                o.commandstate = dr["commandstate"].ToByte();
            }
            //
            if (dr.Table.Columns.Contains("taskid"))
            {
                o.taskid = dr["taskid"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("nodeid"))
            {
                o.nodeid = dr["nodeid"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("commandcreatetime"))
            {
                o.commandcreatetime = dr["commandcreatetime"].ToDateTime();
            }
            //
            if (dr.Table.Columns.Contains("taskname"))
            {
                o.taskname = dr["taskname"].ToString();
            }
            if (dr.Table.Columns.Contains("nodename"))
            {
                o.nodename = dr["nodename"].ToString();
            }
            return o;
        }

        public int UpdateCommand(DbConn PubConn, tb_command_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("command", model.command);
                ps.Add("commandstate", model.commandstate);
                ps.Add("nodeid", model.nodeid);
                ps.Add("commandname", model.commandname);
                ps.Add("id", model.id);
                string sql = "update tb_command set command=@command,commandstate=@commandstate,commandname=@commandname,nodeid=@nodeid where id=@id";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }

        public tb_command_model_Ex GetOneCommand(DbConn PubConn, int id)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", id);
                string sql = "select C.*,T.taskname from tb_command C LEFT JOIN tb_task T on C.taskid=T.id where C.id=@id";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                tb_command_model_Ex model = CreateModelEX(ds.Tables[0].Rows[0]);
                return model;
            });
        }
    }
}