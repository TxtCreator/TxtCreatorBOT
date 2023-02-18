using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Extensions;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Commands;

public class TicketCommand : ApplicationCommandsModule
{
    private readonly BotService _botService;

    public TicketCommand(BotService botService)
    {
        _botService = botService;
    }
    
    [SlashCommand("ticket", "Tworzy wiadomość do ticketów.")]
    public async Task TicketAsync(InteractionContext ctx)
    {
        var embed = _botService.CreateEmbed("Centrum Pomocy","Jeśli natknąłeś na: \n`a) jakiś problem`\n`b) chcesz aplikować na partnera lub twórcę`\nstwórz ticket!");
        await ctx.Channel.SendMessageAsync(
            new DiscordMessageBuilder().AddComponents(new DiscordButtonComponent(ButtonStyle.Primary, "@ticket.problem",
                "Problem"),
                new DiscordButtonComponent(ButtonStyle.Primary, "@ticket.współpraca",
                "Współpraca")).AddEmbed(embed));
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie stworzyłeś wiadomość do ticketów."), true);
    }
}