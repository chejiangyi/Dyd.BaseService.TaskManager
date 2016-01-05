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
    /*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
    public partial class tb_user_dal
    {
        public List<tb_user_model> GetAllUsers(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select * from tb_user s order by id desc");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_user_model> rs = new List<tb_user_model>();
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

        public tb_user_model GetUserName(DbConn PubConn, string userstaffno)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("userstaffno", userstaffno);
                string sql = "select id,username,userrole from tb_user where userstaffno=@userstaffno";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tb_user_model m = CreateModel(ds.Tables[0].Rows[0]);
                    return m;
                }
                else
                    return null;
            });
        }

        public tb_user_model GetUser(DbConn PubConn, string userstaffno, string userpsw)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("userstaffno", userstaffno);
                ps.Add("userpsw", userpsw);
                string sql = "select id,username,userrole from tb_user where userstaffno=@userstaffno and userpsw=@userpsw";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tb_user_model m = CreateModel(ds.Tables[0].Rows[0]);
                    return m;
                }
                else
                    return null;
            });
        }


        public bool DeleteOneNode(DbConn PubConn, int id)
        {
            return SqlHelper.Visit<bool>(ps =>
            {
                ps.Add("id", id);
                string sql = "delete from tb_user where (select count(1) from tb_task where taskcreateuserid=@id)=0 and id=@id";
                int i = PubConn.ExecuteSql(sql, ps.ToParameters());
                return i > 0;
            });
        }
    }
}