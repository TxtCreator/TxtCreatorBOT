using DisCatSharp;
using DisCatSharp.EventArgs;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Events;

[EventHandler]
public class OnMemberLeave
{
    private readonly UserService _userService;

    public OnMemberLeave(UserService userService)
    {
        _userService = userService;
    }

    [Event(DiscordEvent.GuildMemberRemoved)]
    public async Task OnMemberLeaveAsync(DiscordClient client, GuildMemberRemoveEventArgs ctx)
    {
        await _userService.RemoveUserModelAsync(ctx.Member.Id);
    }
    
}