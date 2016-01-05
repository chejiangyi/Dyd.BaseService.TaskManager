using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.TaskManager.Domain.Model;

namespace Dyd.BaseService.TaskManager.Domain.Dal
{
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
	public partial class tb_performance_dal
    {
        public virtual bool Add(DbConn PubConn, tb_performance_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@nodeid",    model.nodeid),
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@cpu",    model.cpu),
					//
					new ProcedureParameter("@memory",    model.memory),
					//
					new ProcedureParameter("@installdirsize",    model.installdirsize),
					//
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_performance(nodeid,taskid,cpu,memory,installdirsize,lastupdatetime)
										   values(@nodeid,@taskid,@cpu,@memory,@installdirsize,@lastupdatetime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_performance_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@nodeid",    model.nodeid),
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@cpu",    model.cpu),
					//
					new ProcedureParameter("@memory",    model.memory),
					//
					new ProcedureParameter("@installdirsize",    model.installdirsize),
					//
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_performance set nodeid=@nodeid,taskid=@taskid,cpu=@cpu,memory=@memory,installdirsize=@installdirsize,lastupdatetime=@lastupdatetime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_performance where id=@id";
            int rev = PubConn.ExecuteSql(Sql, Par);
            if (rev == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual tb_performance_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_performance s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_performance_model CreateModel(DataRow dr)
        {
            var o = new tb_performance_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("nodeid"))
			{
				o.nodeid = dr["nodeid"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("taskid"))
			{
				o.taskid = dr["taskid"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("cpu"))
			{
				o.cpu = dr["cpu"].Todouble();
			}
			//
			if(dr.Table.Columns.Contains("memory"))
			{
				o.memory = dr["memory"].Todouble();
			}
			//
			if(dr.Table.Columns.Contains("installdirsize"))
			{
				o.installdirsize = dr["installdirsize"].Todouble();
			}
			//
			if(dr.Table.Columns.Contains("lastupdatetime"))
			{
				o.lastupdatetime = dr["lastupdatetime"].ToDateTime();
			}
			return o;
        }
    }
}