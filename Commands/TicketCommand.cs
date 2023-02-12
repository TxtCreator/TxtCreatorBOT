using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Commands;

public class TicketCommand : ApplicationCommandModule
{
    private readonly BotService _botService;

    public TicketCommand(BotService botService)
    {
        _botService = botService;
    }
    
    [SlashCommand("ticket", "Tworzy wiadomość do ticketów.")]
    public async Task TicketCommandAsync(InteractionContext ctx)
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