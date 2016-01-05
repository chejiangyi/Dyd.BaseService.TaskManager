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
	public partial class tb_log_dal
    {
        public virtual bool Add(DbConn PubConn, tb_log_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@msg",    model.msg),
					//
					new ProcedureParameter("@logtype",    model.logtype),
					//
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@nodeid",    model.nodeid)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_log(msg,logtype,logcreatetime,taskid,nodeid)
										   values(@msg,@logtype,@logcreatetime,@taskid,@nodeid)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_log_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@msg",    model.msg),
					//
					new ProcedureParameter("@logtype",    model.logtype),
					//
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@nodeid",    model.nodeid)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_log set msg=@msg,logtype=@logtype,logcreatetime=@logcreatetime,taskid=@taskid,nodeid=@nodeid where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_log where id=@id";
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

        public virtual tb_log_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_log s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_log_model CreateModel(DataRow dr)
        {
            var o = new tb_log_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("msg"))
			{
				o.msg = dr["msg"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("logtype"))
			{
				o.logtype = dr["logtype"].ToByte();
			}
			//
			if(dr.Table.Columns.Contains("logcreatetime"))
			{
				o.logcreatetime = dr["logcreatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("taskid"))
			{
				o.taskid = dr["taskid"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("nodeid"))
			{
				o.nodeid = dr["nodeid"].Toint();
			}
			return o;
        }
    }
}