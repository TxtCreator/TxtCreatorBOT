using DisCatSharp;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Events.Modals;

[EventHandler]
public class EmbedModal 
{
    private readonly BotService _botService;

    public EmbedModal(BotService botService)
    {
        _botService = botService;
    }
    
    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task EmbedAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (!ctx.Id.StartsWith("embed")) return;
        var id = ctx.Id.Split(".")[1];
        var components = ctx.Interaction.Data.Components;
        await (await ctx.Guild.GetChannelsAsync()).First(channel => channel.Id.ToString() == id).SendMessageAsync(
            _botService.CreateEmbed(components[0].Value, components[1].Value, components[2].Value)
                .WithImageUrl(components[3].Value).WithThumbnail(components[4].Value));
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Sukces!", "Pomyślnie stworzyłeś embed!", ephemeral: true));
    }
}