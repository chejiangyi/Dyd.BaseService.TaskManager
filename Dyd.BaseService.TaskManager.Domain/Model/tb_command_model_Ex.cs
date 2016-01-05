using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.TaskManager.Domain.Model
{
    public class tb_command_model_Ex:tb_command_model
    {
        public string taskname { get; set; }
        public string nodename { get; set; }
    }
}
