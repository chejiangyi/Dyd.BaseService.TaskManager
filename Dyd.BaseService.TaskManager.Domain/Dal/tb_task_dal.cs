using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.TaskManager.Domain.Model;
using XXF.ProjectTool;
using Dyd.BaseService.TaskManager.Core;

namespace Dyd.BaseService.TaskManager.Domain.Dal
{
	
	public partial class tb_task_dal
    {
        public List<int> GetTaskIDsByState(DbConn PubConn, int taskstate,int nodeid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@taskstate", taskstate);
                ps.Add("@nodeid", nodeid);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select id from tb_task s where s.taskstate=@taskstate and s.nodeid=@nodeid");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<int> rs = new List<int>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rs.Add(Convert.ToInt32(dr[0]));
                    }
                }
                return rs;
            });

        }

        public List<tb_task_model> GetLongRunningTaskIDs(DbConn PubConn, int maxrunningtimeseconds)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@maxrunningtime", DateTime.Parse("1900-01-01").AddSeconds((int)maxrunningtimeseconds));
                ps.Add("@taskstate", (int)EnumTaskState.Running);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select * from tb_task s where (s.tasklastendtime-s.tasklaststarttime)>@maxrunningtime and taskstate=@taskstate");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_task_model> rs = new List<tb_task_model>();
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

        public List<tb_tasklist_model> GetList(DbConn PubConn, string taskid, string keyword, string CStime, string CEtime, int categoryid, int nodeid ,int userid, int state , int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_tasklist_model> model = new List<tb_tasklist_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = "";
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by T.id desc) as rownum,T.*,C.categoryname,N.nodename,U.username from tb_task T ");
                sql.Append("left join tb_category C on C.id=T.categoryid ");
                sql.Append("left join tb_user U on U.id=T.taskcreateuserid ");
                sql.Append("left join tb_node N on N.id=T.nodeid where 1=1 ");
                if (!string.IsNullOrWhiteSpace(taskid))
                {
                    ps.Add("taskid", taskid);
                    sqlwhere += " and ( T.id =@taskid )";
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and ( T.taskname like '%'+@keyword+'%' or T.taskremark like '%'+@keyword+'%' )";
                }
                if (categoryid != -1)
                {
                    ps.Add("categoryid", categoryid);
                    sqlwhere += " and T.categoryid=@categoryid";
                }
                if (nodeid != -1)
                {
                    ps.Add("nodeid", nodeid);
                    sqlwhere += " and T.nodeid=@nodeid";
                }
                if (state != -999)
                {
                    ps.Add("taskstate", state);
                    sqlwhere += " and T.taskstate=@taskstate";
                }
                if (userid != -1)
                {
                    ps.Add("taskcreateuserid", userid);
                    sqlwhere += " and T.taskcreateuserid=@taskcreateuserid";
                }
                DateTime d=DateTime.Now;
                if (DateTime.TryParse(CStime, out d))
                {
                    ps.Add("CStime", Convert.ToDateTime(CStime));
                    sqlwhere += " and T.taskcreatetime>=@CStime";
                }
                if (DateTime.TryParse(CEtime, out d))
                {
                    ps.Add("CEtime", Convert.ToDateTime(CEtime));
                    sqlwhere += " and T.taskcreatetime<=@CEtime";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_task T where 1=1 " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_tasklist_model m = CreateModelList(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public List<tb_task_model> GetListAll(DbConn PubConn)
        {
            List<tb_task_model> model = new List<tb_task_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sql = "select id,taskname from tb_task";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_task_model m = CreateModelList(dr);
                model.Add(m);
            }
            return model;
        }

        public virtual tb_tasklist_model CreateModelList(DataRow dr)
        {
            var o = new tb_tasklist_model();

            //
            if (dr.Table.Columns.Contains("id"))
            {
                o.id = dr["id"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("taskname"))
            {
                o.taskname = dr["taskname"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("categoryid"))
            {
                o.categoryid = dr["categoryid"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("nodeid"))
            {
                o.nodeid = dr["nodeid"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("taskcreatetime"))
            {
                o.taskcreatetime = dr["taskcreatetime"].ToDateTime();
            }
            //
            if (dr.Table.Columns.Contains("taskupdatetime"))
            {
                o.taskupdatetime = dr["taskupdatetime"].ToDateTime();
            }
            //
            if (dr.Table.Columns.Contains("tasklaststarttime"))
            {
                o.tasklaststarttime = dr["tasklaststarttime"].ToDateTime();
            }
            //
            if (dr.Table.Columns.Contains("tasklastendtime"))
            {
                o.tasklastendtime = dr["tasklastendtime"].ToDateTime();
            }
            //
            if (dr.Table.Columns.Contains("tasklasterrortime"))
            {
                o.tasklasterrortime = dr["tasklasterrortime"].ToDateTime();
            }
            //
            if (dr.Table.Columns.Contains("taskerrorcount"))
            {
                o.taskerrorcount = dr["taskerrorcount"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("taskruncount"))
            {
                o.taskruncount = dr["taskruncount"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("taskcreateuserid"))
            {
                o.taskcreateuserid = dr["taskcreateuserid"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("taskstate"))
            {
                o.taskstate = dr["taskstate"].ToByte();
            }
            //
            if (dr.Table.Columns.Contains("taskversion"))
            {
                o.taskversion = dr["taskversion"].Toint();
            }
            //
            if (dr.Table.Columns.Contains("taskappconfigjson"))
            {
                o.taskappconfigjson = dr["taskappconfigjson"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("taskcron"))
            {
                o.taskcron = dr["taskcron"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("taskmainclassnamespace"))
            {
                o.taskmainclassnamespace = dr["taskmainclassnamespace"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("taskremark"))
            {
                o.taskremark = dr["taskremark"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("nodename"))
            {
                o.nodename = dr["nodename"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("categoryname"))
            {
                o.categoryname = dr["categoryname"].Tostring();
            }
            //
            if (dr.Table.Columns.Contains("username"))
            {
                o.username = dr["username"].Tostring();
            }
            return o;
        }

        public tb_task_model GetOneTask(DbConn PubConn, int taskid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", taskid);
                string sql = "select * from tb_task where id=@id";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                tb_task_model model = CreateModel(ds.Tables[0].Rows[0]);
                return model;
            });
        }

        public int AddTask(DbConn PubConn, tb_task_model model)
        {
            return SqlHelper.Visit<int>(ps =>
            {
                ps.Add("@taskname", model.taskname);
                ps.Add("@categoryid", model.categoryid);
                ps.Add("@nodeid", model.nodeid);
                ps.Add("@taskcreatetime", model.taskcreatetime);
                ps.Add("@taskupdatetime", model.taskcreatetime);
                ps.Add("@taskerrorcount", 0);
                ps.Add("@taskruncount", 0);
                ps.Add("@taskcreateuserid", model.taskcreateuserid);
                ps.Add("@taskstate",0);
                ps.Add("@taskversion", 1);
                ps.Add("@taskappconfigjson", model.taskappconfigjson.NullToEmpty());
                ps.Add("@taskcron", model.taskcron);
                ps.Add("@taskmainclassnamespace", model.taskmainclassnamespace);
                ps.Add("@taskremark", model.taskremark);
                ps.Add("@taskmainclassdllfilename", model.taskmainclassdllfilename);
                int rev = Convert.ToInt32(PubConn.ExecuteScalar(@"insert into tb_task(taskname,categoryid,nodeid,taskcreatetime,taskruncount,taskcreateuserid,taskstate,taskversion,taskappconfigjson,taskcron,taskmainclassnamespace,taskremark,taskmainclassdllfilename)
										   values(@taskname,@categoryid,@nodeid,@taskcreatetime,@taskruncount,@taskcreateuserid,@taskstate,@taskversion,@taskappconfigjson,@taskcron,@taskmainclassnamespace,@taskremark,@taskmainclassdllfilename) select @@IDENTITY", ps.ToParameters()));
                return rev;
            });
        }

        public int UpdateTask(DbConn PubConn, tb_task_model model)
        {
            return SqlHelper.Visit<int>(ps =>
            {
                ps.Add("@id", model.id);
                ps.Add("@taskname", model.taskname);
                ps.Add("@categoryid", model.categoryid);
                ps.Add("@nodeid", model.nodeid);
                ps.Add("@taskupdatetime", model.taskupdatetime);
                ps.Add("@taskcreateuserid", model.taskcreateuserid);
                ps.Add("@taskappconfigjson", model.taskappconfigjson.NullToEmpty());
                ps.Add("@taskcron", model.taskcron);
                ps.Add("@taskmainclassnamespace", model.taskmainclassnamespace);
                ps.Add("@taskremark", model.taskremark);
                ps.Add("@taskmainclassdllfilename", model.taskmainclassdllfilename);
                ps.Add("@taskversion", model.taskversion);
                string sql = "Update tb_task Set taskname=@taskname,categoryid=@categoryid,nodeid=@nodeid,taskupdatetime=@taskupdatetime,";
                sql += "taskappconfigjson=@taskappconfigjson,taskcron=@taskcron,taskcreateuserid=@taskcreateuserid,";
                sql += "taskmainclassnamespace=@taskmainclassnamespace,taskremark=@taskremark,taskmainclassdllfilename=@taskmainclassdllfilename,taskversion=@taskversion";
                sql += " where id=@id";
                int i = PubConn.ExecuteSql(sql, ps.ToParameters());
                return i;
            });
        }

        public int CheckTaskState(DbConn PubConn, int id)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", id);
                string sql = "select taskstate from tb_task where id=@id";
                int i = Convert.ToInt32(PubConn.ExecuteScalar(sql, ps.ToParameters()));
                return i;
            });
        }

        public int ChangeTaskState(DbConn PubConn, int id, int state)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("taskstate", state);
                ps.Add("id", id);
                string sql = "update tb_task set taskstate=@taskstate where id=@id";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }

        public int UpdateTaskState(DbConn PubConn, int taskid, int taskstate)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@taskstate", taskstate);
                ps.Add("@id", taskid);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"update tb_task set taskstate=@taskstate where id=@id");
                return PubConn.ExecuteSql(stringSql.ToString(), ps.ToParameters());
            });
        }

        public bool DeleteOneTask(DbConn PubConn, int id)
        {
            return SqlHelper.Visit<bool>(ps =>
            {
                ps.Add("id", id);
                string sql = "delete from tb_task where taskstate=0 and id=@id";
                int i = PubConn.ExecuteSql(sql, ps.ToParameters());
                return i > 0;
            });
        }
        
    }
}