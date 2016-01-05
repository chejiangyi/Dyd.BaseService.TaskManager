using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    /// <summary>
    /// tb_category Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_category_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 分类名
        /// </summary>
        public string categoryname { get; set; }
        
        /// <summary>
        /// 分类创建时间
        /// </summary>
        public DateTime categorycreatetime { get; set; }
        
    }
}