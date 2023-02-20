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
            var message = ctx.Message;
            await message.DeleteAsync();
            if (ctx.Message.Attachments.Count != 0)
            {
                try
                {
                    await ctx.Author.SendMessageAsync(_botService.CreateEmbed("Błąd!", "Nie możesz wysłać propozycji ze zdjęciem!", "red"));
                } catch (Exception) {}
                return;
            }
            var discordMessage = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed(
                $"Propozycja użytkownika {ctx.Author.Username}",
                $"{message.Content}"));
            var sendMessage = await ctx.Channel.SendMessageAsync(discordMessage);
            await sendMessage.CreateReactionAsync(DiscordEmoji.FromName(client, ":white_check_mark:"));
            await sendMessage.CreateReactionAsync(DiscordEmoji.FromName(client, ":x:"));
        }
    }
}