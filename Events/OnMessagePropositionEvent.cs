using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.EventArgs;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Events;

[EventHandler]
public class OnMessagePropositionEvent 
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public OnMessagePropositionEvent(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }
    
    [Event(DiscordEvent.MessageCreated)]
    public async Task OnMessageAsync(DiscordClient client, MessageCreateEventArgs ctx)
    {
        if (ctx.Author.IsBot) return;
        
        if (ctx.Channel.Id == _configService.PropositionChannelId)
        {
            await ctx.Message.DeleteAsync();
            var discordMessage = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed(
                $"Propozycja użytkownika {ctx.Author.Username}",
                $"{ctx.Message.Content}"));
            var message = await ctx.Channel.SendMessageAsync(discordMessage);
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, ":white_check_mark:"));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, ":x:"));
        }
    }
}