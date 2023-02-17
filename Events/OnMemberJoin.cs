using DisCatSharp;
using DisCatSharp.EventArgs;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Events;

[EventHandler]
public class OnMemberJoin 
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public OnMemberJoin(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }
    
    [Event(DiscordEvent.GuildMemberAdded)]
    public async Task OnMemberJoinAsync(DiscordClient client, GuildMemberAddEventArgs ctx)
    {
        await ctx.Guild.Channels[_configService.LobbyChannelId].SendMessageAsync(_botService.CreateEmbed($"Witaj {ctx.Member.Username}", "Serdecznie dziękujemy za dołączenie na serwer!\nNie zapomnij zapoznać się z **regulaminem**.").WithImageUrl("https://cdn.discordapp.com/attachments/920442986339377193/1062506391119593643/siembon.png").WithThumbnail(ctx.Member.AvatarUrl));
    }

}