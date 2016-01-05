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
	public partial class tb_command_dal
    {
        public virtual bool Add(DbConn PubConn, tb_command_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//命令json
					new ProcedureParameter("@command",    model.command),
					//命令名，参考代码枚举
					new ProcedureParameter("@commandname",    model.commandname),
					//命令执行状态，参考代码枚举
					new ProcedureParameter("@commandstate",    model.commandstate),
					//任务id
					new ProcedureParameter("@taskid",    model.taskid),
					//节点id
					new ProcedureParameter("@nodeid",    model.nodeid),
					//命令创建时间
					new ProcedureParameter("@commandcreatetime",    model.commandcreatetime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_command(command,commandname,commandstate,taskid,nodeid,commandcreatetime)
										   values(@command,@commandname,@commandstate,@taskid,@nodeid,@commandcreatetime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_command_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//命令json
					new ProcedureParameter("@command",    model.command),
					//命令名，参考代码枚举
					new ProcedureParameter("@commandname",    model.commandname),
					//命令执行状态，参考代码枚举
					new ProcedureParameter("@commandstate",    model.commandstate),
					//任务id
					new ProcedureParameter("@taskid",    model.taskid),
					//节点id
					new ProcedureParameter("@nodeid",    model.nodeid),
					//命令创建时间
					new ProcedureParameter("@commandcreatetime",    model.commandcreatetime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_command set command=@command,commandname=@commandname,commandstate=@commandstate,taskid=@taskid,nodeid=@nodeid,commandcreatetime=@commandcreatetime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_command where id=@id";
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

        public virtual tb_command_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_command s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_command_model CreateModel(DataRow dr)
        {
            var o = new tb_command_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//命令json
			if(dr.Table.Columns.Contains("command"))
			{
				o.command = dr["command"].Tostring();
			}
			//命令名，参考代码枚举
			if(dr.Table.Columns.Contains("commandname"))
			{
				o.commandname = dr["commandname"].Tostring();
			}
			//命令执行状态，参考代码枚举
			if(dr.Table.Columns.Contains("commandstate"))
			{
				o.commandstate = dr["commandstate"].ToByte();
			}
			//任务id
			if(dr.Table.Columns.Contains("taskid"))
			{
				o.taskid = dr["taskid"].Toint();
			}
			//节点id
			if(dr.Table.Columns.Contains("nodeid"))
			{
				o.nodeid = dr["nodeid"].Toint();
			}
			//命令创建时间
			if(dr.Table.Columns.Contains("commandcreatetime"))
			{
				o.commandcreatetime = dr["commandcreatetime"].ToDateTime();
			}
			return o;
        }
    }
}