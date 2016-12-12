using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers
{
    public class CommandHandler : ArgHandler
    {
        public static CommandHandler Singleton { get; set; }
        static CommandHandler()
        {
            Singleton = new CommandHandler();
            var handlers = new Dictionary<string, Func<List<string>, Task<bool>>>();
            Singleton.Handlers = handlers;
            handlers.Add("create", CreateHandler.Singleton.Handle);
            handlers.Add("update", UpdateHandler.Singleton.Handle);
            handlers.Add("find", QueriesHandler.Singleton.Handle);
            handlers.Add("help", Singleton.Help);
            handlers.Add("quit", Singleton.Quit);
        }

        public static async Task<bool> Handle(string args)
        {
            return await Singleton.Handle(args.Split(' ').ToList());
        }
        public async Task<bool> Quit(List<string> args)
        {
            return false;
        }
    }
}