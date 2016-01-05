using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    public class tb_loginfo_model
    {
        public tb_log_model log_model { get; set; }
        public string taskusername{ get; set; }
        public string tasknodename { get; set; }
        public string taskname { get; set; }
    }
}
