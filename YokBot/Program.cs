using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YokBot.config;

namespace YokBot
{
    public sealed class Program
    {
        static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args)
        {
            var tokenAccesser = new TokenAccess();
            await tokenAccesser.ReadJSON();

            var discConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = tokenAccesser.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discConfig);

            Client.UseInteractivity(new InteractivityConfiguration() { Timeout = TimeSpan.FromMinutes(2) });

            Client.Ready += Client_Ready;

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Client.MessageAcknowledged += MessageHandler;
            Commands = Client.UseCommandsNext(new CommandsNextConfiguration());
            return Task.CompletedTask;
        }
        static Dictionary<string, int> wordCounts = new Dictionary<string, int>()
        {
            {"shite", 0},
            {"sht", 0},
            {"fk", 0},
        };
        private static async Task MessageHandler(DiscordClient client, MessageAcknowledgeEventArgs args)
        {
            string messageContent = args.Message.Content.ToLower();

            if (args.Message.Content.ToLower() == "~swearcount")
            {
                int totalCount = wordCounts.Values.Sum();
                await args.Channel.SendMessageAsync($"You have sworn a total of {totalCount} times.");
            }

                foreach (var word in wordCounts.Keys)
            {
                if (messageContent.Contains(word))
                {
                    wordCounts[word]++;
                    await args.Channel.SendMessageAsync($"{args.Message.Author.Mention} has said {word}. They have said it {wordCounts[word]} times!");
                    break; 
                }
            }
        }
    }
}
