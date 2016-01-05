using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.TaskManager.Core.CustomCorn
{
   
    /// <summary>
    /// 格式[simple,0,1,2012-01-01 17:25,2012-01-01 17:25]
    /// </summary>
    public class SimpleCorn:BaseCustomCorn
    {
        public SimpleCorn(string corn):base(corn)
        {
 
        }
        public SimpleCornInfo ConInfo { get; set; }
        public override void Parse()
        {
            ConInfo = new SimpleCornInfo();
            string[] cmds = Corn.Replace("[","").Replace("]","").Split(',');
            if (cmds.Length == 5)
            {
                ConInfo.RepeatInterval = ParseCmd<int?>("RepeatInterval", cmds[1]);
                ConInfo.RepeatCount = ParseCmd<int?>("RepeatCount", cmds[2]);
                ConInfo.StartTime = ParseCmd<DateTime?>("StartTime", cmds[3]);
                ConInfo.EndTime = ParseCmd<DateTime?>("EndTime", cmds[4]);
            }
            else
            {
                throw new Exception("Corn表达式解析失败,corn:" + Corn);
            }
        }
        
    }

    public class SimpleCornInfo
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? RepeatInterval { get; set; }
        public int? RepeatCount { get; set; }
    }

}
