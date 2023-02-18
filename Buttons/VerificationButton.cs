using DisCatSharp.Entities;
using DisCatSharp.Extensions.Button;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Buttons;

public class VerificationButton : ButtonCommandModule
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public VerificationButton(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }

    [ButtonCommand("verification")]
    public async Task VerificationButtonAsync(ButtonContext ctx)
    {
        await ((DiscordMember)ctx.User).GrantRoleAsync(ctx.Guild.GetRole(_configService.VerificationRoleId));
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Od teraz jesteś zweryfikowany."), true);
    }
}