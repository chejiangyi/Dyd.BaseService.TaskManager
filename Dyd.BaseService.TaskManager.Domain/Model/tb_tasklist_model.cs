using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    public class tb_tasklist_model:tb_task_model
    {
        public string categoryname { get; set; }
        public string nodename { get; set; }
        public string username { get; set; }
    }
}
