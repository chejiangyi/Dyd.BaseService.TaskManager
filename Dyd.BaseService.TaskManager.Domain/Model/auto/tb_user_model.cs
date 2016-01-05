using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// tb_user Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_user_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 员工工号
        /// </summary>
        public string userstaffno { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        
        /// <summary>
        /// 员工角色，查看代码枚举：开发人员，管理员
        /// </summary>
        public Byte userrole { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime usercreatetime { get; set; }
        
        /// <summary>
        /// 员工手机号码
        /// </summary>
        public string usertel { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string useremail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string userpsw { get; set; }
        
        
    }
}