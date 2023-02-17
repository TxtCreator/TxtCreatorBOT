using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;
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
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed($"Profil użytkownika {ctx.TargetUser.Username}", $"Ilość ostrzeżeń: {userModel.Warns}", ephemeral: true));
    }
}