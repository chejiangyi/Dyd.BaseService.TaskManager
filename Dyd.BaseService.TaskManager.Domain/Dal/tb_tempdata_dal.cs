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
    public partial class tb_tempdata_dal
    {
        public int UpdateByTaskID(DbConn PubConn, tb_tempdata_model model)
        {
            return SqlHelper.Visit<int>(ps =>
            {
                ps.Add("@taskid", model.taskid);
                ps.Add("@tempdatajson", model.tempdatajson);
                ps.Add("@tempdatalastupdatetime", model.tempdatalastupdatetime);
                ps.Add("@id", model.id);

                int rev = PubConn.ExecuteSql("update tb_tempdata set tempdatajson=@tempdatajson,tempdatalastupdatetime=@tempdatalastupdatetime where taskid=@taskid", ps.ToParameters());
                return rev;
            });
        }

        public tb_tempdata_model GetByTaskID(DbConn PubConn, int taskid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("taskid", taskid);
                string sql = "select * from tb_tempdata where taskid=@taskid";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                tb_tempdata_model model = CreateModel(ds.Tables[0].Rows[0]);
                return model;
            });
        }
    }
}