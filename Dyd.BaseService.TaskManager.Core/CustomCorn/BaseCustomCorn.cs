using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.TaskManager.Core.CustomCorn
{
    public class BaseCustomCorn
    {
        public string Corn;
        public BaseCustomCorn(string corn)
        {
            Corn = corn;
        }

        public virtual void Parse()
        { }

        protected virtual T ParseCmd<T>(string name, string cmd)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cmd))
                { 
                    if(typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition()== typeof(Nullable<>))
                        return (T)Convert.ChangeType(cmd, Nullable.GetUnderlyingType(typeof(T)));
                    else
                        return (T)Convert.ChangeType(cmd, typeof(T));
                }
                return default(T);
            }
            catch (Exception)
            {
                throw new Exception("Corn表达式解析失败:" + name + " corn:" + Corn);
            }
        }


    }

    public class CustomCornFactory
    {
        public static BaseCustomCorn GetCustomCorn(string corn)
        {
            string[] cmds = corn.Replace("[", "").Replace("]", "").Split(',');
            string cmdname = cmds[0].ToLower();
            if (cmdname == "runonce")
            {
                return new RunOnceCorn(corn);
            }
            else if (cmdname == "simple")
            {
                return new SimpleCorn(corn);
            }
            throw new Exception("不可解析的自定义corn表达式");
        }
    }
}
