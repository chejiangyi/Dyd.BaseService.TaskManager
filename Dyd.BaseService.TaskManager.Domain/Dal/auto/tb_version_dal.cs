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
	public partial class tb_version_dal
    {
        public virtual bool Add(DbConn PubConn, tb_version_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@version",    model.version),
					//
					new ProcedureParameter("@versioncreatetime",    model.versioncreatetime),
					//压缩文件二进制文件
					new ProcedureParameter("@zipfile",    model.zipfile),
					//
					new ProcedureParameter("@zipfilename",    model.zipfilename)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_version(taskid,version,versioncreatetime,zipfile,zipfilename)
										   values(@taskid,@version,@versioncreatetime,@zipfile,@zipfilename)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_version_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@taskid",    model.taskid),
					//
					new ProcedureParameter("@version",    model.version),
					//
					new ProcedureParameter("@versioncreatetime",    model.versioncreatetime),
					//压缩文件二进制文件
					new ProcedureParameter("@zipfile",    model.zipfile),
					//
					new ProcedureParameter("@zipfilename",    model.zipfilename)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_version set taskid=@taskid,version=@version,versioncreatetime=@versioncreatetime,zipfile=@zipfile,zipfilename=@zipfilename where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_version where id=@id";
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

        public virtual tb_version_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_version s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_version_model CreateModel(DataRow dr)
        {
            var o = new tb_version_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("taskid"))
			{
				o.taskid = dr["taskid"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("version"))
			{
				o.version = dr["version"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("versioncreatetime"))
			{
				o.versioncreatetime = dr["versioncreatetime"].ToDateTime();
			}
			//压缩文件二进制文件
			if(dr.Table.Columns.Contains("zipfile"))
			{
				o.zipfile = dr["zipfile"].ToBytes();
			}
			//
			if(dr.Table.Columns.Contains("zipfilename"))
			{
				o.zipfilename = dr["zipfilename"].Tostring();
			}
			return o;
        }
    }
}