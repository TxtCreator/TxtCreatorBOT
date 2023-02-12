using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using TxtCreatorBot.Extend;
using TxtCreatorBot.Services;
using EventHandler = TxtCreatorBot.Extend.EventHandler;

namespace TxtCreatorBot.Events;

public class OnMessagePropositionEvent : EventHandler
{
    [EventHandler(EventType.MessageCreated)]
    public static async Task OnMessagePropositionAsync(DiscordClient discord, MessageCreateEventArgs args, BotService botService, ConfigService configService)
    {
        if (args.Author.IsBot) return;
        
        if (args.Channel.Id == configService.PropositionChannelId)
        {
            await args.Message.DeleteAsync();
            var discordMessage = new DiscordMessageBuilder().AddEmbed(botService.CreateEmbed(
                $"Propozycja użytkownika {args.Author.Username}",
                $"{args.Message.Content}"));
            var message = await args.Channel.SendMessageAsync(discordMessage);
            await message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":white_check_mark:"));
            await message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":x:"));
        }
    }
}