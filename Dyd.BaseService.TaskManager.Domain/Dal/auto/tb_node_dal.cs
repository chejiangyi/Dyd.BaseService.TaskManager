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
	public partial class tb_node_dal
    {
        public virtual bool Add(DbConn PubConn, tb_node_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@nodename",    model.nodename),
					//
					new ProcedureParameter("@nodecreatetime",    model.nodecreatetime),
					//
					new ProcedureParameter("@nodeip",    model.nodeip),
					//
					new ProcedureParameter("@nodelastupdatetime",    model.nodelastupdatetime),
					//
					new ProcedureParameter("@ifcheckstate",    model.ifcheckstate)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_node(nodename,nodecreatetime,nodeip,nodelastupdatetime,ifcheckstate)
										   values(@nodename,@nodecreatetime,@nodeip,@nodelastupdatetime,@ifcheckstate)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_node_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@nodename",    model.nodename),
					//
					new ProcedureParameter("@nodecreatetime",    model.nodecreatetime),
					//
					new ProcedureParameter("@nodeip",    model.nodeip),
					//
					new ProcedureParameter("@nodelastupdatetime",    model.nodelastupdatetime),
					//
					new ProcedureParameter("@ifcheckstate",    model.ifcheckstate)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_node set nodename=@nodename,nodecreatetime=@nodecreatetime,nodeip=@nodeip,nodelastupdatetime=@nodelastupdatetime,ifcheckstate=@ifcheckstate where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_node where id=@id";
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

        public virtual tb_node_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_node s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_node_model CreateModel(DataRow dr)
        {
            var o = new tb_node_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("nodename"))
			{
				o.nodename = dr["nodename"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("nodecreatetime"))
			{
				o.nodecreatetime = dr["nodecreatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("nodeip"))
			{
				o.nodeip = dr["nodeip"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("nodelastupdatetime"))
			{
				o.nodelastupdatetime = dr["nodelastupdatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("ifcheckstate"))
			{
				o.ifcheckstate = dr["ifcheckstate"].Tobool();
			}
			return o;
        }
    }
}