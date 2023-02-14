using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using TxtCreatorBOT.Extensions;
using TxtCreatorBot.Services;
using EventHandler = TxtCreatorBOT.Extensions.EventHandler;

namespace TxtCreatorBot.Events;

public class OnMessagePropositionEvent : EventHandler
{
    [EventHandler(EventType.MessageCreated)]
    public static async Task OnMessagePropositionAsync(DiscordClient discord, MessageCreateEventArgs ctx, BotService botService, ConfigService configService)
    {
        if (ctx.Author.IsBot) return;
        
        if (ctx.Channel.Id == configService.PropositionChannelId)
        {
            await ctx.Message.DeleteAsync();
            var discordMessage = new DiscordMessageBuilder().AddEmbed(botService.CreateEmbed(
                $"Propozycja użytkownika {ctx.Author.Username}",
                $"{ctx.Message.Content}"));
            var message = await ctx.Channel.SendMessageAsync(discordMessage);
            await message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":white_check_mark:"));
            await message.CreateReactionAsync(DiscordEmoji.FromName(discord, ":x:"));
        }
    }
}