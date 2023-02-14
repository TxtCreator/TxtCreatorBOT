using DSharpPlus.EventArgs;
using TxtCreatorBOT.Extensions;
using TxtCreatorBot.Services;
using EventHandler = TxtCreatorBOT.Extensions.EventHandler;

namespace TxtCreatorBot.Events;

public class OnMemberJoin : EventHandler
{
    [EventHandler(EventType.GuildMemberAdded)]
    public static async Task OnMemberJoinAsync(GuildMemberAddEventArgs ctx, BotService botService, ConfigService configService)
    {
        await ctx.Guild.Channels[configService.LobbyChannelId].SendMessageAsync(botService.CreateEmbed($"Witaj {ctx.Member.Username}", "Serdecznie dziękujemy za dołączenie na serwer!\nNie zapomnij zapoznać się z **regulaminem**", thumbnail: ctx.Member.AvatarUrl, image:"https://cdn.discordapp.com/attachments/920442986339377193/1062506391119593643/siembon.png"));
    }

}