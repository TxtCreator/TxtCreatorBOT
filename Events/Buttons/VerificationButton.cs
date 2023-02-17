using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Events.Buttons;

[EventHandler]
public class VerificationButton
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public VerificationButton(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }

    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task VerificationAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (ctx.Id != "verification") return;
        await ((DiscordMember)ctx.User).GrantRoleAsync(ctx.Guild.GetRole(_configService.VerificationRoleId));
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Sukces!", "Od teraz jesteś zweryfikowany.", ephemeral: true));
    }
}