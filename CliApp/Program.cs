using System;
using System.Threading.Tasks;
using Bot;

namespace CliApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        
        public async Task MainAsync()
        {
            var lutin = new LutinBot(Environment.GetEnvironmentVariable("Token"));
            await lutin.Start(); 
        }
    }
}