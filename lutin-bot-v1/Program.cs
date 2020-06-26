using System;
using System.Threading.Tasks;
using bot;

namespace lutin_bot_v1
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