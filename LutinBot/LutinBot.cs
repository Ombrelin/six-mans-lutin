using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Bot
{
    public class LutinBot
    {
        private readonly DiscordSocketClient Client;
        private readonly string Token;
        private readonly QueueEngine Engine = new QueueEngine();


        public LutinBot(string token)
        {
            this.Token = token;
            this.Client = new DiscordSocketClient();
            this.Client.MessageReceived += this.HandleMessage;
        }

        public async Task Start()
        {
            await this.Client.LoginAsync(TokenType.Bot, this.Token);
            await this.Client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task HandleMessage(SocketMessage message)
        {
            if (message.Content == "!q")
            {
                await message.Channel.SendMessageAsync(Engine.EnqueuePlayer(message.Author.Username));
            }

            if (message.Content == "!dq")
            {
                await message.Channel.SendMessageAsync(Engine.DequeuePLayer(message.Author.Username));
            }

            if (message.Content.StartsWith("!c"))
            {
                var queueId = Guid.Parse(message.Content.Split(" ")[1]);
                try
                {
                    var result = Engine.VoteCaptain(queueId);
                    var embed = new EmbedBuilder();
                    embed.WithTitle($"Queue {queueId} drawn");
                    foreach (var resultKey in result.Keys)
                    {
                        embed.AddField(resultKey, result[resultKey]);
                    }

                    await message.Channel.SendMessageAsync("", false, embed.Build());
                }
                catch (Exception e)
                {
                    await message.Channel.SendMessageAsync(e.Message);
                }
            }

            if (message.Content.StartsWith("!r"))
            {
                var givenId = Guid.Parse(message.Content.Split(" ")[1]);
                
            }
        }
    }
}