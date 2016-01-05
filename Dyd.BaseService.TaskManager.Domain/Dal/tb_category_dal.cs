using Dyd.BaseService.TaskManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Domain.Dal
{
    public partial class tb_category_dal
    {
        public virtual bool Add(DbConn PubConn, string categoryname)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					new ProcedureParameter("@categoryname",    categoryname),
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_category(categoryname,categorycreatetime)
										   values(@categoryname,getdate())", Par);
            return rev == 1;
        }

        public bool Update(DbConn PubConn, tb_category_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					new ProcedureParameter("@categoryname",    model.categoryname),
                    new ProcedureParameter("@id",    model.id),
                };
            int rev = PubConn.ExecuteSql(@"update tb_category set categoryname=@categoryname where id=@id", Par);
            return rev == 1;
        }

        public List<tb_category_model> GetList(DbConn PubConn,string keyword)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "select id,categoryname,categorycreatetime from tb_category ";
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sql += "where categoryname like '%'+@keyword+'%'";
                }
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                List<tb_category_model> Model = new List<tb_category_model>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_category_model m = CreateModel(dr);
                    Model.Add(m);
                }
                return Model;
            });
        }

        public bool DeleteOneNode(DbConn PubConn, int id)
        {
            return SqlHelper.Visit<bool>(ps =>
            {
                ps.Add("id", id);
                string sql = "delete from tb_category where (select count(1) from tb_task where categoryid=@id)=0 and id=@id";
                int i = PubConn.ExecuteSql(sql, ps.ToParameters());
                return i > 0;
            });
        }
    }
}
