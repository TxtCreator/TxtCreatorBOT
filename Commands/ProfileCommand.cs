using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;
using DisCatSharp.Extensions;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Commands;

public class ProfileCommand : ApplicationCommandsModule
{
    private readonly UserService _userService;
    private readonly BotService _botService;

    public ProfileCommand(UserService userService, BotService botService)
    {
        _userService = userService;
        _botService = botService;
    }

    [ContextMenu(ApplicationCommandType.User, "Pokaż profil")]
    public async Task ProfileAsync(ContextMenuContext ctx)
    {
        var userModel = await _userService.GetUserModelAsync(ctx.TargetUser.Id);
        await ctx.CreateResponseAsync(_botService.CreateEmbed($"Profil użytkownika {ctx.TargetUser.Username}", $"Ilość ostrzeżeń: {userModel.Warns}"), true);
    }
}