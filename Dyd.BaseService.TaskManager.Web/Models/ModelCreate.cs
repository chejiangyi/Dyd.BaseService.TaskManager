using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Domain.Dal;
using System.Data;
using XXF.Db;

namespace Dyd.BaseService.TaskManager.Web.Models
{
    public class ModelCreate
    {
        #region 节点
        public static List<tb_node_model> GetNodeList(DbConn PubConn, string keyword, int pagesize, int pageindex, out int count)
        {
            DataSet ds = new tb_node_dal().GetList(PubConn, keyword, pagesize, pageindex, out count);
            List<tb_node_model> Model = new List<tb_node_model>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tb_node_model m = CreateModelForNode(dr);
                Model.Add(m);
            }
            return Model;
        }

        public static tb_node_model GetOneNode(DbConn PubConn,int id)
        {
            tb_node_dal dal = new tb_node_dal();
            DataRow dr = dal.GetOneNode(PubConn, id);
            tb_node_model model = CreateModelForNode(dr);
            return model;
        }

        private static tb_node_model CreateModelForNode(DataRow dr)
        {
            return new tb_node_model()
            {
                id = Convert.ToInt32(dr["id"]),
                nodename = dr["nodename"].ToString(),
                nodeip = dr["nodeip"].ToString(),
                nodecreatetime = Convert.ToDateTime(dr["nodecreatetime"]),
                nodelastupdatetime = Convert.ToDateTime(dr["nodelastupdatetime"])
            };
        }
        #endregion

    }
}