using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.TaskManager.Core.CustomCorn
{
    /// <summary>
    /// 格式[runonce]
    /// </summary>
    public class RunOnceCorn : SimpleCorn
    {
        public RunOnceCorn(string corn)
            : base(corn)
        {
            Corn = "[simple,,1,,]";
        }
    }
}
