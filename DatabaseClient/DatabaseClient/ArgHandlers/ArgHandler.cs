using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers
{
    public abstract class ArgHandler
    {
       
        public Dictionary<string, Func<List<string>, Task<bool>>> Handlers { get; set; }

        public virtual async Task<bool> Handle(List<string> args)
        {
            var arg = args[0];
            return await Handlers[arg].Invoke(args.GetRange(1, args.Count - 1));
        }

        public virtual async Task<bool> Help(List<string> args)
        {
            foreach (var key in Handlers.Keys)
            {
                Console.WriteLine(key);
            }
            return true;
        }
    }
}
