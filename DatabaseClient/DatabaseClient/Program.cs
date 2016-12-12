using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseClient.ArgHandlers;

namespace DatabaseClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        public static async Task MainAsync(string[] args)
        {
            
            var shouldContinue = true;
            while (shouldContinue)
            {
                try
                {
                    shouldContinue = await CommandHandler.Handle(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid command. Please try in or enter help to see list of commands");
                }
            }
        }
    }
}
