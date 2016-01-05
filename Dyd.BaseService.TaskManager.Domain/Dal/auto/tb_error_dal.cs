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
	public partial class tb_error_dal
    {
        public virtual bool Add(DbConn PubConn, tb_error_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@msg",    model.msg),
					//
					new ProcedureParameter("@errortype",    model.errortype),
					//
					new ProcedureParameter("@errorcreatetime",    model.errorcreatetime),
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@nodeid",    model.nodeid)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_error(msg,errortype,errorcreatetime,taskid,nodeid)
										   values(@msg,@errortype,@errorcreatetime,@taskid,@nodeid)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_error_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@msg",    model.msg),
					//
					new ProcedureParameter("@errortype",    model.errortype),
					//
					new ProcedureParameter("@errorcreatetime",    model.errorcreatetime),
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@nodeid",    model.nodeid)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_error set msg=@msg,errortype=@errortype,errorcreatetime=@errorcreatetime,taskid=@taskid,nodeid=@nodeid where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_error where id=@id";
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

        public virtual tb_error_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_error s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_error_model CreateModel(DataRow dr)
        {
            var o = new tb_error_model();
			
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
			if(dr.Table.Columns.Contains("errortype"))
			{
				o.errortype = dr["errortype"].ToByte();
			}
			//
			if(dr.Table.Columns.Contains("errorcreatetime"))
			{
				o.errorcreatetime = dr["errorcreatetime"].ToDateTime();
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