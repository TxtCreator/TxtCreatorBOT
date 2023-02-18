using System.Text;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Extensions;
using DisCatSharp.Interactivity;
using DisCatSharp.Interactivity.Extensions;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Commands;

public class LeadBoardCommand : ApplicationCommandsModule
{
    private readonly BotService _botService;
    private readonly UserService _userService;

    public LeadBoardCommand(BotService botService, UserService userService)
    {
        _botService = botService;
        _userService = userService;
    }

    [SlashCommand("ranking", "Pokazuje ranking osób z ostrzeżeniami.")]
    public async Task LeadBoardAsync(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Chwila..", "Trwa generowanie rankingu..."), true);
        var pages = new List<Page>();
        var users = await _userService.GetAllUserModelsAsync();

        var i = 1;
        foreach (var (key, value) in users)
        {
            var description = new StringBuilder();
            value.ForEach(user =>
            {
                description.Append($"`{i}.` <@{user.UserId}> `:` {user.Warns} ostrzeżeń").Append("\n");
                i++;
            });
            var embed = _botService.CreateEmbed("Ranking", description.ToString()).WithFooter($"Na żądanie {ctx.User.UsernameWithDiscriminator} | Strona {key + 1}/{users.Count}", ctx.User.AvatarUrl);
            pages.Add(new Page("", embed));
        }

        await ctx.Channel.SendPaginatedMessageAsync(ctx.Member, pages);
    }

}